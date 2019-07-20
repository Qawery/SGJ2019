namespace SGJ2019
{
	public interface ILifecycleBound
	{
		LifecycleComponent LifecycleComponent { get; set; }
	}
}
/*
	private LifecycleComponent lifecycleComponent = null;
	public LifecycleComponent LifecycleComponent
	{
		get
		{
			return lifecycleComponent;
		}

		set
		{
			Assert.IsNull(lifecycleComponent);
			lifecycleComponent = value;
		}
	}
 */
