using System.Collections.Generic;


namespace SGJ2019
{
	public interface IManagedInitialization
	{
		Dictionary<InitializationPhases, System.Action> InitializationActions { get; }
	}
}

/*
	public Dictionary<InitializationPhases, System.Action> InitializationActions => initializationActions;
	private Dictionary<InitializationPhases, System.Action> initializationActions = new Dictionary<InitializationPhases, System.Action>();


	private void Awake()
	{
		initializationActions.Add(InitializationPhases.FIRST, ManagedInitialize);
	}
 */
