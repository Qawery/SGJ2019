using UnityEngine;
using UnityEngine.SceneManagement;


namespace SGJ2019
{
	public class MainMenu : MonoBehaviour
	{
		public void StartGame()
		{
			SceneManager.LoadScene("GameScene");
		}


		public void QuitGame()
		{
			Application.Quit();
		}
	}
}