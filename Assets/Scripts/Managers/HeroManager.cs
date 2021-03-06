using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    InventoryItem[] Inventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Hero Generate(Hero.HeroClass heroClass, Hero.HeroSex sex)
    {
        string[] namesMale = new string[] { "Landor", "Karakan", "Slingshot", "Lysandre", "Vinicius", "Junius", "Calussa", "Flaccus", "Horatius", "Donohuei", "Obstinante", "Ennisi", "Munghan", "Maoilios", "Yeroke" };
        string[] namesFemale = new string[] { "Starlight", "Windstorm", "Lotus", "Iridescence", "Iris", "Mystify", "Lilac", "Licorice", "Alexandrite", "Tanzanite", "Vertigo", "Venus", "Sisenna", "Mairenn", "Caoilinn" };

        string generatedName = sex == Hero.HeroSex.Female ? namesFemale[Random.Range(0, namesFemale.Length)] : namesMale[Random.Range(0, namesMale.Length)];

        int hitPoints = 0;
        int strength = 0;
        int damageMin = 1;
        int damageMax = 2;
        int armor = 1;
        int luck = 0;
        float moveSpeed = 1;
        float attackSpeed = 1;
        int experienceToLevel = 1000;

        switch (heroClass)
        {
            case Hero.HeroClass.Archer:
                hitPoints = Random.Range(28, 33);
                strength = Random.Range(2, 4);
                damageMin = 2;
                damageMax = 4;
                moveSpeed = 2;
                attackSpeed = 2;
                break;
            case Hero.HeroClass.Mage:
                hitPoints = Random.Range(22, 27);
                strength = Random.Range(1, 3);
                damageMin = 1;
                damageMax = 3;
                moveSpeed = 1.25f;
                attackSpeed = 1.5f;
                break;
            case Hero.HeroClass.Warrior:
                hitPoints = Random.Range(37, 41);
                strength = Random.Range(6, 9);
                damageMin = 6;
                damageMax = 10;
                moveSpeed = 1.85f;
                attackSpeed = 2.25f;
                break;
            case Hero.HeroClass.Priest:
                hitPoints = Random.Range(33, 38);
                strength = Random.Range(5, 8);
                damageMin = 5;
                damageMax = 9;
                moveSpeed = 1.75f;
                attackSpeed = 2.5f;
                break;
            case Hero.HeroClass.Thief:
                hitPoints = Random.Range(26, 32);
                strength = Random.Range(3, 5);
                damageMin = 2;
                damageMax = 4;
                moveSpeed = 1.5f;
                attackSpeed = 3f;
                break;
        }

        //Hero hero = new Hero(generatedName, "", hitPoints, strength, damageMin, damageMax, armor, luck, moveSpeed, attackSpeed, heroClass, experienceToLevel, sex, 0, 1);

        return null;
    }
}
