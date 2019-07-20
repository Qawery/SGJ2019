using UnityEngine;


namespace SGJ2019
{
	public class EnumOverflow : MonoBehaviour
	{
		private InitializationPhases test = InitializationPhases.FIRST;


		private void Update()
		{
			Debug.Log(test);
			++test;
		}
	}
}