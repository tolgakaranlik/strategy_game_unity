using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUILandPlay : MonoBehaviour
{
    public GameObject TabWorldMap;
    public GameObject TemporaryTabWorldMap2;

    private void Start()
    {
        HideWorldMap();
        TemporaryHideWorldMap2();
    }

    public void ShowWorldMap()
    {
        TabWorldMap.SetActive(true);
    }

    public void HideWorldMap()
    {
        TabWorldMap.SetActive(false);
        TemporaryTabWorldMap2.SetActive(false);
    }

    public void TemporaryHideWorldMap2()
    {
        TemporaryTabWorldMap2.SetActive(false);
    }

    public void SwitchToTab(int tab)
    {
        HideWorldMap();
        TemporaryHideWorldMap2();

        switch(tab)
        {
            case 0:
                TabWorldMap.SetActive(true);
                break;
            case 1:
                TemporaryTabWorldMap2.SetActive(true);
                break;
        }
    }
}
