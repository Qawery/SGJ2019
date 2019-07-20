using UnityEngine;
using UnityEngine.Assertions;


namespace SGJ2019
{
	public class CardSlot : MonoBehaviour
	{
		private Card card = null;


		public Card Card => card;


		public void RemoveCard()
		{
			card = null;
		}

		public void PlaceCard(Card newCard)
		{
			Assert.IsNull(card);
			Assert.IsNotNull(newCard);
			card = newCard;
			card.transform.position = transform.position;
			card.transform.parent = transform;
		}
	}
}
