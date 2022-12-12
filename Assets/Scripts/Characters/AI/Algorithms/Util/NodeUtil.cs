using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Characters.AI.Algorithms.Util
{
    /// <summary>
    /// This utility class provides some utility methods used by <see cref="TweakedDijkstra{T}"/>
    /// when working with <see cref="Node{T}"/>.
    /// </summary>
    public static class NodeUtil
    {
        /// <summary>
        /// Transforms the character's map to a <c>IEnumerable</c> of <see cref="Node{T}"/>. Each node contains a value
        /// from the original dictionary.
        /// </summary>
        /// <param name="map">The character's map.</param>
        /// <typeparam name="T">Any struct that represents a position in a custom coordinate system.</typeparam>
        /// <returns>A <c>IEnumerable</c> of <see cref="Node{T}"/></returns>
        public static IEnumerable<Node<T>> MapToNodes<T>(IDictionary<Vector3,T> map) where T : struct => 
            map.Select(kvp => new Node<T>(kvp.Value));

        /// <summary>
        /// Checks whether a neighbour is in a diagonal direction of a target node.
        /// </summary>
        /// <param name="node">The target node.</param>
        /// <param name="neighbour">The neighbour of the node.</param>
        /// <returns>True if neighbour is diagonal, false otherwise.</returns>
        public static bool IsDiagonalNeighbour(Node<Vector2Int> node, Node<Vector2Int> neighbour) =>
            neighbour.Element.Equals(node.Element + new Vector2Int(1, 1)) ||
            neighbour.Element.Equals(node.Element + new Vector2Int(-1,-1)) ||
            neighbour.Element.Equals(node.Element + new Vector2Int(-1,1)) ||
            neighbour.Element.Equals(node.Element + new Vector2Int(1,-1));

        /// <summary>
        /// Checks whether a neighbour is in a diagonal direction of a target node.
        /// </summary>
        /// <param name="node">The target node.</param>
        /// <param name="neighbour">The neighbour of the node.</param>
        /// <returns>True if neighbour is diagonal, false otherwise.</returns>
        public static bool IsDiagonalNeighbour(Node<Vector3Int> node, Node<Vector3Int> neighbour) =>
            throw new NotImplementedException();
    }
}