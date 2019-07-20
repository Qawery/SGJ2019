namespace SGJ2019
{
	public abstract class Action
	{
		public abstract string Name { get; }
		public virtual string Description => "Cost: " + Cost.ToString();
		public abstract int Cost { get; }


		public abstract bool IsActionPossible(CardSlot source, CardSlot target);
		public abstract void ExecuteAction(CardSlot source, CardSlot target);
	}
}