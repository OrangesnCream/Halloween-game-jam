using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // Start is called before the first frame update
    private bool topReachedEnd=false;
    private bool bottomReachedEnd=false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(topReachedEnd&&bottomReachedEnd){
            //win state
            Debug.Log("YOU WIN !!!!!!!!!");
        }
    }
    public void bottomWin(){
        bottomReachedEnd=true;
    }
    public void topWin(){
        topReachedEnd=true;
    }
}
