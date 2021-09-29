using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
///
///  Author: Tolga K, 08/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
/// This is the implementation of Inferno spell from Mage class
/// 
/// </summary>
public class Inferno : Spell
{
    public int MeteorDamageLevel1 = 30;
    public int MeteorDamageLevel2 = 41;
    public int MeteorDamageLevel3 = 55;

    public int FireDamagePerSecondLevel1 = 3;
    public int FireDamagePerSecondLevel2 = 6;
    public int FireDamagePerSecondLevel3 = 10;

    int meteorDamage = 0;
    int fireDamagePerSecond = 1;
    bool canceled = false;

    public override void Cast()
    {
        GameObject caster = GetCaster();
        if (caster == null)
        {
            Debug.LogWarning("Inferno spell needs a caster");
            return;
        }

        StartCoroutine(CastInferno(caster));
    }

    public override void CancelSpell()
    {
        base.CancelSpell();

        canceled = true;
    }

    public override void Cast(GameObject target)
    {
        Cast();
    }

    public int GetMeteorDamage()
    {
        switch (CurrentLevel)
        {
            case 1:
                meteorDamage = MeteorDamageLevel1;
                break;
            case 2:
                meteorDamage = MeteorDamageLevel2;
                break;
            case 3:
                meteorDamage = MeteorDamageLevel3;
                break;
        }

        return meteorDamage;
    }

    public int GetFireDamagePerSecond()
    {
        switch (CurrentLevel)
        {
            case 1:
                fireDamagePerSecond = FireDamagePerSecondLevel1;
                break;
            case 2:
                fireDamagePerSecond = FireDamagePerSecondLevel2;
                break;
            case 3:
                fireDamagePerSecond = FireDamagePerSecondLevel3;
                break;
        }

        return fireDamagePerSecond;
    }

    IEnumerator CastInferno(GameObject caster)
    {
        GetMeteorDamage();
        GetFireDamagePerSecond();
        Unit unit = caster.GetComponent<Unit>();

        caster.GetComponent<Unit>().CanMove = false;
        Animator anim = caster.GetComponent<Animator>();
        anim.CrossFade("Magic1", 0.01f);

        var bottom = Instantiate(Visuals[2], targetPosition, Quaternion.identity);

        bottom.transform.DOScale(Vector3.zero, 0);
        bottom.transform.DOScale(Vector3.one * 2.5f, 2);
        bottom.transform.DORotate(new Vector3(0, 180, 0), 60f);

        // Activate canvas
        GameObject casterCanvas = null;
        for (int i = 0; i < caster.transform.childCount; i++)
        {
            if(caster.transform.GetChild(i).name == "CanvasMagic")
            {
                caster.transform.GetChild(i).gameObject.SetActive(true);

                for (int s = 0; s < caster.transform.GetChild(i).childCount; s++)
                {
                    if(caster.transform.GetChild(i).GetChild(s).name == "ImgMagic2011")
                    {
                        casterCanvas = caster.transform.GetChild(i).GetChild(s).gameObject;
                        casterCanvas.transform.localScale = Vector3.one * 0.06f;
                        casterCanvas.SetActive(true);

                        var black = casterCanvas.transform.Find("ImgBlack").GetComponent<Image>();
                        black.DOFillAmount(1, 0);
                        black.DOFillAmount(0, 5).SetEase(Ease.Linear);

                        break;
                    }
                }
            }
        }

        // Play the sound
        caster.GetComponents<AudioSource>()[0].Play();

        yield return new WaitForSeconds(4.5f);

        casterCanvas.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.5f);

        yield return new WaitForSeconds(0.5f);

        casterCanvas.SetActive(false);
        caster.GetComponent<BattlefieldSimpleUnit>().EnableSearch();
        unit.CanFire = true;
        unit.CanMove = true;
        unit.CastingSpell = false;

        Vector3 meteor1location = targetPosition + Vector3.left * Random.Range(0f, 5f) + Vector3.forward * Random.Range(0f, 5f); //  + Vector3.up * 40
        yield return new WaitForSeconds(0.025f);
        Vector3 meteor2location = targetPosition + Vector3.left * Random.Range(0f, 5f) + Vector3.forward * Random.Range(0f, 5f); //  + Vector3.up * 40
        yield return new WaitForSeconds(0.025f);
        Vector3 meteor3location = targetPosition + Vector3.left * Random.Range(0f, 5f) + Vector3.forward * Random.Range(0f, 5f); //  + Vector3.up * 40
        yield return new WaitForSeconds(0.025f);
        Vector3 meteor4location = targetPosition + Vector3.left * Random.Range(0f, 5f) + Vector3.forward * Random.Range(0f, 5f); //  + Vector3.up * 40
        yield return new WaitForSeconds(0.025f);
        Vector3 meteor5location = targetPosition + Vector3.left * Random.Range(0f, 5f) + Vector3.forward * Random.Range(0f, 5f); //  + Vector3.up * 40

