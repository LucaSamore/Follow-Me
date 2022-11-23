using System;
using System.Collections.Generic;
using System.Linq;
using Characters.AI.Algorithms.Util;
using UnityEngine;
using Random = System.Random;

namespace Characters.AI.Algorithms
{
    public sealed class TweakedDijkstra : IPathStrategy
    {
        private static readonly int HorizontalAndVerticalCost = 10;
        private static readonly int DiagonalCost = 14;
        private static readonly int AlgorithmIterations = 5;

        private IEnumerable<Node<Vector2Int>> Nodes { get; set; }

        public IList<Tuple<Vector3,Vector2Int>> CreatePath(IDictionary<Vector3,Vector2Int> map, Vector2Int startingPosition, int depth)
        {
            Nodes = MapToNodes(map);
            var source = new Node<Vector2Int>(startingPosition) { Cost = 0, State = NodeState.Open };
            var destination = ChooseDestination(Nodes, source, depth);
            var path = FindPath(source, destination);
            
            throw new NotImplementedException();
        }

        private static IEnumerable<Node<Vector2Int>> MapToNodes(IDictionary<Vector3,Vector2Int> map) => 
            map.Select(kvp => new Node<Vector2Int>(kvp.Value));

        private static Node<Vector2Int> ChooseDestination(IEnumerable<Node<Vector2Int>> map, Node<Vector2Int> source, int depth)
        {
            var items = map.Where(
                n => Math.Abs(n.Element.x - source.Element.x) >= depth &&
                                       Math.Abs(n.Element.y - source.Element.y) >= depth).ToArray();

            return items[new Random().Next(0, items.Length - 1)];
        }
        
        private static IEnumerable<Node<Vector2Int>> FindPath(Node<Vector2Int> source, Node<Vector2Int> destination)
        {
            throw new NotImplementedException();
        }
        
        private IEnumerable<Node<Vector2Int>> NextNodes()
        {
            return Nodes
                .Where(n => n.State == NodeState.Open)
                .Where(on => on.Cost == Nodes.Select(n => n.Cost).Min());
        }
    }
}