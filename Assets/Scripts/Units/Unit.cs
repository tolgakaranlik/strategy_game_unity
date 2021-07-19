using UnityEngine;

public class Unit
{
	private string unitName;
	private string avatarFile;
	private int hitPoints;
	private int strength;
	private int armor;
	private int damageMin;
	private int damageMax;
	private float luck = 0.5f;
	private float moveSpeed;
	private float attackSpeed;

	private Capability[] capabilities = null;
	private int remainingLife;

	public string Name
	{
		get
		{
			return unitName;
		}
	}

	public string AvatarFile
	{
		get
		{
			return avatarFile;
		}
	}

	public int HitPoints 
	{
		get 
		{
			return hitPoints;
		}
		set
		{
			hitPoints = value;
		}
	}
	
	public int Strength
	{
		get
		{
			return strength;
		}
		set
		{
			strength = value;
		}
	}

	public int DamageMin
	{
		get
		{
			return damageMin;
		}
		set
		{
			damageMin = value;
		}
	}

	public int DamageMax
	{
		get
		{
			return damageMax;
		}
		set
		{
			damageMax = value;
		}
	}

	public int RemaininigLife
	{
		get 
		{
			return remainingLife;
		}
		set
		{
			remainingLife = value;
		}
	}

	public float MoveSpeed
	{
		get
		{
			return moveSpeed;
		}
		set
		{
			moveSpeed = value;
		}
	}

	public float AttackSpeed
	{
		get
		{
			return attackSpeed;
		}
		set
		{
			attackSpeed = value;
		}
	}

	public int Armor
	{
		get
		{
			return armor;
		}
		set
		{
			armor = value;
		}
	}

	public float Luck
	{
		get
		{
			return luck;
		}
		set
		{
			luck = value;
		}
	}

	public Capability[] Capabilities
	{
		get
		{
			return capabilities;
		}
	}
	
	public Unit(string unitName, string avatarFile, int hitPoints, int strength, int damageMin, int damageMax, int armor, float luck, float moveSpeed, float attackSpeed)
	{
		this.unitName = unitName;
		this.avatarFile = avatarFile;
		this.hitPoints = hitPoints;
		this.strength = strength;
		this.damageMin = damageMin;
		this.damageMax = damageMax;
		this.moveSpeed = moveSpeed;
		this.attackSpeed = attackSpeed;
		this.armor = armor;
		this.luck = luck;

		remainingLife = hitPoints;
	}

	// OnDead
	// OnKilled
	// OnSpell
	// OnCommand
	// OnAttacked
}