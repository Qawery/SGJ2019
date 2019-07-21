using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public static class Utilities
	{
		public static T FindObjectInUpwardHiearchy<T>(GameObject firstObject) where T : MonoBehaviour
		{
			Assert.IsNotNull(firstObject);
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
			Assert.IsNotNull(firstObject);
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

		public static FloatingText SpawnFloatingText(string text, Color color, Transform parent)
		{		
			var floatingText = SpawnFloatingText(text, color, parent.transform.position);
			floatingText.transform.SetParent(parent, true);
			return floatingText;
		}

		public static FloatingText SpawnFloatingText(string text, Color color, Vector3 worldPosition)
		{
			var floatingText = Object.Instantiate(GlobalPrefabLibrary.Instance.FloatingNumber).GetComponent<FloatingText>();
			floatingText.Text.text = text;
			floatingText.Text.color = color;
			floatingText.transform.position = worldPosition;
			return floatingText;
		}
	}
}