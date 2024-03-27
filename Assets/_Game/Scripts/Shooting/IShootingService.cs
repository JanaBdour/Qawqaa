using System;
using UnityEngine;

namespace Scripts.Shooting
{
	public interface IShootingService
	{
		event Action<Vector3> OnShoot;
		event Action<Vector3> OnAim;
	}
}