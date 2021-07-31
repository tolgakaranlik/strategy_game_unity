using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovementManager : MonoBehaviour
{
    public GameObject MainCam;
    public Vector3 FollowDistance;
    public float MoveSpeed = 0.3f;

    public float MaxZ = 562;
    public float MinZ = 0;
    public float MaxX = 150;
    public float MinX = 0;

    public float MouseZoomSpeed = 15.0f;
    public float TouchZoomSpeed = 0.1f;
    public float ZoomMinBound = 0.1f;
    public float ZoomMaxBound = 179.9f;

    Vector3 lastMousePosition = Vector3.zero;
    bool shouldRecordTouchPlace = true;
    int oldTouchCount = 0;

    Vector3 forward;
    Vector3 left;
    Camera cam;
    
    void Start()
    {
        forward = transform.position - FollowDistance;
        forward = new Vector3(forward.x, 0, forward.z).normalized;

        left = Quaternion.Euler(0, 90, 0) * forward;
        left = new Vector3(left.x, 0, left.z).normalized;

        cam = MainCam.GetComponent<Camera>();
    }

    void Update()
    {
        // Handle movement - Keyboard
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= Vector3.left * MoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position -= Vector3.forward * MoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.forward * MoveSpeed * Time.deltaTime;
        }

        // Handle zoom
        if (Input.touchSupported)
        {
            if (Input.touchCount == 2)
            {
                // get current touch positions
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);

                // get touch position from the previous frame
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                // get offset value
                float deltaDistance = oldTouchDistance - currentTouchDistance;
                Zoom(deltaDistance, TouchZoomSpeed);
            }
        } else
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                Zoom(scroll, MouseZoomSpeed);
            }
        }

        // Handle movement - mouse/touch
        if (Input.GetMouseButtonUp(0) || (Input.touchCount == 0 && oldTouchCount != 0))
        {
            shouldRecordTouchPlace = true;
        }

        if (Input.GetMouseButton(0) || Input.touchCount == 1)
        {
            if(shouldRecordTouchPlace)
            {
                lastMousePosition = Input.mousePosition;
                shouldRecordTouchPlace = false;
            }

            transform.position -= (lastMousePosition.x - Input.mousePosition.x) * left * MoveSpeed * Time.deltaTime;
            transform.position -= (lastMousePosition.y - Input.mousePosition.y) * forward * MoveSpeed * Time.deltaTime;

            lastMousePosition = Input.mousePosition;
        }

        // Apply constraints
        Vector3 p = transform.position;
        if (p.z > MaxZ)
        {
            p.z = MaxZ;
        }

        if (p.z < MinZ)
        {
            p.z = MinZ;
        }

        if (p.x > MaxX)
        {
            p.x = MaxX;
        }

        if (p.x < MinX)
        {
            p.x = MinX;
        }

        transform.position = p;
        oldTouchCount = Input.touchCount;
    }

    private void LateUpdate()
    {
        //MainCam.transform.position = transform.position - FollowDistance;
        if (!Input.GetMouseButton(0))
        {
            MainCam.transform.DOMove(transform.position - FollowDistance, 0.3f).SetEase(Ease.Linear);
        } else
        {
            MainCam.transform.position = transform.position - FollowDistance;
        }

        MainCam.transform.LookAt(transform.position);
    }

    void Zoom(float deltaMagnitudeDiff, float speed)
    {
        float old = FollowDistance.y;
        FollowDistance.y -= deltaMagnitudeDiff * speed;
        if(FollowDistance.y < ZoomMinBound)
        {
            FollowDistance.y = ZoomMinBound;
        } else if (FollowDistance.y > ZoomMaxBound)
        {
            FollowDistance.y = ZoomMaxBound;
        }
    }
}
