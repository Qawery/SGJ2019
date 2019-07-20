using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


namespace SGJ2019
{
	[DisallowMultipleComponent]
	public class Card : MonoBehaviour
	{
		[SerializeField] private string cardId = "";
		public string CardId => cardId;
		[SerializeField] private TMPro.TextMeshProUGUI title = null;
		[SerializeField] private Image image = null;
		[SerializeField] private TMPro.TextMeshProUGUI description = null;


		private void Awake()
		{
			Assert.IsNotNull(title);
			Assert.IsNotNull(image);
			Assert.IsNotNull(description);
		}

		public void SetCardData(string newCardId)
		{
			cardId = newCardId;
			//TODO: Pobieranie wartosci z biblioteki prefabow za pomocą samego Id
		}
	}
}