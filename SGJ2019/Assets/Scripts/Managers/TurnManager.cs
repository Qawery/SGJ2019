using UnityEngine.Assertions;
using System;
using System.Collections.Generic;


namespace SGJ2019
{
	public enum TurnPhase
	{
		HUMAN, ENEMY, NATURE
	}


	public class TurnManager : SimpleSingleton<TurnManager>, IManagedUpdate
	{
		public System.Action OnTurnEnd;
		private Dictionary<TurnPhase, List<TurnManagedCard>> turnOrderForObjects = new Dictionary<TurnPhase, List<TurnManagedCard>>();
		private TurnPhase currentTurnPhase = TurnPhase.HUMAN;


		public TurnPhase CurrentTurnPhase => currentTurnPhase;


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
			for (int i = 1; i < Enum.GetValues(typeof(TurnPhase)).Length; ++i)
			{
				turnOrderForObjects.Add((TurnPhase) i, new List<TurnManagedCard>());
			}
		}

		private void ManagedUpdate()
		{
			if (currentTurnPhase != TurnPhase.HUMAN)
			{
				foreach (var card in turnOrderForObjects[currentTurnPhase])
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
				if (currentTurnPhase == TurnPhase.NATURE)
				{
					OnTurnEnd?.Invoke();
					currentTurnPhase = TurnPhase.HUMAN;
				}
				else
				{
					++currentTurnPhase;
				}
			}
		}

		public void EndPlayerTurn()
		{
			if (currentTurnPhase == TurnPhase.HUMAN)
			{
				currentTurnPhase = TurnPhase.ENEMY;
			}
		}

		public void RegisterTurnManagedCard(TurnManagedCard turnManagedCard)
		{
			Assert.IsNotNull(turnManagedCard);
			turnOrderForObjects[turnManagedCard.Ownership].Add(turnManagedCard);
		}

		public void UnregisterTurnManagedCard(TurnManagedCard turnManagedCard)
		{
			Assert.IsNotNull(turnManagedCard);
			turnOrderForObjects[turnManagedCard.Ownership].Remove(turnManagedCard);
		}
	}
}