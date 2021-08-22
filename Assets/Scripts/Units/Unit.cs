using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public int Armor;
	public float Luck;

	[HideInInspector]
	public bool Dead = false;
	[HideInInspector]
	public bool GoingForAttack = false;
	[HideInInspector]
	public bool CanFire = true;
	[HideInInspector]
	public bool CanMove = true;

	public Capability[] Capabilities;

    // General purpose artefacts (such as arrows, projectiles, etc.)
    public GameObject[] Artefacts;

    Animator anim;
    GameObject stun;

    protected void Init()
    {
        anim = GetComponent<Animator>();
        var text = transform.Find("Canvas/ImgSliderBG/TxtNumUnits");

        if (text != null)
        {
            text.GetComponent<Text>().text = NumUnits.ToString();
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

    public void TakeDamage(int damage)
    {
		StartCoroutine(TakeDamageProcess(damage));
    }

	IEnumerator TakeDamageProcess(int damage)
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
                    GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                    GetComponent<Animator>().CrossFade("Die", 0.01f);
                    GetComponents<AudioSource>()[1].Play();

                    //target.transform.DOLocalMoveY(5.25f, 0.75f).SetEase(Ease.Linear);
                    Vector3 p = transform.localPosition;
                    p.y = 5.25f;
                    transform.DOLocalMove(p + transform.forward * 4.5f, 0.75f).SetEase(Ease.Linear);
                    gameObject.tag = "Untagged";

                    StartCoroutine(Bury(gameObject));
                    transform.Find("Canvas").gameObject.SetActive(false);

                    // break the loop
                    damage = 0;
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

        //Debug.Log("damage: " + f_damage + ", remainingLife: " + unit.RemaininigLife + ", numUnits: " + unit.NumUnits);
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
                    break;
                }
            }
        }

        // display number of remaining units
        var text = transform.Find("Canvas/ImgSliderBG/TxtNumUnits");
        if (text != null)
        {
            var label = text.GetComponent<Text>();
            label.text = NumUnits.ToString();

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

        // is target a hero?
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

		CanMove = true;
		anim.CrossFade("GetUp", 0.1f);
	}

    IEnumerator StartRestoreAfter(float duration)
    {
        yield return new WaitForSeconds(duration);

        CanMove = true;
        stun.transform.DOScale(0, 0.35f);
        Destroy(stun, 0.4f);
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