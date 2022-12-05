using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.AI.Algorithms._2D
{
    public sealed class NeighbourAlgorithm2D : NeighbourAlgorithm<Vector2Int>
    {
        protected override IList<Vector2Int> Neighbours(IDictionary<Vector3,Vector2Int> map, Vector2Int position) =>
            Enumerable.Range(position.x - 1, NeighbourDistance)
                .SelectMany(kk => Enumerable.Range(position.y - 1, NeighbourDistance)
                    .Select(vv => new Vector2Int(kk, vv)))
                .Where(p => map.Values.Contains(p))
                .ToList();
    }
}