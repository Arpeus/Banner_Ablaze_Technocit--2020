using System.Collections;
using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(StartDestroy());
    }

    private IEnumerator StartDestroy()
    {
        yield return new WaitForSeconds(GetComponentInChildren<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }

}
