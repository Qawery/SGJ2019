using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public static class Utilities
	{
		public static GameObject Instantiate(GameObject original)
		{
			return Instantiate(original, Vector3.zero, Quaternion.identity, null);
		}

		public static GameObject Instantiate(GameObject original, Transform parent)
		{
			return Instantiate(original, Vector3.zero, Quaternion.identity, parent);
		}

		public static GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, Transform parent = null)
		{
			Assert.IsNotNull(original, "Trying to create null object!");
			GameObject instantiatedObject;
			instantiatedObject = GameObject.Instantiate(original, position, rotation, parent);
			Assert.IsNotNull(instantiatedObject, "Copy of " + original.name + " not created!");
			return instantiatedObject;
		}

		public static T FindObjectInUpwardHiearchy<T>(GameObject firstObject) where T : MonoBehaviour
		{
			T result = null;
			Transform currentTransform = firstObject.transform;
			while (currentTransform != null && result == null)
			{
				result = currentTransform.gameObject.GetComponent<T>();
				currentTransform = currentTransform.parent;
			}
			return result;
		}

		public static List<T> FindObjectsInUpwardHiearchy<T>(GameObject firstObject)
		{
			List<T> results = new List<T>();
			Transform currentTransform = firstObject.transform;
			while (currentTransform != null)
			{
				var foundComponents = currentTransform.gameObject.GetComponents<T>();
				if (foundComponents.Length > 0)
				{
					results.AddRange(foundComponents);
				}
				currentTransform = currentTransform.parent;
			}
			return results;
		}
	}
}