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
		public System.Action OnTurnEnd;
		private Dictionary<OwnerPhase, List<AIManagedCard>> aiManagedCards = new Dictionary<OwnerPhase, List<AIManagedCard>>();
		private OwnerPhase currentTurnPhase = OwnerPhase.HUMAN;


		public OwnerPhase CurrentTurnPhase => currentTurnPhase;
		public Dictionary<UpdatePhases, System.Action> UpdateActions => updateActions;
		private Dictionary<UpdatePhases, System.Action> updateActions = new Dictionary<UpdatePhases, System.Action>();


		protected override void Awake()
		{
			if (Instance != this)
			{
				Destroy(gameObject);
				return;
			}
			updateActions.Add(UpdatePhases.FIRST, ManagedUpdate);
			for (int i = 1; i < Enum.GetValues(typeof(OwnerPhase)).Length; ++i)
			{
				aiManagedCards.Add((OwnerPhase) i, new List<AIManagedCard>());
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
				}
				else
				{
					++currentTurnPhase;
				}
			}
		}

		public void EndPlayerTurn()
		{
			if (currentTurnPhase == OwnerPhase.HUMAN)
			{
				currentTurnPhase = OwnerPhase.ENEMY;
			}
		}

		public void RegisterAICard(AIManagedCard aiCard)
		{
			Assert.IsNotNull(aiCard);
			aiManagedCards[aiCard.Ownership].Add(aiCard);
		}

		public void UnregisterAICard(AIManagedCard aiCard)
		{
			Assert.IsNotNull(aiCard);
			aiManagedCards[aiCard.Ownership].Remove(aiCard);
		}
	}
}