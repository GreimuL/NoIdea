using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    public AudioSource music;
    float[] samples;

    public Transform visualParent;
    public GameObject visualBlock;
    GameObject[] visualBlockArr;
    public float scale;

    // Use this for initialization
    void Start()
    {
        scale = 1000f;
        music = GameObject.Find("SampleMusic").GetComponent<AudioSource>();
        samples = new float[512];
        visualBlockArr = new GameObject[512];
        float angle = 0f;
        for(int i = 0; i < 512; i++)
        {
            GameObject tempBlock = Instantiate(visualBlock);
            RectTransform tempRect = tempBlock.GetComponent<RectTransform>();
            tempRect.localPosition = new Vector2(3 * Mathf.Cos(Mathf.Deg2Rad * angle), 3 * Mathf.Sin(Mathf.Deg2Rad * angle));
            tempRect.localRotation = Quaternion.Euler(0, 0, angle + 90);
            tempRect.SetParent(visualParent);
            tempRect.sizeDelta = new Vector2(1, 1);
            angle += 360.0f / (float)512;
            visualBlockArr[i] = tempBlock;
        }
    }

    // Update is called once per frame
    void Update()
    {
        music.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        for(int i = 0; i < 512; i++)
        {
            visualBlockArr[i].transform.localScale = new Vector3(5, samples[i]*scale, 5);
        }
    }

    void GetAmp()
    {
        
    }
}
