using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    private Camera m_myCamera;

    void Awake()
    {
        m_myCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        if(GameManager.Instance.EType_Phase != PhaseType.EType_EndGamePhase)
            transform.LookAt(transform.position + m_myCamera.transform.rotation * Vector3.forward,
            m_myCamera.transform.rotation * Vector3.up);
    }


}
