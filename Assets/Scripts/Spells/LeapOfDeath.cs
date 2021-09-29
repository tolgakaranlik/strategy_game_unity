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
/// This is the implementation of Leap of Death spell from Warrior class
/// 
/// </summary>
public class LeapOfDeath : Spell
{
    public override void Cast()
    {
        GameObject caster = GetCaster();
        if (caster == null)
        {
            Debug.LogWarning("Leap of Death spell needs a caster");
            return;
        }

        StartCoroutine(Perform(caster));
    }

    public override void Cast(GameObject target)
    {
        Cast();
    }

    IEnumerator Perform(GameObject caster)
    {
        Animator anim = caster.GetComponent<Animator>();
        GetComponents<AudioSource>()[0].Play();
        anim.CrossFade("Leap", 0.01f);

        yield return new WaitForSeconds(0.3f);

        caster.transform.DOMove(caster.transform.position + caster.transform.forward * 20 + caster.transform.up * 8, 0.35f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.35f);

        caster.transform.DOMove(caster.transform.position - caster.transform.up * 8, 0.35f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.35f);

        var ground = Instantiate(caster.GetComponent<Hero>().Artefacts[1], new Vector3(caster.transform.position.x, 12, caster.transform.position.z), Quaternion.identity);
        ground.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        ground.transform.localScale = Vector3.one * 6;
        Destroy(ground, 1);

        GetComponents<AudioSource>()[1].Play();

        // enumerate all enemy units and make the ones which are in the attack range fall
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float dist;
        Hero hero = caster.GetComponent<Hero>();
        Unit unit;
        int damage = Random.Range(7, 16);

        switch(CurrentLevel)
        {
            case 2:
                damage = Random.Range(12, 19);
                break;
            case 3:
                damage = Random.Range(18, 31);
                break;
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            dist = Vector3.Distance(caster.transform.position, enemies[i].transform.position);

            if (dist < hero.AttackRange)
            {
                // you are surely done for >:)
                unit = enemies[i].GetComponent<Unit>();

                //Debug.Log("Enemy " + enemies[i].name + " is falling...");
                unit?.transform.LookAt(caster.transform.position);
                unit?.Fall();
                unit?.GetUpAfter(5);
                unit?.TakeDamage(damage, caster.GetComponent<Unit>());

                var bfunit = enemies[i].GetComponent<BattlefieldSimpleUnit>();
                bfunit?.MovementManager.StopAgent(enemies[i]);
                StartCoroutine(EnableSearchAfter(bfunit, 5));
            }
            else
            {
                //Debug.Log("Enemy " + enemies[i].name + " is out of range (" + dist + " found but " + hero.AttackRange + " required)");
            }
        }

        // continue as regular

        yield return new WaitForSeconds(1.0f);

        anim.CrossFade("Idle", 0.02f);
    }

    IEnumerator EnableSearchAfter(BattlefieldSimpleUnit bfunit, float duration)
    {
        yield return new WaitForSeconds(duration);

        bfunit?.EnableSearch();
    }
}
