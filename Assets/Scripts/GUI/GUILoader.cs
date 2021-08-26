using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
///  Author: Tolga K, 07/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
///  This is the main entry point of whole game. Loader GUI
///  loads the welcome screen and everything starts.
///
/// </summary>

public class GUILoader : MonoBehaviour
{
    public Slider PrgLoading;
    public string TargetScene = "WelcomeScreen";

    void Start()
    {
        PlayerPrefs.SetFloat("LastPlaceOnLandX", 412);
        PlayerPrefs.SetFloat("LastPlaceOnLandY", 10);
        PlayerPrefs.SetFloat("LastPlaceOnLandZ", 412);

        PrgLoading.value = 0;
        StartCoroutine(LoadWorldMap());
    }

    IEnumerator LoadWorldMap()
    {
        yield return new WaitForSeconds(1);

        KNHSceneManager.LoadScene(TargetScene, OnLevelLoadProgressChanged);
    }

    void OnLevelLoadProgressChanged(float percent)
    {
        PrgLoading.value = percent;

        if(PrgLoading.value >= 1.0f)
        {
            // Done loading
            Log.Info("Done loading " + TargetScene);
        }
    }
}
