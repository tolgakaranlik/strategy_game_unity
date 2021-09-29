using System.Collections;
using UnityEngine;

/// <summary>
///
///  Author: Tolga K, 08/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
/// This is the implementation of Polymorph spell from Mage class
/// 
/// </summary>
public class Polymorph : Spell
{
    public override void Cast()
    {
        StartCoroutine(CastNow(null));
    }

    public override void Cast(GameObject target)
    {
        StartCoroutine(CastNow(target));
    }

    // Update is called once per frame
    IEnumerator CastNow(GameObject target)
    {
        if(target == null)
        {
            target = GetTarget();
        }

        // change hero avatar
        Transform portraitToChange = null;
        var globalCanvas = transform.Find("/Canvas/Heroes");
        if(globalCanvas != null)
        {
            Transform subElement;
            for (int currHeroItem = 1; currHeroItem <= 3; currHeroItem++)
            {
                subElement = globalCanvas.Find("Hero" + currHeroItem + "/ImgMagic1");
                if(subElement != null)
                {
                    var caster = subElement.GetComponent<SpellCaster>();
                    if(caster.Caster.gameObject == target)
                    {
                        portraitToChange = globalCanvas.Find("Hero" + currHeroItem + "/ImgPortrait");
                        break;
                    }
                }
            }
        }

        // cast the spell
        var effect = Instantiate(Visuals[0], target.transform.position + Vector3.up * 3f, Quaternion.identity);
        effect.transform.localScale = Vector3.one * 2.5f;
        Destroy(effect, 3);

        BattlefieldSimpleUnit targetUnit = target.GetComponent<BattlefieldSimpleUnit>();
        targetUnit.DisableSearch();
        targetUnit.CurrentTarget = null;
        targetUnit.CanFire = false;
        targetUnit.CanMove = false;

        target.SetActive(false);

        //yield return new WaitForSeconds(0.15f);

        int animalType = Random.Range(1, 3);
        var replacement = Instantiate(Visuals[animalType], target.transform.position, target.transform.rotation, target.transform.parent);
        replacement.SetActive(true);

        if(portraitToChange != null)
        {
            portraitToChange.GetChild(animalType - 1).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(5f);

        replacement.SetActive(false);

        effect = Instantiate(Visuals[0], target.transform.position + Vector3.up * 3f, Quaternion.identity);
        effect.transform.localScale = Vector3.one * 2.5f;
        Destroy(effect, 3);

        target.SetActive(true);

        if (portraitToChange != null)
        {
            portraitToChange.GetChild(animalType - 1).gameObject.SetActive(false);
        }

        targetUnit.EnableSearch();
        targetUnit.CanFire = true;
        targetUnit.CanMove = true;
    }
}
