namespace SGJ2019
{
	public class ShotgunBlast : AttackAction
	{
		public override string Name => "Shotgun Blast";
		public override int Cost => 2;
		public override int Damage => 3;
		public override int MinRange => 2;
		public override int MaxRange => 2;
	}
}