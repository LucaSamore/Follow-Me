using System.Collections.Generic;
using UnityEngine;

namespace Map.CustomNavMesh
{
    public sealed class NavMeshFactory2D : INavMeshFactory<Vector2Int>
    {
        public IDictionary<Vector3, Vector2Int> Bake(Transform zone, Vector3 startingPosition) =>
            MapUtil.Map2D(zone, startingPosition);
    }
}