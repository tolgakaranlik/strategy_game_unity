using System;
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
/// This class responsible for displaying standard dialogs on the screen. From
/// commands here, the GUIDialog prefab is being arranged to give the desired
/// output
/// 
/// </summary>
public class GUIDialog : MonoBehaviour
{
    public Image ImgError;
    public Image ImgInfo;
    public Image ImgWarning;
    public Image ImgProgress;
    public Text Message;
    public Button BtnOK;

    public delegate void OnButton();

    void Start()
    {
    }

    void TurnOffIcons()
    {
        ImgError.gameObject.SetActive(false);
        ImgInfo.gameObject.SetActive(false);
        ImgWarning.gameObject.SetActive(false);
        ImgProgress.gameObject.SetActive(false);

        BtnOK.gameObject.SetActive(true);
    }

    public void Error(string message)
    {
        Error(message, "Close", null);
    }

    public void Info(string message)
    {
        Info(message, "Close", null);
    }

    public void Warning(string message)
    {
        Warning(message, "Close", null);
    }

    public void ProgressOn(string message)
    {
        TurnOffIcons();
        ImgProgress.gameObject.SetActive(true);

        Dialog(message, "", null);
        BtnOK.gameObject.SetActive(false);
    }

    public void ProgressOnWithCancel(string message, OnButton cancelHandler)
    {
        ProgressOnWithCancel(message, "Cancel", cancelHandler);
    }

    public void ProgressOnWithCancel(string message, string buttonCaption, OnButton cancelHandler)
    {
        TurnOffIcons();
        ImgProgress.gameObject.SetActive(true);

        Dialog(message, "Cancel", cancelHandler);
    }

    public void ProgressOff()
    {
        gameObject.SetActive(false);
    }

    public void Error(string message, string buttonCaption, OnButton callback)
    {
        TurnOffIcons();
        ImgError.gameObject.SetActive(true);

        Dialog(message, buttonCaption, callback);
    }

    public void Info(string message, string buttonCaption, OnButton callback)
    {
        TurnOffIcons();
        ImgInfo.gameObject.SetActive(true);

        Dialog(message, buttonCaption, callback);
    }

    public void Warning(string message, string buttonCaption, OnButton callback)
    {
        TurnOffIcons();
        ImgWarning.gameObject.SetActive(true);

        Dialog(message, buttonCaption, callback);
    }

    private void Dialog(string message, string buttonCaption, OnButton callback)
    {
        gameObject.SetActive(true);
        Message.text = message;
        BtnOK.transform.GetChild(0).GetComponent<Text>().text = buttonCaption;

        BtnOK.onClick.RemoveAllListeners();
        BtnOK.onClick.AddListener(() =>
        {
            callback?.Invoke();
            gameObject.SetActive(false);
        });
    }
}
