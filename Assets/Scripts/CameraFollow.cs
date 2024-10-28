using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Player;

    Camera cam;
    float height;
    float width ;
    float edgeOfScreen;
    float smoothTime = 0.2f;
    float zStart,yStart;

    Vector3 target,refVel;

    public int currentScreen=90;
    // Start is called before the first frame update
    void Start()
    {
        
         cam = GetComponent<Camera>();
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
        Vector3 cameraTopRightCornerPos = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));
        edgeOfScreen=cameraTopRightCornerPos.x;
         target=Player.position;
         zStart = transform.position.z;
         yStart =transform.position.y;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 cameraTopRightCornerPos = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));
        float positionDelta=UpdateTargetPos().x-target.x;
        if(positionDelta+cameraTopRightCornerPos.x<(edgeOfScreen+width*currentScreen)){
            target = UpdateTargetPos();
        }
        UpdateCameraPosition();
    }
    Vector3 UpdateTargetPos()
    {
         Vector3 ret;
        
        ret = Player.position;
        ret.z = zStart;
        ret.y= yStart;
        return ret;
    }
    void UpdateCameraPosition()
    {
        Vector3 tempPos;
        tempPos=Vector3.SmoothDamp(transform.position,target,ref refVel,smoothTime);
        transform.position=tempPos;
    }
}
