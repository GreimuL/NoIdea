using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 currentPosition;
    public StageManager stageManager;
    public Camera cam;
    private float jumpStr = 1f;
    private float initSpeed = 10f;
    private float initAcc = 50f;
    private float ground;
    private float aspect;
    private float camHeight;
    private float camWidth;
    bool isDouble = false;
    bool isJump = false;
    bool isDir = true;
    public bool isStart = false;
    int isClick = 0;
    void Start()
    {
        gameObject.SetActive(true);
        isStart = false;
        aspect = cam.aspect;
        camHeight = cam.orthographicSize * 2f;
        camWidth = aspect * camHeight;
        ground = -camHeight / 2 + gameObject.GetComponent<Renderer>().bounds.size.y / 2;
    }
    
    void Update()
    {
        if (isStart)
        {
            if (isDir)
            {
                gameObject.transform.Translate(new Vector2(5f * Time.deltaTime, 0));
            }
            else
            {
                gameObject.transform.Translate(new Vector2(-5f * Time.deltaTime, 0));
            }
            if (gameObject.transform.position.x > camWidth / 2+gameObject.GetComponent<Renderer>().bounds.size.x/2 || gameObject.transform.position.x < -camWidth / 2)
            {
                isDir = !isDir;
            }
            for(int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began&&Input.GetTouch(i).position.x<Screen.width/2)
                {
                    if (isDir)
                    {
                        isDir = false;
                    }
                    else
                    {
                        isDir = true;
                    }
                }
                if (Input.GetTouch(i).phase==TouchPhase.Began&&Input.GetTouch(i).position.x>Screen.width/2 && isJump == false)
                {
                    isClick++;
                    isJump = true;
                    initSpeed = 13f;
                }           
            }
            if (isJump)
            {
                //gameObject.transform.localRotation = new Quaternion(0, 0, gameObject.transform.localRotation.z+10f, 0);
                gameObject.transform.Translate(new Vector2(0, initSpeed * Time.deltaTime));
                initSpeed -= initAcc * Time.deltaTime;
                if (gameObject.transform.position.y <= ground)
                {
                    isJump = false;
                    isDouble = false;
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x, ground);
                    gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                }
            }
            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isDir)
                {
                    isDir = false;
                }
                else
                {
                    isDir = true;
                }
            }
            if (Input.GetMouseButtonDown(0) == true && isJump == false)
            {
                isClick++;
                isJump = true;
                initSpeed = 13f;
            }
            if (isJump)
            {
                //gameObject.transform.localRotation = new Quaternion(0, 0, gameObject.transform.localRotation.z+10f, 0);
                gameObject.transform.Translate(new Vector2(0, initSpeed * Time.deltaTime));
                initSpeed -= initAcc * Time.deltaTime;
                if (gameObject.transform.position.y <= ground)
                {
                    isJump = false;
                    isDouble = false;
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x, ground);
                    gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                }
            }
            */
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stageManager.isDead = true;
        gameObject.SetActive(false);
    }
}
