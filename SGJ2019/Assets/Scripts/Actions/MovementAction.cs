namespace SGJ2019
{
	public class MovementAction : Action
	{
		public override string Name => "Move";
		public override string Description => "Move to adjacent space";


		public override void ExecuteAction(CardSlot source, CardSlot target)
		{
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