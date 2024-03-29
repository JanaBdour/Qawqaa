using System;
using UnityEngine;

namespace Scripts.Obstacles
{
	public interface IObstaclesService
	{
		event Action<ObstacleView> OnSpawnObstacle;
		event Action<ObstacleView> OnDestroyObstacle;
	}
}