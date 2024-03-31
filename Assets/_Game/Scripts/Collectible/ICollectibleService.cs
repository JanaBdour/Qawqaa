using System;
using UnityEngine;

namespace Scripts.Collectible
{
	public interface ICollectibleService 
	{
		int CollectedCount { get; }

		event Action OnUpdateCount;
	}
}