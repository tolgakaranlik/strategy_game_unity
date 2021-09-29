using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernoBurn : MonoBehaviour
{
    List<GameObject> unitsWithin = new List<GameObject>();

    private void Start()
    {
        unitsWithin.Clear();

        InvokeRepeating("DamageWithin", 1, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && !unitsWithin.Contains(other.gameObject))
        {
            unitsWithin.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy" && unitsWithin.Contains(other.gameObject))
        {
            unitsWithin.Remove(other.gameObject);
        }
    }


    private void DamageWithin()
    {
        if(gameObject == null || gameObject.IsDestroyed() || !gameObject.activeSelf)
        { 
            return;
        }

        Unit unit;
        for (int i = 0; i < unitsWithin.Count; i++)
        {
            if (unitsWithin[i] != null && !unitsWithin[i].IsDestroyed())
            {
                unit = unitsWithin[i].GetComponent<Unit>();
                if (unit != null && !unit.Dead)
                {
                    unit.TakeDamage(transform.parent.GetComponent<Inferno>().GetFireDamagePerSecond(), null);
                }
            }
        }
    }
}
