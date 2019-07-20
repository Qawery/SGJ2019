using UnityEngine;
using UnityEngine.SceneManagement;


namespace SGJ2019
{
	public class LoadSceneOnDelay : MonoBehaviour
	{
		[SerializeField] private float time = 1.0f;
		[SerializeField] private string sceneName = "";


		private void Update()
		{
			if (time >= 0.0f)
			{
				time -= Time.deltaTime;
				if (time < 0.0f)
				{
					SceneManager.LoadScene(sceneName);
					Debug.Log(sceneName + " loaded");
				}
			}
		}
	}
}