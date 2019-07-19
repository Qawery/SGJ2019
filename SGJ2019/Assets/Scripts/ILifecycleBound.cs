using UnityEngine;


namespace SGJ2019
{
	public interface ILifecycleBound
	{
		void InitializationPhase(InitializationPhases phase);
		void UpdatePhase(UpdatePhases phase);
	}
}