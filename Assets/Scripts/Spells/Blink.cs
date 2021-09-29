using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
///  Author: Tolga K, 08/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
/// This is the implementation of Blink spell from Mage class
/// 
/// </summary>
public class Blink : Spell
{
    int lastBlink = -1;
    public override void Cast()
    {
        GameObject caster = GetCaster();
        if (caster == null)
        {
            Debug.LogWarning("Blink spell needs a caster");
            return;
        }

        StartCoroutine(CastNow(caster));
    }

    public override void Cast(GameObject target)
    {
        Cast();
    }

    // Update is called once per frame
    IEnumerator CastNow(GameObject caster)
    {
        var effect = Instantiate(Visuals[0], caster.transform.position + Vector3.up * 3f, Quaternion.identity);
        effect.transform.localScale = Vector3.one * 2.5f;
        Destroy(effect, 3);

        caster.SetActive(false);

        yield return new WaitForSeconds(0.65f);

        caster.transform.position = targetPosition;

        effect = Instantiate(Visuals[0], caster.transform.position + Vector3.up * 3f, Quaternion.identity);
        effect.transform.localScale = Vector3.one * 2.5f;
        Destroy(effect, 3);

        yield return new WaitForSeconds(0.15f);
        caster.SetActive(true);
    }
}
