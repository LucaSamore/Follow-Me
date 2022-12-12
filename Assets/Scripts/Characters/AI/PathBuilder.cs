using System;
using System.Collections.Generic;
using Characters.AI.Algorithms;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters.AI
{
    /// <summary>
    /// This class is responsible for the creation of a path navigated by a custom agent.
    /// It uses a path finding algorithm under the hood that can be swapped at runtime.
    /// </summary>
    /// <typeparam name="T">Any struct that represents a position in a custom coordinate system.</typeparam>
    public sealed class PathBuilder<T> where T : struct
    {
        public IDictionary<Vector3,T> Map { get; set; }
        
        public IPathStrategy<T> Algorithm { get; set; }

        /// <summary>
        /// Creates a new <c>PathBuilder</c> instance.
        /// </summary>
        /// <param name="map">A <c>IDictionary</c> describing the map.</param>
        /// <param name="defaultAlgorithm">The default path finding algorithm to use.</param>
        public PathBuilder(IDictionary<Vector3,T> map, [CanBeNull] IPathStrategy<T> defaultAlgorithm)
        {
            Map = map;
            Algorithm = defaultAlgorithm;
        }

        /// <summary>
        /// Creates a new path given the starting position and the length.
        /// </summary>
        /// <param name="startingPosition">Path source.</param>
        /// <param name="steps">Length of the path.</param>
        /// <returns>Generated path as a <c>IList</c>.</returns>
        public IList<Tuple<Vector3,T>> BuildPath(T startingPosition, int steps) 
            => Algorithm.CreatePath(Map, startingPosition, steps);
    }
}