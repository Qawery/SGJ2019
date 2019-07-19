using UnityEngine;


namespace SGJ2019
{
	public class Test : MonoBehaviour
	{
		private void Awake()
		{
			Debug.Log("Awake");
		}

		private void OnEnable()
		{
			Debug.Log("OnEnable");
		}

		private void Start()
		{
			Debug.Log("Start");
		}

		private void Update()
		{
			Debug.Log("Update");
		}

		private void LateUpdate()
		{
			Debug.Log("LateUpdate");
		}

		private void OnDisable()
		{
			Debug.Log("OnDisable");
		}

		private void OnDestroy()
		{
			Debug.Log("OnDestroy");
		}
	}
}