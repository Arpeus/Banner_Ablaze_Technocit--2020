using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryOnLoad : MonoBehaviour
{
    private void Awake()
    {

        Destroy(GameObject.Find("GameManager"));        
        Destroy(GameObject.Find("AudioSource"));
        Destroy(GameObject.Find("PlayerOne"));
        Destroy(GameObject.Find("PlayerTwo"));
    }
}
