using UnityEngine;

namespace Scripts.Platforms
{
	public interface IPlatformsService
	{
		PlatformView GetLowestPlatform( );
		PlatformView GetClosestPlatformOnX( float xPosition );
	}
}