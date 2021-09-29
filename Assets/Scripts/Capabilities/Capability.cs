using System.Collections.Generic;
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
	private bool autoCast = false;
	private bool autoCastAvailable = false;
	private int id = 0;
	private string[] levelDescriptions;
	private GameObject caster = null;
	
	protected Vector3 targetPosition = Vector3.zero;

	public float[] CoolDown = null;
	public float[] GlobalCoolDown = null;
	public int CurrentLevel = 1;
	public int MaxLevel = 3;

	public delegate void CastHandler();
	public delegate void EnableHandler(bool enabled);
	
	public CastHandler OnCast = null;
	public EnableHandler OnEnabled = null;

	[HideInInspector]
	public Unit Target = null;

	public int Id
	{
		get
		{
			return id;
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
		CoolDown = coolDown;

		levelDescriptions = new string[MaxLevel];
	}

	public void Init(int id, string name, string description, string avatarFile)
	{
		this.id = id;
		this.capabilitName = name;
		this.description = description;
		this.avatarFile = avatarFile;

		levelDescriptions = new string[MaxLevel];
	}

	public void Init(int id, string name, string description, string avatarFile, float[] coolDown)
	{
		this.id = id;
		this.capabilitName = name;
		this.description = description;
		this.avatarFile = avatarFile;
		CoolDown = coolDown;

		levelDescriptions = new string[MaxLevel];
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

	public abstract void Cast(GameObject target);

	public abstract void Init();

	public void SetTargetPosition(Vector3 targetPosition)
    {
		this.targetPosition = targetPosition;
	}

	public void SetCaster(GameObject caster)
    {
		this.caster = caster;
    }

	public GameObject GetCaster()
    {
		return caster;
    }

	public Transform FindDeepChild(Transform aParent, string aName)
	{
		foreach (Transform child in aParent)
		{
			if (child.name == aName)
			{
				return child;
			}

			var result = FindDeepChild(child, aName);
			if (result != null)
			{
				return result;
			}

		}

		return null;
	}
}