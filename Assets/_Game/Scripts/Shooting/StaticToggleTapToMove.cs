using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticToggleTapToMove : MonoBehaviour
{
   private Toggle _toggle;

   public static bool TapToMove;
   
   private void Awake( )
   {
      _toggle = GetComponent<Toggle>( );
   }

   private void Update( )
   {
      TapToMove = _toggle.isOn;
   }
}
