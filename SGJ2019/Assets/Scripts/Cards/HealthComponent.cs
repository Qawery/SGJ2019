using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public class HealthComponent : MonoBehaviour, IManagedInitialization
	{
		[SerializeField] private int maxHealth = 2;
		public int MaxHealth => maxHealth;
		private int currentHealth = 0;
		public int CurrentHealth => currentHealth;


		public Dictionary<InitializationPhases, System.Action> InitializationActions =>
			new Dictionary<InitializationPhases, System.Action>()
			{
				[InitializationPhases.EARLY] = ManagedInitialize
			};
		

		private void ManagedInitialize()
		{
			currentHealth = maxHealth;
		}

		public void Damage(int ammount)
		{
			Assert.IsTrue(ammount > 0);
			currentHealth = Mathf.Max(0, currentHealth - ammount);
			if (currentHealth <= 0)
			{
				Destroy(gameObject);
			}
			if (currentHealth > 0)
			{
				Utilities.SpawnFloatingText(ammount.ToString(), Color.red, transform);
			}
			else
			{
				Utilities.SpawnFloatingText(ammount.ToString(), Color.red, transform.position);
			}
		}

		public void Heal(int ammount)
		{
			Assert.IsTrue(ammount > 0);
			currentHealth = Mathf.Min(currentHealth + ammount, maxHealth);
			Utilities.SpawnFloatingText(ammount.ToString(), Color.green, transform);
		}

		public void ForceSetCurrentHealth(int ammount)
		{
			Assert.IsTrue(ammount >= 0 && ammount <= maxHealth);
			currentHealth = ammount;
			if (currentHealth == 0)
			{
				Destroy(gameObject);
			}
		}
	}
}