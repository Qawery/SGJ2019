using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	[DisallowMultipleComponent]
	public abstract class Card : MonoBehaviour, IManagedInitialization, IManagedDestroy
	{
		[SerializeField] private TMPro.TextMeshProUGUI cardName = null;
		[SerializeField] protected TMPro.TextMeshProUGUI description = null;
		[SerializeField] protected OwnerPhase ownership = OwnerPhase.HUMAN;


		public OwnerPhase Ownership => ownership;

		public Dictionary<InitializationPhases, System.Action> InitializationActions =>
			new Dictionary<InitializationPhases, System.Action>()
			{
				[InitializationPhases.FIRST] = ManagedInitialize
			};


		protected virtual void ManagedInitialize()
		{
			Assert.IsNotNull(cardName);
			Assert.IsNotNull(description);
			TurnManager.Instance.OnTurnEnd += OnTurnEnd;
		}

		public virtual void ManagedDestruction()
		{
			TurnManager.Instance.OnTurnEnd -= OnTurnEnd;
		}

		protected virtual void OnTurnEnd()
		{
		}
	}
}