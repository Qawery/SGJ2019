namespace SGJ2019
{
	public interface IManagedLifecycle
	{
		void InitializationPhase(InitializationPhases phase);
		void UpdatePhase(UpdatePhases phase);
	}
}