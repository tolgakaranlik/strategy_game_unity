using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

/// <summary>
///
///  Author: Tolga K, 07/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
/// This class intended to hold data for basic game units
/// 
/// </summary>

public class Unit : MonoBehaviour
{
    public enum UnitFireType { Melee, Ranged };

	public string Name;
	public string AvatarFile;

	public int NumUnits;
	public int HitPoints;
	public int Strength;
	public int DamageMin;
	public int DamageMax;
	public int RemaininigLife;
	public float MoveSpeed;
	public float AttackSpeed;
    public float AttackRange = 1;
    public float AttackRangeOuter = 2;
    public UnitFireType FireType = UnitFireType.Melee;

    public int Armor;
	public float Luck;

	[HideInInspector]
	public bool Dead = false;
	[HideInInspector]
	public bool GoingForAttack = false;
    [HideInInspector]
    public bool CanFire = true;
    [HideInInspector]
    public bool CastingSpell;
    [HideInInspector]
	public bool CanMove = true;

	public Capability[] Capabilities;

    // General purpose artefacts (such as arrows, projectiles, etc.)
    public GameObject[] Artefacts;

    Animator anim;
    GameObject stun;
    float oldAnimSpeed = 1;

    protected void Init()
    {
        CastingSpell = false;
        anim = GetComponent<Animator>();
        if (GetComponent<Hero>() == null)
        {
            var text = transform.Find("Canvas/ImgSliderBG/TxtNumUnits");

            if (text != null)
            {
                text.GetComponent<Text>().text = NumUnits.ToString();
            }
        }
    }
    private void Start()
    {
        Init();
	}

	public void Fall()
    {
		anim.CrossFade("Fall", 0.1f);

		CanMove = false;
    }

    public void Slow()
    {
        oldAnimSpeed = anim.GetFloat("animSpeed");
        anim.SetFloat("animSpeed", oldAnimSpeed / 2f);
    }

    public void Stun()
    {
        anim.CrossFade("Idle", 0.1f);

        CanMove = false;
        stun = Instantiate(Artefacts[1], gameObject.transform.position + Vector3.up * 12, Quaternion.identity, gameObject.transform);
        stun.transform.Rotate(new Vector3(-90, 0, 0));
        stun.transform.localScale = Vector3.one * 0.75f;
    }

    public void GetUpAfter(float recoveryTime)
    {
		StartCoroutine(StartRecoveryAfter(recoveryTime));
	}

    public void RestoreAfter(float recoveryTime)
    {
        StartCoroutine(StartRestoreAfter(recoveryTime));
    }

