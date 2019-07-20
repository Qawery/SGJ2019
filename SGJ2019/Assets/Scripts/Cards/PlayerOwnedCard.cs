using System.Collections.Generic;


namespace SGJ2019
{
	public class PlayerOwnedCard : Card
	{
		private const int MAX_HEALTH_POINTS = 5;
		public int currentHealthPoints = MAX_HEALTH_POINTS;
		private const int MAX_ACTION_POINTS = 2;
		public int currentActionPoints = MAX_ACTION_POINTS;


		protected override void OnTurnEnd()
		{
			currentActionPoints = MAX_ACTION_POINTS;
		}

		public List<Action> GetAvailableActions()
		{
			List<Action> result = new List<Action>();
			result.Add(new MovementAction());
			result.Add(new AttackAction());
			return result;
		}

		private void Update()
		{
			description.text =
				"Health: " + currentHealthPoints.ToString() + "\n" +
				"Action Points left: " + currentActionPoints.ToString();
		}
	}
}