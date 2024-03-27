using UnityEngine;

namespace Scripts.Animations
{
    public class ScaleAnimator : MonoBehaviour
    {
        [SerializeField] private float speed  = 1;
        [SerializeField] private float offset = 1;

        private Transform _transform;
        private Vector3   _scale;

        private void Awake( )
        {
            _transform = transform;
            _scale     = _transform.localScale;
        }

        private void Update( )
        {
            _transform.localScale = _scale + Vector3.one * ( Mathf.Lerp( -offset, offset, Mathf.PingPong( Time.time * speed, 1 ) ) );
        }
    }
}