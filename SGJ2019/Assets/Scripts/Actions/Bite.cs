namespace SGJ2019
{
	public class Bite : AttackAction
	{
		public override string Name => "Bite";
		public override int Cost => 1;
		public override int Damage => 1;
		public override int MinRange => 1;
		public override int MaxRange => 1;
	}
}