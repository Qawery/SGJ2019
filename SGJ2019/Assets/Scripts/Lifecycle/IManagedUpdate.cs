using System.Collections.Generic;


namespace SGJ2019
{
	public interface IManagedUpdate
	{
		Dictionary<UpdatePhases, System.Action> UpdateActions { get; }
	}
}

/*
	public Dictionary<UpdatePhases, System.Action> UpdateActions => updateActions;
	private Dictionary<UpdatePhases, System.Action> updateActions = new Dictionary<UpdatePhases, System.Action>();


	private void Awake()
	{
		updateActions.Add(UpdatePhases.FIRST, ManagedUpdate);
	}
 */
