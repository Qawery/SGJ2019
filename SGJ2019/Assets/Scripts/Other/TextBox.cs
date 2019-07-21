using UnityEngine;
using UnityEngine.Assertions;


namespace SGJ2019
{
	public class TextBox : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI text = null;


		private void Start()
		{
			Assert.IsNotNull(text);
			SetText("");
		}

		public void SetText(string newText)
		{
			text.text = newText;
			if (newText.Length == 0)
			{
				gameObject.SetActive(false);
			}
			else
			{
				gameObject.SetActive(true);
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