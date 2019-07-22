using UnityEngine;


namespace SGJ2019
{
	public class SwitchRow : PlayerAction
	{
		public override string Name => "Switch row";
		public override string Description => "Move to edge of different row" + "\n" +
												base.Description;
		public override int Cost => 2;


		public override bool IsActionPossible(CardSlot source, CardSlot target)
		{
			var actionPoints = source.Card.GetComponent<ActionPointsComponent>();
			if (actionPoints != null && actionPoints.CurrentActionPoints < Cost)
			{
				Utilities.SpawnFloatingText("Insufficient action points!", Color.red, source.Card.transform);
				return false;
			}
			if (source.transform.parent.GetComponent<RowManager>() == target.transform.parent.GetComponent<RowManager>())
			{
				Utilities.SpawnFloatingText("Invalid row!", Color.red, source.Card.transform);
				return false;
			}
			var rowManager = target.transform.parent.GetComponent<RowManager>();
			int otherIndex = rowManager.GetIndexOfCard(target.Card);
			if (otherIndex == 0 || otherIndex == rowManager.AllSlots.Length - 1)
			{
				return true;
			}
			else
			{
				Utilities.SpawnFloatingText("Select edge!", Color.red, source.Card.transform);
				return false;
			}
		}

		public override void ExecuteAction(CardSlot source, CardSlot target)
		{
			if (IsActionPossible(source, target))
			{
				var actionPoints = source.Card.GetComponent<ActionPointsComponent>();
				Utilities.SpawnFloatingText("Switch row", Color.grey, source.Card.transform);
				if (actionPoints != null)
				{
					actionPoints.Spend(Cost);
				}
				var targetRowManager = target.transform.parent.GetComponent<RowManager>();
				int targetIndex = targetRowManager.GetIndexOfCard(target.Card);
				if (targetIndex == 0)
				{
					targetRowManager.AddCardToRowOnLeft(source.Card);
				}
				else
				{
					targetRowManager.AddCardToRowOnRight(source.Card);
				}
				source.transform.parent.GetComponent<RowManager>().RemoveCard(source);
			}
		}
	}
}