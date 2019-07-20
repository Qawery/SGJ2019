using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public class CardSlot : MonoBehaviour, IManagedInitialization
	{
		private Card card = null;
		public Card Card => card;

		public Dictionary<InitializationPhases, System.Action> InitializationActions => initializationActions;
		private Dictionary<InitializationPhases, System.Action> initializationActions = new Dictionary<InitializationPhases, System.Action>();


		private void Awake()
		{
			initializationActions.Add(InitializationPhases.FIRST, ManagedInitialize);			
		}

		private void ManagedInitialize()
		{
			GetComponent<Canvas>().worldCamera = GlobalPrefabLibrary.Instance.MainCamera;
		}

		public void RemoveCard()
		{
			card = null;
		}

		public void PlaceCard(Card newCard)
		{
			Assert.IsNull(card);
			Assert.IsNotNull(newCard);
			card = newCard;
			card.transform.SetParent(transform, false);
			card.transform.position = transform.position;
		}

		public void OnClicked()
		{
		}
	}
}
