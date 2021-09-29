using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIRTS : MonoBehaviour
{
    public GameObject SelectionBox;
    public string[] SelectableTags;
    public Camera Cam;

    RectTransform selectionRT;
    Vector2 initialClickPosition = Vector2.zero;

    private void Start()
    {
        selectionRT = SelectionBox.GetComponent<RectTransform>();
        SelectionBox.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SelectionBox.SetActive(true);

            initialClickPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            selectionRT.anchoredPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 difference = currentMousePosition - initialClickPosition;
            Vector2 startPoint = initialClickPosition;

            Vector2 p1, p2, p3, p4;

            // The following code accounts for dragging in various directions.
            if (difference.x < 0)
            {
                startPoint.x = currentMousePosition.x;
                difference.x = -difference.x;
            }
            if (difference.y < 0)
            {
                startPoint.y = currentMousePosition.y;
                difference.y = -difference.y;
            }

            // Set the anchor, width and height every frame.
            selectionRT.anchoredPosition = startPoint;
            selectionRT.sizeDelta = difference;

            // Test if any selected objects are present
            GameObject[] objects;
            Vector3 objectPoint;
            var rect = SelectionBox.GetComponent<RectTransform>();
            for (int i = 0; i < SelectableTags.Length; i++)
            {
                objects = GameObject.FindGameObjectsWithTag(SelectableTags[i]);
                UnselectObjects(objects);

                p1 = SelectionBox.GetComponent<RectTransform>().anchoredPosition;
                p2 = SelectionBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(rect.sizeDelta.x, 0);
                p3 = SelectionBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, rect.sizeDelta.y);
                p4 = SelectionBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(rect.sizeDelta.x, rect.sizeDelta.y);

                for (int u = 0; u < objects.Length; u++)
                {
                    objectPoint = Cam.WorldToScreenPoint(objects[u].transform.position);

                    if(objectPoint.x >= p1.x && objectPoint.y >= p1.y && objectPoint.x <= p2.x && objectPoint.y <= p3.y)
                    {
                        for (int j = 0; j < objects[u].transform.childCount; j++)
                        {
                            if (objects[u].transform.GetChild(j).name == "CanvasSelection")
                            {
                                objects[u].transform.GetChild(j).gameObject.SetActive(true);
                                break;
                            }
                        }
                    }
                }

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Disable the selection box
            SelectionBox.SetActive(false);

            initialClickPosition = Vector2.zero;
            selectionRT.anchoredPosition = Vector2.zero;
            selectionRT.sizeDelta = Vector2.zero;
        }
    }

    private void UnselectObjects(GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            for (int j = 0; j < objects[i].transform.childCount; j++)
            {
                if(objects[i].transform.GetChild(j).name == "CanvasSelection")
                {
                    objects[i].transform.GetChild(j).gameObject.SetActive(false);
                    break;
                }
            }            
        }
    }
}
