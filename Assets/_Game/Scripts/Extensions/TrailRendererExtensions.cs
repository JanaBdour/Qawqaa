using System.Collections;
using UnityEngine;

namespace Scripts.Extensions
{
    public static class TrainRendererExtensions
    {
        public static void Reset( this TrailRenderer trail, MonoBehaviour instance )
        {
            instance.StartCoroutine( ResetTrail( trail ) );
        }
        
        static IEnumerator ResetTrail( TrailRenderer trail )
        {
            var trailTime = trail.time;
            trail.time = 0;
            yield return 0;
            trail.time = trailTime;
        }
    }
}