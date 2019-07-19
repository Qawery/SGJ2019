using UnityEngine;


namespace SGJ2019
{
	public class LifecyclePhasesShouter : MonoBehaviour, IManagedLifecycle
	{
		public void InitializationPhase(InitializationPhases phase)
		{
			Debug.Log(gameObject.name + " initialization phase: " + phase.ToString());
		}

		public void UpdatePhase(UpdatePhases phase)
		{
			Debug.Log(gameObject.name + " update phase: " + phase.ToString());
		}
	}
}