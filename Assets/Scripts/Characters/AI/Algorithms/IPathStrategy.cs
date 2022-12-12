using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters.AI.Algorithms
{
    /// <summary>
    /// This interface describes the strategy for creating a path.
    /// </summary>
    /// <typeparam name="T">Any struct that represents a position in a custom coordinate system.</typeparam>
    public interface IPathStrategy<T> where T : struct
    {
        /// <summary>
        /// Builds and returns a path using a path finding algorithm.
        /// </summary>
        /// <param name="map">The character's map.</param>
        /// <param name="startingPosition">The source of the path.</param>
        /// <param name="steps">The length of the path.</param>
        /// <returns>The generated path as a <c>IList</c>.</returns>
        IList<Tuple<Vector3,T>> CreatePath(IDictionary<Vector3,T> map, T startingPosition, int steps);
    }
}