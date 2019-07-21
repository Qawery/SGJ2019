using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


namespace SGJ2019
{
	[RequireComponent(typeof(Button))]
	public class ActionButton : MonoBehaviour
	{
		private Button button;
		private ColorBlock defaultColorBlock;
		private ColorBlock selectedColorBlock;
		[SerializeField] private TMPro.TextMeshProUGUI titleText = null;
		private int actionIndex = -1;


		private void Start()
		{
			Assert.IsNotNull(titleText);
			button = GetComponent<Button>();
			defaultColorBlock = button.colors;
			selectedColorBlock = button.colors;
			selectedColorBlock.normalColor = selectedColorBlock.pressedColor;
			InputManager.Instance.OnSelectedActionIndexChange += OnSelectedActionIndexChange;
			OnSelectedActionIndexChange();
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

		private void OnSelectedActionIndexChange()
		{
			if (InputManager.Instance.SelectedActionIndex == actionIndex)
			{
				button.colors = selectedColorBlock;
			}
			else
			{
				button.colors = defaultColorBlock;
			}
		}

		public void OnClicked()
		{
			InputManager.Instance.ActionSelected(actionIndex);
		}

		private void OnDestroy()
		{
			if (InputManager.Instance != null)
			{
				InputManager.Instance.OnSelectedActionIndexChange -= OnSelectedActionIndexChange;
			}
		}
	}
}