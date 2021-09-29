using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCamera : MonoBehaviour
{
    public float BoundLeft = -100;
    public float BoundRight = 100;
    public float BoundTop = -100;
    public float BoundBottom = 100;
    public GameObject Camera;
    public Vector3 Distance;

    public bool CanMoveWithKeys = true;
    public float MoveSpeed = 0.5f;
    public KeyCode KeyLeft = KeyCode.LeftArrow;
    public KeyCode KeyRight = KeyCode.RightArrow;
    public KeyCode KeyUp = KeyCode.UpArrow;
    public KeyCode KeyDown = KeyCode.DownArrow;

    void LateUpdate()
    {
        if(CanMoveWithKeys)
        {
            if(Input.GetKey(KeyLeft))
            {
                // move left
                transform.position += Vector3.left * MoveSpeed;
            } 
            
            if (Input.GetKey(KeyRight))
            {
                // move right
                transform.position -= Vector3.left * MoveSpeed;
            }
            
            if (Input.GetKey(KeyDown))
            {
                // move up
                transform.position -= Vector3.forward * MoveSpeed;
            }
            
            if (Input.GetKey(KeyUp))
            {
                // move down
                transform.position += Vector3.forward * MoveSpeed;
            }

            // Apply constraints
            if (transform.position.x < BoundLeft)
            {
                transform.position = new Vector3(BoundLeft, transform.position.y, transform.position.z);
            }

            if (transform.position.x > BoundRight)
            {
                transform.position = new Vector3(BoundRight, transform.position.y, transform.position.z);
            }

            if (transform.position.z < BoundTop)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, BoundTop);
            }

            if (transform.position.z > BoundBottom)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, BoundBottom);
            }
        }

        Camera.transform.position = transform.position - Distance;
        Camera.transform.LookAt(transform.position);
    }
}
