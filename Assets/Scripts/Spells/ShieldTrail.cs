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
/// This is the implementation of Shield Trail spell from Warrior class
/// 
/// </summary>
public class ShieldTrail : Spell
{
    public override void Cast()
    {
        GameObject caster = GetCaster();
        if (caster == null)
        {
            Debug.LogWarning("Shield Trail spell needs a caster");
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
        anim.CrossFade("Throw", 0.01f);

        yield return new WaitForSeconds(1.0f);

        var shield = Instantiate(Visuals[0], new Vector3(caster.transform.position.x, 17, caster.transform.position.z), Quaternion.identity);
        shield.transform.localScale = Vector3.one * 4.5f;
        shield.transform.Rotate(new Vector3(0, 0, 90));
        shield.transform.DORotate(Vector3.up * 1440, 20);

        var trail = Instantiate(Visuals[1], new Vector3(caster.transform.position.x, 17, caster.transform.position.z), Quaternion.identity);
        trail.transform.localScale = Vector3.one * 4f;

        var bigShield = FindDeepChild(caster.transform, "Shield");
        bigShield.gameObject.SetActive(false);

        // enumerate all enemy units and make the ones which are in the attack range fall
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float[] distances = new float[enemies.Length];
        int[] indices = new int[enemies.Length];

        for (int i = 0; i < distances.Length; i++)
        {
            distances[i] = Vector3.Distance(enemies[i].transform.position, caster.transform.position);
            indices[i] = i;
        }

        // sort enemies according to distances
        for (int i = 0; i < indices.Length; i++)
        {
            for (int j = i + 1; j < indices.Length; j++)
            {
                if(distances[i] > distances[j])
                {
                    float temp = distances[i];
                    distances[i] = distances[j];
                    distances[j] = temp;

                    int t1 = indices[i];
                    indices[i] = indices[j];
                    indices[j] = t1;
                }
            }
        }

        // throw to first three
        float distance;
        float throwSpeed = 0.01f;
        shield.transform.DOMove(new Vector3(enemies[indices[0]].transform.position.x, shield.transform.position.y, enemies[indices[0]].transform.position.z), distances[0] * throwSpeed).SetEase(Ease.Linear);
        trail.transform.DOMove(new Vector3(enemies[indices[0]].transform.position.x, shield.transform.position.y, enemies[indices[0]].transform.position.z), distances[0] * throwSpeed).SetEase(Ease.Linear);
        yield return new WaitForSeconds(distances[0] * throwSpeed);

        int damage = Random.Range(25, 36);
        switch (CurrentLevel)
        {
            case 2:
                damage = Random.Range(32, 48);
                break;
            case 3:
                damage = Random.Range(44, 61);
                break;
        }

        // Enemy 1
        if (enemies.Length >= 1)
        {
            var unit = enemies[indices[0]].GetComponent<Unit>();
            unit?.TakeDamage(damage, caster.GetComponent<Unit>());

            var explosion1 = Instantiate(Visuals[2], enemies[indices[0]].transform.position, Quaternion.identity);
            explosion1.transform.localScale = Vector3.one * 4f;
            Destroy(explosion1, 1);

            unit?.Slow();
            unit?.RestoreAfter(3);

            // Enemy 2
            if (enemies.Length >= 2)
            {
                distance = Vector3.Distance(enemies[indices[0]].transform.position, enemies[indices[1]].transform.position);
                shield.transform.DOMove(new Vector3(enemies[indices[1]].transform.position.x, shield.transform.position.y, enemies[indices[1]].transform.position.z), distance * throwSpeed).SetEase(Ease.Linear);
                trail.transform.DOMove(new Vector3(enemies[indices[1]].transform.position.x, shield.transform.position.y, enemies[indices[1]].transform.position.z), distance * throwSpeed).SetEase(Ease.Linear);
                yield return new WaitForSeconds(distance * throwSpeed);

                unit = enemies[indices[1]].GetComponent<Unit>();
                unit?.TakeDamage(damage, caster.GetComponent<Unit>());

                var explosion2 = Instantiate(Visuals[2], enemies[indices[1]].transform.position, Quaternion.identity);
                explosion2.transform.localScale = Vector3.one * 4f;
                Destroy(explosion2, 1);

                unit?.Slow();
                unit?.RestoreAfter(3);

                // Enemy 3
                if (enemies.Length >= 3)
                {
                    distance = Vector3.Distance(enemies[indices[1]].transform.position, enemies[indices[2]].transform.position);
                    shield.transform.DOMove(new Vector3(enemies[indices[2]].transform.position.x, shield.transform.position.y, enemies[indices[2]].transform.position.z), distance * throwSpeed).SetEase(Ease.Linear);
                    trail.transform.DOMove(new Vector3(enemies[indices[2]].transform.position.x, shield.transform.position.y, enemies[indices[2]].transform.position.z), distance * throwSpeed).SetEase(Ease.Linear);
                    yield return new WaitForSeconds(distance * throwSpeed);

                    unit = enemies[indices[2]].GetComponent<Unit>();
                    unit?.TakeDamage(damage, caster.GetComponent<Unit>());

                    var explosion3 = Instantiate(Visuals[2], enemies[indices[2]].transform.position, Quaternion.identity);
                    explosion3.transform.localScale = Vector3.one * 4f;
                    Destroy(explosion3, 1);

                    unit?.Slow();
                    unit?.RestoreAfter(3);
                }
            }

            // take it back
            shield.transform.DOMove(new Vector3(caster.transform.position.x, shield.transform.position.y, caster.transform.position.z), distances[Mathf.Min(2, enemies.Length - 1)] * throwSpeed).SetEase(Ease.Linear);
            trail.transform.DOMove(new Vector3(caster.transform.position.x, shield.transform.position.y, caster.transform.position.z), distances[Mathf.Min(2, enemies.Length - 1)] * throwSpeed).SetEase(Ease.Linear);
            yield return new WaitForSeconds(distances[Mathf.Min(2, enemies.Length - 1)] * throwSpeed);

            // clear up
            Destroy(shield, 0.1f);
            Destroy(trail, 0.5f);

            bigShield.gameObject.SetActive(true);
        }

        anim.CrossFade("Idle", 0.02f);
    }
}
