using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	[DisallowMultipleComponent]
	public abstract class Card : MonoBehaviour, IManagedInitialization, IManagedDestroy, ILifecycleBound
	{
		[SerializeField] private TMPro.TextMeshProUGUI cardName = null;
		public string CardName => cardName.text;
		[SerializeField] protected TMPro.TextMeshProUGUI description = null;
		[SerializeField] protected OwnerPhase ownership = OwnerPhase.HUMAN;
		private LifecycleComponent lifecycleComponent = null;


		public LifecycleComponent LifecycleComponent
		{
			get
			{
				return lifecycleComponent;
			}

			set
			{
				if (lifecycleComponent == null)
				{
					lifecycleComponent = value;
				}
			}
		}

		public OwnerPhase Ownership => ownership;

		public Dictionary<InitializationPhases, System.Action> InitializationActions =>
			new Dictionary<InitializationPhases, System.Action>()
			{
				[InitializationPhases.LATE] = ManagedInitialize
			};


		protected virtual void ManagedInitialize()
		{
			Assert.IsNotNull(cardName);
			Assert.IsNotNull(description);
			TurnManager.Instance.OnTurnEnd += OnTurnEnd;
		}

		public virtual void ManagedDestruction()
		{
			if (TurnManager.Instance != null)
			{
				TurnManager.Instance.OnTurnEnd -= OnTurnEnd;
			}
		}

		protected virtual void OnTurnEnd()
		{
		}
	}
}