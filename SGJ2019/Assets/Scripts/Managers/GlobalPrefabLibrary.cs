﻿using UnityEngine;
using UnityEngine.Assertions;


namespace SGJ2019
{
	public class GlobalPrefabLibrary : SimpleSingleton<GlobalPrefabLibrary>
	{
		[SerializeField] private Camera mainCamera = null;
		public Camera MainCamera => mainCamera;
		[SerializeField] private FloatingNumber floatingNumber = null;
		public FloatingNumber FloatingNumber => floatingNumber;


		protected override void ManagedInitialize()
		{
			base.ManagedInitialize();
			if (Instance == this)
			{
				Assert.IsNotNull(mainCamera);
				Assert.IsNotNull(floatingNumber);
			}
		}
	}
}