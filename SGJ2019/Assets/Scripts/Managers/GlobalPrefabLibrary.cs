using UnityEngine;
using UnityEngine.Assertions;


namespace SGJ2019
{
	public class GlobalPrefabLibrary : SimpleSingleton<GlobalPrefabLibrary>
	{
		[SerializeField] private CardsLibrary cardsLibrary = null;
		public CardsLibrary CardsLibrary => cardsLibrary;


		protected override void Awake()
		{
			if (Instance != this)
			{
				Destroy(gameObject);
				return;
			}
			Assert.IsNotNull(cardsLibrary);
		}
	}
}