using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

/// <summary>
///
///  Author: Tolga K, 07/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
/// This is the implementation of Bash spell from Warrior class
/// 
/// </summary>
public class SummonWaterElemental : Spell
{
    GameObject elemental = null;
    int Duration = 20;
    int Life = 60;

    public override void Cast()
    {
        GameObject caster = GetCaster();
        if(caster == null)
        {
            Debug.LogWarning("Summon Water Elemental spell needs a caster");
            return;
        }

        StartCoroutine(SummonElemental(caster));
    }

    public override void Cast(GameObject target)
    {
        Cast();
    }

    IEnumerator SummonElemental(GameObject caster)
    {
        Unit unit = caster.GetComponent<Unit>();
        unit.CastingSpell = true;
        unit.CanFire = false;
        unit.CanMove = false;

        if (elemental != null)
        {
            StartCoroutine(DestroyElemental(caster));
        }

        Animator anim = caster.GetComponent<Animator>();
        anim.CrossFade("Magic2", 0.01f);

        yield return new WaitForSeconds(1.0f);

        Vector3 position = caster.transform.position;
        elemental = Instantiate(Visuals[0], position + caster.transform.forward * 10 - caster.transform.up * 5, Quaternion.LookRotation(caster.transform.forward, caster.transform.up), caster.transform.parent);
        var blast = Instantiate(Visuals[1], elemental.transform.position + Vector3.up * 5 - Vector3.forward, Quaternion.LookRotation(caster.transform.forward, caster.transform.up), elemental.transform);
        blast.transform.localScale = Vector3.one * 0.9f;

        Vector3 s = caster.transform.localScale;
        s.x = Mathf.Abs(s.x);
        s.y = Mathf.Abs(s.y);
        s.z = Mathf.Abs(s.z);

        elemental.transform.localScale = Vector3.zero;
        elemental.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        elemental.transform.DOLocalMoveY(11, 0);
        elemental.transform.DOScale(s * 0.65f, 0.5f);
        //elemental.transform.SetParent(null);

        blast.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        Destroy(blast, 2.1f);

        Duration = 20;
        switch(SpellLevel)
        {
            case 2:
                Duration = 25;
                Life = 90;
                break;
            case 3:
                Duration = 30;
                Life = 150;
                break;
        }

        caster.GetComponent<BattlefieldSimpleUnit>().EnableSearch();
        unit.CanFire = true;
        unit.CanMove = true;
        unit.CastingSpell = false;

        StartCoroutine(SummonElementalNow(caster));
    }

    IEnumerator DestroyElemental(GameObject caster)
    {
        elemental.transform.DOScale(0f, 0.5f);

        var blast = Instantiate(Visuals[1], Vector3.zero, Quaternion.LookRotation(caster.transform.forward, caster.transform.up), elemental.transform);
        blast.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        Destroy(blast, 1);

        yield return new WaitForSeconds(1);

        Destroy(elemental);
        elemental = null;
    }

    IEnumerator SummonElementalNow(GameObject caster)
    {
        Image imgLifeSlider = elemental.transform.Find("Canvas/ImgSliderBG/ImgSlider").GetComponent<Image>();

        for (int i = 0; i < Duration * 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            if (elemental == null)
            {
                yield break;
            }

            try
            {
                imgLifeSlider.fillAmount = Mathf.Max(0, imgLifeSlider.fillAmount - 1f / (Duration * 10));
                if (imgLifeSlider.fillAmount <= 0)
                {
                    break;
                }
            } catch
            {
                // falls to this state when events not synced properly
                yield break;
            }
        }

        StartCoroutine(DestroyElemental(caster));
    }
}
