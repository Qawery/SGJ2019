using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public class SimpleSingleton<T> : MonoBehaviour, IManagedInitialization where T : MonoBehaviour
	{
		private static T instance = null;


		public virtual Dictionary<InitializationPhases, System.Action> InitializationActions =>
				new Dictionary<InitializationPhases, System.Action>()
				{
					[InitializationPhases.FIRST] = ManagedInitialize
				};
		

		public static T Instance
		{
			get
			{
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

		protected virtual void ManagedInitialize()
		{
			if (Instance != this)
			{
				Destroy(gameObject);
				return;
			}
		}

		protected virtual void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}
	}
}