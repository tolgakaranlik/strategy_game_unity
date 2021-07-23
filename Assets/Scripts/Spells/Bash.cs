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
/// This is the implementation of Bash spell from Warrior class
/// 
/// </summary>
public class Bash : Spell
{
    public int SpellId = 1001;

    public string SpellName = "Bash";
    public string SpellDescription = "Your warrior charges through enemies with his shield";
    public string SpellAvatar = "Warriorskill_16";

    public string Level1Desc = "1 sec stun, 10 damage";
    public string Level2Desc = "1.5 sec stun, 12 damage";
    public string Level3Desc = "2 sec stun, 14 damage";
    public string Level4Desc = "2.5 sec stun, 17 damage";
    public string Level5Desc = "3 sec stun, 20 damage";

    public override void Cast()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        Init(SpellId, SpellName, SpellDescription, SpellAvatar);

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
