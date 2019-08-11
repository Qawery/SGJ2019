using UnityEngine;


namespace SGJ2019
{
	public class SoundOnHit : MonoBehaviour
	{
		private void Awake()
		{
			GetComponent<HealthComponent>().OnDamage += OnDamage;
		}

		private void OnDamage()
		{
			AudioManager.Instance.PlaySound("Soldier Hit");
		}
	}
}