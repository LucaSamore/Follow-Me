using System;
using System.Collections.Generic;
using System.Linq;
using Characters.AI.Algorithms.Util;
using UnityEngine;

namespace Characters.AI.Algorithms
{
    public sealed class TweakedDijkstra : IPathStrategy
    {
        private IEnumerable<Node> _nodes;
        
        public IList<Tuple<Vector3,Vector2Int>> CreatePath(
            IDictionary<Vector3,Vector2Int> map, 
            Vector2Int startingPosition, 
            int steps)
        {
            _nodes = MapToNodes(map);
            _nodes.ToList().ForEach(n => Debug.Log(n));
            return null;
        }

        private static IEnumerable<Node> MapToNodes(IDictionary<Vector3,Vector2Int> map) => 
            map.Select(kvp => new Node(kvp.Key, kvp.Value));
    }
}