using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // Start is called before the first frame update
    private bool topReachedEnd=false;
    private bool bottomReachedEnd=false;
    public GameObject loseScreen;
    public GameObject winScreen;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(topReachedEnd&&bottomReachedEnd){
            //win state
            //gives option for back to menu or next level
            Time.timeScale=0f;
            winScreen.SetActive(true);
            loseScreen.SetActive(false);
        }
    }
    public void bottomWin(){
        bottomReachedEnd=true;
    }
    public void topWin(){
        topReachedEnd=true;
    }
    public void GameLost(){
        //pause game and turn on lose screen
        Time.timeScale=0f;
        winScreen.SetActive(false);
        loseScreen.SetActive(true);
    }
}
