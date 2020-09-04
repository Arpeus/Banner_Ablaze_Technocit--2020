using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryOnLoad : MonoBehaviour
{
    private void Awake()
    {
        Destroy(GameObject.Find("DontDestroyOnLoad"));
    }
}
