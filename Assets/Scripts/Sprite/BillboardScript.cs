using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    [SerializeField] private Camera m_myCamera;

    void Awake()
    {
        m_myCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        transform.LookAt(transform.position + m_myCamera.transform.rotation * Vector3.forward,
            m_myCamera.transform.rotation * Vector3.up);
    }
}
