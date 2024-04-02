using UnityEngine;

namespace Scripts.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 WithX( this Vector2 original, float x )
        {
            return new Vector2( x, original.y );
        }
        
        public static Vector2 WithY( this Vector2 original, float y )
        {
            return new Vector2( original.x, y );
        }

        public static Vector2 AddX( this Vector2 original, float x )
        {
            return new Vector2( original.x + x, original.y );
        }
        
        public static Vector2 AddY( this Vector2 original, float y )
        {
            return new Vector2( original.x, original.y + y );
        }

        public static Vector2 ZeroX( this Vector2 original )
        {
            return new Vector2( 0, original.y );
        }
        
        public static Vector2 ZeroY( this Vector2 original )
        {
            return new Vector2( original.x, 0 );
        }
    }
}