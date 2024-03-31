using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Feedback
{
    public class MusicPlayer : MonoBehaviour
    {
        static MusicPlayer instance;

        private void Awake( )
        {
            if ( instance )
                Destroy( gameObject );
            else
            {
                DontDestroyOnLoad( this );
                instance = this;
            }
        }
    }
}