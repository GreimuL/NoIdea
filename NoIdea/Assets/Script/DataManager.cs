using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DataManager : MonoBehaviour
{
    public static int hiscore = 0;
    public GameObject manager;
    public Text hiscoreTxt;
    // Start is called before the first frame update
    void Start()
    {
        hiscore = PlayerPrefs.GetInt("hiscore");
        hiscoreTxt.text = hiscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHiscore()
    {
        int tempscore = manager.GetComponent<StageManager>().getScore();
        hiscore = Math.Max(hiscore, tempscore);
        PlayerPrefs.SetInt("hiscore", hiscore);
        hiscoreTxt.text = hiscore.ToString();
    }
}
