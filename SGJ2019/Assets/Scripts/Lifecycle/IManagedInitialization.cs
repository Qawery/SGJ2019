using System.Collections.Generic;


namespace SGJ2019
{
	public interface IManagedInitialization
	{
		Dictionary<InitializationPhases, System.Action> InitializationActions { get; }
	}
}

/*
	public Dictionary<InitializationPhases, System.Action> InitializationActions =>
			new Dictionary<InitializationPhases, System.Action>()
			{
				[InitializationPhases.FIRST] = ManagedInitialize
			};
 */
