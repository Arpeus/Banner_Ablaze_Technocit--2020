
using UnityEngine;

public class SoundButton : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    public AudioSource OveraudioSource;
    public AudioClip OveraudioClip;

    public void playClip()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void OnMouseOver()
    {
        OveraudioSource.clip = OveraudioClip;
        OveraudioSource.Play();
    }
}
