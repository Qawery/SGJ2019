﻿using UnityEngine;
using UnityEngine.Assertions;


namespace SGJ2019
{
	public class RowWalking : AITurnManaged
	{
		[SerializeField] private Card card = null;
		[SerializeField] private RowManager row = null;
		bool moveLeft = true;


		protected override void ManagedInitialize()
		{
			Assert.IsNotNull(card);
			Assert.IsNotNull(row);
			base.ManagedInitialize();
		}

		public override void ExecuteTurn()
		{
			if (row.ContainsCard(card))
			{
				int cardIndex = row.GetIndexOfCard(card);
				if (moveLeft)
				{				
					if (cardIndex >= 1)
					{
						row.MoveCardLeft(card);
					}
					else if(cardIndex == 0)
					{
						moveLeft = !moveLeft;
					}
				}
				else
				{
					if (cardIndex < row.GetNumberOfCards() - 1)
					{
						row.MoveCardRight(card);
					}
					else if (cardIndex == row.GetNumberOfCards() - 1)
					{
						moveLeft = !moveLeft;
					}
				}
			}
			base.ExecuteTurn();		
		}
	}
}