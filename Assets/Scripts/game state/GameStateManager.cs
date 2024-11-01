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
    public GameObject GameUI;

    public GameObject[] hearts;
    void Start()
    {
        Time.timeScale = 1f; 
        
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
            GameUI.SetActive(false);
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
        GameUI.SetActive(false);
        loseScreen.SetActive(true);
    }
    public void removeHeartUI(){
        bottomReachedEnd=false;
        topReachedEnd=false;
        for(int i=hearts.Length-1;i>=0;i--){
            if(hearts[i].activeSelf){
                hearts[i].SetActive(false);
                break;
            }
        }
    }
    public void addHeartUI(){
        for(int i=hearts.Length-1;i>=0;i--){
            if(hearts[i].gameObject.activeSelf&&i<hearts.Length-1){
                hearts[i+1].gameObject.SetActive(true);
                break;
            }
        }
    }
}
