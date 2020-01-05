using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    public const int BarCntSize = 64;
    public const int SpectrumSize = 128;
    public AudioSource music;
    public GameObject visual;
    public Transform visP;
    public RectTransform[] visuals = new RectTransform[BarCntSize];
    public Light visualLight;
    public float[] spectrum = new float[SpectrumSize];
    // Use this for initialization
    void Start()
    {
        music = GameObject.Find("SampleMusic").GetComponent<AudioSource>();
        float angle = 0.0f;
        for (int i = 0; i < BarCntSize; i++)
        {
            visuals[i] = Instantiate(visual).GetComponent<RectTransform>();
            visuals[i].localPosition = new Vector2(3 * Mathf.Cos(Mathf.Deg2Rad * angle), 3 * Mathf.Sin(Mathf.Deg2Rad * angle));
            visuals[i].localRotation = Quaternion.Euler(0, 0, angle + 90);
            visuals[i].SetParent(visP);
            visuals[i].sizeDelta = new Vector2(1, 1);
            angle += 360.0f / (float)BarCntSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        music.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
        visualLight.intensity = spectrum[20]*20f;
        for (int i = 0; i < BarCntSize; i++)
        {            
            if (spectrum[i] * 5 > 0.5f)
            {
                visuals[i].sizeDelta = new Vector2(0.2f, 0.5f);
            }
            else
            {
                visuals[i].sizeDelta = new Vector2(0.2f, spectrum[i] * 20);
            }
        }
    }
}
