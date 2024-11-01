using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // This script will keep track of any info specific to each player 
    public GameObject gameManager;
    

    private int lives=6; 

    private Vector2 starting;

    private bool isUpsidedown;

    void Start()
    {
        isUpsidedown=gameObject.GetComponent<PlayerController>().isUpsidedown;
        starting=gameObject.GetComponent<Rigidbody2D>().position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetLives(int input){
        lives=input;
    }
    public int GetLives(){
        return lives;
    }

    public void KillPlayer(){
        if(lives==0){
            gameManager.GetComponent<GameStateManager>().GameLost();
            //call lose in game state

        }
        lives--;
        //reset position
        gameObject.GetComponent<Rigidbody2D>().position=starting;
        
    }
    public void AddLives(int newLives){
        lives+=newLives;
    }
    public void LevelEnd(){
        gameObject.GetComponent<PlayerController>().pauseMovement=true;
        if(isUpsidedown){
            gameManager.GetComponent<GameStateManager>().bottomWin();
        }
        if(!isUpsidedown){
            gameManager.GetComponent<GameStateManager>().topWin();
        }
    }

}
