using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    public AudioSource music;
    float[] samples;

    float[] samples64;
    float[] buffer;
    float[] bufferDecrease;
    float[] samplesHighest;

    float[] audioBand;
    float[] audioBuffer;

    float amp;
    public float ampBuffer;
    float ampHighest;

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
        ampLight.intensity = ampBuffer*5f;
        for (int i = 0; i < 64; i++)
        {
            visualBlockArr[i].transform.localScale = new Vector3(10, audioBuffer[i] * scale + 1f, 10);
        }
    }

    void frequencyBand()
    {
        int piv = 0;
        int samplePiv = 1;
        int power = 0;
        for (int i = 0; i < 64; i++)
        {
            float average = 0;
            if (i == 16 || i == 32 || i == 40 || i == 48 || i == 56)
            {
                samplePiv = 2 << power;
                power++;
                if (power == 3)
                {
                    samplePiv -= 2;
                }
            }
            for (int j = 0; j < samplePiv; j++)
            {
                average += samples[piv] * (piv + 1);
                piv++;
            }
            average /= piv;
            samples64[i] = average * 80;
        }
    }

    void bandBuffer()
    {
        for (int i = 0; i < 64; i++)
        {
            if (samples64[i] > buffer[i])
            {
                buffer[i] = samples64[i];
                bufferDecrease[i] = 0.005f;
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
            samplesHighest[i] = Mathf.Max(samplesHighest[i], samples64[i]);
            if (samplesHighest[i] != 0)
            {
                audioBand[i] = samples64[i] / samplesHighest[i];
                audioBuffer[i] = buffer[i] / samplesHighest[i];
            }
        }
    }

    void getAmp()
    {
        float currentAmp = 0;
        float currentAmpBuffer = 0;
        for(int i = 0; i < 64; i++)
        {
            currentAmp += audioBand[i];
            currentAmpBuffer += audioBuffer[i];
        }
        ampHighest = Mathf.Max(ampHighest, currentAmp);
        if (ampHighest != 0)
        {
            amp = currentAmp / ampHighest;
            ampBuffer = currentAmpBuffer / ampHighest;
        }
    }
}
