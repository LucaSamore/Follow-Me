using System;
using System.Collections.Generic;
using Characters.AI.Algorithms;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters.AI
{
    public sealed class PathBuilder<T> where T : struct
    {
        private readonly IDictionary<Vector3,T> _map;
        public IPathStrategy<T> Algorithm { get; set; }

        public PathBuilder(IDictionary<Vector3,T> map, [CanBeNull] IPathStrategy<T> defaultAlgorithm)
        {
            _map = map;
            Algorithm = defaultAlgorithm;
        }

        public IList<Tuple<Vector3,T>> BuildPath(T startingPosition, int steps) 
            => Algorithm.CreatePath(_map, startingPosition, steps);
    }
}