    public void TakeDamage(int damage, Unit attacker)
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(TakeDamageProcess(damage, attacker));
        }
    }

	IEnumerator TakeDamageProcess(int damage, Unit attacker)
    {
        int f_damage = damage;

        while (damage > 0)
        {
            if (RemaininigLife <= damage)
            {
                if (NumUnits <= 1)
                {
                    // kill the unit
                    Dead = true;
                    CanMove = false;

                    if (GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
                    {
                        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                    }

                    GetComponent<Animator>().CrossFade("Die", 0.01f);

                    try
                    {
                        GetComponents<AudioSource>()[1].Play();
                    } catch
                    {

                    }

                    if (attacker != null)
                    {
                        attacker.CanFire = true;
                        attacker.gameObject.GetComponent<BattlefieldSimpleUnit>().CurrentTarget = null;
                        attacker.gameObject.GetComponent<BattlefieldSimpleUnit>().EnableSearch();
                    }

                    gameObject.tag = "Untagged";

                    StartCoroutine(Bury(gameObject));
                    Transform canvas = transform.Find("Canvas");
                    if (canvas != null)
                    {
                        canvas.gameObject.SetActive(false);
                    }

                    // break the loop
                    RemaininigLife = 0;
                    break;
                }
                else
                {
                    NumUnits -= 1;
                    damage -= RemaininigLife;
                    RemaininigLife = HitPoints;
                }
            }
            else
            {
                RemaininigLife -= damage;
                damage = 0;
            }
        }

        // update hero's life gauge from UI
        Hero hero = gameObject.GetComponent<Hero>();
        SpellCaster caster;
        if (hero != null)
        {
            var heroButtons = GameObject.Find("/Canvas/Heroes");

            for (int i = 0; i < heroButtons.transform.childCount; i++)
            {
                caster = heroButtons.transform.GetChild(i).Find("ImgMagic1").GetComponent<SpellCaster>();

                if (caster != null && caster.Caster == hero)
                {
                    heroButtons.transform.GetChild(i).Find("ImgHealth/ImgBar").GetComponent<Image>().DOFillAmount((float)RemaininigLife / HitPoints, 0.3f);

                    if (RemaininigLife <= 0)
                    {
                        // Disable magic buttons for those who are dead
                        caster.GlobalCooldown(float.MaxValue);
                        heroButtons.transform.GetChild(i).Find("ImgMagic2").GetComponent<SpellCaster>().GlobalCooldown(float.MaxValue);
                        heroButtons.transform.GetChild(i).Find("ImgMagic3").GetComponent<SpellCaster>().GlobalCooldown(float.MaxValue);

                        StartCoroutine(DeathAnimOfDisplay(heroButtons, i));
                    }

                    break;
                }
            }
        }

        // display number of remaining units
        var text = transform.Find("Canvas/ImgSliderBG/TxtNumUnits");
        if (text != null)
        {
            var label = text.GetComponent<Text>();
            if (GetComponent<Hero>() == null)
            {
                label.text = NumUnits.ToString();
            }

            // display how much damage you have made
            var newText = Instantiate(label, label.gameObject.transform.position, label.gameObject.transform.rotation, label.transform.parent);
            newText.color = Color.red;
            newText.transform.localScale *= 1.5f;
            newText.text = f_damage.ToString();

            RectTransform anchor = newText.gameObject.GetComponent<RectTransform>();
            anchor.DOAnchorPosY(anchor.anchoredPosition.y + 2, 0.75f);

            yield return new WaitForSeconds(0.5f);

            newText.DOFade(0, 0.75f);

            Destroy(newText.gameObject, 0.75f);
        }

        // display remaining life for water elemental
        var slider = transform.Find("Canvas/ImgHealthBG/ImgSlider");
        if(slider != null)
        {
            float amnt = Mathf.Clamp(RemaininigLife / (float)HitPoints, 0f, 1f);
            slider.GetComponent<Image>().DOFillAmount(amnt, 0.2f);
        }
    }

    IEnumerator DeathAnimOfDisplay(GameObject heroButtons, int i)
    {
        heroButtons.transform.GetChild(i).Find("ImgMagic1").DOScale(0, 0.35f);
        heroButtons.transform.GetChild(i).Find("ImgMagic2").DOScale(0, 0.35f);
        heroButtons.transform.GetChild(i).Find("ImgMagic3").DOScale(0, 0.35f);
        heroButtons.transform.GetChild(i).Find("ImgPassive").DOScale(0, 0.35f);
        heroButtons.transform.GetChild(i).Find("TxtName").gameObject.SetActive(false);
        heroButtons.transform.GetChild(i).Find("ImgHealth").gameObject.SetActive(false);
        heroButtons.transform.GetChild(i).Find("Selection")?.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.35f);

        heroButtons.transform.GetChild(i).Find("ImgPortrait").GetComponent<RectTransform>().DOAnchorPosX(310f, 0.35f);
        heroButtons.transform.GetChild(i).Find("ImgPortrait").GetComponent<RectTransform>().DOScale(Vector3.one * 0.85f, 0.35f);
        heroButtons.transform.GetChild(i).Find("ImgPortrait").GetComponent<Button>().interactable = false;

    }

    IEnumerator Bury(GameObject target)
    {
        yield return new WaitForSeconds(5);

        target.transform.DOLocalMoveY(2, 3);

        Destroy(target.gameObject, 3);
    }
    IEnumerator StartRecoveryAfter(float duration)
    {
		yield return new WaitForSeconds(duration);

		anim.CrossFade("GetUp", 0.1f);

        yield return new WaitForSeconds(1);

        CanMove = true;
    }

    IEnumerator StartRestoreAfter(float duration)
    {
        yield return new WaitForSeconds(duration);

        CanMove = true;
        anim.SetFloat("animSpeed", oldAnimSpeed);

        if (stun != null)
        {
            stun.transform.DOScale(0, 0.35f);
            Destroy(stun, 0.4f);
        }
    }

    private void OnDrawGizmos()
    {
        // inner attack range
        Vector3 point;
        Gizmos.color = Color.yellow;
        Vector3 previousPoint = transform.position + Quaternion.Euler(0, 0, 0) * Vector3.forward * AttackRange;
        for (int i = 1; i <= 36; i++)
        {
            point = transform.position + Quaternion.Euler(0, i * 10, 0) * Vector3.forward * AttackRange;
            Gizmos.DrawLine(previousPoint, point);

            previousPoint = point;
        }

        Gizmos.color = Color.red;
        previousPoint = transform.position + Quaternion.Euler(0, 0, 0) * Vector3.forward * AttackRangeOuter;
        for (int i = 1; i <= 36; i++)
        {
            point = transform.position + Quaternion.Euler(0, i * 10, 0) * Vector3.forward * AttackRangeOuter;
            Gizmos.DrawLine(previousPoint, point);

            previousPoint = point;
        }
    }
}