using System;
using UnityEngine;

namespace Scripts.Platforms
{
	public interface IPlatformsService
	{
		event Action               OnResetPlatforms;
		event Action<PlatformView> OnSpawnPlatform;
		event Action<PlatformView> OnDestroyPlatform;
		
		PlatformView GetLowestPlatform( );
		PlatformView GetClosestPlatformOnX( float xPosition );
	}
}