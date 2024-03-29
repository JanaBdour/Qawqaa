using System;
using UnityEngine;

namespace Scripts.Projectiles
{
	public interface IProjectilesService
	{
		event Action<Vector3> OnExplode; 
		
		void Explode( ProjectileView projectile );
	}
}