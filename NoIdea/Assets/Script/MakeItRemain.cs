using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeItRemain : MonoBehaviour
{
    public static MakeItRemain instance = null;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
