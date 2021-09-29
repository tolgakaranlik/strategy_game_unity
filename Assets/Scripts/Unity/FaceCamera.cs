using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public GameObject Cam = null;

    void Start()
    {
        if(Cam == null)
        {
            Cam = Camera.main.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Cam.transform.rotation;
        transform.Rotate(Vector3.up * 180);
    }
}
