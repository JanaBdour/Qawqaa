using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Obstacles
{
    public class MeshRandomizer : MonoBehaviour
    {
        [SerializeField] private Mesh[] meshes;

        public int Index { get; private set; }
        
        private void Awake( )
        {
            Index = Random.Range( 0, meshes.Length );

            GetComponent<MeshFilter>( ).sharedMesh = meshes[Index];
        }
    }
}
