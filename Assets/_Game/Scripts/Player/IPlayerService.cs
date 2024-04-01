using System;
using UnityEngine;

namespace Scripts.Player
{
	public interface IPlayerService 
	{
		PlayerView   Player { get; }
		event Action OnMove;
		event Action OnLongMove;
		
		void OnHitObstacle( Collider2D obstacle );
	}
}