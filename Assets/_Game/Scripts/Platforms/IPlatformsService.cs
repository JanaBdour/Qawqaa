using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Platforms
{
	public interface IPlatformsService
	{
		 List<PlatformView> Platforms { get; }

		event Action               OnResetPlatforms;
		event Action<PlatformView> OnSpawnPlatform;
		event Action<PlatformView> OnDestroyPlatform;
		
		PlatformView GetLowestPlatform( );
		PlatformView GetClosestPlatformOnX( float xPosition );
	}
}