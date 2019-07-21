using System.Collections.Generic;


namespace SGJ2019
{
	public class Heavy : PlayerSoldier
	{
		public override List<PlayerAction> GetAvailableActions()
		{
			List<PlayerAction> result = base.GetAvailableActions();
			result.Add(new ShotgunBlast());
			return result;
		}
	}
}