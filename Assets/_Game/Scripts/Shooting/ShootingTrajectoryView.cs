using System;
using UnityEngine;

namespace Scripts.Shooting
{
    public class ShootingTrajectoryView : MonoBehaviour
    {
        public LineRenderer lineCached;

        private void Reset( )
        {
            lineCached = GetComponent<LineRenderer>( );
        }
    }
}