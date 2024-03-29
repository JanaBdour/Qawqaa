using System.Collections;
using UnityEngine;

namespace Scripts.Extensions
{
    public static class TrainRendererExtensions
    {
        public static void Reset( this TrailRenderer trail, MonoBehaviour instance, bool disableTrail = false )
        {
            instance.StartCoroutine( ResetTrail( trail ) );
        }
        
        static IEnumerator ResetTrail( TrailRenderer trail, bool disableTrail = false )
        {
            var trailTime = trail.time;
            trail.time = 0;
            yield return 0;
            trail.time    = trailTime;
            trail.enabled = !disableTrail;
        }
    }
}