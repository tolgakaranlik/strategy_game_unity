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
/// This is the implementation of Bleed spell from Warrior class
/// 
/// </summary>
public class Bleed : Spell
{
    public override void Cast()
    {
    }

    public override void Cast(GameObject target)
    {
        Cast();
    }

}
