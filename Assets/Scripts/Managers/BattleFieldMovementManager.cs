using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class BattleFieldMovementManager : MonoBehaviour
{
    public GameObject[] Heroes;
    public HeroUI HeroGUI;
    public GUIBattleField BattleGUI;

    [HideInInspector]
    public bool CanMoveOnTheField = false;

    int selectedHero = -1;
    NavMeshAgent selectedHeroAgent;
    GameObject[] enemyUnits;
    GameObject[] playerArmy;
    bool battleEnded = false;

    private void Awake()
    {
        CanMoveOnTheField = false;
    }

    private void Start()
    {
        enemyUnits = GameObject.FindGameObjectsWithTag("Enemy");
        playerArmy = GameObject.FindGameObjectsWithTag("PlayerArmy");

        InvokeRepeating("TestIfBattleIsOver", 0, 1);
        //StartAnimation();
    }

    void Update()
    {
        // Touch to navigate to
        if (!battleEnded && CanMoveOnTheField && Input.GetMouseButton(0) && !IsPointerOverUIElement())
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 300, ~(1 << LayerMask.NameToLayer("Ignore Raycast"))))
            {
                if (selectedHero != -1 && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") && Heroes[selectedHero] != null && Heroes[selectedHero].GetComponent<Unit>().CanMove && !Heroes[selectedHero].GetComponent<Unit>().Dead)
                {
                    Heroes[selectedHero].GetComponent<Hero>().DisableSearch();
                    Heroes[selectedHero].GetComponent<Hero>().CanFire = true;
                    MoveUnitTo(Heroes[selectedHero], hitInfo.point);

                    return;
                } else if (hitInfo.collider.gameObject.tag == "Hero")
                {
                    for (int i = 0; i < Heroes.Length; i++)
                    {
                        if (Heroes[i] == hitInfo.collider.gameObject)
                        {
                            SelectHero(i);
                            HeroGUI.Press(i);

                            break;
                        }
                    }
                } else if (selectedHero != -1 && hitInfo.collider.gameObject.tag == "Enemy")
                {
                    // fire to
                    Hero hero = Heroes[selectedHero].GetComponent<Hero>();
                    if (hero.Class == Hero.HeroClass.Mage && hero.CanFire)
                    {
                        // stop
                        Heroes[selectedHero].GetComponent<NavMeshAgent>().isStopped = true;

                        // fire a projectile
                        GameObject target = hitInfo.collider.gameObject;
                        Heroes[selectedHero].GetComponent<BattlefieldSimpleUnit>().CurrentTarget = hitInfo.collider.gameObject;
                        MagicAttack(Heroes[selectedHero], target);
                    } else if (hero.Class == Hero.HeroClass.Archer && hero.CanFire)
                    {
                        // stop
                        Heroes[selectedHero].GetComponent<NavMeshAgent>().isStopped = true;

                        // fire an arrow
                        GameObject target = hitInfo.collider.gameObject;
                        Heroes[selectedHero].GetComponent<BattlefieldSimpleUnit>().CurrentTarget = hitInfo.collider.gameObject;
                        RangedAttack(Heroes[selectedHero], target);
                    }
                    else if (hero.Class == Hero.HeroClass.Warrior && hero.CanFire)
                    {
                        // go and hit him
                        float distance = Vector3.Distance(Heroes[selectedHero].transform.position, hitInfo.collider.gameObject.transform.position);

                        if (distance <= hero.AttackRange)
                        {
                            Heroes[selectedHero].GetComponent<BattlefieldSimpleUnit>().CurrentTarget = hitInfo.collider.gameObject;
                            MeleeAttack(Heroes[selectedHero], hitInfo.collider.gameObject);
                        } else
                        {
                            // run to him and hit when you are close enough
                            MoveUnitTo(Heroes[selectedHero], hitInfo.collider.gameObject.transform.position);
                            StartCoroutine(RunToHit(hitInfo.collider.gameObject, selectedHero));
                        }
                    }
                }
            }
        }

        // Test if hero has reached to its destination
        NavMeshAgent nma;
        for (int i = 0; i < Heroes.Length; i++)
        {
            if (Heroes[i] != null)
            {
                nma = Heroes[i].GetComponent<NavMeshAgent>();
                if (nma.velocity.magnitude >= 0.01f && nma.pathStatus == NavMeshPathStatus.PathComplete && nma.remainingDistance <= 0.05)
                {
                    StopAgent(Heroes[i]);
                }
            }
        }
    }

    public void MoveUnitTo(GameObject unit, Vector3 point)
    {
        BattlefieldSimpleUnit sunit = unit.GetComponent<BattlefieldSimpleUnit>();
        if (sunit != null)
        {
            sunit.CurrentTarget = null;
        }

        unit.GetComponent<Animator>().CrossFade("Run", 0.1f);
        unit.GetComponent<Animator>().SetBool("isRun", true);

        try
        {
            NavMeshAgent nma = unit.GetComponent<NavMeshAgent>();
            nma.isStopped = false;
            nma.speed = unit.GetComponent<Unit>().MoveSpeed;

            nma.SetDestination(point);
        } catch
        {
            // TEMPORARY - to be removed later on
        }
    }

    IEnumerator RunToHit(GameObject target, int selectedHero)
    {
        var hero = Heroes[selectedHero].GetComponent<Hero>();

        float distance = Vector3.Distance(Heroes[selectedHero].transform.position, target.transform.position);
        while (distance > hero.AttackRange)
        {
            yield return new WaitForSeconds(0.05f);

            if (target == null)
            {
                // target is probably dead
                yield break;
            }

            distance = Vector3.Distance(Heroes[selectedHero].transform.position, target.transform.position);
        }

        Heroes[selectedHero].GetComponent<NavMeshAgent>().isStopped = true;
        StopAgent(Heroes[selectedHero]);

        yield return new WaitForSeconds(0.1f);

        Heroes[selectedHero].GetComponent<BattlefieldSimpleUnit>().CurrentTarget = target;
        MeleeAttack(Heroes[selectedHero], target);
    }

    public void MagicAttack(GameObject unit, GameObject target)
    {
        Unit playerUnit = unit.GetComponent<Unit>();
        if (playerUnit.CanFire && target != null)
        {
            Unit targetUnit = target.GetComponent<Unit>();
            StartCoroutine(MageProjectileLoop(playerUnit, targetUnit));
        }
    }

    public void RangedAttack(GameObject unit, GameObject target)
    {
        Unit playerUnit = unit.GetComponent<Unit>();
        if (playerUnit.CanFire && target != null)
        {
            Unit targetUnit = target.GetComponent<Unit>();
            StartCoroutine(RangedAttackLoop(playerUnit, targetUnit));
        }
    }

    public void MeleeAttack(GameObject unit, GameObject target)
    {
        Unit playerUnit = unit.GetComponent<Unit>();
        if (playerUnit.CanFire && target != null)
        {
            Unit targetUnit = target.GetComponent<Unit>();
            StartCoroutine(MeleeAttackLoop(playerUnit, targetUnit));
        }
    }

    IEnumerator RangedAttackLoop(Unit playerUnit, Unit targetUnit)
    {
        GameObject unit = playerUnit.gameObject;
        GameObject target = targetUnit.gameObject;
        BattlefieldSimpleUnit bfunit = unit.GetComponent<BattlefieldSimpleUnit>();

        Animator anim = unit.GetComponent<Animator>();
        while (!playerUnit.Dead && playerUnit.CanMove && !targetUnit.Dead && target == bfunit.CurrentTarget && !playerUnit.CastingSpell)
        {
            anim.Play("Ranged", -1, 0);
            playerUnit.CanFire = false;

            unit.transform.DOLookAt(target.transform.position, 0.15f);
            StartCoroutine(TakeDamage(target, unit));

            yield return new WaitForSeconds(0.75f);

            // fire projectile
            var projectile = Instantiate(playerUnit.Artefacts[0], playerUnit.transform.position + Vector3.up * 7f, Quaternion.identity);
            projectile.SetActive(true);
            projectile.transform.LookAt(target.transform.position + Vector3.up * 7f);
            projectile.transform.Rotate(new Vector3(-90, 0, 0));
            float distance = Vector3.Distance(target.transform.position, playerUnit.transform.position);
            float projetcileSpeed = 200f;
            projectile.transform.DOMove(target.transform.position + Vector3.up * 7f, distance / projetcileSpeed).SetEase(Ease.Linear);
            StartCoroutine(ExplodeAt(target.transform.position + Vector3.up * 7f, playerUnit, distance / projetcileSpeed, projectile, target, Vector3.one / 1.5f));

            yield return new WaitForSeconds(1f / playerUnit.AttackSpeed - 0.55f);
        }
    }

    IEnumerator MeleeAttackLoop(Unit playerUnit, Unit targetUnit)
    {
        GameObject unit = playerUnit.gameObject;
        GameObject target = targetUnit.gameObject;
        BattlefieldSimpleUnit bfunit = unit.GetComponent<BattlefieldSimpleUnit>();

        float distance = Vector3.Distance(unit.transform.position, target.transform.position);
        Animator anim = unit.GetComponent<Animator>();
        while (!playerUnit.Dead && playerUnit.CanMove && !targetUnit.Dead && target == bfunit.CurrentTarget && !playerUnit.CastingSpell)
        {
            try
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    anim.CrossFade("Attack1", 0.1f);
                }
                else
                {
                    anim.CrossFade("Attack2", 0.1f);
                }

                playerUnit.CanFire = false;

                unit.transform.DOLookAt(target.transform.position, 0.15f);
                StartCoroutine(TakeDamage(target, unit));
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            yield return new WaitForSeconds(1f / playerUnit.AttackSpeed + 0.2f);
        }

        if (!playerUnit.Dead && targetUnit.Dead)
        {
            playerUnit.CanFire = true;
            bfunit.CurrentTarget = null;
            bfunit.EnableSearch();
        }
    }

    IEnumerator MageProjectileLoop(Unit playerUnit, Unit targetUnit)
    {
        GameObject unit = playerUnit.gameObject;
        GameObject target = targetUnit.gameObject;
        BattlefieldSimpleUnit bfunit = unit.GetComponent<BattlefieldSimpleUnit>();

        Animator anim = unit.GetComponent<Animator>();
        while (!playerUnit.Dead && playerUnit.CanMove && !targetUnit.Dead && target == bfunit.CurrentTarget && !playerUnit.CastingSpell)
        {
            playerUnit.CanFire = false;

            // face the enemy
            unit.transform.DOLookAt(target.transform.position, 0.15f);

            yield return new WaitForSeconds(0.15f);

            anim.CrossFade("Magic2Fast", 0.1f);

            var projectile = Instantiate(unit.GetComponent<Hero>().Artefacts[0], unit.transform.position + Vector3.up * 7f, Quaternion.identity);
            float distance = Vector3.Distance(target.transform.position, unit.transform.position);
            float projetcileSpeed = 100f;
            projectile.transform.DOMove(target.transform.position + Vector3.up * 7f, distance / projetcileSpeed).SetEase(Ease.Linear);
            StartCoroutine(ExplodeAt(target.transform.position + Vector3.up * 7f, playerUnit, distance / projetcileSpeed, projectile, target, Vector3.one / 1.5f));

            yield return new WaitForSeconds(1f / unit.GetComponent<Hero>().AttackSpeed);
        }

        if (!playerUnit.Dead && targetUnit.Dead)
        {
            playerUnit.CanFire = true;
            bfunit.CurrentTarget = null;
            bfunit.EnableSearch();
        }
    }

    IEnumerator TakeDamage(GameObject target, GameObject unit)
    {
        yield return new WaitForSeconds(0.5f);

        unit.GetComponents<AudioSource>()[0].Play();

        yield return new WaitForSeconds(0.25f);

        Unit selectedUnit = unit.GetComponent<Unit>();

        // instantiate effect
        var explosion = Instantiate(selectedUnit.Artefacts[0], target.transform.position + Vector3.up * 5, Quaternion.identity);
        explosion.transform.localScale = selectedUnit.gameObject.transform.localScale / 3f;
        Destroy(explosion.gameObject, 1);

        StartCoroutine(TargetTakeDamage(target, selectedUnit, true));
    }

    IEnumerator ExplodeAt(Vector3 position, Unit unit, float duration, GameObject projectile, GameObject target, Vector3 scale)
    {
        yield return new WaitForSeconds(duration);

        var explosion = Instantiate(unit.Artefacts[1], position, Quaternion.identity);
        explosion.transform.localScale = scale;
        Destroy(explosion.gameObject, 1);
        Destroy(projectile.gameObject, 1);

        StartCoroutine(TargetTakeDamage(target, unit, false));
    }

    IEnumerator TargetTakeDamage(GameObject target, Unit attacker, bool releaseFire)
    {
        Unit unit = target.GetComponent<Unit>();
        if (unit != null && !unit.Dead)
        {
            int damage = UnityEngine.Random.Range(attacker.DamageMin, attacker.DamageMax + 1);
            int f_damage = attacker.NumUnits * Mathf.Max(damage + attacker.Strength - unit.Armor, 1);
            damage = f_damage;

            unit.TakeDamage(damage, attacker);

            yield return new WaitForSeconds(0.5f);

            if (releaseFire)
            {
                attacker.CanFire = true;
            }
        }
    }

    public GameObject SelectWeakestTargetFromTag(string tag, Vector3 position, out int health)
    {
        health = 0;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
        int[] healths = new int[enemies.Length];
        int lowestHealth = int.MaxValue;
        int lowestIndex = -1;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<Unit>().RemaininigLife < lowestHealth)
            {
                lowestHealth = enemies[i].GetComponent<Unit>().RemaininigLife;
                lowestIndex = i;
            }
        }

        if (lowestIndex != -1)
        {
            health = lowestHealth;
            return enemies[lowestIndex];
        }

        return null;
    }

    public GameObject SelectClosestTargetFromTag(string tag, Vector3 position, out float distance)
    {
        distance = 0;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
        float[] distances = new float[enemies.Length];
        float longestDistance = float.MaxValue;
        int longestIndex = -1;
        float dist;

        for (int i = 0; i < enemies.Length; i++)
        {
            dist = Vector3.Distance(position, enemies[i].transform.position);

            if (dist < longestDistance)
            {
                longestDistance = dist;
                longestIndex = i;
            }
        }

        if (longestIndex != -1)
        {
            distance = longestDistance;
            return enemies[longestIndex];
        }

        return null;
    }

    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }

        return false;
    }
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);

        return raysastResults;
    }

    public void StopAgent(GameObject unit)
    {
        if (unit != null && !unit.IsDestroyed() && !unit.GetComponent<Unit>().Dead)
        {
            unit.GetComponent<Animator>().SetBool("isRun", false);
            unit.GetComponent<Animator>().CrossFade("Idle", 0.1f);
            unit.GetComponent<NavMeshAgent>().isStopped = true;
            unit.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        }
    }

    public void SelectHero(int which)
    {
        if(Heroes[which] == null || Heroes[which].IsDestroyed() || Heroes[which].GetComponent<Unit>().Dead)
        {
            return;
        }

        selectedHero = which;
        selectedHeroAgent = Heroes[selectedHero].GetComponent<NavMeshAgent>();
    }

    private void TestIfBattleIsOver()
    {
        // player won?
        if (battleEnded)
        {
            return;
        }

        bool allUnitsDead = true;

        for (int i = 0; i < enemyUnits.Length; i++)
        {
            if (enemyUnits[i] != null && !enemyUnits[i].GetComponent<Unit>().Dead)
            {
                allUnitsDead = false;
                break;
            }
        }

        if (allUnitsDead)
        {
            Victory();
            return;
        }

        // computer won?
        allUnitsDead = true;

        for (int i = 0; i < Heroes.Length; i++)
        {
            if (Heroes[i] != null && !Heroes[i].GetComponent<Unit>().Dead)
            {
                allUnitsDead = false;
                break;
            }
        }

        if(allUnitsDead)
        {
            for (int i = 0; i < playerArmy.Length; i++)
            {
                if (playerArmy[i] != null && !playerArmy[i].GetComponent<Unit>().Dead)
                {
                    allUnitsDead = false;
                    break;
                }
            }

            if(allUnitsDead)
            {
                Defeat();
            }
        }
    }

    void Victory()
    {
        for (int i = 0; i < Heroes.Length; i++)
        {
            if (Heroes[i] != null && !Heroes[i].GetComponent<Unit>().Dead)
            {
                Heroes[i].GetComponent<Animator>().CrossFade("Win" + UnityEngine.Random.Range(1, 4), 0.1f);
            }
        }

        for (int i = 0; i < playerArmy.Length; i++)
        {
            if (playerArmy[i] != null && !playerArmy[i].GetComponent<Unit>().Dead)
            {
                playerArmy[i].GetComponent<Animator>().CrossFade("Win" + UnityEngine.Random.Range(1, 4), 0.1f);
            }
        }

        battleEnded = true;
        StartAnimation();
        BattleGUI.ShowVictoryWindow();
    }

    void Defeat()
    {
        for (int i = 0; i < enemyUnits.Length; i++)
        {
            if (enemyUnits[i] != null && !enemyUnits[i].GetComponent<Unit>().Dead)
            {
                enemyUnits[i].GetComponent<Animator>().CrossFade("Win" + UnityEngine.Random.Range(1, 4), 0.1f);
            }
        }

        battleEnded = true;
        StartAnimation();
        BattleGUI.ShowDefeatWindow();
    }

    void StartAnimation()
    {
        Camera.main.transform.SetParent(transform.Find("/CameraOrbiter"));
        Camera.main.transform.position += Camera.main.transform.forward * 20;
        InvokeRepeating("CameraOrbit", 0, 1);
    }

    void CameraOrbit()
    {
        Transform orbiter = transform.Find("/CameraOrbiter");

        orbiter.DOLocalRotate(new Vector3(0, orbiter.localRotation.eulerAngles.y + 10, 0), 1f).SetEase(Ease.Linear);
    }

    public void BackToWorldMap(int fromWhich)
    {
        if(fromWhich == 0)
        {
            PlayerPrefs.SetFloat("LastPlaceOnLandX", 107);
            PlayerPrefs.SetFloat("LastPlaceOnLandY", 10);
            PlayerPrefs.SetFloat("LastPlaceOnLandZ", 110);
        }
        else
        {
            PlayerPrefs.SetFloat("LastPlaceOnLandX", 423);
            PlayerPrefs.SetFloat("LastPlaceOnLandY", 10);
            PlayerPrefs.SetFloat("LastPlaceOnLandZ", 151);
        }

        SceneManager.LoadScene("TemporaryLand");
    }

    public void SetCanMove(bool canMove)
    {
        CanMoveOnTheField = canMove;
    }
}
