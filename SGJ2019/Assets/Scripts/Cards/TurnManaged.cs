using UnityEngine;
using System.Collections.Generic;


namespace SGJ2019
{
	public enum CardExecutionState
	{
		READY, WAITING, WORKING, DONE
	}


	public abstract class TurnManaged : MonoBehaviour, IManagedInitialization, IManagedDestroy
	{
		protected CardExecutionState executionState = CardExecutionState.READY;
		[SerializeField] protected TurnPhase ownership = TurnPhase.NATURE;


		public CardExecutionState ExecutionState => executionState;
		public TurnPhase Ownership => ownership;

		public Dictionary<InitializationPhases, System.Action> InitializationActions =>
			new Dictionary<InitializationPhases, System.Action>()
			{
				[InitializationPhases.FIRST] = ManagedInitialize
			};


		protected virtual void ManagedInitialize()
		{
			TurnManager.Instance.OnTurnEnd += OnTurnEnd;
			TurnManager.Instance.RegisterTurnManagedCard(this);
		}

		public void ManagedDestruction()
		{
			TurnManager.Instance.OnTurnEnd -= OnTurnEnd;
			TurnManager.Instance.UnregisterTurnManagedCard(this);
		}

		public abstract bool CanContinue();

		public abstract void ExecuteTurn();

		protected abstract void OnTurnEnd();
	}
}