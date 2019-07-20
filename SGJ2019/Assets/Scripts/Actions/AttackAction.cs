namespace SGJ2019
{
	public class AttackAction : Action
	{
		public override string Name => "Attack";
		public override string Description => "Destroy adjacent card";


		public override void ExecuteAction(CardSlot source, CardSlot target)
		{
			var playerCard = (source.Card as PlayerOwnedCard);
			if (playerCard.currentActionPoints > 0)
			{
				--playerCard.currentActionPoints;
				var rowManager = source.transform.parent.GetComponent<RowManager>();
				int selectedIndex = rowManager.GetIndexOfCard(source.Card);
				int otherIndex = rowManager.GetIndexOfCard(target.Card);
				if (selectedIndex + 1 == otherIndex || selectedIndex - 1 == otherIndex)
				{
					rowManager.RemoveCard(target);
				}
			}
		}
	}
}