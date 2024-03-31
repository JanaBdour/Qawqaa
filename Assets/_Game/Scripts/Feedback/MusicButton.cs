using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts.Feedback
{
    public class MusicButton : AudioButton
    {
        protected override bool IsEnabled( )
        {
            return _feedbackService.IsMusicEnabled;
        }

        protected override void CallSwitch( )
        {
            _feedbackService.SwitchMusic( );
        }
    }
}