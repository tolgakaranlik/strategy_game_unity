using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

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
    // TODO: Replace window switching commands with a unified Window Framework
    public Text TxtVersion;
    public Image WindowRegister;
    public Image WindowRegister2;
    public Image WindowRegisterBuildHero;
    public Image WindowCustomizeHero;
    public Image WindowLogin;
    public Image WindowSpellDetail;
    public Image WindowBuyHero;
    public Image WindowLandDetail;
    public Image WindowBuyLand;
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

    // account type (0:mail, 101-199: one of oauth types)
    int accountType = 0;
    int selectedClass = 0;
    int sex = SEX_MALE;
    string governorName;
    string emailAddress = "";
    string townName = "";

    // Start is called before the first frame update
    void Start()
    {
        TxtVersion.text = "v" + VersionManager.GetVersion();

        WindowRegister.GetComponent<CanvasGroup>().alpha = 0;
        WindowLogin.GetComponent<CanvasGroup>().alpha = 0;
        WindowRegister2.GetComponent<CanvasGroup>().alpha = 0;
        WindowRegisterBuildHero.GetComponent<CanvasGroup>().alpha = 0;
        WindowSpellDetail.GetComponent<CanvasGroup>().alpha = 0;
        WindowCustomizeHero.GetComponent<CanvasGroup>().alpha = 0;
        WindowBuyHero.GetComponent<CanvasGroup>().alpha = 0;
        WindowLandDetail.GetComponent<CanvasGroup>().alpha = 0;
        WindowBuyLand.GetComponent<CanvasGroup>().alpha = 0;

        WindowRegister.gameObject.SetActive(false);
        WindowLogin.gameObject.SetActive(false);
        WindowRegister2.gameObject.SetActive(false);
        WindowRegisterBuildHero.gameObject.SetActive(false);
        WindowSpellDetail.gameObject.SetActive(false);
        WindowCustomizeHero.gameObject.SetActive(false);
        WindowBuyHero.gameObject.SetActive(false);
        WindowLandDetail.gameObject.SetActive(false);
        WindowBuyLand.gameObject.SetActive(false);

        TestForLogin();
    }

    private void TestForLogin()
    {
        try
        {
            Log.Info("Checking for previous session data...");

            EncryptedFileDataProvider loginData = new EncryptedFileDataProvider();
            loginData.Open("knh.dat");

            DataContainer data = loginData.Deserialize();

            if(data.Get("user_name") != null || data.Get("session_token") != null)
            {
                Dialog.ProgressOn("Magic in progress...");
                VerifySessionData(data.Get("user_name"), data.Get("session_token"));
            }
        }
        catch
        {
            Log.Info("No previous login data found");
            GetComponent<Animator>().enabled = true;
        }
    }

    private void VerifySessionData(object v1, object v2)
    {
        StartCoroutine(TemporaryVerifySessionData());
    }

    public void DisplayRegisterWindow()
    {
        PlayButton.DOFade(0, 0.2f);

        SwitchBetweenWindows(WindowLogin, WindowRegister, 0.2f);
    }

    public void DisplayLoginWindow()
    {
        SwitchBetweenWindows(WindowRegister, WindowLogin, 0.2f);
    }

    public void DisplayCustomizeHero()
    {
        SwitchBetweenWindows(WindowRegisterBuildHero, WindowCustomizeHero, 0.2f);
    }

    public void DisplayBuyHero()
    {
        WindowRegisterBuildHero.GetComponent<CanvasGroup>().DOFade(0, 0.2f);

        SwitchBetweenWindows(WindowCustomizeHero, WindowBuyHero, 0.2f);
    }

    public void DisplayBuyLand()
    {
        SwitchBetweenWindows(WindowLandDetail, WindowBuyLand, 0.2f);
    }

    public void DisplaySelectLand()
    {
        WindowCustomizeHero.GetComponent<CanvasGroup>().DOFade(0, 0.2f);

        StartCoroutine(HideCustomizeHero());
        StartCoroutine(FindLand());
    }

    IEnumerator HideCustomizeHero()
    {
        yield return new WaitForSeconds(0.2f);

        WindowCustomizeHero.gameObject.SetActive(false);
    }

    IEnumerator FindLand()
    {
        Dialog.ProgressOn("Finding land for your kingdom...");

        yield return new WaitForSeconds(1.2f);

        Dialog.ProgressOff();

        // TEMPORRY - Replace below with a proper land finding algorithm
        bool displayFirst = UnityEngine.Random.Range(0, 2) == 0;

        WindowLandDetail.gameObject.SetActive(true);
        WindowLandDetail.transform.Find("ImgLand1").gameObject.SetActive(displayFirst);
        WindowLandDetail.transform.Find("ImgLand2").gameObject.SetActive(!displayFirst);

        string[] regionNames = new string[] { "Goodsprings", "Oredere", "Radiant", "Borg", "New Heaven" };
        townName = regionNames[UnityEngine.Random.Range(0, regionNames.Length)];
        WindowLandDetail.transform.Find("TxtSpellInformation").GetComponent<Text>().text = townName;

        WindowLandDetail.transform.Find("TxtResource1/PrgStat").GetComponent<Slider>().value = UnityEngine.Random.Range(0.1f, 0.22f);
        WindowLandDetail.transform.Find("TxtResource2/PrgStat").GetComponent<Slider>().value = UnityEngine.Random.Range(0.1f, 0.22f);
        WindowLandDetail.transform.Find("TxtResource3/PrgStat").GetComponent<Slider>().value = UnityEngine.Random.Range(0.1f, 0.22f);
        WindowLandDetail.transform.Find("TxtResource4/PrgStat").GetComponent<Slider>().value = UnityEngine.Random.Range(0.1f, 0.22f);
        WindowLandDetail.transform.Find("TxtResource5/PrgStat").GetComponent<Slider>().value = UnityEngine.Random.Range(0.1f, 0.22f);
        WindowLandDetail.transform.Find("TxtResource6/PrgStat").GetComponent<Slider>().value = UnityEngine.Random.Range(0.1f, 0.22f);

        WindowLandDetail.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
    }

    public void FindAnotherPlace()
    {
        WindowLandDetail.GetComponent<CanvasGroup>().DOFade(0, 0.2f);

        StartCoroutine(FindLand());
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
        SwitchBetweenWindows(WindowRegister2, WindowRegister, 0.2f);
    }

    public void BackFromCustomizeHero()
    {
        SwitchBetweenWindows(WindowCustomizeHero, WindowRegisterBuildHero, 0.2f);
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
        emailAddress = WindowRegister2.transform.Find("InpMailAddress").GetComponent<InputField>().text.Trim();
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
        SwitchBetweenWindows(WindowRegisterBuildHero, WindowRegister2, 0.2f);
    }

    public void BackFromBuyHero()
    {
        SwitchBetweenWindows(WindowBuyHero, WindowRegisterBuildHero, 0.2f);
    }

    public void BackFromBuyLand()
    {
        SwitchBetweenWindows(WindowBuyLand, WindowLandDetail, 0.2f);
    }

    public void BackFromSelectLand()
    {
        SwitchBetweenWindows(WindowLandDetail, WindowCustomizeHero, 0.2f);
    }

    public void SaveAndProceedToWorldMap()
    {
        StartCoroutine(ProceedToWorldMapCoroutine());
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
                // Archer
                case 2:
                    StartCoroutine(ChangeClassDescription("Archers are equiped with ranged weapons. They have an advantage over stronger enemies because if you want to punish them, you should catch them first.", "Archer"));
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
        SwitchBetweenWindows(WindowSpellDetail, WindowRegisterBuildHero, 0.2f);
    }

    IEnumerator ProceedToWorldMapCoroutine()
    {
        WindowLandDetail.GetComponent<CanvasGroup>().DOFade(0, 0.5f);

        yield return new WaitForSeconds(0.5f);

        Dialog.ProgressOn("Creating your account...");

        yield return new WaitForSeconds(2.0f);

        DataContainer dcUser = new DataContainer();
        DataContainer dcQuests = new DataContainer();
        DataContainer dcKingdom = new DataContainer();
        DataContainer dcTown = new DataContainer();

        List<DataValue> activeQuests = new List<DataValue>();
        activeQuests.Add(new DataValue(1));

        List<DataValue> activeTowns = new List<DataValue>();
        activeTowns.Add(new DataValue(dcTown));

        dcTown.Add("town_id", 1);
        dcTown.Add("town_name", townName);
        dcTown.Add("coord_x", 0);
        dcTown.Add("coord_y", 0);

        dcQuests.Add("active_quests", activeQuests);
        dcKingdom.Add("towns", activeTowns);

        dcUser.Add("account_type", accountType);
        dcUser.Add("user_name", emailAddress);
        dcUser.Add("session_token", "-");
        dcUser.Add("full_name", governorName);
        dcUser.Add("hero_class", selectedClass);
        dcUser.Add("center_town", 1);
        dcUser.Add("sex", sex);
        dcUser.Add("appearance_suit_color", 0);
        dcUser.Add("appearance_hair_style", 0);
        dcUser.Add("appearance_hair_color", 0);
        dcUser.Add("appearance_skin_color", 0);
        dcUser.Add("quest_data", dcQuests);
        dcUser.Add("kingdom_data", dcKingdom);

        EncryptedFileDataProvider userFile = new EncryptedFileDataProvider();

        try
        {
            userFile.Open("knh.dat");
            userFile.Serialize(dcUser);

            LoadWorldMap();
        } catch(Exception ex)
        {
            Dialog.Error("Something went wrong while saving your user information. Are you sure you are not out of disk space?", "Back", () =>
            {
                WindowLogin.gameObject.SetActive(true);
                WindowLogin.GetComponent<CanvasGroup>().alpha = 0;
                WindowLogin.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
            });

            Log.Error("Could not save knh.dat, details: " + ex.Message + "\n" + ex.StackTrace);
        }
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

    IEnumerator DisplayRegister2Error(string message)
    {
        WindowRegister2.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.2f);

        WindowRegister2.gameObject.SetActive(false);
        Dialog.Error(message, "Close", () => {
            StartCoroutine(DisplayRegisterStep2(0));
        });
    }

    void LoadWorldMap()
    {
        KNHSceneManager.LoadScene("WorldMap", OnLevelLoadProgressChanged);
    }

    void OnLevelLoadProgressChanged(float percent)
    {
        if (percent >= 0.9f)
        {
            // Done loading
            Log.Info("Done loading WorldMap");
        }
    }

    // Window framework
    public void SwitchBetweenWindows(Image sourceWindow, Image targetWindow, float transitionTime)
    {
        StartCoroutine(SwitchBetweenWindowsNow(sourceWindow, targetWindow, transitionTime));
    }

    IEnumerator SwitchBetweenWindowsNow(Image sourceWindow, Image targetWindow, float transitionTime)
    {
        sourceWindow.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.5f);

        sourceWindow.gameObject.SetActive(false);
        targetWindow.gameObject.SetActive(true);

        targetWindow.GetComponent<CanvasGroup>().alpha = 0;
        targetWindow.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetEase(Ease.Linear);
    }

    // Temporary functions, which will be replaced in real game
    IEnumerator TemporaryVerifySessionData()
    {
        yield return new WaitForSeconds(2f);

        Dialog.ProgressOff();
        LoadWorldMap();
    }
}
