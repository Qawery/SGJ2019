using System.Collections.Generic;


namespace SGJ2019
{
	public class Sniper : PlayerOwnedCard
	{
		public override List<Action> GetAvailableActions()
		{
			List<Action> result = base.GetAvailableActions();
			result.Add(new SniperShot());
			return result;
		}
	}
}