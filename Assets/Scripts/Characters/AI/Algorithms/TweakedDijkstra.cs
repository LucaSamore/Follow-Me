using System;
using System.Collections.Generic;
using System.Linq;
using Characters.AI.Algorithms.Util;
using UnityEngine;

namespace Characters.AI.Algorithms
{
    public sealed class TweakedDijkstra : IPathStrategy
    {
        private static readonly int HorizontalAndVerticalCost = 10;
        private static readonly int DiagonalCost = 14;

        private Node<Vector2Int> _source;
        private IEnumerable<Node<Vector2Int>> _nodes;
        
        public IList<Tuple<Vector3,Vector2Int>> CreatePath(
            IDictionary<Vector3,Vector2Int> map, 
            Vector2Int startingPosition, 
            int steps)
        {
            _source = new Node<Vector2Int>(startingPosition) { Cost = 0 };
            _nodes = MapToNodes(map);
            _nodes.ToList().ForEach(Debug.Log);
            return null;
        }

        private static IEnumerable<Node<Vector2Int>> MapToNodes(IDictionary<Vector3,Vector2Int> map) => 
            map.Select(kvp => new Node<Vector2Int>(kvp.Value));
    }
}