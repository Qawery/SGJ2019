using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public class ActionPointsComponent : MonoBehaviour, IManagedInitialization, IManagedDestroy
	{
		[SerializeField] private int maxActionPoints = 2;
		public int MaxActionPoints => maxActionPoints;
		[SerializeField] private int currentActionPoints = 2;
		public int CurrentActionPoints => currentActionPoints;


		public Dictionary<InitializationPhases, System.Action> InitializationActions =>
				new Dictionary<InitializationPhases, System.Action>()
				{
					[InitializationPhases.FIRST] = ManagedInitialize
				};
		

		private void ManagedInitialize()
		{
			currentActionPoints = maxActionPoints;
			TurnManager.Instance.OnTurnEnd += OnTurnEnd;
		}

		public void ManagedDestruction()
		{
			if (TurnManager.Instance != null)
			{
				TurnManager.Instance.OnTurnEnd -= OnTurnEnd;
			}
		}

		private void OnTurnEnd()
		{
			currentActionPoints = maxActionPoints;
		}

		public void Spend(int ammount)
		{
			Assert.IsTrue(ammount > 0);
			currentActionPoints = Mathf.Max(0, currentActionPoints - ammount);
		}
	}
}