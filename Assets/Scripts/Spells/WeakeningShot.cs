using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
///  Author: Tolga K, 07/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
/// This is the implementation of WeakeningShot spell from Archer class
/// 
/// </summary>
public class WeakeningShot : Spell
{
    public override void Cast()
    {
    }

    public override void Cast(GameObject target)
    {
        Cast();
    }
}
