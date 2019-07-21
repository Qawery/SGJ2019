using UnityEngine;


namespace SGJ2019
{
	public class MovementAction : PlayerAction
	{
		public override string Name => "Move";
		public override string Description => "Move to adjacent space" + "\n" +
												base.Description;
		public override int Cost => 1;


		public override bool IsActionPossible(CardSlot source, CardSlot target)
		{
			var actionPoints = source.Card.GetComponent<ActionPointsComponent>();
			if (actionPoints != null && actionPoints.CurrentActionPoints < Cost)
			{
				Utilities.SpawnFloatingText("Insufficient action points!", Color.red, source.Card.transform);
				return false;
			}
			var rowManager = source.transform.parent.GetComponent<RowManager>();
			if (rowManager != target.transform.parent.GetComponent<RowManager>())
			{
				Utilities.SpawnFloatingText("Invalid row!", Color.red, source.Card.transform);
				return false;
			}
			int selectedIndex = rowManager.GetIndexOfCard(source.Card);
			int otherIndex = rowManager.GetIndexOfCard(target.Card);
			if (Mathf.Abs(selectedIndex - otherIndex) == 1)
			{
				return true;
			}
			else
			{
				Utilities.SpawnFloatingText("Outside range!", Color.red, source.Card.transform);
				return false;
			}
		}

		public override void ExecuteAction(CardSlot source, CardSlot target)
		{
			if (IsActionPossible(source, target))
			{
				var actionPoints = source.Card.GetComponent<ActionPointsComponent>();
				if (actionPoints != null)
				{
					actionPoints.Spend(Cost);
				}
				var rowManager = source.transform.parent.GetComponent<RowManager>();
				int sourceIndex = rowManager.GetIndexOfCard(source.Card);
				int targetIndex = rowManager.GetIndexOfCard(target.Card);
				Utilities.SpawnFloatingText("Move", Color.grey, source.Card.transform);
				if (sourceIndex < targetIndex)
				{
					rowManager.MoveCardRight(source);
				}
				else if (sourceIndex > targetIndex)
				{
					rowManager.MoveCardLeft(source);
				}
			}
		}
	}
}