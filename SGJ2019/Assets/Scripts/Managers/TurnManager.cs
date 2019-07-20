using UnityEngine.Assertions;
using System;
using System.Collections.Generic;


namespace SGJ2019
{
	public enum OwnerPhase
	{
		HUMAN, ENEMY, NATURE
	}


	public class TurnManager : SimpleSingleton<TurnManager>, IManagedUpdate
	{
		private int roundNumber = 1;
		public int RoundNumber => roundNumber;
		public System.Action OnTurnEnd;
		private Dictionary<OwnerPhase, List<AIManagedCard>> aiManagedCards = new Dictionary<OwnerPhase, List<AIManagedCard>>();
		private OwnerPhase currentTurnPhase = OwnerPhase.HUMAN;


		public OwnerPhase CurrentTurnPhase => currentTurnPhase;
		public Dictionary<UpdatePhases, System.Action> UpdateActions => updateActions;
		private Dictionary<UpdatePhases, System.Action> updateActions = new Dictionary<UpdatePhases, System.Action>();

		public override Dictionary<InitializationPhases, System.Action> InitializationActions =>
			new Dictionary<InitializationPhases, System.Action>()
			{
				[InitializationPhases.LAST] = ManagedInitialize
			};


		protected override void ManagedInitialize()
		{
			base.ManagedInitialize();
			if (Instance == this)
			{
				updateActions.Add(UpdatePhases.FIRST, ManagedUpdate);
				for (int i = 1; i < Enum.GetValues(typeof(OwnerPhase)).Length; ++i)
				{
					aiManagedCards.Add((OwnerPhase)i, new List<AIManagedCard>());
				}
				LogManager.Instance.AddMessage("Level start, round " + roundNumber + ", player turn");
			}
		}

		private void ManagedUpdate()
		{
			if (currentTurnPhase != OwnerPhase.HUMAN)
			{
				foreach (var card in aiManagedCards[currentTurnPhase])
				{
					if (card.ExecutionState == CardExecutionState.DONE)
					{
						continue;
					}
					else if (card.ExecutionState == CardExecutionState.WORKING)
					{
						return;
					}
					else if (card.ExecutionState == CardExecutionState.WAITING)
					{
						if (card.CanContinue())
						{
							card.ExecuteTurn();
							return;
						}
						else
						{
							continue;
						}
					}
					else if (card.ExecutionState == CardExecutionState.READY)
					{
						card.ExecuteTurn();
						return;
					}
				}
				if (currentTurnPhase == OwnerPhase.NATURE)
				{
					OnTurnEnd?.Invoke();
					currentTurnPhase = OwnerPhase.HUMAN;
					++roundNumber;
					LogManager.Instance.AddMessage("Round " + roundNumber.ToString() + " start, player turn");
				}
				else
				{
					++currentTurnPhase;
					LogManager.Instance.AddMessage("Nature turn");
				}
			}
		}

		public void EndPlayerTurn()
		{
			if (currentTurnPhase == OwnerPhase.HUMAN)
			{
				currentTurnPhase = OwnerPhase.ENEMY;
				LogManager.Instance.AddMessage("Enemy turn");
			}
		}

		public void RegisterAICard(AIManagedCard aiCard)
		{
			Assert.IsNotNull(aiCard);
			Assert.IsFalse(aiManagedCards[aiCard.Ownership].Contains(aiCard));
			aiManagedCards[aiCard.Ownership].Add(aiCard);
		}

		public void UnregisterAICard(AIManagedCard aiCard)
		{
			Assert.IsNotNull(aiCard);
			Assert.IsTrue(aiManagedCards[aiCard.Ownership].Contains(aiCard));
			aiManagedCards[aiCard.Ownership].Remove(aiCard);
		}
	}
}