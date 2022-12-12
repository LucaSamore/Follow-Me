using System.Collections.Generic;
using UnityEngine;

namespace Map.CustomNavMesh
{
    /// <summary>
    /// A concrete implementation of <c>INavMeshFactory</c>
    /// <see cref="INavMeshFactory"/>
    /// </summary>
    public sealed class NavMeshFactory : INavMeshFactory
    {
        /// <inheritdoc cref="INavMeshFactory.BakeMesh2D"/>
        public IDictionary<Vector3,Vector2Int> BakeMesh2D(Transform zone, Vector3 startingPosition) =>
            MapUtil.Map2D(zone, startingPosition);

        /// <inheritdoc cref="INavMeshFactory.BakeMesh3D"/>
        public IDictionary<Vector3,Vector3Int> BakeMesh3D(Transform zone, Vector3 startingPosition) =>
            MapUtil.Map3D(zone, startingPosition);
    }
}