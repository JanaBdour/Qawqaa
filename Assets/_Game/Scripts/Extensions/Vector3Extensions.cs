using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 WithX( this Vector3 original, float x )
        {
            return new Vector3( x, original.y, original.z );
        }
        
        public static Vector3 WithY( this Vector3 original, float y )
        {
            return new Vector3( original.x, y, original.z );
        }
        
        public static Vector3 WithZ( this Vector3 original, float z )
        {
            return new Vector3( original.x, original.y, z );
        }
        
        public static Vector3 AddX( this Vector3 original, float x )
        {
            return new Vector3( original.x + x, original.y, original.z );
        }
        
        public static Vector3 AddY( this Vector3 original, float y )
        {
            return new Vector3( original.x, original.y + y, original.z );
        }
        
        public static Vector3 AddZ( this Vector3 original, float z )
        {
            return new Vector3( original.x, original.y, original.z + z );
        }
        
        public static Vector3 ZeroX( this Vector3 original )
        {
            return new Vector3( 0, original.y, original.z );
        }
        
        public static Vector3 ZeroY( this Vector3 original )
        {
            return new Vector3( original.x, 0, original.z );
        }
        
        public static Vector3 ZeroZ( this Vector3 original )
        {
            return new Vector3( original.x, original.y, 0 );
        }
        
        public static Vector3 GetRandomVector( Vector3 minVector, Vector3 maxVector )
        {
            Random.InitState( (int) DateTime.Now.Ticks );
            var x = Random.Range( minVector.x, maxVector.x );
            var y = Random.Range( minVector.y, maxVector.y );
            var z = Random.Range( minVector.z, maxVector.z );
            return new Vector3( x, y, z );
        }
    }
}