﻿using System.Collections.Generic;
using UnityEngine;


namespace SGJ2019
{
	[RequireComponent(typeof(Camera))]
	public class CameraController : MonoBehaviour, IManagedInitialization, IManagedUpdate
	{
		private Camera camera = null;
		[SerializeField] private float panSpeed = 100.0f;
		[SerializeField] private float maxPositionOffsetX = 100.0f;
		[SerializeField] private float maxPositionOffsetY = 100.0f;
		[SerializeField] private float zoomSpeed = 100.0f;
		[SerializeField] private float minOrthographicSize = 10.0f;
		[SerializeField] private float maxOrthographicSize = 1000.0f;


		public Dictionary<InitializationPhases, System.Action> InitializationActions =>
			new Dictionary<InitializationPhases, System.Action>()
			{
				[InitializationPhases.FIRST] = ManagedInitialize
			};

		public Dictionary<UpdatePhases, System.Action> UpdateActions => updateActions;
		private Dictionary<UpdatePhases, System.Action> updateActions = new Dictionary<UpdatePhases, System.Action>();


		private void ManagedInitialize()
		{
			updateActions.Add(UpdatePhases.FIRST, ManagedUpdate);
			camera = GetComponent<Camera>();
		}

		private void ManagedUpdate()
		{
			camera.orthographicSize += Input.GetAxis("Zoom") * zoomSpeed;
			camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minOrthographicSize, maxOrthographicSize);
			Vector3 newCameraPosition = transform.position;
			newCameraPosition.x += Input.GetAxis("MainHorizontal") * zoomSpeed;
			newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, -maxPositionOffsetX, maxPositionOffsetX);
			newCameraPosition.y += Input.GetAxis("MainVertical") * zoomSpeed;
			newCameraPosition.y = Mathf.Clamp(newCameraPosition.y, -maxPositionOffsetY, maxPositionOffsetY);
			transform.position = newCameraPosition;
		}
	}
}