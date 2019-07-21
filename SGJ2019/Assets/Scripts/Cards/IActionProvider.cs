using System.Collections.Generic;


namespace SGJ2019
{
	public interface IActionProvider
	{
		List<PlayerAction> GetAvailableActions();
	}
}