using UnityEngine;

namespace Scripts.Projectiles
{
	public interface IProjectilesService
	{
		void Explode( ProjectileView projectile );
	}
}