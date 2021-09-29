using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public GameObject Camera;
    public Vector3 Distance;
    
    void LateUpdate()
    {
        Camera.transform.position = transform.position - Distance;
        Camera.transform.LookAt(transform.position);
    }
}
