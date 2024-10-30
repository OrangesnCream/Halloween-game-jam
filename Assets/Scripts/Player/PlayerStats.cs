using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // This script will keep track of any info specific to each player 
    private int lives=6; 
    void Start()
    {
        
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
            //restart the level
        }
        lives--;
        //reset position
    }
    public void AddLives(int newLives){
        lives+=newLives;
    }

}
