using UnityEngine.Assertions;


namespace SGJ2019
{
	public enum CardExecutionState
	{
		READY, WAITING, WORKING, DONE
	}


	public class AIManagedCard : Card
	{
		protected CardExecutionState executionState = CardExecutionState.READY;
		public CardExecutionState ExecutionState => executionState;


		protected override void ManagedInitialize()
		{
			Assert.IsFalse(ownership == OwnerPhase.HUMAN);
			base.ManagedInitialize();
			TurnManager.Instance.RegisterAICard(this);
		}

		public override void ManagedDestruction()
		{
			base.ManagedDestruction();
			if (TurnManager.Instance != null)
			{
				TurnManager.Instance.UnregisterAICard(this);
			}
		}

		public virtual bool CanContinue()
		{
			Assert.IsTrue(executionState == CardExecutionState.WAITING);
			executionState = CardExecutionState.READY;
			return true;
		}

		public virtual void ExecuteTurn()
		{
			Assert.IsTrue(executionState == CardExecutionState.READY);
			executionState = CardExecutionState.DONE;
		}

		protected override void OnTurnEnd()
		{
			Assert.IsTrue(executionState == CardExecutionState.DONE || executionState == CardExecutionState.WAITING);
			executionState = CardExecutionState.READY;
		}

		public void ForcePass()
		{
			executionState = CardExecutionState.DONE;
		}
	}
}