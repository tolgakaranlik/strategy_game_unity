using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TemporaryBattlefield : MonoBehaviour
{
    public GameObject effect1;
    public GameObject effect2;
    public GameObject effect3;
    public GameObject effect4;
    public GameObject effect5;
    public Image Button1;
    public Image Button1BW;
    public Image Button2;
    public Image Button2BW;
    public Image Button3;
    public Image Button3BW;
    public Image Button3Glow;
    public Image Button4;
    public Image Button4BW;
    public Image Button5;
    public Image Button5BW;
    public Image Button6;
    public Image Button6BW;
    public Image EnemyButton1;
    public Image EnemyTarget1;
    public Image EnemyButton2;
    public Image EnemyTarget2;
    public Image EnemyButton3;
    public Slider SliderP1;
    public Slider SliderP2;

    int button1State = 0;
    int button6State = 0;
    bool canButton2 = true;
    bool canButton3 = true;
    bool canButton4 = true;
    bool canButton5 = true;

    void Start()
    {
        
    }

    public void Button1Press()
    {
        switch(button1State)
        {
            case 0:
                effect1.SetActive(true);
                Button1.gameObject.transform.DOScale(1.15f, 0.5f);
                break;
            case 1:
                effect1.SetActive(false);
                Button1.gameObject.transform.DOScale(1f, 0.5f);
                break;
            case 2:
                Button1.gameObject.transform.DOScale(0.85f, 0.5f);
                Button1BW.gameObject.transform.DOScale(0.85f, 0.5f);
                Button1BW.DOFade(1, 0.5f);

                break;
            case 3:
                Button1.gameObject.transform.DOScale(1f, 0.5f);
                Button1BW.gameObject.transform.DOScale(1f, 0.5f);
                Button1BW.DOFade(0, 0.5f);

                break;
        }

        button1State = (button1State + 1) % 4;
    }

    public void Button6Press()
    {
        switch (button6State)
        {
            case 0:
                effect2.SetActive(true);
                Button6.gameObject.transform.DOScale(1.15f, 0.5f);
                break;
            case 1:
                effect2.SetActive(false);
                Button6.gameObject.transform.DOScale(1f, 0.5f);
                break;
            case 2:
                Button6.gameObject.transform.DOScale(0.85f, 0.5f);
                Button6BW.gameObject.transform.DOScale(0.85f, 0.5f);
                Button6BW.DOFade(1, 0.5f);

                break;
            case 3:
                Button6.gameObject.transform.DOScale(1f, 0.5f);
                Button6BW.gameObject.transform.DOScale(1f, 0.5f);
                Button6BW.DOFade(0, 0.5f);

                break;
        }

        button6State = (button6State + 1) % 4;
    }

    public void Button2Press()
    {
        if (!canButton2)
        {
            return;
        }

        StartCoroutine(Button2Phase1());
    }

    public void Button3Press()
    {
        if (!canButton3)
        {
            return;
        }

        StartCoroutine(Button3Phase1());
    }

    public void Button5Press()
    {
        if (!canButton5)
        {
            return;
        }

        StartCoroutine(Button5Phase1());
    }

    IEnumerator Button2Phase1()
    {
        canButton2 = false;

        Button2BW.color = new Color(0.5f, 0.5f, 0.5f, 0);
        Button2BW.DOFade(1, 0f);
        Button2BW.DOFillAmount(1, 0);

        Button2.gameObject.transform.DOScale(0.85f, 0.5f);
        Button2BW.gameObject.transform.DOScale(0.85f, 0.5f);
        Button2BW.DOFillAmount(0, 2.5f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(2.5f);

        Button2BW.DOFade(0, 0.5f);
        Button2.gameObject.transform.DOScale(1, 0.5f);
        Button2BW.gameObject.transform.DOScale(1, 0.5f);

        canButton2 = true;
    }

    IEnumerator Button3Phase1()
    {
        canButton3 = false;

        Button3Glow.DOFillAmount(1, 0);
        Button3Glow.DOFade(0, 0f);
        Button3.gameObject.transform.DOScale(1.1f, 0.5f);
        Button3Glow.gameObject.transform.DOScale(1.1f, 0.5f);
        Button3Glow.DOFade(1, 0.5f);

        yield return new WaitForSeconds(0.5f);

        Button3Glow.DOFillAmount(0, 4.5f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(4.5f);

        Button3Glow.DOFade(0, 0.5f);
        Button3.gameObject.transform.DOScale(1, 0.5f);
        Button3Glow.gameObject.transform.DOScale(1, 0.5f);

        canButton3 = true;
    }

    public void Button4Press()
    {
        if (!canButton4)
        {
            return;
        }

        StartCoroutine(Button4Phase1());
    }

    IEnumerator Button4Phase1()
    {
        canButton4 = false;

        Button4BW.color = new Color(0.5f, 0.5f, 0.5f, 0);
        Button4BW.DOFade(1, 0f);
        Button4BW.DOFillAmount(1, 0);

        Button4.gameObject.transform.DOScale(0.85f, 0.5f);
        Button4BW.gameObject.transform.DOScale(0.85f, 0.5f);
        Button4BW.DOFillAmount(0, 2.5f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(2.5f);

        Button4BW.DOFade(0, 0.5f);
        Button4.gameObject.transform.DOScale(1, 0.5f);
        Button4BW.gameObject.transform.DOScale(1, 0.5f);

        canButton4 = true;
    }

    IEnumerator Button5Phase1()
    {
        canButton5 = false;

        Button5BW.DOFillAmount(0, 0);
        Button5BW.DOFade(0, 0f);
        Button5.gameObject.transform.DOScale(1.1f, 0.5f);
        Button5BW.gameObject.transform.DOScale(1.1f, 0.5f);
        Button5BW.DOFade(1, 0.5f);

        yield return new WaitForSeconds(0.5f);

        Button5BW.DOFillAmount(1, 4.5f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(4.5f);

        Button5BW.DOFade(0, 0.5f);
        Button5.gameObject.transform.DOScale(1, 0.5f);
        Button5BW.gameObject.transform.DOScale(1, 0.5f);

        canButton5 = true;
    }

    // Enemy hero
    int heroIndex = 0;

    public void CastEnemyHeroMagic()
    {
        switch(heroIndex)
        {
            case 0:
                StartCoroutine(EnemyMagic1());
                break;
            case 1:
                StartCoroutine(EnemyMagic2());
                break;
            case 2:
                StartCoroutine(EnemyMagic3());
                break;
            case 3:
                StartCoroutine(EnemyMagic3Off());
                break;
        }

        heroIndex = (heroIndex + 1) % 4;
    }

    IEnumerator EnemyMagic1()
    {
        EnemyButton1.GetComponent<RectTransform>().DOAnchorPosX(182, 0); //.SetEase(Ease.Linear);
        EnemyButton1.GetComponent<RectTransform>().DOScale(0, 0f);
        EnemyButton1.GetComponent<RectTransform>().DOScale(1, 0.25f);

        yield return new WaitForSeconds(0.15f);

        EnemyTarget1.gameObject.SetActive(true);
        EnemyTarget1.GetComponent<RectTransform>().DOAnchorPosX(182, 0); //.SetEase(Ease.Linear);
        EnemyTarget1.GetComponent<RectTransform>().DOAnchorPosX(322, 0.5f); //.SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.25f);

        effect3.SetActive(true);

        SliderP1.DOValue(SliderP1.value - 0.09f, 0.5f);
        yield return new WaitForSeconds(0.5f);

        effect3.SetActive(false);

        EnemyTarget1.GetComponent<RectTransform>().DOAnchorPosX(182, 0.25f); //.SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.15f);

        EnemyTarget1.gameObject.SetActive(false);
        EnemyButton1.GetComponent<RectTransform>().DOScale(0, 0.25f);
    }

    IEnumerator EnemyMagic2()
    {
        EnemyButton2.GetComponent<RectTransform>().DOAnchorPosX(182, 0); //.SetEase(Ease.Linear);
        EnemyButton2.GetComponent<RectTransform>().DOScale(0, 0f);
        EnemyButton2.GetComponent<RectTransform>().DOScale(1, 0.25f);

        yield return new WaitForSeconds(0.25f);

        EnemyTarget2.gameObject.SetActive(true);
        EnemyTarget2.GetComponent<RectTransform>().DOAnchorPosX(182, 0); //.SetEase(Ease.Linear);
        EnemyTarget2.GetComponent<RectTransform>().DOAnchorPosX(322, 0.25f); //.SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.25f);

        effect4.SetActive(true);

        SliderP2.DOValue(SliderP2.value - 0.09f, 0.5f);
        yield return new WaitForSeconds(0.5f);

        effect4.SetActive(false);

        EnemyTarget2.GetComponent<RectTransform>().DOAnchorPosX(182, 0.25f); //.SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.25f);

        EnemyTarget2.gameObject.SetActive(false);
        EnemyButton2.GetComponent<RectTransform>().DOScale(0, 0.25f);
    }

    IEnumerator EnemyMagic3()
    {
        EnemyButton3.GetComponent<RectTransform>().DOAnchorPosX(182, 0); //.SetEase(Ease.Linear);
        EnemyButton3.GetComponent<RectTransform>().DOScale(0, 0f);
        EnemyButton3.GetComponent<RectTransform>().DOScale(1, 0.25f);

        yield return new WaitForSeconds(0.25f);

        effect5.SetActive(true);
    }

    IEnumerator EnemyMagic3Off()
    {
        effect5.SetActive(false);
        EnemyButton3.GetComponent<RectTransform>().DOScale(0, 0.25f);

        yield break;
    }
}
