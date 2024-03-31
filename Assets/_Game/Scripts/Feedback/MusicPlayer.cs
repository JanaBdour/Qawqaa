using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Feedback
{
    public class MusicPlayer : MonoBehaviour
    {
        public static MusicPlayer Instance;

        private void Awake( )
        {
            if ( Instance )
                Destroy( gameObject );
            else
            {
                DontDestroyOnLoad( this );
                Instance = this;
            }
        }
    }
}