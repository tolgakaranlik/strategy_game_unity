using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporaryDisplayFPS : MonoBehaviour
{
    public Text Display;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DisplayFPS", 1, 1);
    }

    void DisplayFPS()
    {
        Display.text = "FPS: " + (1.0f / Time.deltaTime).ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
