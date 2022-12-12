using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Algorithms._3D
{
    /// <summary>
    /// Gives an implementation for all abstract method from <see cref="NeighbourAlgorithm{T}"/>
    /// </summary>
    public sealed class NeighbourAlgorithm3D : NeighbourAlgorithm<Vector3Int>
    {
        /// <inheritdoc cref="NeighbourAlgorithm{T}.Neighbours"/>
        protected override IList<Vector3Int> Neighbours(IDictionary<Vector3,Vector3Int> map, Vector3Int position) =>
            throw new System.NotImplementedException();
    }
}