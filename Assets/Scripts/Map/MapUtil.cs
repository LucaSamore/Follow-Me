using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Map
{
    /// <summary>
    /// Provides a set of utility methods used for managing the character's map.
    /// </summary>
    public static class MapUtil
    {
        /// <summary>
        /// Converts the Unity coordinate system to a custom coordinate system, which is the distance from a starting
        /// point. This method is used by <c>INavMeshFactory</c> for creating a custom NavMesh.
        /// </summary>
        /// <param name="zone">Where the character can move, using Unity coordinate system.</param>
        /// <param name="startingPosition">The origin of the custom coordinate system.</param>
        /// <returns>
        /// A <c>IDictionary</c> describing the NavMesh. Keys are the original positions.
        /// Values are all the positions mapped to the custom coordinate system.
        /// </returns>
        public static IDictionary<Vector3,Vector2Int> Map2D(Transform zone, Vector3 startingPosition) =>
            zone
                .Cast<Transform>()
                .Select(t => t.position)
                .ToDictionary(k => k, 
                    v => Vector2Int.RoundToInt(
                        new Vector2(v.x - startingPosition.x, v.z - startingPosition.z)));

        /// <summary>
        /// Converts the Unity coordinate system to a custom coordinate system, which is the distance from a starting
        /// point. This method is used by <c>INavMeshFactory</c> for creating a custom NavMesh.
        /// </summary>
        /// <param name="zone">Where the character can move, using Unity coordinate system.</param>
        /// <param name="startingPosition">The origin of the custom coordinate system.</param>
        /// <returns>
        /// A <c>IDictionary</c> describing the NavMesh. Keys are the original positions.
        /// Values are all the positions mapped to the custom coordinate system.
        /// </returns>
        public static IDictionary<Vector3, Vector3Int> Map3D(Transform zone, Vector3 startingPosition) =>
            throw new NotImplementedException();

        /// <summary>
        /// Given an <c>IDictionary</c> and a value, it returns the first key associated with that value.
        /// </summary>
        /// <param name="map">An <c>IDictionary</c> instance.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="K">The type of the key.</typeparam>
        /// <typeparam name="V">The type of the value.</typeparam>
        /// <returns>The first key associated with that value.</returns>
        public static K GetKeyFromValue<K,V>(IDictionary<K,V> map, V value) =>
            map.FirstOrDefault(kvp => kvp.Value.Equals(value)).Key;
    }
}
