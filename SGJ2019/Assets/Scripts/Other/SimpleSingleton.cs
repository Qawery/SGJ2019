using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public class SimpleSingleton<T> : MonoBehaviour, IManagedInitialization, IManagedDestroy where T : MonoBehaviour
	{
		private static bool isSceneBeingReloaded = false;
		private static T instance = null;


		public virtual Dictionary<InitializationPhases, System.Action> InitializationActions =>
				new Dictionary<InitializationPhases, System.Action>()
				{
					[InitializationPhases.SINGLETON_SETUP] = ManagedInitialize
				};
		

		public static T Instance
		{
			get
			{
				if (isSceneBeingReloaded)
				{
					return null;
				}
				if (instance == null)
				{
					var foundSingletons = FindObjectsOfType<T>();
					Assert.IsTrue(foundSingletons.Length == 1, "Inappriopriate number of singletons on scene: " + foundSingletons.Length);
					instance = foundSingletons[0];
				}
				return instance;
			}

			private set
			{
				if (value != null)
				{
					Assert.IsNull(instance);
				}
				else
				{
					Assert.IsNotNull(instance); ;
				}
				instance = value;
			}
		}


		private void Awake()
		{
			if (instance == null)
			{
				isSceneBeingReloaded = false;
			}
		}

		protected virtual void ManagedInitialize()
		{
			if (Instance != this)
			{
				Destroy(gameObject);
				return;
			}
		}

		public virtual void ManagedDestruction()
		{
			if (Instance == this)
			{
				isSceneBeingReloaded = true;
				Instance = null;
			}
		}
	}
}