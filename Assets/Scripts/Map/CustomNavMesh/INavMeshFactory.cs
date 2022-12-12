using System.Collections.Generic;
using UnityEngine;

namespace Map.CustomNavMesh
{
    /// <summary>
    /// Represents an abstract factory that is responsible for the creation of a custom NavMesh.
    /// It provides methods for baking a 2D and a 3D NavMesh.
    /// </summary>
    public interface INavMeshFactory
    {
        /// <summary>
        /// Bakes a 2D NavMesh that is the zone where a custom agent can move.
        /// This method maps the Unity coordinate system into a custom coordinate system
        /// where all positions are determined by the distance from a given starting point.
        /// </summary>
        /// <param name="zone">Where the agent can move, using Unity coordinate system.</param>
        /// <param name="startingPosition">The origin of the custom coordinate system used by the new mesh.</param>
        /// <returns>
        /// A <c>IDictionary</c> describing the NavMesh. Keys are the original positions.
        /// Values are all the positions mapped to the custom coordinate system.
        /// </returns>
        IDictionary<Vector3,Vector2Int> BakeMesh2D(Transform zone, Vector3 startingPosition);
        
        /// <summary>
        /// Bakes a 3D NavMesh that is the zone where a custom agent can move.
        /// This method maps the Unity coordinate system into a custom coordinate system
        /// where all positions are determined by the distance from a given starting point.
        /// </summary>
        /// <param name="zone">Where the agent can move, using Unity coordinate system.</param>
        /// <param name="startingPosition">The origin of the custom coordinate system used by the new mesh.</param>
        /// <returns>
        /// A <c>IDictionary</c> describing the NavMesh. Keys are the original positions.
        /// Values are all the positions mapped to the custom coordinate system.
        /// </returns>
        IDictionary<Vector3,Vector3Int> BakeMesh3D(Transform zone, Vector3 startingPosition);
    }
}