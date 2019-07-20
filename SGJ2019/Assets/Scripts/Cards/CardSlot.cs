using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using System.Collections.Generic;


namespace SGJ2019
{
	public class CardSlot : MonoBehaviour, IManagedInitialization, ILifecycleBound, IPointerClickHandler
	{
		public static System.Action<CardSlot> OnSlotClicked;
		private Card card = null;
		private LifecycleComponent lifecycleComponent = null;


		public Card Card => card;

		public Dictionary<InitializationPhases, System.Action> InitializationActions =>
			new Dictionary<InitializationPhases, System.Action>()
			{
				[InitializationPhases.FIRST] = ManagedInitialize
			};

		public LifecycleComponent LifecycleComponent
		{
			get
			{
				return lifecycleComponent;
			}

			set
			{
				Assert.IsNull(lifecycleComponent);
				lifecycleComponent = value;
			}
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

		public void OnPointerClick(PointerEventData eventData)
		{
			OnSlotClicked?.Invoke(this);
		}
	}
}
