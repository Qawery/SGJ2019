using UnityEngine;


namespace SGJ2019
{
	public static class Utilities
	{
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