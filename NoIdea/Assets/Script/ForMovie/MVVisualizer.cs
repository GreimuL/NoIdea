using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MVVisualizer : MonoBehaviour
{
    public AudioSource music;
    float[] samples;

    public AnimationCurve animationCurve;

    float[] curve;

    static float[] samples64;
    static float[] buffer;
    static float[] bufferDecrease;
    static float[] samplesHighest;

    static float[] audioBand;
    static float[] audioBuffer;

    float amp;
    public static float ampBuffer;
    static float ampHighest;

    public Transform visualParent;
    public GameObject visualBlock;
    GameObject[] visualBlockArr;
    public float scale;

    public Light ampLight;

    public GameObject player;

    public float blockNum;

    public Camera cam;

    public Slider slider;

    float camWidth;
    float camHeight;

    // Use this for initialization
    void Start()
    {
        slider.value = 0f;
        camHeight = cam.orthographicSize * 2f;
        camWidth = cam.aspect * camHeight;
        scale = 50f;
        music = GameObject.Find("SampleMusic").GetComponent<AudioSource>();
        samples = new float[512];
        visualBlockArr = new GameObject[512];

        samples64 = new float[512];
        buffer = new float[512];
        bufferDecrease = new float[512];
        samplesHighest = new float[512];

        audioBand = new float[512];
        audioBuffer = new float[512];

        curve = new float[512];

        presetHighest();

        for (int i = 0; i < 64; i++)
        {
            curve[i] = animationCurve.Evaluate(Mathf.RoundToInt(((float)i) / 64));
        }

        float angle = 0f;
        /*
        for (int i = 0; i < blockNum; i++)
        {
            GameObject tempBlock = Instantiate(visualBlock);

            RectTransform tempRect = tempBlock.GetComponent<RectTransform>();
            tempRect.localPosition = new Vector2(3 * Mathf.Cos(Mathf.Deg2Rad * angle), 3 * Mathf.Sin(Mathf.Deg2Rad * angle));
            tempRect.localRotation = Quaternion.Euler(0, 0, angle + 90);
            tempRect.SetParent(visualParent);
            tempRect.sizeDelta = new Vector2(1, 1);
            angle += 360.0f / (float)blockNum;
            visualBlockArr[i] = tempBlock;
        }
        */

        for(int i = 0; i < blockNum; i++)
        {
            GameObject tempBlock = Instantiate(visualBlock);

            RectTransform tempRect = tempBlock.GetComponent<RectTransform>();
            tempRect.localPosition = new Vector2(camWidth/blockNum*i-camWidth/2+ camWidth / blockNum/2, -camHeight/8);
            tempRect.SetParent(visualParent);
            tempRect.sizeDelta= new Vector2(1.3f, 3);
            visualBlockArr[i] = tempBlock;
        }
    }

    float musicPosition()
    {
        return Mathf.Clamp(music.time / music.clip.length, 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = musicPosition();
        Band64();
        ampLight.intensity = ampBuffer * 1.2f;
        //player.transform.localScale = new Vector2(ampBuffer + 1f,ampBuffer+1f);
        for (int i = 0; i < blockNum; i++)
        {
            visualBlockArr[i].transform.localScale = new Vector2(3, audioBuffer[i] * scale + 1f);
        }
    }

    void Band512()
    {
        music.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        frequency512();
        bandBuffer();
        makeAudioBand();
        getAmp();
    }

    void Band64()
    {
        music.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        frequencyBand();
        bandBuffer();
        makeAudioBand();
        getAmp();
    }

    void presetHighest()
    {
        ampHighest = 30f;
        for (int i = 0; i < blockNum; i++)
            samplesHighest[i] = 0.1f;
    }

    void frequencyBand()
    {
        int piv = 0;
        for (int i = 0; i < 512; i++)
        {
            float average = 0;
            float cur = animationCurve.Evaluate(Mathf.RoundToInt(((float)i) / 512));

            average += samples[i] * (i + 1);

            if (cur == curve[piv])
            {
                if (i != 0)
                    average /= i;
                samples64[piv] = average * 10;
                piv++;
            }
            if (piv == 64)
            {
                break;
            }
        }
    }

    void frequency512()
    {
        int piv = 0;
        for (int i = 0; i < 512; i++)
        {
            float average = 0;

            average += samples[i] * (i + 1);

            samples64[i] = average * 10;
        }
    }
    void bandBuffer()
    {
        for (int i = 0; i < blockNum; i++)
        {
            if (samples64[i] > buffer[i])
            {
                buffer[i] = samples64[i];
                bufferDecrease[i] = 0.0005f;
            }
            if (samples64[i] < buffer[i])
            {
                buffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }
    

    void makeAudioBand()
    {

        for (int i = 0; i < blockNum; i++)
        {
            if (samples64[i] > samplesHighest[i])
                samplesHighest[i] = samples64[i];
            audioBand[i] = Mathf.Clamp01(samples64[i] / samplesHighest[i]);
            audioBuffer[i] = Mathf.Clamp01(buffer[i] / samplesHighest[i]);
        }
    }

    void getAmp()
    {
        float currentAmp = 0;
        float currentAmpBuffer = 0;
        for (int i = 0; i < blockNum; i++)
        {
            currentAmp += audioBand[i];
            currentAmpBuffer += audioBuffer[i];
        }
        if (currentAmp > ampHighest)
        {
            ampHighest = currentAmp;
        }
        amp = Mathf.Clamp01(currentAmp / ampHighest);
        ampBuffer = Mathf.Clamp01(currentAmpBuffer / ampHighest);
    }
}
