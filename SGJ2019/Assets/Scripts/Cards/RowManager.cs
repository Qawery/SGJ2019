using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public class RowManager : MonoBehaviour, IManagedInitialization
	{
		[SerializeField] private CardSlot cardSlotPrefab = null;
		private List<CardSlot> currentCardSlots = new List<CardSlot>();
		public CardSlot[] AllSlots => currentCardSlots.ToArray();


		public Dictionary<InitializationPhases, System.Action> InitializationActions =>
				new Dictionary<InitializationPhases, System.Action>()
				{
					[InitializationPhases.LAST] = ManagedInitialize
				};


		private void ManagedInitialize()
		{
			Assert.IsNotNull(cardSlotPrefab);
			Assert.IsTrue(GetComponentsInChildren<CardSlot>().Length == 0);
			var test = GetComponentsInChildren<Card>();
			foreach (var startingCard in GetComponentsInChildren<Card>())
			{
				AddCardToRowOnRight(startingCard);
			}
		}

		public void AddCardToRowOnIndex(int index, Card newCard)
		{
			Assert.IsNotNull(newCard);
			Assert.IsTrue(index >= 0 && index <= currentCardSlots.Count);
			var newCardSlot = Instantiate(cardSlotPrefab, transform);
			newCardSlot.LifecycleComponent.OnLifecycleComponentDestroyed += OnSlotDestroyed;
			currentCardSlots.Add(newCardSlot);
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
			Assert.IsTrue(destinationIndex >= 0 && destinationIndex < currentCardSlots.Count);
			while (oldIndex < destinationIndex)
			{
				MoveCardRight(oldIndex);
				++oldIndex;
			}
			while (oldIndex > destinationIndex)
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
			currentCardSlots[index].LifecycleComponent.OnLifecycleComponentDestroyed -= OnSlotDestroyed;
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
			slotWithCard.LifecycleComponent.OnLifecycleComponentDestroyed -= OnSlotDestroyed;
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

		public int GetNumberOfCards()
		{
			return currentCardSlots.Count;
		}

		public Card GetCardOnIndex(int index)
		{
			Assert.IsTrue(index >= 0 && index < currentCardSlots.Count);
			return currentCardSlots[index].Card;
		}

		private void OnSlotDestroyed(LifecycleComponent lifecycleComponent)
		{
			RemoveCard(lifecycleComponent.GetComponent<CardSlot>());
		}

		public int GetDistanceBetweenCards(Card card1, Card card2)
		{
			Assert.IsNotNull(card1);
			Assert.IsNotNull(card2);
			Assert.IsFalse(card1 == card2);
			return Mathf.Abs(GetIndexOfCard(card1) - GetIndexOfCard(card2));
		}

		public int GetDistanceBetweenCards(CardSlot slot1, CardSlot slot2)
		{
			Assert.IsNotNull(slot1);
			Assert.IsNotNull(slot2);
			Assert.IsFalse(slot1 == slot2);
			return Mathf.Abs(currentCardSlots.IndexOf(slot1) - currentCardSlots.IndexOf(slot2));
		}

		public Card GetCardOnLeftOfIndex(int index)
		{
			Assert.IsTrue(index >= 0 && index < currentCardSlots.Count);
			if (index == 0)
			{
				return null;
			}
			else
			{
				return currentCardSlots[index - 1].Card;
			}
		}

		public Card GetCardOnRightOfIndex(int index)
		{
			Assert.IsTrue(index >= 0 && index < currentCardSlots.Count);
			if (index == currentCardSlots.Count - 1)
			{
				return null;
			}
			else
			{
				return currentCardSlots[index + 1].Card;
			}
		}
	}
}