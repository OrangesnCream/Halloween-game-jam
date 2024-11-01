using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;



        private  Vector3 startingPosition;
        public static Transform playerPosition;

        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        public int jumpCount=2;
        private int remainingJumps=2;
        private float speedBuff=1;

        public bool isUpsidedown;

        public bool pauseMovement=false;

        private Animator anim;

        private SpriteRenderer m_SpriteRenderer;
        public float GetSpeedBuff(){
            return speedBuff;
        }
        public void SetSpeedBuff(float speedUp) {
            speedBuff=speedUp;
        }

        public void ResetToSpawn(){
            transform.position=startingPosition;
        }
        private void Awake()
        {
            pauseMovement=false;
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
            playerPosition = transform;
            startingPosition=transform.position;
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            anim=GetComponent<Animator>();

        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }
        }

        private void FixedUpdate()
        {

            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();
            if(!pauseMovement){
                ApplyMovement();
            }else{

                _frameVelocity.x=0;
                _frameVelocity.y=0;
                _rb.velocity = _frameVelocity;
            }
            
        }

        #region Collisions
        
        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);
            // Ground and Ceiling
            if(isUpsidedown){
                ceilingHit= Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
                groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);
                // Hit a Ceiling
                if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);
            }else{
                // Hit a Ceiling
                if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);
            }

            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                remainingJumps=jumpCount;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void HandleJump()
        {
            if(isUpsidedown){
                if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y < 0) _endedJumpEarly = true;
            }else{
                if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;
            }
            if (!_jumpToConsume && !HasBufferedJump) return;

            if (_grounded || CanUseCoyote||remainingJumps>0) ExecuteJump();

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            if(isUpsidedown){
                _frameVelocity.y = _stats.JumpPower;
            }else{
                _frameVelocity.y = _stats.JumpPower;
            }
            

            
            remainingJumps-=1;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration*speedBuff * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed*speedBuff, _stats.Acceleration * Time.fixedDeltaTime);
                
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        private void ApplyMovement(){ 
            if(isUpsidedown){
               _frameVelocity.y = -_frameVelocity.y;
               //flip velocity
              // Debug.Log("framevelocity.y" + _frameVelocity.y);

            }
            _rb.velocity = _frameVelocity;
            if(isUpsidedown){
                //unflip velocity so the rest of the function does not notice 
                _frameVelocity.y= -_frameVelocity.y; 
            }else{
                if(_frameVelocity.x!=0){
                    anim.SetBool("isRunning",true);
                }else{
                    anim.SetBool("isRunning",false);
                }
                
                //player anim flip
                if(_frameVelocity.x > 0){
                    m_SpriteRenderer.flipX=false;
                // AnimationDirection.localScale = new Vector3(1f, 1f, 1f);
                } else if(_frameVelocity.x<0) {
                    m_SpriteRenderer.flipX=true;
                //  AnimationDirection.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        //public event Action Run;
        public Vector2 FrameInput { get; }
    }
