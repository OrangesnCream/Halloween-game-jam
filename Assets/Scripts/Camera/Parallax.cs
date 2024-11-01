using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame update
    private float length,startPos;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPos=transform.position.y;
        length= GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp=(cam.transform.position.x*(1-parallaxEffect));
        float dist = (cam.transform.position.x*parallaxEffect);
        float cameraCorner=cam.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1,1,cam.GetComponent<Camera>().nearClipPlane)).x;
        transform.position=new Vector3(startPos+dist, transform.position.y, transform.position.z);

        if(temp>startPos+length){
            startPos+=length;
        }else if (temp<startPos-length){
            startPos-=length;
        }
        

    }
}
