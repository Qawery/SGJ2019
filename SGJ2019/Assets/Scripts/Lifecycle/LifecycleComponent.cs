using UnityEngine;
using System.Collections.Generic;


namespace SGJ2019
{
	[DisallowMultipleComponent]
	public class LifecycleComponent : MonoBehaviour
	{
		public static System.Action<LifecycleComponent> GlobalOnLifecycleComponentCreated;
		public static System.Action<LifecycleComponent> GlobalOnLifecycleComponentDestroyed;
		public System.Action<LifecycleComponent> OnLifecycleComponentDestroyed;
		private List<IManagedInitialization> componentsToInitialize = new List<IManagedInitialization>();
		private List<IManagedUpdate> componentsToUpdate = new List<IManagedUpdate>();
		private List<IManagedDestroy> componentsToNotifyOnDestruction = new List<IManagedDestroy>();


		private void Awake()
		{			
			CollectDependantComponents(gameObject);
			if (LifecycleManager.Instance == null);
			GlobalOnLifecycleComponentCreated?.Invoke(this);
		}

		private void CollectDependantComponents(GameObject analyzedObject)
		{
			foreach (var componentToInitialize in GetComponents<IManagedInitialization>())
			{
				if (!componentsToInitialize.Contains(componentToInitialize))
				{
					componentsToInitialize.Add(componentToInitialize);
				}
			}
			foreach (var componentToUpdate in GetComponents<IManagedUpdate>())
			{
				if (!componentsToUpdate.Contains(componentToUpdate))
				{
					componentsToUpdate.Add(componentToUpdate);
				}
			}
			foreach (var componentToNotifyOnDestruction in GetComponents<IManagedDestroy>())
			{
				if (!componentsToNotifyOnDestruction.Contains(componentToNotifyOnDestruction))
				{
					componentsToNotifyOnDestruction.Add(componentToNotifyOnDestruction);
				}
			}
			foreach (var lifecycleBoundComponent in GetComponents<ILifecycleBound>())
			{
				lifecycleBoundComponent.LifecycleComponent = this;
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
			foreach (var componentToInitialize in componentsToInitialize)
			{
				if (componentToInitialize.InitializationActions.ContainsKey(phase))
				{
					componentToInitialize.InitializationActions[phase].Invoke();
				}
			}
		}

		public void SynchronizedUpdate(UpdatePhases phase)
		{
			if (isActiveAndEnabled)
			{
				foreach (var componentToUpdate in componentsToUpdate)
				{
					if (componentToUpdate.UpdateActions.ContainsKey(phase))
					{
						componentToUpdate.UpdateActions[phase].Invoke();
					}
				}
			}
		}

		private void OnDestroy()
		{			
			foreach (var componentToNotifyOnDestruction in componentsToNotifyOnDestruction)
			{
				componentToNotifyOnDestruction.ManagedDestruction();
			}
			GlobalOnLifecycleComponentDestroyed?.Invoke(this);
			OnLifecycleComponentDestroyed?.Invoke(this);
		}
	}
}