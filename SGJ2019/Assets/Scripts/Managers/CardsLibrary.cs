using UnityEngine;
using System.Collections.Generic;


namespace SGJ2019
{
	[CreateAssetMenu(fileName = "CardsLibrary", menuName = "ScriptableObject/CardsLibrary", order = 1)]
	public class CardsLibrary : ScriptableObject
	{
		[SerializeField] private List<Card> cardPrefabs = new List<Card>();


		public Card GetCardById(string cardId)
		{
			foreach (var cardPrefab in cardPrefabs)
			{
				if (cardPrefab.CardId == cardId)
				{
					return cardPrefab;
				}
			}
			return null;
		}
	}
}