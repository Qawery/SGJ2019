﻿namespace SGJ2019
{
	public class PistolShot : AttackAction
	{
		public override string Name => "Pistol Shot";
		public override int Cost => 1;
		public override int Damage => 1;
		public override int MinRange => 1;
		public override int MaxRange => 2;
	}
}