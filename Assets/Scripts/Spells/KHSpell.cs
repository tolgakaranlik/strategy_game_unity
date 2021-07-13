public abstract class KHSpell : KHCapability
{
	private int manaCost;

	public int ManaCost
	{
		get
		{
			return manaCost;
		}
	}
	
	public KHSpell(string name, string avatarFile, int coolDown) : base(name, avatarFile, coolDown)
    {

    }

	// OnCast
}