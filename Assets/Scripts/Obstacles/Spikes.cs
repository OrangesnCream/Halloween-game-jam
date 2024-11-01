using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] players;
    void Start()
    {
        if (players == null)
            players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Player") ){
            foreach(GameObject player in players){
                player.gameObject.GetComponent<PlayerStats>().KillPlayer();
            }
        }
    }
}
