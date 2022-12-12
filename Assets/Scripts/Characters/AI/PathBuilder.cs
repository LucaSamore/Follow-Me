using System;
using System.Collections.Generic;
using Characters.AI.Algorithms;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters.AI
{
    public sealed class PathBuilder<T> where T : struct
    {
        public IDictionary<Vector3,T> Map { get; set; }
        public IPathStrategy<T> Algorithm { get; set; }

        public PathBuilder(IDictionary<Vector3,T> map, [CanBeNull] IPathStrategy<T> defaultAlgorithm)
        {
            Map = map;
            Algorithm = defaultAlgorithm;
        }

        public IList<Tuple<Vector3,T>> BuildPath(T startingPosition, int steps) 
            => Algorithm.CreatePath(Map, startingPosition, steps);
    }
}