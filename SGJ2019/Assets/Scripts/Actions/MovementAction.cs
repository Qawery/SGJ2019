namespace SGJ2019
{
	public class MovementAction : Action
	{
		public override string Name => "Move";
		public override string Description => "Move to adjacent space" + "\n" +
												base.Description;
		public override int Cost => 1;


		public override void ExecuteAction(CardSlot source, CardSlot target)
		{
			var playerCard = (source.Card as PlayerOwnedCard);
			if (playerCard.ActionPointsComponent.CurrentActionPoints >= Cost)
			{
				playerCard.ActionPointsComponent.Spend(Cost);
				var rowManager = source.transform.parent.GetComponent<RowManager>();
				int selectedIndex = rowManager.GetIndexOfCard(source.Card);
				int otherIndex = rowManager.GetIndexOfCard(target.Card);
				if (selectedIndex + 1 == otherIndex)
				{
					rowManager.MoveCardRight(source);
					source = target;
				}
				else if (selectedIndex - 1 == otherIndex)
				{
					rowManager.MoveCardLeft(source);
					source = target;
				}
			}
		}
	}
}