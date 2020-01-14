using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
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

    // Use this for initialization
    void Start()
    {
        scale = 100f;
        music = GameObject.Find("SampleMusic").GetComponent<AudioSource>();
        samples = new float[512];
        visualBlockArr = new GameObject[64];

        samples64 = new float[64];
        buffer = new float[64];
        bufferDecrease = new float[64];
        samplesHighest = new float[64];

        audioBand = new float[64];
        audioBuffer = new float[64];

        curve = new float[512];

        presetHighest();

        for (int i = 0; i < 64; i++)
        {
            curve[i] = animationCurve.Evaluate(Mathf.RoundToInt(((float)i) / 64));
        }

        float angle = 0f;
        for (int i = 0; i < 64; i++)
        {
            GameObject tempBlock = Instantiate(visualBlock);

            RectTransform tempRect = tempBlock.GetComponent<RectTransform>();
            tempRect.localPosition = new Vector2(3 * Mathf.Cos(Mathf.Deg2Rad * angle), 3 * Mathf.Sin(Mathf.Deg2Rad * angle));
            tempRect.localRotation = Quaternion.Euler(0, 0, angle + 90);
            tempRect.SetParent(visualParent);
            tempRect.sizeDelta = new Vector2(1, 1);
            angle += 360.0f / (float)64;
            visualBlockArr[i] = tempBlock;
        }
    }

    // Update is called once per frame
    void Update()
    {
        music.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        frequencyBand();
        bandBuffer();
        makeAudioBand();
        getAmp();
        ampLight.intensity = ampBuffer * 0.8f;
        for (int i = 0; i < 64; i++)
        {
            visualBlockArr[i].transform.localScale = new Vector2(10, audioBuffer[i] * scale + 1f);
        }
    }

    void presetHighest()
    {
        ampHighest = 30f;
        for (int i = 0; i < 64; i++)
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
        }
    }

    void bandBuffer()
    {
        for (int i = 0; i < 64; i++)
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

        for (int i = 0; i < 64; i++)
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
        for (int i = 0; i < 64; i++)
        {
            currentAmp += audioBand[i];
            currentAmpBuffer += audioBuffer[i];
        }
        if (currentAmp > ampHighest) {
            ampHighest = currentAmp;
        }
        amp = Mathf.Clamp01(currentAmp / ampHighest);
        ampBuffer = Mathf.Clamp01(currentAmpBuffer / ampHighest);
    }
}
