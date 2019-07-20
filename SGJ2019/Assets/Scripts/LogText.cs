using UnityEngine;


namespace SGJ2019
{
	public class LogText : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI text;


		private void Start()
		{
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
	}
}