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

		Vector3 GetStartPosition( PlatformView platform );
		Vector3 GetEndPosition( PlatformView   platform );
		float   GetXDistance( PlatformView     startPlatform, PlatformView endPlatform );
		
		PlatformView GetClosestPlatformOnX( float xPosition );
		PlatformView GetClosestPlatformOnX( float xPosition, out int index );
	}
}