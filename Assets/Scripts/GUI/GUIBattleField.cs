using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class GUIBattleField : MonoBehaviour
{
    public BattleFieldMovementManager MgrMovement;
    public GameObject PnlVictory;
    public GameObject PnlDefeat;
    public Text TxtCountdownTop;
    public Text TxtCountdownBottom;
    public GameObject SpellIndicator;

    public delegate void OnCancel();
    public OnCancel CancelSpellHandler = null;
    void Start()
    {
        PnlVictory?.SetActive(false);
        PnlDefeat?.SetActive(false);
        SpellIndicator?.SetActive(false);

        StartCoroutine(CountdownStart());
        CooldownSpells();
    }

    public void DisplaySpellIndicator(string message, OnCancel cancelHandler)
    {
        SpellIndicator?.SetActive(true);

        var slider = SpellIndicator.transform.Find("ResourceBar").GetComponent<Slider>();
        StartCoroutine(SpellIndicatorCountdown(slider, 5f));

        var cancelButton = SpellIndicator.transform.Find("BtnCancel").GetComponent<Button>();
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            cancelHandler?.Invoke();
        });
    }

    public void DisplaySpellIndicator(OnCancel cancelHandler)
    {
        DisplaySpellIndicator("tap on the battle field to cast your spell", cancelHandler);
    }

    IEnumerator SpellIndicatorCountdown(Slider slider, float duration)
    {
        slider.DOValue(1, 0);
        slider.DOValue(0, duration).SetEase(Ease.Linear);

        yield return new WaitForSeconds(duration);

        SpellIndicator.transform.Find("BtnCancel").GetComponent<Button>().onClick.RemoveAllListeners();

        if (SpellIndicator.activeSelf)
        {
            CancelSpell();
        }
    }

    public void CancelSpell()
    {
        SpellIndicator.SetActive(false);

        CancelSpellHandler?.Invoke();
        CancelSpellHandler = null;
    }

    private void CooldownSpells()
    {
        var heroes1 = GameObject.Find("Canvas/Heroes/Hero1");
        var heroes2 = GameObject.Find("Canvas/Heroes/Hero2");
        var heroes3 = GameObject.Find("Canvas/Heroes/Hero3");

        CooldownSpellsOf(heroes1);
        CooldownSpellsOf(heroes2);
        CooldownSpellsOf(heroes3);
    }

    private void CooldownSpellsOf(GameObject spellLine)
    {
        if(spellLine == null)
        {
            return;
        }

        var img1 = spellLine.transform.Find("ImgMagic1");
        var img2 = spellLine.transform.Find("ImgMagic2");
        var img3 = spellLine.transform.Find("ImgMagic3");

        img1?.GetComponent<SpellCaster>()?.GlobalCooldown(4.7f);
        img2?.GetComponent<SpellCaster>()?.GlobalCooldown(4.7f);
        img3?.GetComponent<SpellCaster>()?.GlobalCooldown(4.7f);
    }

    public void ShowDefeatWindow()
    {
        PnlDefeat?.SetActive(true);
    }

    public void ShowVictoryWindow()
    {
        PnlVictory?.SetActive(true);
    }

    IEnumerator CountdownStart()
    {
        TxtCountdownTop.text = "3";
        TxtCountdownBottom.text = "3";

        yield return new WaitForSeconds(1);

        TxtCountdownTop.DOFade(1, 0).SetEase(Ease.Linear);
        TxtCountdownTop.DOFade(0, 0.5f);
        TxtCountdownTop.transform.DOScale(2, 0.5f);
        TxtCountdownBottom.text = "2";

        yield return new WaitForSeconds(1);

        TxtCountdownTop.text = "2";
        TxtCountdownTop.DOFade(1, 0).SetEase(Ease.Linear);
        TxtCountdownTop.DOFade(0, 0.5f);
        TxtCountdownTop.transform.DOScale(1, 0);
        TxtCountdownTop.transform.DOScale(2, 0.5f);
        TxtCountdownBottom.text = "1";

        yield return new WaitForSeconds(1);

        TxtCountdownTop.text = "1";
        TxtCountdownTop.DOFade(1, 0).SetEase(Ease.Linear);
        TxtCountdownTop.DOFade(0, 0.5f);
        TxtCountdownTop.transform.DOScale(1, 0);
        TxtCountdownTop.transform.DOScale(2, 0.5f);
        TxtCountdownBottom.text = "0";

        yield return new WaitForSeconds(1);

        TxtCountdownTop.text = "0";
        TxtCountdownTop.DOFade(1, 0).SetEase(Ease.Linear);
        TxtCountdownTop.DOFade(0, 0.5f);
        TxtCountdownTop.transform.DOScale(1, 0);
        TxtCountdownTop.transform.DOScale(2, 0.5f);
        TxtCountdownBottom.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        TxtCountdownTop.gameObject.SetActive(false);
        MgrMovement.SetCanMove(true);
    }
}
