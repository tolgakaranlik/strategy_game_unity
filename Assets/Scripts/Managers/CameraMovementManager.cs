using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementManager : MonoBehaviour
{
    public GameObject CameraTarget;
    public Vector3 FollowDistance;
    public float MoveSpeed = 0.3f;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            CameraTarget.transform.position -= Vector3.left * MoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            CameraTarget.transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            CameraTarget.transform.position -= Vector3.forward * MoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            CameraTarget.transform.position += Vector3.forward * MoveSpeed * Time.deltaTime;
        }

        transform.position = CameraTarget.transform.position - FollowDistance;
        transform.LookAt(CameraTarget.transform.position);
    }
}
