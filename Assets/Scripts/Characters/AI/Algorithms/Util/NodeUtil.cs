using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Characters.AI.Algorithms.Util
{
    public static class NodeUtil
    {
        public static IEnumerable<Node<Vector2Int>> MapToNodes(IDictionary<Vector3,Vector2Int> map) => 
            map.Select(kvp => new Node<Vector2Int>(kvp.Value));

        public static IEnumerable<Node<Vector2Int>> RemovePath(IEnumerable<Node<Vector2Int>> oldMap,
            IEnumerable<Node<Vector2Int>> path)
            => oldMap.Except(path);
        
        public static bool IsDiagonalNeighbour(Node<Vector2Int> node, Node<Vector2Int> neighbour) =>
            neighbour.Element.Equals(node.Element + new Vector2Int(1, 1)) ||
            neighbour.Element.Equals(node.Element + new Vector2Int(-1,-1)) ||
            neighbour.Element.Equals(node.Element + new Vector2Int(-1,1)) ||
            neighbour.Element.Equals(node.Element + new Vector2Int(1,-1));
    }
}