using UnityEngine;
using UnityEngine.Assertions;


namespace SGJ2019
{
	public class SimpleSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T instance = null;


		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					var foundSingletons = FindObjectsOfType<T>();
					Assert.IsTrue(foundSingletons.Length == 1, "Inappriopriate number of singletons on scene: " + foundSingletons.Length);
					instance = foundSingletons[0];
					DontDestroyOnLoad(instance.gameObject);
				}
				return instance;
			}

			private set
			{
				Assert.IsNull(instance, "Singleton instance already set to " + instance.gameObject.name);
				instance = value;
			}
		}

		protected virtual void Awake()
		{
			if (Instance != this)
			{
				Destroy(gameObject);
				return;
			}
		}
	}
}