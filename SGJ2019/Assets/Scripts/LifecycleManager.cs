using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;


namespace SGJ2019
{
	public enum InitializationPhases
	{
		FIRST, LAST
	}


	public enum UpdatePhases
	{
		FIRST, LAST
	}


	public class LifecycleManager : MonoBehaviour
	{
		private static LifecycleManager instance = null;
		private List<LifecycleComponent> registeredLifecycleComponents = new List<LifecycleComponent>();
		private List<LifecycleComponent> pendingToAddComponents = new List<LifecycleComponent>();
		private List<GameObject> pendingToDestroyObjects = new List<GameObject>();


		public static LifecycleManager Instance
		{
			get
			{
				if (instance == null)
				{
					var foundLifecycleManagers = FindObjectsOfType<LifecycleManager>();
					Assert.IsTrue(foundLifecycleManagers.Length == 1, "Inappriopriate number of LifecycleManagers: " + foundLifecycleManagers.Length);
					instance = foundLifecycleManagers[0];
					Resources.FindObjectsOfTypeAll<>();
				}
				return instance;
			}

			private set
			{
				Assert.IsNull(instance, "LifecycleManager instance already set to " + instance.gameObject.name);
				instance = value;
			}
		}

		private void Awake()
		{
			if (Instance != this)
			{
				Destroy(gameObject);
				return;
			}
			DontDestroyOnLoad(this.gameObject);
			SceneManager.sceneLoaded += SceneLoaded;
		}

		private void SceneLoaded(Scene scene, LoadSceneMode loadMode)
		{
			for (int i = 0; i < Enum.GetValues(typeof(InitializationPhases)).Length - 1; i++)
			{
				foreach (var registeredLifecycleComponent in registeredLifecycleComponents)
				{
					if (!IsMarkedForDestruction(registeredLifecycleComponent.gameObject))
					{
						registeredLifecycleComponent.SynchronizedInitialize((InitializationPhases)i);
					}
				}
			}
		}

		private void Update()
		{
			if (pendingToAddComponents.Count > 0)
			{
				List<LifecycleComponent> workingComponents = new List<LifecycleComponent>();
				workingComponents.AddRange(pendingToAddComponents);
				pendingToAddComponents.Clear();
				for (int i = 0; i < Enum.GetValues(typeof(InitializationPhases)).Length - 1; i++)
				{
					foreach (var workingComponent in workingComponents)
					{
						if (!IsMarkedForDestruction(workingComponent.gameObject))
						{
							workingComponent.SynchronizedInitialize((InitializationPhases) i);
						}
					}
				}
				foreach (var workingComponent in workingComponents)
				{
					if (!IsMarkedForDestruction(workingComponent.gameObject))
					{
						registeredLifecycleComponents.Add(workingComponent);
					}
				}
			}
			for (int i = 0; i < Enum.GetValues(typeof(UpdatePhases)).Length - 1; i++)
			{
				foreach (var registeredLifecycleComponent in registeredLifecycleComponents)
				{
					if (!IsMarkedForDestruction(registeredLifecycleComponent.gameObject))
					{
						registeredLifecycleComponent.SynchronizedUpdate((UpdatePhases) i);
					}
				}
			}
			for (int i = 0; i < pendingToDestroyObjects.Count;)
			{
				if (pendingToDestroyObjects[i] != null)
				{
					DestroyObject(pendingToDestroyObjects[i]);
					++i;
				}
				else
				{
					pendingToDestroyObjects.RemoveAt(i);
				}
			}
		}

		#region Instantiation and destruction
		public GameObject Instantiate(GameObject original)
		{
			return Instantiate(original, Vector3.zero, Quaternion.identity, null);
		}

		public GameObject Instantiate(GameObject original, Transform parent)
		{
			return Instantiate(original, Vector3.zero, Quaternion.identity, parent);
		}

		public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, Transform parent = null)
		{
			Assert.IsNotNull(original);
			GameObject instantiatedObject;
			instantiatedObject = GameObject.Instantiate(original, position, rotation, parent);
			Assert.IsNotNull(instantiatedObject);
			pendingToAddComponents.AddRange(instantiatedObject.GetComponentsInChildren<LifecycleComponent>());
			return instantiatedObject;
		}

		public bool IsLifecycleComponentRegistered(LifecycleComponent lifecycleComponent)
		{
			Assert.IsNotNull(lifecycleComponent);
			return pendingToAddComponents.Contains(lifecycleComponent) || registeredLifecycleComponents.Contains(lifecycleComponent);
		}

		public bool IsMarkedForDestruction(GameObject objectToChecked)
		{
			Assert.IsNotNull(objectToChecked);
			return pendingToDestroyObjects.Contains(objectToChecked);
		}

		public void MarkForDestruction(GameObject objectToDestroy)
		{
			Assert.IsNotNull(objectToDestroy);
			pendingToDestroyObjects.Add(objectToDestroy);
		}

		/*
		public void MarkToDestroy(GameObject objectToDestroy)
		{
			foreach (var lifecycleComponent in objectToDestroy.GetComponentsInChildren<LifecycleComponent>())
			{
				MarkToDestroy(lifecycleComponent);
			}
		}

		private void MarkToDestroy(LifecycleComponent lifecycleComponent)
		{
			if (registeredLifecycleComponents.Contains(lifecycleComponent))
			{
				pendingToDestroyComponents.Add(lifecycleComponent);
			}
			else if (pendingToAddComponents.Contains(lifecycleComponent))
			{
				pendingToAddComponents.Remove(lifecycleComponent);
			}
			else
			{
				Assert.IsTrue(false);
			}
		}
		*/
		#endregion
	}
}
