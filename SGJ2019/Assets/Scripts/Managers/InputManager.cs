﻿using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public delegate void CardSlotSelectionChange(CardSlot previous, CardSlot current);
	public class InputManager : SimpleSingleton<InputManager>, IManagedInitialization
	{
		private const int NO_ACTION = -1;
		[SerializeField] private LogText logText = null;
		public event CardSlotSelectionChange OnCardSlotSelectionChange;
		private CardSlot selectedCardSlot = null;
		private bool inputBlockade = false; //HAXOR: na ograniczenie input'u
		private int selectedActionIndex = NO_ACTION;


		private int SelectedActionIndex
		{
			get
			{
				return selectedActionIndex;
			}

			set
			{
				if (value != NO_ACTION)
				{
					Assert.IsNotNull(SelectedCardSlot);
					Assert.IsTrue(SelectedCardSlot.Card.Ownership == OwnerPhase.HUMAN);
					var playerOwnedCard = SelectedCardSlot.Card as PlayerOwnedCard;
					Assert.IsNotNull(playerOwnedCard);
					var availableActions = playerOwnedCard.GetAvailableActions();
					Assert.IsTrue(value >= 0 && value < availableActions.Count);
					logText.SetText(availableActions[value].Description);
				}
				else
				{
					logText.SetText("");
				}
				selectedActionIndex = value;
			}
		}

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
					if (SelectedCardSlot.Card.Ownership == OwnerPhase.HUMAN)
					{
						var playerOwnedCard = SelectedCardSlot.Card as PlayerOwnedCard;
						Assert.IsNotNull(playerOwnedCard);
						if (playerOwnedCard.GetAvailableActions().Count > 0)
						{
							SelectedActionIndex = 0;
						}
						else
						{
							SelectedActionIndex = NO_ACTION;
						}
					}
					else
					{
						SelectedActionIndex = NO_ACTION;
					}
				}
				else
				{
					SelectedActionIndex = NO_ACTION;
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
			Assert.IsNotNull(logText);
			CardSlot.OnSlotClicked += CardSlotClicked;
			TurnManager.Instance.OnTurnEnd += OnTurnEnd;
		}

		private void LateUpdate()
		{
			inputBlockade = false;
		}

		private void OnTurnEnd()
		{
			SelectedCardSlot = null;
		}

		private void OnSelectedCardDestroyed(LifecycleComponent lifecycleComponent)
		{
			SelectedCardSlot = null;
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
				if (SelectedActionIndex >= 0)
				{
					ProcessAction(cardSlot);
				}
				else
				{
					SelectedCardSlot = cardSlot;
				}
			}			
		}

		private void ProcessAction(CardSlot otherSlot)
		{
			var playerOwnedCard = SelectedCardSlot.Card as PlayerOwnedCard;
			var availableActions = playerOwnedCard.GetAvailableActions();
			availableActions[SelectedActionIndex].ExecuteAction(SelectedCardSlot, otherSlot);
			SelectedCardSlot = null;
		}

		public void ActionSelected(int actionIndex)
		{
			SelectedActionIndex = actionIndex;
		}

		public List<Action> GetAvailableActions()
		{
			if (SelectedCardSlot != null && SelectedCardSlot.Card.Ownership == OwnerPhase.HUMAN)
			{
				var playerOwnedCard = SelectedCardSlot.Card as PlayerOwnedCard;
				Assert.IsNotNull(playerOwnedCard);
				return playerOwnedCard.GetAvailableActions();
			}
			else
			{
				return new List<Action>();
			}
		}
	}
}