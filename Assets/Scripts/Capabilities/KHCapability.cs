using UnityEngine;

public abstract class KHCapability : ScriptableObject
{
	private string capabilitName;
	private string avatarFile;
	private int coolDown = 0;
	private bool autoCast = false;

	public delegate void CastHandler();
	public delegate void EnableHandler(bool enabled);
	
	public CastHandler OnCast = null;
	public EnableHandler OnEnabled = null;
	
	public string Name
	{
		get
		{
			return capabilitName;
		}
	}
	
	public int CoolDown
	{
		get
		{
			return coolDown;
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
	
	public KHCapability(string name, string avatarFile, int coolDown)
	{
		this.capabilitName = name;
		this.avatarFile = avatarFile;
		this.coolDown = coolDown;
	}
	
	public abstract void Cast();
}