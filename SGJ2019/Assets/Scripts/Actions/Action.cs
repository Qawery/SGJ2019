using UnityEngine;


namespace SGJ2019
{
	public abstract class Action
	{
		public abstract string Name { get; }
		public abstract string Description { get; }


		public abstract void ExecuteAction(CardSlot source, CardSlot target);
	}
}