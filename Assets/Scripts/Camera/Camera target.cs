using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameratarget : MonoBehaviour
{
     public float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   

    void Update()
    {
    // Moves the object forward at two units per second.
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
