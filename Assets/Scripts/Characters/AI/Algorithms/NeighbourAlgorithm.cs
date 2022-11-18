using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.AI.Algorithms
{
    public sealed class NeighbourAlgorithm : IPathStrategy
    {
        public IDictionary<Vector3, Vector2Int> CreatePath(IDictionary<Vector3, Vector2Int> map,
            Vector2Int startingPosition, int steps)
        {
            throw new System.NotImplementedException();
        }

        private IList<Vector2Int> Neighbours(IDictionary<Vector3,Vector2Int> map, Vector2Int position)
        {
            return Enumerable.Range(position.x - 1, position.y + 1)
                .SelectMany(kk => Enumerable.Range(position.y - 1, position.y + 1)
                    .Select(vv => new Vector2Int(kk, vv)))
                .Where(p => map.Values.Contains(p))
                .ToList();
        }
    }
}