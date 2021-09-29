using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporaryDisplayFPS : MonoBehaviour
{
    public Text Display;
    public bool Detailed = false;
    public float Frequency = 0.25f;

    // Start is called before the first frame update
    float totalFps = 0;
    int totalRead = 0;
    void Start()
    {
        StartCoroutine(DisplayFPS());
    }

    IEnumerator DisplayFPS()
    {
        while (true)
        {
            yield return new WaitForSeconds(Frequency);

            float fps = (1.0f / Time.deltaTime);
            totalFps += fps;
            totalRead += 1;

            if (Detailed)
            {
                Display.text = "FPS: " + fps.ToString("N0") + " (Avg: " + (totalFps / totalRead).ToString("N0") + ")";
            }
            else
            {
                Display.text = "FPS: " + (totalFps / totalRead).ToString("N0");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
