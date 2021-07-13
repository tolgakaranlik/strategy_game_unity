public class KHHero : KHUnit
{
	public enum HeroClass { Paladin, Mage, Archer, Thief };
	public enum HeroSex { Male, Female };

	private KHSpell[] spells;
	private int experience;	
	private int experienceToLevel;	
	private int level;
	private HeroClass heroClass;
	private HeroSex sex;
	private int skillPoints;
	
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

	public KHSpell[] Spells
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
	
	public KHHero(string name, string avatarFile, int hitPoints, int strength, int damageMin, int damageMax, int armor, float luck, float moveSpeed, float attackSpeed, HeroClass heroClass, HeroSex sex) : base(name, avatarFile, hitPoints, strength, damageMin, damageMax, armor, luck, moveSpeed, attackSpeed)
	{
		this.heroClass = heroClass;
		this.sex = sex;
		experience = 0;
		level = 1;
	}
	
	public KHHero(string name, string avatarFile, int hitPoints, int strength, int damageMin, int damageMax, int armor, float luck, float moveSpeed, float attackSpeed, HeroClass heroClass, HeroSex sex, int experience, int level) : base(name, avatarFile, hitPoints, strength, damageMin, damageMax, armor, luck, moveSpeed, attackSpeed)
	{
		this.heroClass = heroClass;
		this.experience = experience;
		this.level = level;
		this.sex = sex;
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