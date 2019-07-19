using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	[DisallowMultipleComponent]
	public class LifecycleComponent : MonoBehaviour
	{
		private List<ILifecycleBound> boundObjects;


		private void Awake()
		{			
			AttachToDependantComponents(gameObject);
		}

		private void Start()
		{
			Assert.IsTrue(LifecycleManager.Instance.IsLifecycleComponentRegistered(this));
		}

		private void AttachToDependantComponents(GameObject analyzedObject)
		{
			var dependantComponents = GetComponents<ILifecycleBound>();
			foreach (var dependantComponent in dependantComponents)
			{
				boundObjects.Add(dependantComponent);
			}
			for (int i = 0; i < analyzedObject.transform.childCount; ++i)
			{
				if (analyzedObject.transform.GetChild(i).gameObject.GetComponent<LifecycleComponent>() == null)
				{
					AttachToDependantComponents(analyzedObject.transform.GetChild(i).gameObject);
				}
			}
		}

		public void SynchronizedInitialize(InitializationPhases phase)
		{
			foreach (var boundObject in boundObjects)
			{
				boundObject.InitializationPhase(phase);
			}
		}

		public void SynchronizedUpdate(UpdatePhases phase)
		{
			foreach (var boundObject in boundObjects)
			{
				boundObject.UpdatePhase(phase);
			}
		}

		private void OnDestroy()
		{
			Assert.IsTrue(LifecycleManager.Instance.IsMarkedForDestruction(gameObject));
		}
	}
}