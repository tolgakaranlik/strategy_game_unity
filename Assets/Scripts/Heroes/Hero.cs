using System;
using System.Collections.Generic;

public class Hero : Unit
{
	public enum HeroClass { Paladin, Mage, Archer, Thief, Warrior };
	public enum HeroSex { Male, Female };

	private Spell[] spells;
	private int experience;	
	private int experienceToLevel = 1000;	
	private int level;
	private HeroClass heroClass;
	private HeroSex sex;
	private int skillPoints;
	private int manaCapacity;
	private float remainingMana;

	public delegate void LevelUpHandler(int level);
	
	public LevelUpHandler OnLevelUp = null;
	
	// Inventory

	public int Experience
	{
		get
		{
			return experience;
		}
	}
	
	public int ExperienceToLevel
	{
		get
		{
			return experienceToLevel;
		}
	}

	public HeroSex Sex
	{ 
		get
        {
			return sex;
        }
	}
		
	public int Level
	{
		get
		{
			return level;
		}
	}
	
	private HeroClass Class
	{
		get
		{
			return heroClass;
		}
	}

	public Spell[] Spells
	{
		get
		{
			return spells;
		}
	}

	public int SkillPoints
	{
		get
		{
			return skillPoints;
		}
	}

	public int ManaCapacity
    {
		get
        {
			return manaCapacity;

		}
    }

	public float RemainingMana
    {
		get
        {
			return remainingMana;

		}
    }

	public Hero(string name, string avatarFile, int hitPoints, int strength, int damageMin, int damageMax, int armor, float luck, float moveSpeed, float attackSpeed, HeroClass heroClass, int experienceToLevel, HeroSex sex) : base(name, avatarFile, hitPoints, strength, damageMin, damageMax, armor, luck, moveSpeed, attackSpeed)
	{
		this.experienceToLevel = experienceToLevel;
		this.heroClass = heroClass;
		this.sex = sex;
		experience = 0;
		level = 1;

		AutoAddSpells();
	}

	public Hero(string name, string avatarFile, int hitPoints, int strength, int damageMin, int damageMax, int armor, float luck, float moveSpeed, float attackSpeed, HeroClass heroClass, int experienceToLevel, HeroSex sex, int experience, int level) : base(name, avatarFile, hitPoints, strength, damageMin, damageMax, armor, luck, moveSpeed, attackSpeed)
	{
		this.experienceToLevel = experienceToLevel;
		this.heroClass = heroClass;
		this.experience = experience;
		this.level = level;
		this.sex = sex;

		AutoAddSpells();
	}

	private void AutoAddSpells()
	{
		List<Spell> temporarySpells = new List<Spell>();
		SpellManager spellManager = SpellManager.GetInstance();
		switch (heroClass)
		{
			case HeroClass.Warrior:
				temporarySpells.Add(spellManager.Find(1001));
				temporarySpells.Add(spellManager.Find(1002));
				break;
		}


		spells = temporarySpells.ToArray();
	}

	public void AddExperience(int amount)
	{
		if(experience + amount > (level + 1) * experienceToLevel)
		{
			level += 1;			
			
			if(OnLevelUp != null)
			{
				OnLevelUp(level);
			}
		}
	}
	
	// TODO: Specify where to use skill point
	public void UseSkillPointFor()
	{
		if(skillPoints <= 0)
		{
			return;
		}
		
		skillPoints -= 1;
	}	
}