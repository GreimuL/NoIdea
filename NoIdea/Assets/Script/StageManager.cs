using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static bool isOpen = false;

    public GameObject needle;
    public GameObject narrowNeedle;
    public GameObject player;
    public GameObject guideline;
    public Text scoreText;
    public GameObject retrybtn;
    public GameObject music;
    private PoolingManager poolingmanager;
    public bool isDead;
    private Needle needleSetting;

    public Camera cam;
    private float screenWidth;
    private float screenHeight;
    private float aspect;
    private float camHeight;
    private float camWidth;
    private bool isStart = false;
    private float alpha = 0f;
    private bool isAlphaIncrease = true;
    private int alphaCnt = 0;
    private int score = 0;
    void Start()
    {
        poolingmanager = GetComponent<PoolingManager>();
        retrybtn.SetActive(false);
        guideline.SetActive(false);
        isDead = false;
        score = 0;
        Random.InitState((int)System.DateTime.Now.Ticks);
        alpha = 0f;
        alphaCnt = 0;
        aspect = cam.aspect;
        camHeight = cam.orthographicSize*2f;
        camWidth = aspect * camHeight;
        player.transform.position = new Vector2(0, -camHeight / 2 + player.GetComponent<Renderer>().bounds.size.y/2);

        if (isOpen == true)
            startGame();
    }
    
    void Update()
    {
        if (isStart&&isDead==false)
        {
            score++;
            scoreText.text = score.ToString();
        }
        if (isDead)
        {
            retrybtn.SetActive(true);
        }
    }

    public void startGame()
    {
        guideline.SetActive(true);
        StartCoroutine("GuideLine");
    }

    public int getScore()
    {
        return score;
    }
    
    IEnumerator GuideLine()
    {
        Color curColor = guideline.GetComponent<Image>().color;
        while (true)
        {
            guideline.GetComponent<Image>().color = new Color(curColor.r, curColor.g, curColor.b, alpha);
            if (isAlphaIncrease)
            {
                alpha += 2f * Time.deltaTime;
                if (alpha >= 1f)
                {
                    isAlphaIncrease = false;
                    alphaCnt++;
                }
            }
            else
            {
                alpha -= 1f * Time.deltaTime;
                if (alpha <= 0f)
                {
                    isAlphaIncrease = true;
                    alphaCnt++;
                }
            }
            if (alphaCnt >= 4)
            {
                break;
            }
            yield return new WaitForSeconds(0.005f * Time.deltaTime);
        }
        guideline.SetActive(false);
        isStart = true;
        player.GetComponent<Player>().isStart = true;
        StartCoroutine("CreateNeedle");
        StartCoroutine("CreateNeedle2");
        StartCoroutine("CreateNarrowNeedle");
        yield return null;
    }

    IEnumerator CreateNeedle()
    {
        while (true)
        {
            GameObject activeNeedle = poolingmanager.getNeedle(0);
            activeNeedle.transform.position = new Vector2(-camWidth / 2, -camHeight / 2 + needle.GetComponent<Renderer>().bounds.size.y / 2);
            needleSetting = activeNeedle.GetComponent<Needle>();
            needleSetting.setVarialbe(1, 0);
            needleSetting.randomizeState();
            activeNeedle.SetActive(true);
            yield return new WaitForSeconds(Random.Range(80f,200f)*Time.deltaTime);
        }
    }
    IEnumerator CreateNeedle2()
    {
        while (true)
        {
            GameObject activeNeedle = poolingmanager.getNeedle(0);
            activeNeedle.transform.position = new Vector2(camWidth / 2, -camHeight / 2 + needle.GetComponent<Renderer>().bounds.size.y / 2);
            needleSetting = activeNeedle.GetComponent<Needle>();
            needleSetting.setVarialbe(-1, 0);
            needleSetting.randomizeState();
            activeNeedle.SetActive(true);
            yield return new WaitForSeconds(Random.Range(80f,200f)*Time.deltaTime);
        }
    }
    IEnumerator CreateNarrowNeedle()
    {
        while (true)
        {
            GameObject activeNeedle = poolingmanager.getNeedle(1);
            activeNeedle.transform.position = new Vector2(Random.Range(-camWidth / 2, camWidth / 2), camHeight / 2 - narrowNeedle.GetComponent<Renderer>().bounds.size.y / 2);
            activeNeedle.SetActive(true);

            yield return new WaitForSeconds(Random.Range(80f,200f)*Time.deltaTime);
        }
    }
}
