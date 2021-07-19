using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
///
///  Author: Tolga K, 07/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
/// Class to handle events and basic logic on welcome screen
/// 
/// </summary>
public class GUIWelcome : MonoBehaviour
{
    public Text TxtVersion;
    public Image WindowRegister;
    public Image WindowRegister2;
    public Image WindowLogin;
    public Image PlayButton;
    public GUIDialog Dialog;

    // Register
    public Button BtnSexM;
    public Button BtnSexF;
    public Text TxtRegisterMsg;

    // Start is called before the first frame update
    void Start()
    {
        TxtVersion.text = "v" + VersionManager.GetVersion();

        WindowRegister.GetComponent<CanvasGroup>().alpha = 0;
        WindowLogin.GetComponent<CanvasGroup>().alpha = 0;
        WindowRegister2.GetComponent<CanvasGroup>().alpha = 0;

        WindowRegister.gameObject.SetActive(false);
        WindowLogin.gameObject.SetActive(false);
        WindowRegister2.gameObject.SetActive(false);
    }

    public void DisplayRegisterWindow()
    {
        WindowRegister.gameObject.SetActive(true);

        WindowLogin.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);
        WindowRegister.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetEase(Ease.Linear);
        PlayButton.GetComponent<Image>().DOFade(0, 0.2f);

        StartCoroutine(DisableLoginWindow());
    }

    public void DisplayLoginWindow()
    {
        WindowLogin.gameObject.SetActive(true);

        WindowRegister.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);
        WindowLogin.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetEase(Ease.Linear);

        StartCoroutine(DisableRegisterWindow());
    }

    IEnumerator DisableRegisterWindow()
    {
        yield return new WaitForSeconds(0.2f);

        WindowRegister.gameObject.SetActive(false);
        WindowRegister2.gameObject.SetActive(false);
    }

    IEnumerator DisableLoginWindow()
    {
        yield return new WaitForSeconds(0.2f);

        WindowLogin.gameObject.SetActive(false);
    }

    public void LoginWithOAuth(string method)
    {
        // temporarily set as giving an error mesage
        WindowRegister.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);
        WindowRegister2.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);
        WindowLogin.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);

        Dialog.ProgressOn("Please wait...");

        StartCoroutine(DisplayOAuthError());
    }

    public void SexMale()
    {
        TxtRegisterMsg.text = "With what name you want to be called my king?";
        BtnSexM.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 1f);
        BtnSexF.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

    public void SexFemale()
    {
        TxtRegisterMsg.text = "With what name you want to be called my queen?";
        BtnSexM.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        BtnSexF.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 1f);
    }

    public void SetDisplayName()
    {
        string fullName = WindowRegister.transform.Find("Content/InpFullName/Text").GetComponent<Text>().text.Trim();

        WindowRegister.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);
        WindowLogin.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);

        if (fullName == "")
        {
            Dialog.Error("Please enter the name that you want to be called my majesty", "Close", () => {
                DisplayRegisterWindow();
            });
        } else
        {
            Dialog.ProgressOn("Please wait...");

            StartCoroutine(DisplayRegisterStep2());
        }
    }

    public void BackFromRegister2()
    {
        StartCoroutine(CloseRegister2());
    }

    public void LoginWithUserName()
    {
        string enteredUserName = WindowLogin.transform.Find("Content/InpUserName/Text").GetComponent<Text>().text.Trim();

        WindowRegister.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);
        WindowLogin.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);

        if(enteredUserName == "")
        {
            Dialog.Error("Please enter your user name my Lord", "Close", () => {
                DisplayLoginWindow();
            });
        }
        else
        {
            Dialog.ProgressOn("Please wait...");

            StartCoroutine(DisplayLoginStep2());
        }
    }

    IEnumerator DisplayOAuthError()
    {
        yield return new WaitForSeconds(2f);

        Dialog.ProgressOff();
        Dialog.Error("An error occured", "Close", () =>
        {
            DisplayLoginWindow();
        });
    }

    IEnumerator DisplayLoginStep2()
    {
        yield return new WaitForSeconds(2f);

        Dialog.ProgressOff();
    }

    IEnumerator DisplayRegisterStep2()
    {
        yield return new WaitForSeconds(2f);

        Dialog.ProgressOff();

        WindowRegister2.gameObject.SetActive(true);
        WindowRegister2.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetEase(Ease.Linear);
    }

    IEnumerator CloseRegister2()
    {
        WindowRegister2.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.5f);

        WindowRegister2.gameObject.SetActive(false);
        WindowRegister.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetEase(Ease.Linear);
    }
}
