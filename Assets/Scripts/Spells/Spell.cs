/// <summary>
///
///  Author: Tolga K, 07/2021
///  -----------------------------------------------------------
///  Modification History:
///  -----------------------------------------------------------
///
/// This class intended to hold spell data
/// 
/// </summary>
public abstract class Spell : Capability
{
	private int manaCost;

	public int ManaCost
	{
		get
		{
			return manaCost;
		}
	}

	// OnCast
}