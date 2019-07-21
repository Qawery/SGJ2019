using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


namespace SGJ2019
{
	public class TextBox : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI text = null;
		private Image background = null;


		private void Start()
		{
			Assert.IsNotNull(text);
			background = GetComponent<Image>();
			text.raycastTarget = false;
			SetText("");
		}

		public void SetText(string newText)
		{
			text.text = newText;
			if (background != null)
			{
				if (newText.Length > 0)
				{
					background.enabled = true;
				}
				else
				{
					background.enabled = false;
				}
			}
		}

		public void SetText(string newText, Color color)
		{
			SetText(newText);
			SetColor(color);
		}

		public void SetColor(Color color)
		{
			text.color = color;
		}
	}
}