using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;


namespace SGJ2019
{
	public class LifecycleManager : MonoBehaviour
	{
		public enum InitializationPhases
		{
			First, Last
		}


		public enum UpdatePhases
		{
			First, Last
		}


		private static LifecycleManager instance = null;
		private InitializationPhases currentInitializationPhase = (InitializationPhases) 0;
		private HashSet<LifecycleComponent> registeredLifecycleComponents = new HashSet<LifecycleComponent>();


		public static LifecycleManager Instance
		{
			get
			{
				if (instance == null)
				{
					var foundLifecycleManagers = FindObjectsOfType<LifecycleManager>();
					Assert.IsTrue(foundLifecycleManagers.Length > 0, "No LifecycleManager on scene");
					instance = foundLifecycleManagers[0];
				}
				return instance;
			}

			private set
			{
				instance = value;
			}
		}


		private void Awake()
		{
			if (instance != null && instance != this)
			{
				Destroy(gameObject);
				return;
			}
			instance = this;
			var allLifecycleComponents = Resources.FindObjectsOfTypeAll<LifecycleComponent>();
			foreach (var foundLifecycleComponent in allLifecycleComponents)
			{
				TryRegisterLifecycleComponent(foundLifecycleComponent);
			}
			/*
			for(int i = 0; i < Enum.GetValues(typeof(InitializationPhases)).Length - 1; i++)
			{
				
			}
			*/
		}

		private void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
		}

		public void TryRegisterLifecycleComponent(LifecycleComponent lifecycleComponent)
		{
			if (!registeredLifecycleComponents.Contains(lifecycleComponent))
			{
				registeredLifecycleComponents.Add(lifecycleComponent);
			}
		}
	}
}