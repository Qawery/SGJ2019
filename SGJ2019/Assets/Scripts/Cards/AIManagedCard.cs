using UnityEngine.Assertions;


namespace SGJ2019
{
	public class AIManagedCard : TurnManagedCard
	{
		public override bool CanContinue()
		{
			Assert.IsTrue(executionState == CardExecutionState.WAITING);
			executionState = CardExecutionState.READY;
			return true;
		}

		public override void ExecuteTurn()
		{
			Assert.IsTrue(executionState == CardExecutionState.READY);
			executionState = CardExecutionState.DONE;
		}

		protected override void OnTurnEnd()
		{
			Assert.IsTrue(executionState == CardExecutionState.DONE || executionState == CardExecutionState.WAITING);
			executionState = CardExecutionState.READY;
		}
	}
}