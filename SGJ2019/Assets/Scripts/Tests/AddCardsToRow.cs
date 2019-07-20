using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public class AddCardsToRow : AITurnManaged
	{
		[SerializeField] private List<Card> cards = new List<Card>();
		[SerializeField] private RowManager row = null;
		bool moveLeft = true;


		protected override void Awake()
		{
			Assert.IsNotNull(row);
			base.Awake();
		}

		public override void ExecuteTurn()
		{
			foreach (var card in cards)
			{
				if (!row.ContainsCard(card))
				{
					row.AddCardToRowOnRight(card);
				}
			}
			base.ExecuteTurn();
		}
	}
}