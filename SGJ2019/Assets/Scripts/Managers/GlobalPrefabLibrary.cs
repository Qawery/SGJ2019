using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public class GlobalPrefabLibrary : SimpleSingleton<GlobalPrefabLibrary>
	{
		[SerializeField] private Camera mainCamera = null;
		public Camera MainCamera => mainCamera;
		public List<Card> cardPrefabs = new List<Card>();


		protected override void Awake()
		{
			if (Instance != this)
			{
				Destroy(gameObject);
				return;
			}
			Assert.IsNotNull(mainCamera);
			Assert.IsNotNull(cardPrefabs);
			Assert.IsTrue(cardPrefabs.Count > 0);
		}
	}
}