using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public delegate void CardSlotSelectionChange(CardSlot previous, CardSlot current);
	public class InputManager : SimpleSingleton<InputManager>, IManagedInitialization
	{
		public static event CardSlotSelectionChange OnCardSlotSelectionChange;
		private CardSlot selectedCardSlot = null;
		private bool inputBlockade = false;	//HAXOR: na ograniczenie input'u


		public CardSlot SelectedCardSlot
		{
			get
			{
				return selectedCardSlot;
			}

			private set
			{
				if (selectedCardSlot != null)
				{
					selectedCardSlot.LifecycleComponent.OnLifecycleComponentDestroyed -= OnSelectedCardDestroyed;
				}
				var previousSlot = selectedCardSlot;
				selectedCardSlot = value;
				if (selectedCardSlot != null)
				{
					selectedCardSlot.LifecycleComponent.OnLifecycleComponentDestroyed += OnSelectedCardDestroyed;
				}
				OnCardSlotSelectionChange?.Invoke(previousSlot, selectedCardSlot);
			}
		}

		public Dictionary<InitializationPhases, System.Action> InitializationActions =>
			new Dictionary<InitializationPhases, System.Action>()
			{
				[InitializationPhases.FIRST] = ManagedInitialize
			};


		private void ManagedInitialize()
		{
			CardSlot.OnSlotClicked += CardSlotClicked;
		}

		private void LateUpdate()
		{
			inputBlockade = false;
		}

		private void CardSlotClicked(CardSlot cardSlot)
		{
			Assert.IsNotNull(cardSlot);
			if (inputBlockade)
			{
				return;
			}
			inputBlockade = true;
			if (SelectedCardSlot == cardSlot)
			{
				SelectedCardSlot = null;
			}
			else
			{
				SelectedCardSlot = cardSlot;
			}			
		}

		private void OnSelectedCardDestroyed(LifecycleComponent lifecycleComponent)
		{
			SelectedCardSlot = null;
		}
	}
}
