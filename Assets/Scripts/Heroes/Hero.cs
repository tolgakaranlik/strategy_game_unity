using System;
using System.Collections.Generic;
using UnityEngine;

public class Hero : BattlefieldSimpleUnit
{
	public enum HeroClass { Warrior, Mage, Archer, Thief, Priest };
	public enum HeroSex { Male, Female };

	public delegate void LevelUpHandler(int level);

	public LevelUpHandler OnLevelUp = null;
	public Spell[] Spells;

	// Inventory

	public int Experience;
	public int ExperienceToLevel;
	public HeroSex Sex;
	public int Level;
	public HeroClass Class;
	public int SkillPoints;

    private void Start()
    {
		AutoAddSpells();

		base.Init();
	}

    private void AutoAddSpells()
	{
		List<Spell> temporarySpells = new List<Spell>();
		switch (Class)
		{
			case HeroClass.Warrior:
				temporarySpells.Add(SpellMgr.Find(1001));
				temporarySpells.Add(SpellMgr.Find(1102));
				temporarySpells.Add(SpellMgr.Find(1006));
				break;
			case HeroClass.Mage:
				temporarySpells.Add(SpellMgr.Find(2002));
				temporarySpells.Add(SpellMgr.Find(2011));
				temporarySpells.Add(SpellMgr.Find(2023));
				break;
		}

		Spells = temporarySpells.ToArray();
	}

	public void AddExperience(int amount)
	{
		if(Experience + amount > (Level + 1) * ExperienceToLevel)
		{
			Level += 1;			
			
			if(OnLevelUp != null)
			{
				OnLevelUp(Level);
			}
		}
	}
	
	// TODO: Specify where to use skill point
	public void UseSkillPointFor()
	{
		if(SkillPoints <= 0)
		{
			return;
		}
		
		SkillPoints -= 1;
	}	

	public void CastSpell(int id, out float cooldown, out float globalCooldown)
    {
        for (int i = 0; i < Spells.Length; i++)
        {
			if(Spells[i].SpellId == id && !gameObject.IsDestroyed())
            {
				Spells[i].SetCaster(gameObject);
				Spells[i].Cast();

				cooldown = Spells[i].CoolDown.Length <= 0 ? 0 : Spells[i].CoolDown[Spells[i].CurrentLevel - 1];
				globalCooldown = Spells[i].GlobalCoolDown.Length <= Spells[i].CurrentLevel - 1 ? 0 : Spells[i].GlobalCoolDown[Spells[i].CurrentLevel - 1];

				return;
			}
        }

		cooldown = 0;
		globalCooldown = 0;
	}
}