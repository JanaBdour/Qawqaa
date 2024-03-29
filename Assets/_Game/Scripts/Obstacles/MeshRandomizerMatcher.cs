using System.Collections;
using System.Collections.Generic;
using Scripts.Obstacles;
using UnityEngine;

public class MeshRandomizerMatcher : MonoBehaviour
{
    [SerializeField] private Mesh[] meshes;

    [SerializeField] private MeshRandomizer randomizer;

    private void Start( )
    {
        GetComponent<MeshFilter>( ).sharedMesh = meshes[randomizer.Index];
    }
}
