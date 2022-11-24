using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Characters.AI.Algorithms
{
    public sealed class NeighbourAlgorithm2D : IPathStrategy<Vector2Int>
    {
        private static readonly int NeighbourDistance = 3;
        
        private readonly Func<IDictionary<Vector3,Vector2Int>, Vector2Int, Vector3> _getKeyFromValue = (map, v2) =>
            map.FirstOrDefault(x => x.Value.Equals(v2)).Key;
        
        public IList<Tuple<Vector3,Vector2Int>> CreatePath(IDictionary<Vector3,Vector2Int> map,
            Vector2Int startingPosition, int steps)
        {
            var path = new List<Tuple<Vector3,Vector2Int>>();
            var currentPosition = startingPosition;

            for (var i = 0; i < steps; i++)
            {
                var newPosition = Next(map, currentPosition);
                path.Add(newPosition);
                currentPosition = newPosition.Item2;
            }
            
            return path;
        }

        private static IList<Vector2Int> Neighbours(IDictionary<Vector3,Vector2Int> map, Vector2Int position)
        {
            return Enumerable.Range(position.x - 1, NeighbourDistance)
                .SelectMany(kk => Enumerable.Range(position.y - 1, NeighbourDistance)
                    .Select(vv => new Vector2Int(kk, vv)))
                .Where(p => map.Values.Contains(p))
                .ToList();
        }

        private Tuple<Vector3,Vector2Int> Next(IDictionary<Vector3,Vector2Int> map, Vector2Int position)
        {
            return TakeRandomNeighbour(Neighbours(map, position))
                .Select(v2 =>
                    new Tuple<Vector3,Vector2Int>(_getKeyFromValue(map, v2), v2))
                .First();
        }

        private static IEnumerable<Vector2Int> TakeRandomNeighbour(IList<Vector2Int> neighbours)
        {
            yield return neighbours[new Random().Next(0, neighbours.Count - 1)];
        }
    }
}