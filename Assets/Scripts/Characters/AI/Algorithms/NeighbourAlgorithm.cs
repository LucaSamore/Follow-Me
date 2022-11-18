using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Characters.AI.Algorithms
{
    public sealed class NeighbourAlgorithm : IPathStrategy
    {
        private readonly Func<IDictionary<Vector3, Vector2Int>, Vector2Int, Vector3> _getKeyFromValue = (map, v2) =>
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

        private IList<Vector2Int> Neighbours(IDictionary<Vector3,Vector2Int> map, Vector2Int position)
        {
            return Enumerable.Range(position.x - 1, position.y + 1)
                .SelectMany(kk => Enumerable.Range(position.y - 1, position.y + 1)
                    .Select(vv => new Vector2Int(kk, vv)))
                .Where(p => map.Values.Contains(p))
                .ToList();
        }

        private Tuple<Vector3,Vector2Int> Next(IDictionary<Vector3,Vector2Int> map, Vector2Int position)
        {
            Neighbours(map,position).ToList().ForEach(i => Debug.Log(i));
            return TakeRandomNeighbour(Neighbours(map, position))
                .Select(v2 =>
                    new Tuple<Vector3,Vector2Int>(_getKeyFromValue(map, v2), v2))
                .First();
        }

        private IEnumerable<Vector2Int> TakeRandomNeighbour(IList<Vector2Int> neighbours)
        {
            yield return neighbours[new Random().Next(0, neighbours.Count - 1)];
        }
    }
}