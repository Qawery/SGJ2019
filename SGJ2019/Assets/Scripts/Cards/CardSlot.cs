using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using System.Collections.Generic;


namespace SGJ2019
{
	[RequireComponent(typeof(Button))]
	public class CardSlot : MonoBehaviour, IManagedInitialization, IManagedDestroy, ILifecycleBound, IPointerClickHandler
	{
		private ColorBlock defaultColorBlock;
		private ColorBlock selectedColorBlock;
		private Button button = null;
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
			InputManager.Instance.OnCardSlotSelectionChange += OnSlotSelected;
			button = GetComponent<Button>();
			defaultColorBlock = button.colors;
			selectedColorBlock = button.colors;
			selectedColorBlock.normalColor = selectedColorBlock.pressedColor;
		}

		public void ManagedDestruction()
		{
			if (card != null)
			{
				Destroy(card.gameObject);
				RemoveCard();
			}
			if (InputManager.Instance != null)
			{
				InputManager.Instance.OnCardSlotSelectionChange -= OnSlotSelected;
			}
		}

		public void RemoveCard()
		{
			if (card != null)
			{
				card.LifecycleComponent.OnLifecycleComponentDestroyed -= OnCardDestroyed;
				card = null;
			}
		}

		public void PlaceCard(Card newCard)
		{
			Assert.IsNull(card);
			Assert.IsNotNull(newCard);
			card = newCard;
			card.transform.SetParent(transform, false);
			card.transform.position = transform.position;
			card.LifecycleComponent.OnLifecycleComponentDestroyed += OnCardDestroyed;
		}

		private void OnCardDestroyed(LifecycleComponent lifecycleComponent)
		{
			RemoveCard();
			Destroy(gameObject);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			OnSlotClicked?.Invoke(this);
		}

		private void OnSlotSelected(CardSlot previous, CardSlot current)
		{
			if (current == this)
			{
				button.colors = selectedColorBlock;
			}
			else
			{
				button.colors = defaultColorBlock;
			}
		}
	}
}
