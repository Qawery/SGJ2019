namespace SGJ2019
{
	public class SniperShot : AttackAction
	{
		public override string Name => "Sniper Shot";
		public override int Cost => 2;
		public override int Damage => 3;
		public override int MinRange => 3;
		public override int MaxRange => 4;
	}
}