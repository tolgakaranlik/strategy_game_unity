using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
public class Bash : Spell
{
    public override void Cast()
    {
        GameObject caster = GetCaster();
        if (caster == null)
        {
            Debug.LogWarning("Bash spell needs a caster");
            return;
        }

        StartCoroutine(Perform(caster));
    }

    IEnumerator Perform(GameObject caster)
    {
        Animator anim = caster.GetComponent<Animator>();
        GetComponents<AudioSource>()[0].Play();
        anim.CrossFade("Bash", 0.01f);

        yield return new WaitForSeconds(0.3f);

        var trail = Instantiate(Visuals[0], new Vector3(caster.transform.position.x, 17, caster.transform.position.z), Quaternion.identity);
        trail.transform.localScale = new Vector3(6, 12, 6);
        Destroy(trail, 0.3f);

        caster.transform.DOMove(caster.transform.position + caster.transform.forward * 30, 0.1f).SetEase(Ease.Linear);
        trail.transform.DOMove(trail.transform.position + caster.transform.forward * 30, 0.1f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.2f);

        // enumerate all enemy units and make the ones which are in the attack range fall
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float dist;
        Hero hero = caster.GetComponent<Hero>();
        Unit unit;
        int damage = Random.Range(12, 21);

        switch (CurrentLevel)
        {
            case 2:
                damage = Random.Range(17, 24);
                break;
            case 3:
                damage = Random.Range(22, 36);
                break;
        }

        bool hitDisplayed = false;
        for (int i = 0; i < enemies.Length; i++)
        {
            dist = Vector3.Distance(caster.transform.position, enemies[i].transform.position);

            if (dist < hero.AttackRange / 1.5f)
            {
                if(!hitDisplayed)
                {
                    hitDisplayed = true;

                    var ground = Instantiate(caster.GetComponent<Hero>().Artefacts[2], caster.transform.forward + new Vector3(caster.transform.position.x, 17, caster.transform.position.z), Quaternion.identity);
                    ground.transform.localScale = Vector3.one * 6;
                    Destroy(ground, 1);

                    GetComponents<AudioSource>()[1].Play();
                }

                // you are surely done for >:)
                unit = enemies[i].GetComponent<Unit>();

                Debug.Log("Enemy " + enemies[i].name + " is stunning...");
                unit?.Stun();
                unit?.RestoreAfter(4);
                unit?.TakeDamage(damage);
            }
            else
            {
                Debug.Log("Enemy " + enemies[i].name + " is out of range (" + dist + " found but " + hero.AttackRange + " required)");
            }
        }

        // revert back to original state
        yield return new WaitForSeconds(0.5f);

        anim.CrossFade("Idle", 0.02f);
    }
}
