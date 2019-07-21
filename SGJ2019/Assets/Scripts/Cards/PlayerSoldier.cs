using UnityEngine;
using System.Collections.Generic;


namespace SGJ2019
{
	[RequireComponent(typeof(HealthComponent))]
	[RequireComponent(typeof(ActionPointsComponent))]	
	public class PlayerSoldier : Card, IActionProvider
	{
		protected HealthComponent healthCompnent = null;
		public HealthComponent HealthCompnent => healthCompnent;
		protected ActionPointsComponent actionPointsComponent = null;
		public ActionPointsComponent ActionPointsComponent => actionPointsComponent;


		protected override void ManagedInitialize()
		{
			base.ManagedInitialize();
			healthCompnent = GetComponent<HealthComponent>();
			actionPointsComponent = GetComponent<ActionPointsComponent>(); 
		}

		public virtual List<PlayerAction> GetAvailableActions()
		{
			List<PlayerAction> result = new List<PlayerAction>();
			result.Add(new MovementAction());
			result.Add(new SwitchRow());
			result.Add(new PistolShot());
			return result;
		}

		private void Update()
		{
			description.text =
				"Health: " + healthCompnent.CurrentHealth.ToString() + " / " + healthCompnent.MaxHealth.ToString() + "\n" +
				"Action Points left: " + actionPointsComponent.CurrentActionPoints.ToString();
		}
	}
}