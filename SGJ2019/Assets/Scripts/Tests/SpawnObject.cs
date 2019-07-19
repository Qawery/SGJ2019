using UnityEngine;


namespace SGJ2019
{
	public class SpawnObject : MonoBehaviour
	{
		[SerializeField] private float time = 1.0f;
		[SerializeField] private GameObject prefab;


		private void Update()
		{
			if (time >= 0.0f)
			{
				time -= Time.deltaTime;
				if (time < 0.0f)
				{
					var newObject = Instantiate(prefab);
					Debug.Log(newObject.name + " created");
				}
			}
		}
	}
}