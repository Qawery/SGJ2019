using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


namespace SGJ2019
{
	[RequireComponent(typeof(HealthComponent))]
	public class Monster : AIManagedCard
	{
		private const int MULTIPLY_HEALTH_THRESHOLD = 1;
		[SerializeField] private int minDamage = 1;
		[SerializeField] private int maxDamage = 2;
		[SerializeField] private int healChance = 30;
		[SerializeField] private int multiplyChance = 5;
		[SerializeField] private Image leftArrow = null;
		[SerializeField] private Image rightArrow = null;
		private bool facingLeft = false;
		protected HealthComponent healthCompnent = null;
		public HealthComponent HealthCompnent => healthCompnent;


		private bool FacingLeft
		{
			get
			{
				return facingLeft;
			}

			set
			{
				facingLeft = value;
				if (facingLeft)
				{
					leftArrow.enabled = true;
					rightArrow.enabled = false;
				}
				else
				{
					leftArrow.enabled = false;
					rightArrow.enabled = true;
				}
			}
		}


		protected override void ManagedInitialize()
		{
			base.ManagedInitialize();
			healthCompnent = GetComponent<HealthComponent>();
			Assert.IsTrue(minDamage >= 0);
			Assert.IsTrue(maxDamage >= 1);
			Assert.IsTrue(maxDamage >= minDamage);
			Assert.IsTrue(healChance >= 0 && healChance <= 100);
			Assert.IsTrue(multiplyChance >= 0 && multiplyChance <= 100);
			Assert.IsTrue(ownership == OwnerPhase.ENEMY);
			FacingLeft = Random.Range(0.0f, 1.0f) >= 0.5f;
		}

		public override void ExecuteTurn()
		{
			var row = GetComponentInParent<RowManager>();
			var slot = GetComponentInParent<CardSlot>();
			int ourIndex = row.GetIndexOfCard(this);
			
			//Attacking
			var nearCard = FacingLeft ? row.GetCardOnLeftOfIndex(ourIndex) : row.GetCardOnRightOfIndex(ourIndex);
			if (nearCard == null || nearCard.Ownership != OwnerPhase.HUMAN)
			{
				nearCard = FacingLeft ? row.GetCardOnRightOfIndex(ourIndex) : row.GetCardOnLeftOfIndex(ourIndex);
				if (nearCard != null && nearCard.Ownership == OwnerPhase.HUMAN)
				{
					FacingLeft = !FacingLeft;
				}
			}
			if (nearCard != null && nearCard.Ownership == OwnerPhase.HUMAN)
			{
				var soldier = nearCard as PlayerSoldier;
				Assert.IsNotNull(soldier);
				int damage = minDamage + (int)Mathf.Round((maxDamage - minDamage) * Random.Range(0.0f, 1.0f));
				soldier.HealthCompnent.Damage(damage);
				Utilities.SpawnFloatingText("Attack", Color.grey, transform);
				LogManager.Instance.AddMessage(CardName + " attacked " + soldier.CardName + " for " + damage.ToString() + " damage");
				executionState = CardExecutionState.DONE;
				return;
			}

			//Healing
			if (healthCompnent.CurrentHealth < healthCompnent.MaxHealth && healChance > 0 && Random.Range(0, 100) < healChance)
			{
				Utilities.SpawnFloatingText("Heal", Color.grey, transform);
				healthCompnent.Heal(1 + (int)Mathf.Round(Random.Range(0.0f, 1.0f)));
			}


			//Multiplying
			if (healthCompnent.CurrentHealth >= MULTIPLY_HEALTH_THRESHOLD && multiplyChance > 0 && Random.Range(0, 100) < multiplyChance)
			{
				var clone = Instantiate(this);
				row.AddCardToRowOnIndex(ourIndex, clone);
				healthCompnent.Damage(healthCompnent.CurrentHealth / 2);
				multiplyChance /= 2;
				clone.ForcePass();
				clone.multiplyChance = multiplyChance / 2;
				clone.GetComponent<HealthComponent>().ForceSetCurrentHealth(healthCompnent.CurrentHealth);
				Utilities.SpawnFloatingText("Multiply", Color.grey, transform);
			}

			//Movement
			if (FacingLeft && ourIndex == 0)
			{
				FacingLeft = !FacingLeft;
			}
			else if (!FacingLeft && ourIndex == row.AllSlots.Length - 1)
			{
				FacingLeft = !FacingLeft;
			}
			nearCard = FacingLeft ? row.GetCardOnLeftOfIndex(ourIndex) : row.GetCardOnRightOfIndex(ourIndex);
			if (nearCard != null)
			{
				var otherMonster = nearCard.GetComponent<Monster>();
				if (otherMonster != null && otherMonster.ExecutionState == CardExecutionState.READY)
				{
					executionState = CardExecutionState.WAITING;
				}
				else
				{
					int destinationIndex = row.GetIndexOfCard(nearCard);
					if (ourIndex < destinationIndex)
					{
						row.MoveCardRight(this);
					}
					else if (ourIndex > destinationIndex)
					{
						row.MoveCardLeft(this);
					}
					Utilities.SpawnFloatingText("Move", Color.grey, transform);
				}
			}
			else
			{
				Utilities.SpawnFloatingText("Wait", Color.grey, transform);
				LogManager.Instance.AddMessage(this.CardName + " is waiting");
			}
			executionState = CardExecutionState.DONE;
		}

		private void Update()
		{
			description.text =
				"Health: " + healthCompnent.CurrentHealth.ToString() + " / " + healthCompnent.MaxHealth.ToString() + "\n" +
				"Damage: " + minDamage.ToString() + ((minDamage == maxDamage) ? "" : "-" + maxDamage.ToString()) + "\n" +
				"Heal chance: " + healChance.ToString() + "\n" +
				"Multiply chance: " + multiplyChance.ToString();
		}
	}
}