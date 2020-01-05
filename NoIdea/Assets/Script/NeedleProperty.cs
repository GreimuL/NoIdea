using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleProperty : MonoBehaviour
{
    private float currentPositionX;
    private float currentPositionY;
    private float aspect;
    private float camHeight;
    private float camWidth;
    public int index;
    private Camera cam;
    // Start is called before the first frame update
    public virtual void Start()
    {
        cam = Camera.main;
        aspect = cam.aspect;
        camHeight = cam.orthographicSize * 2f;
        camWidth = aspect * camHeight;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        currentPositionX = gameObject.transform.position.x;
        currentPositionY = gameObject.transform.position.y;
        if (currentPositionX > camWidth/2+gameObject.GetComponent<Renderer>().bounds.size.x || currentPositionX < -camWidth/2-gameObject.GetComponent<Renderer>().bounds.size.x|| currentPositionY>camHeight/2||currentPositionY<-camHeight/2-gameObject.GetComponent<Renderer>().bounds.size.y)
        {
            gameObject.SetActive(false);
            if(this is Needle)
            {
                PoolingManager.inactive_needle_queue[0].Enqueue(gameObject);
            }
            else
            {
                PoolingManager.inactive_needle_queue[1].Enqueue(gameObject);
            }
        }
    }
}
