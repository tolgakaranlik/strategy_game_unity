using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
///  Author: Tolga K, 07/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
///  This class is intended to handle loading and caching
///  game scenes. It is also intended to monitoring system
///  resources and release cached items when device is running
///  low on resources
///
/// </summary>

public class SceneLoadingManager : MonoBehaviour
{
    public delegate void OnProgressChanged(float percent);

    OnProgressChanged progressChanger = null;

    /// <summary>
    /// Loads the scene (scene name should be given from levelName argument). If an
    /// optional event handler is supplied, it will be called automatically on change
    /// of loading progress
    /// </summary>
    /// <param name="levelName"></param>
    /// <param name="progress"></param>
    public void LoadScene(string levelName, OnProgressChanged progress)
    {
        // TODO: caching on scene load

        progressChanger = progress;
        StartCoroutine(BeginLoadScene(levelName));
    }

    IEnumerator BeginLoadScene(string levelName)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelName, UnityEngine.SceneManagement.LoadSceneMode.Single);

        float oldProgress = 0;
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            if(oldProgress != asyncLoad.progress)
            {
                progressChanger?.Invoke(asyncLoad.progress);
                oldProgress = asyncLoad.progress;
            }

            progressChanger?.Invoke(1);
            yield return null;
        }
    }
}
