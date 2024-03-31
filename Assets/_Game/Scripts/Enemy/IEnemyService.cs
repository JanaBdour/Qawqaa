using System;
using UnityEngine;

namespace Scripts.Enemy
{
	public interface IEnemyService
	{
		event Action OnKillEnemy;
	}
}