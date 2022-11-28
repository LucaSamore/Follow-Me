using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Characters.AI.Algorithms.Util
{
    public static class NodeUtil
    {
        public static IEnumerable<Node<T>> MapToNodes<T>(IDictionary<Vector3,T> map) where T : struct => 
            map.Select(kvp => new Node<T>(kvp.Value));

        public static bool IsDiagonalNeighbour(Node<Vector2Int> node, Node<Vector2Int> neighbour) =>
            neighbour.Element.Equals(node.Element + new Vector2Int(1, 1)) ||
            neighbour.Element.Equals(node.Element + new Vector2Int(-1,-1)) ||
            neighbour.Element.Equals(node.Element + new Vector2Int(-1,1)) ||
            neighbour.Element.Equals(node.Element + new Vector2Int(1,-1));

        public static bool IsDiagonalNeighbour(Node<Vector3Int> node, Node<Vector3Int> neighbour) =>
            throw new NotImplementedException();
    }
}