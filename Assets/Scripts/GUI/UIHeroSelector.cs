using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroSelector : MonoBehaviour
{
    public int Hero = 0;
    public HeroUI UI;

    void Start()
    {
        var button = GetComponent<Button>();

        if(button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                if(UI != null)
                {
                    UI.Press(Hero);
                }
            });
        }
    }
}
