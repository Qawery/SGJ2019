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
		private List<LifecycleComponent> lifecycleComponents = new List<LifecycleComponent>();
		private List<LifecycleComponent> pendingComponents = new List<LifecycleComponent>();
		private bool running = false;

		public static LifecycleManager Instance
		{
			get
			{
				if (instance == null)
				{
					var foundLifecycleManagers = FindObjectsOfType<LifecycleManager>();
					Assert.IsTrue(foundLifecycleManagers.Length == 1, "Inappriopriate number of LifecycleManagers: " + foundLifecycleManagers.Length);
					instance = foundLifecycleManagers[0];
					instance.Initialize();
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
		}

		private void Initialize()
		{
			DontDestroyOnLoad(this.gameObject);
			SceneManager.sceneLoaded += SceneLoaded;
			LifecycleComponent.OnLifecycleComponentCreated += OnLifecycleComponentCreated;
			LifecycleComponent.OnLifecycleComponentDestroyed += OnLifecycleComponentDestroyed;
		}

		//TODO: Managing persistent components
		private void SceneLoaded(Scene scene, LoadSceneMode loadMode)
		{
			running = true;
			for (int i = 0; i < Enum.GetValues(typeof(InitializationPhases)).Length; ++i)
			{
				foreach (var lifecycleComponent in lifecycleComponents)
				{
					lifecycleComponent.SynchronizedInitialize((InitializationPhases) i);
				}
			}
		}

		private void Update()
		{
			if (pendingComponents.Count > 0)
			{
				List<LifecycleComponent> newComponents = new List<LifecycleComponent>();
				newComponents.AddRange(pendingComponents);
				pendingComponents.Clear();
				for (int i = 0; i < Enum.GetValues(typeof(InitializationPhases)).Length; ++i)
				{
					foreach (var initializedComponent in newComponents)
					{
						initializedComponent.SynchronizedInitialize((InitializationPhases)i);
					}
				}
				foreach (var initializedComponent in newComponents)
				{
					lifecycleComponents.Add(initializedComponent);
				}
				newComponents.Clear();
			}
			for (int i = 0; i < Enum.GetValues(typeof(UpdatePhases)).Length; i++)
			{
				foreach (var lifecycleComponent in lifecycleComponents)
				{
					lifecycleComponent.SynchronizedUpdate((UpdatePhases) i);
				}
			}
		}

		private void OnLifecycleComponentCreated(LifecycleComponent lifecycleComponent)
		{
			if (running)
			{
				pendingComponents.Add(lifecycleComponent);
			}
			else
			{
				lifecycleComponents.Add(lifecycleComponent);
			}
		}

		private void OnLifecycleComponentDestroyed(LifecycleComponent lifecycleComponent)
		{
			lifecycleComponents.Remove(lifecycleComponent);
		}
	}
}
