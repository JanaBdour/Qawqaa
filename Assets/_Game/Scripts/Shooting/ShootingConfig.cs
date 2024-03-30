using UnityEngine;

namespace Scripts.Shooting
{
	[CreateAssetMenu( fileName = "ShootingConfig", menuName = "Configs/ShootingConfig" )]
	public class ShootingConfig : ScriptableObject
	{
		public ShootingSimulationView simulationPrefab;
		public ShootingTrajectoryView trajectoryPrefab;
		public Vector2                throwForce           = new Vector2( 40, 30 );
		public Vector2                maxForce             = new Vector2( 45, 35 );
		public int                    maxUpJumps             = 3;
		public int                    trajectorySteps      = 10;
		public float                  collisionCheckRadius = 0.1f;
		public float                  minForceMagnitude    = 0.1f;
	}
}