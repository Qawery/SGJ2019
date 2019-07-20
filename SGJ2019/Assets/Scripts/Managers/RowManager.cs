using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public class RowManager : MonoBehaviour
	{
		[SerializeField] private CardSlot cardSlotPrefab = null;
		private List<CardSlot> currentCardSlots = new List<CardSlot>();


		private void Awake()
		{
			Assert.IsNotNull(cardSlotPrefab);
			Assert.IsTrue(transform.childCount == 0);
		}

		public void AddCardToRowOnIndex(int index, Card newCard)
		{
			Assert.IsNotNull(newCard);
			Assert.IsTrue(index >= 0 && index <= currentCardSlots.Count);
			currentCardSlots.Add(Instantiate(cardSlotPrefab, transform));
			currentCardSlots[currentCardSlots.Count - 1].PlaceCard(newCard);
			if (index != currentCardSlots.Count - 1)
			{
				MoveCardToIndex(currentCardSlots.Count - 1, index);
			}
		}

		public void AddCardToRowOnLeft(Card newCard)
		{
			AddCardToRowOnIndex(0, newCard);
		}

		public void AddCardToRowOnRight(Card newCard)
		{
			AddCardToRowOnIndex(currentCardSlots.Count, newCard);
		}

		public void MoveCardToIndex(int oldIndex, int destinationIndex)
		{
			Assert.IsTrue(oldIndex != destinationIndex);
			Assert.IsTrue(oldIndex >= 0 && oldIndex < currentCardSlots.Count);
			Assert.IsTrue(destinationIndex >= 0 && destinationIndex <= currentCardSlots.Count);
			while (oldIndex > destinationIndex)
			{
				MoveCardRight(oldIndex);
				++oldIndex;
			}
			while (oldIndex < destinationIndex)
			{
				MoveCardLeft(oldIndex);
				--oldIndex;
			}
		}

		public void MoveCardToIndex(Card cardToMove, int destinationIndex)
		{
			var slot = FindSlotWithCard(cardToMove);
			Assert.IsNotNull(slot);
			MoveCardToIndex(slot, destinationIndex);
		}

		public void MoveCardToIndex(CardSlot slotWithCard, int destinationIndex)
		{
			Assert.IsTrue(currentCardSlots.Contains(slotWithCard));
			MoveCardToIndex(currentCardSlots.IndexOf(slotWithCard), destinationIndex);
		}

		public void MoveCardLeft(int index)
		{
			Assert.IsTrue(index > 0);
			var movedCard = currentCardSlots[index].Card;
			currentCardSlots[index].RemoveCard();
			currentCardSlots[index].PlaceCard(currentCardSlots[index - 1].Card);
			currentCardSlots[index - 1].RemoveCard();
			currentCardSlots[index - 1].PlaceCard(movedCard);
		}

		public void MoveCardLeft(Card cardToMove)
		{
			var slot = FindSlotWithCard(cardToMove);
			Assert.IsNotNull(slot);
			MoveCardLeft(slot);
		}

		public void MoveCardLeft(CardSlot slotWithCard)
		{
			Assert.IsTrue(currentCardSlots.Contains(slotWithCard));
			MoveCardLeft(currentCardSlots.IndexOf(slotWithCard));
		}

		public void MoveCardRight(int index)
		{
			Assert.IsTrue(index < currentCardSlots.Count - 1);
			var movedCard = currentCardSlots[index].Card;
			currentCardSlots[index].RemoveCard();
			currentCardSlots[index].PlaceCard(currentCardSlots[index + 1].Card);
			currentCardSlots[index + 1].RemoveCard();
			currentCardSlots[index + 1].PlaceCard(movedCard);
		}

		public void MoveCardRight(Card cardToMove)
		{
			var slot = FindSlotWithCard(cardToMove);
			Assert.IsNotNull(slot);
			MoveCardRight(slot);
		}

		public void MoveCardRight(CardSlot slotWithCard)
		{
			Assert.IsTrue(currentCardSlots.Contains(slotWithCard));
			MoveCardRight(currentCardSlots.IndexOf(slotWithCard));
		}

		public void RemoveCard(int index)
		{
			Assert.IsTrue(index >= 0 && index <= currentCardSlots.Count - 1);
			currentCardSlots[index].RemoveCard();
			Destroy(currentCardSlots[index].gameObject);
			currentCardSlots.RemoveAt(index);
		}

		public void RemoveCard(Card cardToMove)
		{
			var slot = FindSlotWithCard(cardToMove);
			Assert.IsNotNull(slot);
			RemoveCard(slot);
		}

		public void RemoveCard(CardSlot slotWithCard)
		{
			Assert.IsTrue(currentCardSlots.Contains(slotWithCard));
			slotWithCard.RemoveCard();
			currentCardSlots.Remove(slotWithCard);
			Destroy(slotWithCard.gameObject);
		}

		private CardSlot FindSlotWithCard(Card card)
		{
			foreach (var slot in currentCardSlots)
			{
				if (slot.Card == card)
				{
					return slot;
				}
			}
			return null;
		}

		public bool ContainsCard(Card card)
		{
			return FindSlotWithCard(card) != null;
		}

		public int GetIndexOfCard(Card card)
		{
			var slot = FindSlotWithCard(card);
			if (slot == null)
			{
				return -1;
			}
			else
			{
				return currentCardSlots.IndexOf(slot);
			}
		}

		public int GetNumberOFCards()
		{
			return currentCardSlots.Count;
		}
	}
}