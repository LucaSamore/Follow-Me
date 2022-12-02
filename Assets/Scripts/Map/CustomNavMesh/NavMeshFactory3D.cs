using System.Collections.Generic;
using UnityEngine;

namespace Map.CustomNavMesh
{
    public sealed class NavMeshFactory3D : INavMeshFactory<Vector3Int>
    {
        public IDictionary<Vector3, Vector3Int> Bake(Transform zone, Vector3 startingPosition) =>
            MapUtil.Map3D(zone, startingPosition);
    }
}