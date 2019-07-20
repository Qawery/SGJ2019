using System.Collections.Generic;


namespace SGJ2019
{
	public class Heavy : PlayerOwnedCard
	{
		public override List<Action> GetAvailableActions()
		{
			List<Action> result = base.GetAvailableActions();
			result.Add(new ShotgunBlast());
			return result;
		}
	}
}