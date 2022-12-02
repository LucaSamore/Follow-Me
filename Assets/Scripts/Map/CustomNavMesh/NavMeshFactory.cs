using System.Collections.Generic;
using UnityEngine;

namespace Map.CustomNavMesh
{
    public sealed class NavMeshFactory : INavMeshFactory
    {
        public IDictionary<Vector3,Vector2Int> BakeMesh2D(Transform zone, Vector3 startingPosition) =>
            MapUtil.Map2D(zone, startingPosition);

        public IDictionary<Vector3,Vector3Int> BakeMesh3D(Transform zone, Vector3 startingPosition) =>
            MapUtil.Map3D(zone, startingPosition);
    }
}