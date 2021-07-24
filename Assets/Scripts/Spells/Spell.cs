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
    public int SpellId = 1001;

    public string SpellName = "";
    public string SpellDescription = "";
    public string SpellAvatar = "";

    public string Level1Desc = "";
    public string Level2Desc = "";
    public string Level3Desc = "";
    public string Level4Desc = "";
    public string Level5Desc = "";

    private int manaCost;

	public int ManaCost
	{
		get
		{
			return manaCost;
		}
		set
        {
			manaCost = value;
        }
	}

    public override void Init()
    {
        Init(SpellId, SpellName, SpellDescription, SpellAvatar);

        SetLevelDescription(0, Level1Desc);
        SetLevelDescription(1, Level2Desc);
        SetLevelDescription(2, Level3Desc);
        SetLevelDescription(3, Level4Desc);
        SetLevelDescription(4, Level5Desc);
    }

    // OnCast
}