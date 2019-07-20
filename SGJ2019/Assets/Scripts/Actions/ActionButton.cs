using UnityEngine;
using UnityEngine.Assertions;


namespace SGJ2019
{
	public class ActionButton : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI titleText = null;
		private int actionIndex = 0;


		private void Start()
		{
			Assert.IsNotNull(titleText);
		}

		public void SetActionIndex(int newActionIndex)
		{
			if (newActionIndex < 0)
			{
				Destroy(gameObject);
			}
			else
			{
				actionIndex = newActionIndex;
				var availableActions = InputManager.Instance.GetAvailableActions();
				Assert.IsTrue(actionIndex >= 0 && actionIndex < availableActions.Count);
				titleText.text = availableActions[actionIndex].Name;
			}
		}

		public void OnClicked()
		{
			InputManager.Instance.ActionSelected(actionIndex);
		}
	}
}