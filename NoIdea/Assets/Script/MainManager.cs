using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject manager;
    public GameObject touchText;
    public GameObject fadeImg;
    public GameObject score;
    private float alpha = 0f;
    private bool isAlphaIncrease = true;
    private bool isStart = false;
    private float fadeAlpha = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (StageManager.isOpen == true)
            gameObject.SetActive(false);
        else
        {
            score.SetActive(false);
            StartCoroutine("TouchStart");           
        }
        isStart = false;
        alpha = 0f;
        isAlphaIncrease = true;
        fadeAlpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            score.SetActive(true);
            isStart = true;
            StageManager.isOpen = true;
            manager.GetComponent<StageManager>().startGame();
            gameObject.SetActive(false);
        }
        for (int i = 0; i < Input.touchCount; i++)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetTouch(0).phase == TouchPhase.Began) && isStart == false)
            {
                score.SetActive(true);
                isStart = true;
                StageManager.isOpen = true;
                manager.GetComponent<StageManager>().startGame();
                gameObject.SetActive(false);
            }
        }
    }
    IEnumerator TouchStart()
    {
        Color curColor = touchText.GetComponent<Image>().color;
        while (true)
        {
            touchText.GetComponent<Image>().color = new Color(curColor.r, curColor.g, curColor.b, alpha);
            if (isAlphaIncrease)
            {
                alpha += 2f * Time.deltaTime;
                if (alpha >= 1f)
                {
                    isAlphaIncrease = false;
                    yield return new WaitForSeconds(50f * Time.deltaTime);
                }
            }
            else
            {
                alpha -= 2f * Time.deltaTime;
                if (alpha <= 0f)
                {
                    isAlphaIncrease = true;
                }
            }
            yield return new WaitForSeconds(0.005f * Time.deltaTime);
        }
    }
    IEnumerator Fade()
    {
        Color curColor = fadeImg.GetComponent<Image>().color;
        for (; fadeAlpha<1f;)
        {
            fadeImg.GetComponent<Image>().color = new Color(curColor.r, curColor.g, curColor.b, fadeAlpha);
            fadeAlpha += 1f * Time.deltaTime;
            yield return new WaitForSeconds(0.005f * Time.deltaTime);
        }
        SceneManager.LoadScene("InGame");
        yield return null;
    }
}
