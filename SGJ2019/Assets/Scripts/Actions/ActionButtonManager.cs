using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


namespace SGJ2019
{
	[RequireComponent(typeof(Image))]
	public class ActionButtonManager : MonoBehaviour
	{
		[SerializeField] private ActionButton actionButtonPrefab = null;
		private ActionButton[] buttons = null;

		private void Awake()
		{
			Assert.IsNotNull(actionButtonPrefab);
			GetComponent<Image>().enabled = false;
			int neededButtons = 4;
			while (neededButtons > 0)
			{
				Instantiate(actionButtonPrefab, transform);
				--neededButtons;
			}
			buttons = GetComponentsInChildren<ActionButton>();
			for (int i = 0; i < buttons.Length; ++i)
			{
				buttons[i].SetActionIndex(-1);
			}
		}

		private void Start()
		{
			InputManager.Instance.OnCardSlotSelectionChange += OnCardSlotSelectionChange;
		}

		private void OnCardSlotSelectionChange(CardSlot previous, CardSlot current)
		{
			if (InputManager.Instance != null)
			{
				var availableActions = InputManager.Instance.GetAvailableActions();
				if (availableActions.Count > 0)
				{
					for (int i = 0; i < buttons.Length; ++i)
					{
						if (i >= availableActions.Count)
						{
							buttons[i].SetActionIndex(-1);
						}
						else
						{
							buttons[i].SetActionIndex(i);
						}
					}
					GetComponent<Image>().enabled = true;
				}
				else
				{
					for (int i = 0; i < buttons.Length; ++i)
					{
						buttons[i].SetActionIndex(-1);
					}
					GetComponent<Image>().enabled = false;
				}
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