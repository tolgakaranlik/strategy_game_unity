using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernoExplode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<Unit>()?.TakeDamage(transform.parent.GetComponent<Inferno>().GetMeteorDamage(), null);
        }
    }
}
