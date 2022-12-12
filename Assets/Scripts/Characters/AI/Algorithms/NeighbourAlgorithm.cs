using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;
using Map;

namespace Characters.AI.Algorithms
{
    /// <summary>
    /// This class represents a template for a path finding algorithm. It implements <see cref="IPathStrategy{T}"/>
    /// </summary>
    /// <typeparam name="T">Any struct that represents a position in a custom coordinate system.</typeparam>
    public abstract class NeighbourAlgorithm<T> : IPathStrategy<T> where T : struct
    {
        protected static readonly int NeighbourDistance = 3;
        
        protected IList<T> _alreadyVisited = new List<T>();

        /// <inheritdoc cref="IPathStrategy{T}.CreatePath"/>
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

        /// <summary>
        /// Returns all neighbours given a position.
        /// </summary>
        /// <param name="map">The character's map.</param>
        /// <param name="position">The target position.</param>
        /// <returns>A <c>IList</c> containing all neighbours found.</returns>
        protected abstract IList<T> Neighbours(IDictionary<Vector3,T> map, T position);

        
        /// <summary>
        /// Selects a new element to be added to the path.
        /// The new position is chosen randomly from all the neighbours of the current position.
        /// </summary>
        /// <param name="map">The character's map.</param>
        /// <param name="position">The current position, nay the last element inserted.</param>
        /// <returns>The next position to insert.</returns>
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

        /// <summary>
        /// Chooses a random element from a <c>IList</c> of neighbours.
        /// </summary>
        /// <param name="neighbours">The list of neighbours.</param>
        /// <returns>A random neighbour.</returns>
        private IEnumerable<T> TakeRandomNeighbour(IList<T> neighbours)
        {
            yield return neighbours[new System.Random().Next(0, neighbours.Count - 1)];
        }
    }
}