using UnityEngine;
using UnityEngine.Assertions;


namespace SGJ2019
{
	public class AttackAction : Action
	{
		public override string Name => "Attack";
		public override string Description => "Damage: " + Damage.ToString() + "\n" +
												"Range: " + MinRange.ToString() + (MinRange == MaxRange ? "" : "-" + MaxRange.ToString()) + "\n" +
												base.Description;
		public override int Cost => 1;
		public virtual int Damage => 1;
		public virtual int MinRange => 1;
		public virtual int MaxRange => 1;


		public AttackAction()
		{
			Assert.IsTrue(MinRange > 0);
			Assert.IsTrue(MaxRange > 0);
			Assert.IsTrue(MaxRange >= MinRange);
		}

		public override void ExecuteAction(CardSlot source, CardSlot target)
		{
			var playerCard = (source.Card as PlayerOwnedCard);
			if (playerCard.ActionPointsComponent.CurrentActionPoints < Cost)
			{
				return;
			}
			var targetHealthComponent = target.Card.GetComponent<HealthComponent>();
			if (targetHealthComponent == null)
			{
				return;
			}
			var rowManager = source.transform.parent.GetComponent<RowManager>();
			if (IsTargetOutsideRange(rowManager, source, target))
			{
				return;
			}
			playerCard.ActionPointsComponent.Spend(Cost);
			targetHealthComponent.Damage(Damage);
		}

		private bool IsTargetOutsideRange(RowManager rowManager, CardSlot source, CardSlot target)
		{
			int selectedIndex = rowManager.GetIndexOfCard(source.Card);
			int otherIndex = rowManager.GetIndexOfCard(target.Card);
			int distance = Mathf.Abs(selectedIndex - otherIndex);
			if (distance >= MinRange && distance <= MaxRange)
			{
				return false;
			}
			return true;
		}
	}
}