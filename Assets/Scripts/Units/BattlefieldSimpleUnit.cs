using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldSimpleUnit : Unit
{
    public enum PlaySide { Player, Enemy };
    public BattleFieldMovementManager MovementManager = null;
    public SpellManager SpellMgr = null;
    public PlaySide Side;
    public GameObject CurrentTarget;

    Unit unit;
    bool allowedToSelectNewTargets = true;

    // TEMPORARY
    bool spellCasted = false;

    public new void Init()
    {
        base.Init();

        if (MovementManager == null)
        {
            MovementManager = FindObjectOfType<BattleFieldMovementManager>();
        }

        if(SpellMgr == null)
        {
            SpellMgr = FindObjectOfType<SpellManager>();
        }

        unit = GetComponent<Unit>();
        var hero = GetComponent<Hero>();

        if ((hero != null && (hero.Class == Hero.HeroClass.Warrior || hero.Class == Hero.HeroClass.Thief)) || (hero == null && unit.FireType == UnitFireType.Melee))
        {
            InvokeRepeating("SelectMeleeTarget", 0, 0.5f);
        } else {
            InvokeRepeating("SelectRangedTarget", 0, 0.5f);
        }
    }
    private void Start()
    {
        Init();
    }

    void SelectRangedTarget()
    {
        if (!Dead && CanMove && MovementManager != null && MovementManager.CanMoveOnTheField)
        {
            switch (Side)
            {
                case PlaySide.Player:
                    SelectRangedTargetFromEnemyForces();
                    break;
                case PlaySide.Enemy:
                    // TEMPORARY - begin
                    var hero = GetComponent<Hero>();
                    if(hero != null && hero.Class == Hero.HeroClass.Mage && !spellCasted && UnityEngine.Random.Range(0, 11) >= 5)
                    {
                        SpellMgr.Cast(2024, gameObject);
                        spellCasted = true;
                    }
                    // TEMPORARY - end

                    SelectRangedTargetFromPlayerForces();
                    break;
            }
        }
    }

    void SelectMeleeTarget()
    {
        if (!Dead && CanMove && MovementManager != null && MovementManager.CanMoveOnTheField)
        {
            switch(Side)
            {
                case PlaySide.Player:
                    SelectMeleeTargetFromEnemyForces();
                    break;
                case PlaySide.Enemy:
                    SelectMeleeTargetFromPlayerForces();
                    break;
            }
        }
    }

    public void EnableSearch()
    {
        allowedToSelectNewTargets = true;
    }

    public void DisableSearch()
    {
        allowedToSelectNewTargets = false;
    }

    private void SelectRangedTargetFromEnemyForces()
    {
        int health;

        if (allowedToSelectNewTargets)
        {
            // select the one with the lowet health
            var weakest = MovementManager.SelectWeakestTargetFromTag("Enemy", transform.position, out health);
            CurrentTarget = weakest;

            allowedToSelectNewTargets = false;
            var hero = GetComponent<Hero>();
            if (hero == null || hero.Class == Hero.HeroClass.Archer)
            {
                MovementManager.RangedAttack(gameObject, weakest);
            } else
            {
                MovementManager.MagicAttack(gameObject, weakest);
            }
        } else
        {
            if(CurrentTarget == null || CurrentTarget.GetComponent<Unit>().Dead)
            {
                allowedToSelectNewTargets = true;
            }
        }
    }

    private void SelectRangedTargetFromPlayerForces()
    {
        int healthHero;
        int healthTarget;

        if (allowedToSelectNewTargets)
        {
            // select the one with the lowet health
            var weakestTarget = MovementManager.SelectWeakestTargetFromTag("PlayerArmy", transform.position, out healthTarget);
            var weakestHero = MovementManager.SelectWeakestTargetFromTag("Hero", transform.position, out healthHero);
            allowedToSelectNewTargets = false;

            var hero = GetComponent<Hero>();
            if (weakestHero != null && weakestTarget != null)
            {
                if (healthHero < healthTarget)
                {
                    CurrentTarget = weakestHero;
                } else
                {
                    CurrentTarget = weakestTarget;
                }

                if(hero != null && hero.Class == Hero.HeroClass.Mage)
                {
                    MovementManager.MagicAttack(gameObject, CurrentTarget);
                }
                else
                {
                    MovementManager.RangedAttack(gameObject, CurrentTarget);
                }
            }
            else if (weakestHero == null && weakestTarget != null)
            {
                CurrentTarget = weakestTarget;
                if (hero != null && hero.Class == Hero.HeroClass.Mage)
                {
                    MovementManager.MagicAttack(gameObject, weakestTarget);
                }
                else
                {
                    MovementManager.RangedAttack(gameObject, weakestTarget);
                }
            }
            else if (weakestHero != null && weakestTarget == null)
            {
                CurrentTarget = weakestHero;
                if (hero != null && hero.Class == Hero.HeroClass.Mage)
                {
                    MovementManager.MagicAttack(gameObject, weakestHero);
                }
                else
                {
                    MovementManager.RangedAttack(gameObject, weakestHero);
                }
            }
        }
        else
        {
            if (CurrentTarget == null || CurrentTarget.GetComponent<Unit>().Dead)
            {
                allowedToSelectNewTargets = true;
            }
        }
    }

    private void SelectMeleeTargetFromEnemyForces()
    {
        float distance;
        if (!allowedToSelectNewTargets && CurrentTarget != null)
        {
            distance = Vector3.Distance(transform.position, CurrentTarget.transform.position);
            if (distance > AttackRangeOuter)
            {
                CurrentTarget = null;
                allowedToSelectNewTargets = true;
            }
            else
            {
                return;
            }
        }

        if (allowedToSelectNewTargets)
        {
            var closest = MovementManager.SelectClosestTargetFromTag("Enemy", transform.position, out distance);

            if (closest != null)
            {
                if (distance <= unit.AttackRange)
                {
                    CurrentTarget = closest;

                    // attack
                    allowedToSelectNewTargets = false;
                    MovementManager.StopAgent(gameObject);
                    //StartCoroutine(KeepAttackingToTarget(g));
                    MovementManager.MeleeAttack(gameObject, closest);
                }
                else
                {
                    // move
                    if (!unit.Dead)
                    {
                        MovementManager.MoveUnitTo(gameObject, closest.transform.position);
                    }
                }
            }
        }
        else
        {
            if (CurrentTarget == null || CurrentTarget.GetComponent<Unit>().Dead)
            {
                allowedToSelectNewTargets = true;
            }
        }
    }

    private void SelectMeleeTargetFromPlayerForces()
    {
        float distance;
        if (!allowedToSelectNewTargets && CurrentTarget != null)
        {
            distance = Vector3.Distance(transform.position, CurrentTarget.transform.position);
            if (distance > AttackRangeOuter)
            {
                CurrentTarget = null;
                allowedToSelectNewTargets = true;
            }
            else
            {
                return;
            }
        }

        if (allowedToSelectNewTargets)
        {
            float distanceUnit, distanceHero;
            var closestTarget = MovementManager.SelectClosestTargetFromTag("PlayerArmy", transform.position, out distanceUnit);
            var closestHero = MovementManager.SelectClosestTargetFromTag("Hero", transform.position, out distanceHero);

            if (closestHero != null && closestTarget != null)
            {
                if (distanceUnit <= distanceHero)
                {
                    // attack to unit
                    if (distanceUnit <= unit.AttackRange)
                    {
                        CurrentTarget = closestTarget;

                        // attack
                        allowedToSelectNewTargets = false;
                        MovementManager.StopAgent(gameObject);
                        MovementManager.MeleeAttack(gameObject, closestTarget);
                    }
                    else
                    {
                        // move
                        MovementManager.MoveUnitTo(gameObject, closestTarget.transform.position);
                    }
                }
                else
                {
                    // attack to hero
                    if (distanceHero <= unit.AttackRange)
                    {
                        CurrentTarget = closestHero;

                        // attack
                        allowedToSelectNewTargets = false;
                        MovementManager.StopAgent(gameObject);
                        MovementManager.MeleeAttack(gameObject, closestHero);
                    }
                    else
                    {
                        // move
                        MovementManager.MoveUnitTo(gameObject, closestHero.transform.position);
                    }
                }
            }
            else if (closestHero == null && closestTarget != null)
            {
                // attack to closest target
                if (distanceUnit <= unit.AttackRange)
                {
                    CurrentTarget = closestTarget;

                    // attack
                    allowedToSelectNewTargets = false;
                    MovementManager.StopAgent(gameObject);
                    MovementManager.MeleeAttack(gameObject, closestTarget);
                }
                else
                {
                    // move
                    MovementManager.MoveUnitTo(gameObject, closestTarget.transform.position);
                }
            }
            else if (closestHero != null && closestTarget == null)
            {
                // attack to closest hero
                if (distanceHero <= unit.AttackRange)
                {
                    CurrentTarget = closestHero;

                    // attack
                    allowedToSelectNewTargets = false;
                    MovementManager.StopAgent(gameObject);
                    MovementManager.MeleeAttack(gameObject, closestHero);
                }
                else
                {
                    // move
                    MovementManager.MoveUnitTo(gameObject, closestHero.transform.position);
                }
            }
        }
    }
}
