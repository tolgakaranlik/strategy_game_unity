using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SpellCaster : MonoBehaviour
{
    public bool Preemptive = false;
    public Hero Caster;
    public int SpellId = 1001;
    public GUIBattleField GUI;

    Button button;
    RectTransform rect;
    Image black;
    Image group;
    float cooldown = 30;
    float globalCooldown = 30;
    Spell.SpellTarget SpellType = Spell.SpellTarget.Random;

    void Start()
    {
        button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
        group = transform.Find("ImgSpell").GetComponent<Image>();
        black = transform.Find("ImgBlack").GetComponent<Image>();

        button.onClick.AddListener(() =>
        {
            Spell spell = Caster.GetSpellInfo(SpellId);
            SpellType = spell.TargetType;

            if (spell.TargetType == Spell.SpellTarget.Random)
            {
                Cast(cooldown);
                Caster.CastSpell(SpellId, out cooldown, out globalCooldown);
                StartCoroutine(SetGlobalCooldown(globalCooldown));
            } else if(spell.TargetType == Spell.SpellTarget.SelectedPoint)
            {
                GUI.DisplaySpellIndicator("tap on the battle field to cast your spell", () =>
                {
                    Caster.CastSpell(SpellId, out cooldown, out globalCooldown);
                    StartCoroutine(SetGlobalCooldown(globalCooldown));

                    GUI.CancelSpellHandler = () =>
                    {
                        SpellType = Spell.SpellTarget.Random;
                    };
                });
            }
            else if (spell.TargetType == Spell.SpellTarget.SelectedPlayer)
            {
                GUI.DisplaySpellIndicator("tap on a friendly unit to cast your spell", () =>
                {

                    GUI.CancelSpellHandler = () =>
                    {
                        SpellType = Spell.SpellTarget.Random;
                    };
                });
            }
            else if (spell.TargetType == Spell.SpellTarget.SelectedEnemy)
            {
                GUI.DisplaySpellIndicator("tap on an enemy unit to cast your spell", () =>
                {

                    GUI.CancelSpellHandler = () =>
                    {
                        SpellType = Spell.SpellTarget.Random;
                    };
                });
            }
            else if (spell.TargetType == Spell.SpellTarget.SelectedPoint)
            {
                GUI.DisplaySpellIndicator("tap on a unit to cast your spell", () =>
                {

                    GUI.CancelSpellHandler = () =>
                    {
                        SpellType = Spell.SpellTarget.Random;
                    };
                });
            }
        });
    }

    private void Update()
    {
        if(SpellType == Spell.SpellTarget.SelectedPoint && Input.GetMouseButtonUp(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 300, ~(1 << LayerMask.NameToLayer("Ignore Raycast"))))
            {
                if(!BattleFieldMovementManager.IsPointerOverUIElement())
                {
                    bool found = false;
                    Vector3 targetPoint = Vector3.zero;

                    if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                    {
                        found = true;
                        targetPoint = hitInfo.point;
                    } else if(hitInfo.collider.gameObject.tag == "Enemy" || hitInfo.collider.gameObject.tag == "PlayerArmy")
                    {
                        found = true;
                        targetPoint = hitInfo.collider.gameObject.transform.position;
                    }

                    if (found)
                    {
                        Cast(cooldown);
                        Caster.SetTargetPosition(targetPoint + Vector3.up * 0.25f);
                        Caster.CastSpell(SpellId, out cooldown, out globalCooldown);
                        StartCoroutine(SetGlobalCooldown(globalCooldown));

                        SpellType = Spell.SpellTarget.Random;
                        GUI.CancelSpell();
                    }
                }
            }
        }
    }

    public void GlobalCooldown(float duration)
    {
        StartCoroutine(SetGlobalCooldown(duration));
    }

    public void Cast(float cooldown)
    {
        button.interactable = false;
        black.DOFade(0, 0);
        black.DOFade(0.9f, 0.35f);
        rect.DOScale(0.85f, 0.35f);
        group.DOFade(0.45f, 0.35f);
        black.fillAmount = 1;
        black.DOFillAmount(0, cooldown).SetEase(Ease.Linear);

        StartCoroutine(ClearAnim());
    }

    IEnumerator ClearAnim()
    {
        yield return new WaitForSeconds(cooldown);

        if (!Caster.Dead)
        {
            button.interactable = true;
            rect.DOScale(1f, 0.35f);
            group.DOFade(1f, 0.35f);
        }
    }

    IEnumerator SetGlobalCooldown(float duration)
    {
        var parent = transform.parent;

        var magic1 = parent.Find("ImgMagic1");
        var magic2 = parent.Find("ImgMagic2");
        var magic3 = parent.Find("ImgMagic3");

        bool canMagic1 = false;
        bool canMagic2 = false;
        bool canMagic3 = false;

        if (magic1.gameObject != gameObject)
        {
            if (magic1.GetComponent<RectTransform>().localScale.x == 1)
            {
                magic1.transform.Find("ImgBlack").GetComponent<Image>().fillAmount = 1;
                magic1.transform.Find("ImgBlack").GetComponent<Image>().DOFade(0.9f, 0.35f);
                magic1.GetComponent<RectTransform>().DOScale(0.85f, 0.35f);
                magic1.GetComponent<Button>().interactable = false;

                canMagic1 = true;
            }
        }

        if (magic2.gameObject != gameObject)
        {
            if (magic2.GetComponent<RectTransform>().localScale.x == 1)
            {
                magic2.transform.Find("ImgBlack").GetComponent<Image>().fillAmount = 1;
                magic2.transform.Find("ImgBlack").GetComponent<Image>().DOFade(0.9f, 0.35f);
                magic2.GetComponent<RectTransform>().DOScale(0.85f, 0.35f);
                magic2.GetComponent<Button>().interactable = false;

                canMagic2 = true;
            }
        }

        if (magic3.gameObject != gameObject)
        {
            if (magic3.GetComponent<RectTransform>().localScale.x == 1)
            {
                magic3.transform.Find("ImgBlack").GetComponent<Image>().fillAmount = 1;
                magic3.transform.Find("ImgBlack").GetComponent<Image>().DOFade(0.9f, 0.35f);
                magic3.GetComponent<RectTransform>().DOScale(0.85f, 0.35f);
                magic3.GetComponent<Button>().interactable = false;

                canMagic3 = true;
            }
        }

        yield return new WaitForSeconds(Mathf.Max(0, duration - 0.7f));

        if (magic1.gameObject != gameObject && canMagic1 && !Caster.Dead)
        {
            magic1.transform.Find("ImgBlack").GetComponent<Image>().DOFade(0f, 0.35f);
            magic1.GetComponent<RectTransform>().DOScale(1.0f, 0.35f);
        }

        if (magic2.gameObject != gameObject && canMagic2 && !Caster.Dead)
        {
            magic2.transform.Find("ImgBlack").GetComponent<Image>().DOFade(0f, 0.35f);
            magic2.GetComponent<RectTransform>().DOScale(1.0f, 0.35f);
        }

        if (magic3.gameObject != gameObject && canMagic3 && !Caster.Dead)
        {
            magic3.transform.Find("ImgBlack").GetComponent<Image>().DOFade(0f, 0.35f);
            magic3.GetComponent<RectTransform>().DOScale(1.0f, 0.35f);
        }

        yield return new WaitForSeconds(0.35f);

        if (magic1.gameObject != gameObject && canMagic1 && !Caster.Dead)
        {
            magic1.transform.Find("ImgBlack").GetComponent<Image>().DOFade(1f, 0f);
            magic1.transform.Find("ImgBlack").GetComponent<Image>().fillAmount = 0;
            magic1.GetComponent<Button>().interactable = true;
        }

        if (magic2.gameObject != gameObject && canMagic2 && !Caster.Dead)
        {
            magic2.transform.Find("ImgBlack").GetComponent<Image>().DOFade(1f, 0f);
            magic2.transform.Find("ImgBlack").GetComponent<Image>().fillAmount = 0;
            magic2.GetComponent<Button>().interactable = true;
        }

        if (magic3.gameObject != gameObject && canMagic3 && !Caster.Dead)
        {
            magic3.transform.Find("ImgBlack").GetComponent<Image>().DOFade(1f, 0f);
            magic3.transform.Find("ImgBlack").GetComponent<Image>().fillAmount = 0;
            magic3.GetComponent<Button>().interactable = true;
        }
    }
}
