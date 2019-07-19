using UnityEngine;
using System.Collections.Generic;


namespace SGJ2019
{
	[DisallowMultipleComponent]
	public class LifecycleComponent : MonoBehaviour
	{
		public static System.Action<LifecycleComponent> OnLifecycleComponentCreated;
		public static System.Action<LifecycleComponent> OnLifecycleComponentDestroyed;
		private List<IManagedLifecycle> dependantObjects = new List<IManagedLifecycle>();


		private void Awake()
		{			
			CollectDependantComponents(gameObject);
			if (LifecycleManager.Instance == null);
			OnLifecycleComponentCreated?.Invoke(this);
		}

		private void CollectDependantComponents(GameObject analyzedObject)
		{
			var dependantComponents = GetComponents<IManagedLifecycle>();
			foreach (var dependantComponent in dependantComponents)
			{
				dependantObjects.Add(dependantComponent);
			}
			for (int i = 0; i < analyzedObject.transform.childCount; ++i)
			{
				if (analyzedObject.transform.GetChild(i).gameObject.GetComponent<LifecycleComponent>() == null)
				{
					CollectDependantComponents(analyzedObject.transform.GetChild(i).gameObject);
				}
			}
		}

		public void SynchronizedInitialize(InitializationPhases phase)
		{
			foreach (var dependantObject in dependantObjects)
			{
				dependantObject.InitializationPhase(phase);
			}
		}

		public void SynchronizedUpdate(UpdatePhases phase)
		{
			if (isActiveAndEnabled)
			{
				foreach (var dependantObject in dependantObjects)
				{
					dependantObject.UpdatePhase(phase);
				}
			}
		}

		private void OnDestroy()
		{
			OnLifecycleComponentDestroyed?.Invoke(this);
		}
	}
}