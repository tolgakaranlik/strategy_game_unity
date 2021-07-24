using UnityEngine;

/// <summary>
///
///  Author: Tolga K, 07/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
/// This class intended to hold capability data. Also the Spell class derives
/// from this one
/// 
/// </summary>
public abstract class Capability : MonoBehaviour
{
	private string capabilitName;
	private string description;
	private string avatarFile;
	private float[] coolDown = null;
	private float coolDownRemaining = 0;
	private bool autoCast = false;
	private bool autoCastAvailable = false;
	private int id = 0;
	private int currentLevel = 1;
	private int maxLevel = 5;
	private string[] levelDescriptions;

	public delegate void CastHandler();
	public delegate void EnableHandler(bool enabled);
	
	public CastHandler OnCast = null;
	public EnableHandler OnEnabled = null;

	public int Id
	{
		get
		{
			return id;
		}
	}

	public int CurrentLevel
	{
		get
		{
			return currentLevel;
		}
		set
        {
			currentLevel = value;
		}
	}

	public int MaxLevel
    {
        get
        {
			return maxLevel;

		}
    }

	public string Name
	{
		get
		{
			return capabilitName;
		}
	}

	public string Description
    {
		get
        {
			return description;
        }
    }
	
	public float[] CoolDown
	{
		get
		{
			return coolDown;
		}
	}

	public float CoolDownRemaining
	{
		get
		{
			return coolDownRemaining;
		}
		set
        {
			coolDownRemaining = value;
		}
	}


	public string AvatarFile
	{
		get
		{
			return avatarFile;
		}
	}
	
	public bool AutoCast
	{
		get
		{
			return autoCast;
		}
		set
		{
			autoCast = value;
			OnEnabled?.Invoke(autoCast);
		}
	}

	public bool AutoCastAvailable
	{
		get
		{
			return autoCastAvailable;
		}
		set
		{
			autoCastAvailable = value;
		}
	}

	public string[] LevelDescriptions
    {
		get
        {
			return levelDescriptions;
		}
    }

	public void Init(string name, string description, string avatarFile, float[] coolDown)
	{
		this.capabilitName = name;
		this.description = description;
		this.avatarFile = avatarFile;
		this.coolDown = coolDown;
		this.coolDownRemaining = 0;

		levelDescriptions = new string[maxLevel];
	}

	public void Init(int id, string name, string description, string avatarFile)
	{
		this.id = id;
		this.capabilitName = name;
		this.description = description;
		this.avatarFile = avatarFile;
		this.coolDownRemaining = 0;

		levelDescriptions = new string[maxLevel];
	}

	public void Init(int id, string name, string description, string avatarFile, float[] coolDown)
	{
		this.id = id;
		this.capabilitName = name;
		this.description = description;
		this.avatarFile = avatarFile;
		this.coolDown = coolDown;
		this.coolDownRemaining = 0;

		levelDescriptions = new string[maxLevel];
	}

	public void SetLevelDescription(int level, string description)
    {
		levelDescriptions[level] = description;

	}

	public void SetId(int id)
    {
		this.id = id;
    }
	
	public abstract void Cast();

	public abstract void Init();
}