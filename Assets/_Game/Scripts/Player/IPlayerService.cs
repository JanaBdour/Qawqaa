using UnityEngine;

namespace Scripts.Player
{
	public interface IPlayerService 
	{
		PlayerView Player { get; }
		void       OnHitObstacle( );
	}
}