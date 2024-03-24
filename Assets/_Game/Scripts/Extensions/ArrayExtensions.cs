using System;
using Random = UnityEngine.Random;

namespace Scripts.Extensions
{
    public static class ArrayExtensions
    {
        public static T GetRandomElement<T>( this T[] array )
        {
            Random.InitState( (int) DateTime.Now.Ticks );
            return array[Random.Range( 0, array.Length )];
        }
    }
}