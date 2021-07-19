using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
	private string itemName;
	private string itemAvatar;
	private string itemDescription;
	private bool consumable = false;
	private bool cannotSell = false;
	private int sellValue = 1;
	
	public string Name
	{
		get
		{
			return itemName;
		}
	}
    
	public string Avatar
	{
		get
		{
			return itemAvatar;
		}
	}
    
	public string Description
	{
		get
		{
			return itemDescription;
		}
	}
    
	public bool Consumable
	{
		get
		{
			return consumable;
		}
		set
		{
			consumable = value;
		}
	}
    
	public int SellValue
	{
		get
		{
			return sellValue;
		}
		set
		{
			sellValue = value;
		}
	}
	
	public bool CannotSell
	{
		get
		{
			return cannotSell;
		}
		set
		{
			cannotSell = value;
		}
	}
	
	public InventoryItem(string itemName, string itemAvatar, string itemDescription)
	{
		this.itemName = itemName;
		this.itemAvatar = itemAvatar;
		this.itemDescription = itemDescription;
	}
}