        // Start falling
        var meteor1 = Instantiate(Visuals[1], meteor1location + Vector3.up * 40, Quaternion.identity);
        meteor1.transform.DOMoveY(11, 0.75f).SetEase(Ease.Linear);
        meteor1.transform.localScale = Vector3.one * 5;
        Destroy(meteor1, 2);
        yield return new WaitForSeconds(0.75f);

        var meteor2 = Instantiate(Visuals[1], meteor2location + Vector3.up * 40, Quaternion.identity);
        meteor2.transform.DOMoveY(11, 0.75f).SetEase(Ease.Linear);
        meteor2.transform.localScale = Vector3.one * 5;
        Destroy(meteor2, 2);

        var expl = transform.Find("Explosion").gameObject;
        var explosion1 = Instantiate(expl, meteor1location, Quaternion.identity);
        explosion1.transform.localScale = Vector3.one * 6;
        explosion1.SetActive(true);
        explosion1.transform.SetParent(gameObject.transform);
        Destroy(explosion1, 3);

        yield return new WaitForSeconds(0.75f);

        var meteor3 = Instantiate(Visuals[1], meteor3location + Vector3.up * 40, Quaternion.identity);
        meteor3.transform.DOMoveY(11, 0.75f).SetEase(Ease.Linear);
        meteor3.transform.localScale = Vector3.one * 5;
        Destroy(meteor3, 2);

        //yield return new WaitForSeconds(0.5f);

        var explosion2 = Instantiate(expl, meteor2location, Quaternion.identity);
        explosion2.transform.localScale = Vector3.one * 6;
        explosion2.SetActive(true);
        explosion2.transform.SetParent(gameObject.transform);
        Destroy(explosion2, 3);
        
        yield return new WaitForSeconds(0.75f);

        var meteor4 = Instantiate(Visuals[1], meteor4location + Vector3.up * 40, Quaternion.identity);
        meteor4.transform.DOMoveY(11, 0.75f).SetEase(Ease.Linear);
        meteor4.transform.localScale = Vector3.one * 5;
        Destroy(meteor4, 2);

        var explosion3 = Instantiate(expl, meteor3location, Quaternion.identity);
        explosion3.transform.localScale = Vector3.one * 6;
        explosion3.SetActive(true);
        explosion3.transform.SetParent(gameObject.transform);
        Destroy(explosion3, 3);

        yield return new WaitForSeconds(0.75f);

        var meteor5 = Instantiate(Visuals[1], meteor5location + Vector3.up * 40, Quaternion.identity);
        meteor5.transform.DOMoveY(11, 0.75f).SetEase(Ease.Linear);
        meteor5.transform.localScale = Vector3.one * 5;
        Destroy(meteor5, 2);

        var explosion4 = Instantiate(expl, meteor4location, Quaternion.identity);
        explosion4.transform.localScale = Vector3.one * 6;
        explosion4.SetActive(true);
        explosion4.transform.SetParent(gameObject.transform);
        Destroy(explosion4, 3);

        yield return new WaitForSeconds(0.75f);

        var explosion5 = Instantiate(expl, meteor5location, Quaternion.identity);
        explosion5.transform.localScale = Vector3.one * 6;
        explosion5.SetActive(true);
        explosion5.transform.SetParent(gameObject.transform);
        Destroy(explosion5, 3);

        bottom.transform.DOScale(Vector3.zero, 0.5f);
        Destroy(bottom, 0.6f);

        caster.GetComponent<Unit>().CanMove = true;

        var fireBase = transform.Find("Fire").gameObject;
        var fire = Instantiate(fireBase, targetPosition, Quaternion.identity);
        fire.SetActive(true);
        fire.transform.DOScale(Vector3.zero, 0);
        fire.transform.DOScale(Vector3.one * 3f, 2);
        fire.transform.SetParent(gameObject.transform);

        yield return new WaitForSeconds(4.5f);

        fire.transform.DOScale(Vector3.zero, 1);
        Destroy(fire, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
