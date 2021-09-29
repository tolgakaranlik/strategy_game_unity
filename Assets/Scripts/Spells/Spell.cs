using UnityEngine;
/// <summary>
///
///  Author: Tolga K, 07/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
/// This class intended to hold spell data
/// 
/// </summary>
public abstract class Spell : Capability
{
    public enum SpellTarget { Random, SelectedPoint, SelectedEnemy, SelectedPlayer, SelectedUnit };
    public enum TargetSide { Player, Computer };

    public int SpellId = 1001;
    public int SpellLevel = 1;
    public GameObject[] Visuals;
    public SpellTarget TargetType = SpellTarget.Random;
    public TargetSide Side = TargetSide.Computer;

    public string SpellName = "";
    public string SpellDescription = "";
    public string SpellAvatar = "";

    public string Level1Desc = "";
    public string Level2Desc = "";
    public string Level3Desc = "";
    public string Level4Desc = "";

    protected GameObject target = null;

    public override void Init()
    {
        Init(SpellId, SpellName, SpellDescription, SpellAvatar);

        SetLevelDescription(0, Level1Desc);
        SetLevelDescription(1, Level2Desc);
        SetLevelDescription(2, Level3Desc);
        //SetLevelDescription(3, Level4Desc);
    }

    public virtual void CancelSpell()
    {

    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public GameObject GetTarget()
    {
        if(Side == TargetSide.Computer)
        {
            return GetPlayerTarget();
        } else
        {
            return GetCPUTarget();
        }
    }

    GameObject GetCPUTarget()
    {
        GameObject target = null;

        GameObject[] enemyArmy = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemyArmy.Length > 0)
        {
            target = enemyArmy[Random.Range(0, enemyArmy.Length)];
        }
        else
        {
            Debug.LogWarning("POLYMORPH: No AI units could be found");
        }

        return target;
    }

    GameObject GetPlayerTarget()
    {
        GameObject target = null;

        // auto select target
        GameObject[] playerArmy = GameObject.FindGameObjectsWithTag("PlayerArmy");
        GameObject[] playerHeroes = GameObject.FindGameObjectsWithTag("Hero");

        if (playerArmy.Length > 0 && playerHeroes.Length > 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                target = playerArmy[Random.Range(0, playerArmy.Length)];
            }
            else
            {
                target = playerHeroes[Random.Range(0, playerHeroes.Length)];
            }
        }
        else if (playerArmy.Length > 0 && playerHeroes.Length == 0)
        {
            target = playerArmy[Random.Range(0, playerArmy.Length)];
        }
        else if (playerArmy.Length == 0 && playerHeroes.Length > 0)
        {
            target = playerHeroes[Random.Range(0, playerHeroes.Length)];
        }
        else
        {
            Debug.LogWarning("POLYMORPH: No player units could be found");
        }

        return target;
    }
}