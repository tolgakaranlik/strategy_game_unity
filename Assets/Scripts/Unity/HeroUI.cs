using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeroUI : MonoBehaviour
{
    public GameObject[] HeroSections;
    public BattleFieldMovementManager BattleFieldManager;

    void Start()
    {
        UnpressAll();
    }

    void UnpressAll()
    {
        Transform selectionEffect;
        Image picture;
        Image frame;
        for (int i = 0; i < HeroSections.Length; i++)
        {
            // Disable selection effect
            selectionEffect = HeroSections[i].transform.Find("Selection");
            if(selectionEffect != null)
            {
                selectionEffect.gameObject.SetActive(false);
            }

            // Set frame color to white
            picture = HeroSections[i].transform.Find("ImgPortrait").GetComponent<Image>();
            picture.color = new Color(0.6f, 0.6f, 0.6f, 1);
            picture.GetComponent<RectTransform>().DOScale(Vector3.one * 0.85f, 0.35f);
            
            frame = HeroSections[i].transform.Find("ImgPortrait/ImgFrame").GetComponent<Image>();
            frame.color = Color.white;
        }
    }

    public void Press(int which)
    {
        UnpressAll();
        BattleFieldManager.SelectHero(which);

        // Enable selection effect
        for (int i = 0; i < HeroSections[which].transform.childCount; i++)
        {
            if(HeroSections[which].transform.GetChild(i).name == "Selection")
            {
                HeroSections[which].transform.GetChild(i).gameObject.SetActive(true);
                break;
            }
        }

        // Set frame color to white
        Image frame = HeroSections[which].transform.Find("ImgPortrait/ImgFrame").GetComponent<Image>();
        Image picture = HeroSections[which].transform.Find("ImgPortrait").GetComponent<Image>();
        picture.GetComponent<RectTransform>().DOScale(Vector3.one, 0.35f);
        picture.color = Color.white;
        frame.color = Color.yellow;
    }
}
