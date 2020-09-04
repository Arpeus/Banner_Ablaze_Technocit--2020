using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _Instance;

    
   

    public static GameAssets Instance
    {
        get
        {
            if (_Instance == null) _Instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _Instance;
        }
    }

    public SoundAudioClip[] soundAudioClipArray;

    
    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
        [Range(0, 1)]
        public float volume = 1f;
        [Range(0, 1)]
        public float pitch = 1f;
    }
}
