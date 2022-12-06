using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;
using Map;

namespace Characters.AI.Algorithms
{
    public abstract class NeighbourAlgorithm<T> : IPathStrategy<T> where T : struct
    {
        protected static readonly int NeighbourDistance = 3;
        
        protected IList<T> _alreadyVisited = new List<T>();

        public IList<Tuple<Vector3,T>> CreatePath(IDictionary<Vector3,T> map, T startingPosition, int steps)
        {
            var path = new List<Tuple<Vector3,T>>();
            var currentPosition = startingPosition;

            for (var i = 0; i < steps; i++)
            {
                var newPosition = Next(map, currentPosition);
                if (newPosition is null) break;
                path.Add(newPosition);
                currentPosition = newPosition.Item2;
            }
            
            return path;
        }

        protected abstract IList<T> Neighbours(IDictionary<Vector3,T> map, T position);

        [CanBeNull]
        private Tuple<Vector3,T> Next(IDictionary<Vector3, T> map, T position)
        {
            var allNeighbours = Neighbours(map, position);

            if (allNeighbours.Count == 0)
            {
                return null;
            }
            
            var neighbour = TakeRandomNeighbour(allNeighbours).ToList();
            _alreadyVisited.Add(neighbour.First());
            
            return neighbour
                .Select(v2 =>
                    new Tuple<Vector3,T>(MapUtil.GetKeyFromValue(map, v2), v2))
                .First();
        }

        private IEnumerable<T> TakeRandomNeighbour(IList<T> neighbours)
        {
            yield return neighbours[new System.Random().Next(0, neighbours.Count - 1)];
        }
    }
}