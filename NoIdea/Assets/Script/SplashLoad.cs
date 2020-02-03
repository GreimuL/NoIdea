using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class SplashLoad : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Screen.SetResolution(1280, 720, true);
        while (!SplashScreen.isFinished)
        {
            SplashScreen.Draw();
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
}
