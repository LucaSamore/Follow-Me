using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
                path.Add(newPosition);
                currentPosition = newPosition.Item2;
            }
            
            return path;
        }

        protected abstract IList<T> Neighbours(IDictionary<Vector3,T> map, T position);

        protected virtual Tuple<Vector3, T> Next(IDictionary<Vector3, T> map, T position)
        {
            var neighbour = TakeRandomNeighbour(Neighbours(map, position)).ToList();
            _alreadyVisited.Add(neighbour.First());
            return neighbour
                .Select(v2 =>
                    new Tuple<Vector3,T>(MapUtil.GetKeyFromValue(map, v2), v2))
                .First();
        }
            

        protected virtual IEnumerable<T> TakeRandomNeighbour(IList<T> neighbours)
        {
            yield return neighbours[new System.Random().Next(0, neighbours.Count - 1)];
        }
    }
}