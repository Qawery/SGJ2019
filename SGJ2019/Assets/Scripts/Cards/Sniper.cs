using System.Collections.Generic;


namespace SGJ2019
{
	public class Sniper : PlayerSoldier
	{
		public override List<PlayerAction> GetAvailableActions()
		{
			List<PlayerAction> result = base.GetAvailableActions();
			result.Add(new SniperShot());
			return result;
		}
	}
}