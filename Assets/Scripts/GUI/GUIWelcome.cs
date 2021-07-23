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
    public Image WindowRegisterBuildHero;
    public Image WindowLogin;
    public Image WindowSpellDetail;
    public Image PlayButton;
    public GUIDialog Dialog;

    public GameObject[] ClassIcons;
    public GameObject[] ClassSpellImages;
    public Text ClassDescription;
    public Text ClassSpells;

    // Register
    public Button BtnSexM;
    public Button BtnSexF;
    public Text TxtRegisterMsg;

    public const int SEX_MALE = 0;
    public const int SEX_FEMALE = 1;

    int selectedClass = 0;
    int sex = SEX_MALE;
    string governorName;

    // Start is called before the first frame update
    void Start()
    {
        TxtVersion.text = "v" + VersionManager.GetVersion();

        WindowRegister.GetComponent<CanvasGroup>().alpha = 0;
        WindowLogin.GetComponent<CanvasGroup>().alpha = 0;
        WindowRegister2.GetComponent<CanvasGroup>().alpha = 0;
        WindowRegisterBuildHero.GetComponent<CanvasGroup>().alpha = 0;
        WindowSpellDetail.GetComponent<CanvasGroup>().alpha = 0;

        WindowRegister.gameObject.SetActive(false);
        WindowLogin.gameObject.SetActive(false);
        WindowRegister2.gameObject.SetActive(false);
        WindowRegisterBuildHero.gameObject.SetActive(false);
        WindowSpellDetail.gameObject.SetActive(false);
    }

    public void DisplayRegisterWindow()
    {
        WindowRegister.gameObject.SetActive(true);

        WindowLogin.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);
        WindowRegister.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetEase(Ease.Linear);
        PlayButton.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);

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
        sex = SEX_MALE;

        TxtRegisterMsg.text = "With what name you want to be called my king?";
        BtnSexM.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 1f);
        BtnSexF.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

    public void SexFemale()
    {
        sex = SEX_FEMALE;

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
            governorName = fullName;
            Dialog.ProgressOn("Please wait...");

            StartCoroutine(DisplayRegisterStep2(1.5f));
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

    public void RegisterAccount()
    {
        string emailAddress = WindowRegister2.transform.Find("InpMailAddress").GetComponent<InputField>().text.Trim();
        string password = WindowRegister2.transform.Find("InpPassword").GetComponent<InputField>().text.Trim();
        string passwordRetype = WindowRegister2.transform.Find("InpPasswordRetype").GetComponent<InputField>().text.Trim();

        if(emailAddress == "")
        {
            StartCoroutine(DisplayRegister2Error("Please enter your mail address my " + (sex == SEX_FEMALE ? "queen" : "king")));
        } else
        {
            if(password == "")
            {
                StartCoroutine(DisplayRegister2Error("Please enter your password my " + (sex == SEX_FEMALE ? "queen" : "king")));
            }
            else
            {
                if (password.Length < 8)
                {
                    StartCoroutine(DisplayRegister2Error("Your password must contain at least 8 characters my " + (sex == SEX_FEMALE ? "queen" : "king")));
                }
                else
                {
                    if (password != passwordRetype)
                    {
                        StartCoroutine(DisplayRegister2Error("Your password and its retype must match my " + (sex == SEX_FEMALE ? "queen" : "king")));
                    }
                    else
                    {
                        StartCoroutine(DisplayRegisterStep3());
                    }
                }
            }
        }
    }

    public void BackFromBuildHero()
    {
        StartCoroutine(GoBackFromBuildHero());
    }

    public void SwitchToClass(int classIndex)
    {
        if(selectedClass != classIndex)
        {
            StartCoroutine(ChangeClassSpells(classIndex));

            switch (classIndex)
            {
                // Warrior
                case 0:
                    StartCoroutine(ChangeClassDescription("Warriors are fast, strong and agile. You don't want to make them angry! It is easy to proceed when you are in lower levels with them.", "Warrior"));
                    break;
                // Mage
                case 1:
                    StartCoroutine(ChangeClassDescription("Mages are tough enemies with strong capabilities casting various spells. A mage from high levels is always hard to fight against.", "Mage"));
                    break;
                // Hunter
                case 2:
                    StartCoroutine(ChangeClassDescription("Hunters are equiped with weapons and various equipment in order to gain advantage afainst more powerful enemies.", "Hunter"));
                    break;
                // Thief
                case 3:
                    StartCoroutine(ChangeClassDescription("Thieves are good at resource collecting so they can improve fast when compared with other classes.", "Thief"));
                    break;
                // Paladin
                case 4:
                    StartCoroutine(ChangeClassDescription("Paladins are strong and durable fighters. They are excellent in melee and close combat. Can walk long distances and can carry heavy weight.", "Paladin"));
                    break;
            }
        }

        for (int i = 0; i < ClassIcons.Length; i++)
        {
            if (i == classIndex)
            {
                ClassIcons[i].GetComponent<RectTransform>().DOScale(1f, 0.3f).SetEase(Ease.Linear);
                ClassIcons[i].GetComponent<CanvasGroup>().DOFade(1f, 0.3f).SetEase(Ease.Linear);
                ClassIcons[i].GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1), 0.3f);
            }
            else
            {
                ClassIcons[i].GetComponent<RectTransform>().DOScale(0.85f, 0.3f).SetEase(Ease.Linear);
                ClassIcons[i].GetComponent<CanvasGroup>().DOFade(0.65f, 0.3f).SetEase(Ease.Linear);
                ClassIcons[i].GetComponent<Image>().DOColor(new Color(0.5f, 0.5f, 0.5f, 1), 0.3f);
            }
        }

        selectedClass = classIndex;
    }

    public void DisplaySpellDetails(int spellIndex)
    {
        StartCoroutine(DisplaySpellDetailsNow(spellIndex));
    }

    public void HideSpellDetails()
    {
        StartCoroutine(HideSpellDetailsNow());
    }

    IEnumerator HideSpellDetailsNow()
    {
        WindowSpellDetail.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.35f);

        WindowSpellDetail.gameObject.SetActive(false);
        WindowRegisterBuildHero.gameObject.SetActive(true);
        WindowRegisterBuildHero.GetComponent<CanvasGroup>().DOFade(1f, 0.3f).SetEase(Ease.Linear);
    }

    IEnumerator DisplaySpellDetailsNow(int spellIndex)
    {
        WindowRegisterBuildHero.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).SetEase(Ease.Linear);
        WindowSpellDetail.GetComponent<CanvasGroup>().alpha = 0;
        WindowSpellDetail.gameObject.SetActive(true);

        Spell spell = SpellManager.GetInstance().Find(spellIndex);
        if (spell != null)
        {
            WindowSpellDetail.transform.Find("ImgSpell").GetComponent<Image>().sprite = Resources.Load<Sprite>("SpellIcons/" + spell.AvatarFile);
            WindowSpellDetail.transform.Find("TxtSpellName").GetComponent<Text>().text = spell.Name;
            WindowSpellDetail.transform.Find("TxtSpellDescription").GetComponent<Text>().text = spell.Description;
            WindowSpellDetail.transform.Find("TxtLevel1/TxtLevelDesc").GetComponent<Text>().text = spell.LevelDescriptions[0];
            WindowSpellDetail.transform.Find("TxtLevel2/TxtLevelDesc").GetComponent<Text>().text = spell.LevelDescriptions[1];
            WindowSpellDetail.transform.Find("TxtLevel3/TxtLevelDesc").GetComponent<Text>().text = spell.LevelDescriptions[2];
            WindowSpellDetail.transform.Find("TxtLevel4/TxtLevelDesc").GetComponent<Text>().text = spell.LevelDescriptions[3];
            WindowSpellDetail.transform.Find("TxtLevel5/TxtLevelDesc").GetComponent<Text>().text = spell.LevelDescriptions[4];
        }

        yield return new WaitForSeconds(0.35f);

        WindowRegisterBuildHero.gameObject.SetActive(false);
        WindowSpellDetail.GetComponent<CanvasGroup>().DOFade(1f, 0.3f).SetEase(Ease.Linear);
    }

    IEnumerator ChangeClassSpells(int classIndex)
    {
        for (int i = 0; i < ClassSpellImages.Length; i++)
        {
            ClassSpellImages[i].GetComponent<CanvasGroup>().DOFade(0, 0.3f);
        }

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < ClassSpellImages.Length; i++)
        {
            ClassSpellImages[i].SetActive(false);
        }

        ClassSpellImages[classIndex].SetActive(true);
        ClassSpellImages[classIndex].GetComponent<CanvasGroup>().DOFade(1, 0.3f);
    }

    IEnumerator ChangeClassDescription(string message, string className)
    {
        ClassDescription.DOFade(0, 0.3f).SetEase(Ease.Linear);
        ClassSpells.DOFade(0, 0.3f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.3f);
        ClassDescription.text = message;
        ClassSpells.text = "Level 1 spells of " + className + " class:";

        ClassDescription.DOFade(1, 0.3f).SetEase(Ease.Linear);
        ClassSpells.DOFade(1, 0.3f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator GoBackFromBuildHero()
    {
        WindowRegisterBuildHero.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.3f);

        WindowRegisterBuildHero.gameObject.SetActive(false);

        WindowRegister2.gameObject.SetActive(true);
        WindowRegister2.GetComponent<CanvasGroup>().DOFade(1, 0.2f).SetEase(Ease.Linear);
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

    IEnumerator DisplayRegisterStep3()
    {
        WindowRegister2.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);
        Dialog.ProgressOn("Please wait...");

        yield return new WaitForSeconds(1.1f);

        Dialog.ProgressOff();

        WindowRegister2.GetComponent<CanvasGroup>().alpha = 0;
        WindowRegisterBuildHero.gameObject.SetActive(true);

        WindowRegisterBuildHero.GetComponent<CanvasGroup>().DOFade(1, 0.2f).SetEase(Ease.Linear);
    }

    IEnumerator DisplayRegisterStep2(float delay)
    {
        yield return new WaitForSeconds(delay);

        Dialog.ProgressOff();

        WindowRegister2.transform.Find("TxtLongLiveMsg").GetComponent<Text>().text = "Long live " + (sex == SEX_MALE ? "king" : "queen") + " " + governorName + "! Welcome to your throne";

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

    IEnumerator DisplayRegister2Error(string message)
    {
        WindowRegister2.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.2f);

        WindowRegister2.gameObject.SetActive(false);
        Dialog.Error(message, "Close", () => {
            StartCoroutine(DisplayRegisterStep2(0));
        });
    }
}
