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
    public int SpellId = 1002;

    public string SpellName = "Bleed";
    public string SpellDescription = "Attacked enemy starts bleeding, keeps taking damage over time";
    public string SpellAvatar = "Warriorskill_07";

    public string Level1Desc = "125% melee damage, 1 damage per second";
    public string Level2Desc = "150% melee damage, 2 damage per second";
    public string Level3Desc = "175% melee damage, 3 damage per second";
    public string Level4Desc = "200% melee damage, 1 damage per second";
    public string Level5Desc = "250% melee damage, 1 damage per second";

    public override void Cast()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        Init(SpellId, SpellName, SpellDescription, SpellAvatar, new float[] { 5, 5, 4, 4, 3 });

        SetLevelDescription(0, Level1Desc);
        SetLevelDescription(1, Level2Desc);
        SetLevelDescription(2, Level3Desc);
        SetLevelDescription(3, Level4Desc);
        SetLevelDescription(4, Level5Desc);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
