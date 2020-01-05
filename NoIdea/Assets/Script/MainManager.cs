using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public GameObject touchText;
    public GameObject fadeImg;
    private float alpha = 0f;
    private bool isAlphaIncrease = true;
    private bool isStart = false;
    private float fadeAlpha = 0f;
    // Start is called before the first frame update
    void Start()
    {
        isStart = false;
        alpha = 0f;
        isAlphaIncrease = true;
        fadeAlpha = 0f;
        StartCoroutine("TouchStart");
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0)||Input.GetTouch(0).phase==TouchPhase.Began)&&isStart==false)
        {
            isStart = true;
            StartCoroutine("Fade");
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
                alpha += 1f * Time.deltaTime;
                if (alpha >= 1f)
                {
                    isAlphaIncrease = false;
                }
            }
            else
            {
                alpha -= 1f * Time.deltaTime;
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
