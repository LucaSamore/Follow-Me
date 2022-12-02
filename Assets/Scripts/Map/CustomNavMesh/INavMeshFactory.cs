using System.Collections.Generic;
using UnityEngine;

namespace Map.CustomNavMesh
{
    public interface INavMeshFactory
    {
        IDictionary<Vector3,Vector2Int> BakeMesh2D(Transform zone, Vector3 startingPosition);
        IDictionary<Vector3,Vector3Int> BakeMesh3D(Transform zone, Vector3 startingPosition);
    }
}