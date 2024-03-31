using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Feedback
{
    public abstract class AudioButton : MonoBehaviour
    {
        [Inject] protected IFeedbackService _feedbackService;

        protected abstract bool IsEnabled( );
        protected abstract void CallSwitch( );

        private Button _button;

        [SerializeField] private Image  iconImage;
        [SerializeField] private Sprite onSprite;
        [SerializeField] private Sprite offSprite;

        private void Awake( )
        {
            _button = GetComponent<Button>( );
            _button.onClick.AddListener( Switch );

            SetSprite( );
        }

        private void OnDestroy( )
        {
            _button.onClick.RemoveListener( Switch );
        }

        private void Switch( )
        {
            CallSwitch( );
            SetSprite( );
        }

        private void SetSprite( )
        {
            iconImage.sprite = IsEnabled( ) ? onSprite : offSprite;
        }
    }
}