using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


namespace SGJ2019
{
	[DisallowMultipleComponent]
	public class Card : MonoBehaviour
	{
		[SerializeField] private string cardId = "";
		[SerializeField] private TMPro.TextMeshProUGUI title = null;
		[SerializeField] private Image image = null;
		[SerializeField] private TMPro.TextMeshProUGUI description = null;


		public string CardId => cardId;


		private void Awake()
		{
			Assert.IsNotNull(title);
			Assert.IsNotNull(image);
			Assert.IsNotNull(description);
		}

		public void SetCardData(CardData cardData)
		{
			title.text = cardData.title;

			description.text = cardData.description;
		}
	}
}