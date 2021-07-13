using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    KHInventoryItem[] Inventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public KHHero Generate(KHHero.HeroClass heroClass, KHHero.HeroSex sex)
    {
        string[] namesMale = new string[] { "Landor", "Karakan", "Slingshot", "Lysandre", "Vinicius", "Junius", "Calussa", "Flaccus", "Horatius", "Donohuei", "Obstinante", "Ennisi", "Munghan", "Maoilios", "Yeroke" };
        string[] namesFemale = new string[] { "Starlight", "Windstorm", "Lotus", "Iridescence", "Iris", "Mystify", "Lilac", "Licorice", "Alexandrite", "Tanzanite", "Vertigo", "Venus", "Sisenna", "Mairenn", "Caoilinn" };

        string generatedName = sex == KHHero.HeroSex.Female ? namesFemale[Random.Range(0, namesFemale.Length)] : namesMale[Random.Range(0, namesMale.Length)];

        int hitPoints = 0;
        int strength = 0;
        int damageMin = 1;
        int damageMax = 2;
        int armor = 1;
        int luck = 0;
        float moveSpeed = 1;
        float attackSpeed = 1;

        switch (heroClass)
        {
            case KHHero.HeroClass.Archer:
                hitPoints = Random.Range(28, 33);
                strength = Random.Range(2, 4);
                damageMin = 2;
                damageMax = 4;
                moveSpeed = 2;
                attackSpeed = 2;
                break;
            case KHHero.HeroClass.Mage:
                hitPoints = Random.Range(22, 27);
                strength = Random.Range(1, 3);
                damageMin = 1;
                damageMax = 3;
                moveSpeed = 1.25f;
                attackSpeed = 1.5f;
                break;
            case KHHero.HeroClass.Paladin:
                hitPoints = Random.Range(33, 38);
                strength = Random.Range(5, 8);
                damageMin = 5;
                damageMax = 9;
                moveSpeed = 1.75f;
                attackSpeed = 2.5f;
                break;
            case KHHero.HeroClass.Thief:
                hitPoints = Random.Range(26, 32);
                strength = Random.Range(3, 5);
                damageMin = 2;
                damageMax = 4;
                moveSpeed = 1.5f;
                attackSpeed = 3f;
                break;
        }

        KHHero hero = new KHHero(generatedName, "", hitPoints, strength, damageMin, damageMax, armor, luck, moveSpeed, attackSpeed, heroClass, sex, 0, 1);

        return hero;
    }
}
