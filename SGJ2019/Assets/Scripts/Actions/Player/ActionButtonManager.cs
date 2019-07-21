using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


namespace SGJ2019
{
	[RequireComponent(typeof(Image))]
	public class ActionButtonManager : MonoBehaviour
	{
		[SerializeField] private ActionButton actionButtonPrefab = null;


		private void Awake()
		{
			Assert.IsNotNull(actionButtonPrefab);
			GetComponent<Image>().enabled = false;
		}

		private void Start()
		{
			InputManager.Instance.OnCardSlotSelectionChange += OnCardSlotSelectionChange;
		}

		private void OnCardSlotSelectionChange(CardSlot previous, CardSlot current)
		{
			var existingButtons = GetComponentsInChildren<ActionButton>();
			var availableActions = InputManager.Instance.GetAvailableActions();
			if (availableActions.Count > 0)
			{				
				int neededButtons = availableActions.Count - existingButtons.Length;
				while (neededButtons > 0)
				{
					Instantiate(actionButtonPrefab, transform);
					--neededButtons;
				}
				existingButtons = GetComponentsInChildren<ActionButton>();
				for (int i = 0; i < existingButtons.Length; ++i)
				{
					if (i >= availableActions.Count)
					{
						existingButtons[i].SetActionIndex(-1);
					}
					else
					{
						existingButtons[i].SetActionIndex(i);
					}
				}
				GetComponent<Image>().enabled = true;
			}
			else
			{
				for (int i = 0; i < existingButtons.Length; ++i)
				{
					existingButtons[i].SetActionIndex(-1);
				}
				GetComponent<Image>().enabled = false;
			}
		}

		private void OnDestroy()
		{
			if (InputManager.Instance != null)
			{
				InputManager.Instance.OnCardSlotSelectionChange -= OnCardSlotSelectionChange;
			}
		}
	}
}