using System;
using UnityEngine;

namespace Scripts.Player
{
	public interface IPlayerService 
	{
		PlayerView Player { get; }

		event Action OnHitGround;
		
		void OnHitObstacle( );
	}
}