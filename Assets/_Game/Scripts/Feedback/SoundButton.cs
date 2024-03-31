using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts.Feedback
{
    public class SoundButton : AudioButton
    {
        protected override bool IsEnabled( )
        {
            return _feedbackService.IsSoundEnabled;
        }

        protected override void CallSwitch( )
        {
            _feedbackService.SwitchSound( );
        }
    }
}