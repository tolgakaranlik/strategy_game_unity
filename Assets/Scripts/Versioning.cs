using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Versioning : MonoBehaviour
{
    [HideInInspector]
    public string CurrentVersion = "0.006";
    public Text Label;

    string versionText = "v%1 - Work in progress...";

    // Start is called before the first frame update
    void Start()
    {
        if(Label != null)
        {
            Label.text = versionText.Replace("%1", CurrentVersion);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
