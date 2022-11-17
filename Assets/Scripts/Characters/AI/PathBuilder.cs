using System.Collections.Generic;
using Characters.AI.Algorithms;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters.AI
{
    public sealed class PathBuilder
    {
        private readonly IDictionary<Vector3,Vector2> _map;
        public IPathStrategy Algorithm { get; set; }

        public PathBuilder(IDictionary<Vector3,Vector2> map, [CanBeNull] IPathStrategy defaultAlgorithm)
        {
            _map = map;
            Algorithm = defaultAlgorithm ?? new NeighbourAlgorithm();
        }

        public IDictionary<Vector3,Vector2> BuildPath(Vector2 startingPosition, int steps) 
            => Algorithm.CreatePath(_map, startingPosition, steps);

        public void ChangeAlgorithm(IPathStrategy newAlgorithm) => Algorithm = newAlgorithm;
    }
}