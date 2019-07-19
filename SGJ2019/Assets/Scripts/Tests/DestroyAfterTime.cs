using UnityEngine;


namespace SGJ2019
{
	public class DestroyAfterTime : MonoBehaviour
	{
		[SerializeField] private float time = 1.0f;


		private void Update()
		{
			if (time >= 0.0f)
			{
				time -= Time.deltaTime;
				if (time < 0.0f)
				{
					Destroy(gameObject);
					Debug.Log(gameObject.name + " destroyed");
				}
			}
		}
	}
}