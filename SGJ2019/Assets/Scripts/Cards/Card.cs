using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	[DisallowMultipleComponent]
	public abstract class Card : MonoBehaviour, IManagedInitialization, IManagedDestroy
	{
		[SerializeField] private TMPro.TextMeshProUGUI cardName = null;
		public string Name => cardName.text;
		[SerializeField] private TMPro.TextMeshProUGUI description = null;
		public string Description => description.text;
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

		protected abstract void OnTurnEnd();
	}
}