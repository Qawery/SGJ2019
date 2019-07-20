using System.Collections.Generic;


namespace SGJ2019
{
	public class PlayerOwnedCard : Card
	{
		private const int MAX_ACTION_POINTS = 2;
		private int currentActionPoints = MAX_ACTION_POINTS;


		protected override void OnTurnEnd()
		{
			currentActionPoints = MAX_ACTION_POINTS;
		}

		public List<Action> GetAvailableActions()
		{
			List<Action> result = new List<Action>();
			result.Add(new MovementAction());
			return result;
		}
	}
}