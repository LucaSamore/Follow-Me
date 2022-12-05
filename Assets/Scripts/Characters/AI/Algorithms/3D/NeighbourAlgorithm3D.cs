using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Algorithms._3D
{
    public sealed class NeighbourAlgorithm3D : NeighbourAlgorithm<Vector3Int>
    {
        protected override IList<Vector3Int> Neighbours(IDictionary<Vector3,Vector3Int> map, Vector3Int position) =>
            throw new System.NotImplementedException();
    }
}