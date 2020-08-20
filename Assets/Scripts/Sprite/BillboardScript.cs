using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    public Camera _myCamera;

    void Update()
    {
        transform.LookAt(transform.position + _myCamera.transform.rotation * Vector3.forward,
            _myCamera.transform.rotation * Vector3.up);
    }
}
