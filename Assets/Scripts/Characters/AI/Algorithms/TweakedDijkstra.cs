using System;
using System.Collections.Generic;
using System.Linq;
using Characters.AI.Algorithms.Util;
using UnityEngine;
using Random = System.Random;

namespace Characters.AI.Algorithms
{
    public sealed class TweakedDijkstra : IPathStrategy<Vector2Int>
    {
        private static readonly int NeighbourDistance = 3;
        private static readonly int HorizontalAndVerticalCost = 10;
        private static readonly int DiagonalCost = 14;
        private static readonly int AlgorithmIterations = 5;
        
        private readonly Func<IDictionary<Vector3,Vector2Int>, Vector2Int, Vector3> _getKeyFromValue = (map, v2) =>
            map.FirstOrDefault(x => x.Value.Equals(v2)).Key;

        private IEnumerable<Node<Vector2Int>> Nodes { get; set; }

        public IList<Tuple<Vector3,Vector2Int>> CreatePath(IDictionary<Vector3,Vector2Int> map, Vector2Int startingPosition, int depth)
        {
            Nodes = NodeUtil.MapToNodes(map);
            var source = new Node<Vector2Int>(startingPosition) { Cost = 0, State = NodeState.Open };
            var destination = ChooseDestination(source, depth);
            Debug.Log($"Destination: {destination.Element}");
            Nodes = Nodes.Append(source);
            var path = FindShortestPath(destination).ToList();
            path.Reverse();

            return path
                .Select(node => new Tuple<Vector3, Vector2Int>(_getKeyFromValue(map, node.Element), node.Element))
                .ToList();
            
            //throw new NotImplementedException();
        }

        private Node<Vector2Int> ChooseDestination(Node<Vector2Int> source, int depth)
        {
            var items = Nodes.Where(
                n => Math.Abs(n.Element.x - source.Element.x) >= depth &&
                                       Math.Abs(n.Element.y - source.Element.y) >= depth).ToArray();

            return items[new Random().Next(0, items.Length - 1)];
        }
        
        private IEnumerable<Node<Vector2Int>> FindShortestPath(Node<Vector2Int> destination)
        {
            var i = 0;
            var path = new List<Node<Vector2Int>>();
            var closedNodes = new List<Node<Vector2Int>>();
            var openNodes = OpenNodesWithMinimumCost();

            while (i < 5)
            {
                foreach (var node in openNodes)
                {
                    node.State = NodeState.Close;
                    var ncn = NonClosedNeighbours(node).ToList();
                    ncn.ForEach(n => n.State = NodeState.Open);
                    ncn.ForEach(n =>
                    {
                        var neighbourWithMinCost = NeighbourWithMinimumCost(n);
                    
                        if (TryUpdateCost(n, neighbourWithMinCost))
                        {
                            n.Parent = neighbourWithMinCost;
                        }
                    });
                    closedNodes.Add(node);
                }
                openNodes = OpenNodesWithMinimumCost();
                i++;
            }
            
            closedNodes.ForEach(n=> Debug.Log($"Nodo: {n.Element} | {n.Cost} | {n.State}"));

            var current = closedNodes.Find(n => n.Equals(destination));

            while (current.Parent is not null)
            {
                path.Add(current.Parent);
                current = current.Parent;
            }

            return path;
        }
        
        private IEnumerable<Node<Vector2Int>> OpenNodesWithMinimumCost()
        {
            return Nodes
                .Where(n => n.State == NodeState.Open)
                .Where(on => on.Cost == NodeWithMinimumCost().Cost);
        }

        private Node<Vector2Int> NodeWithMinimumCost() => 
            Nodes.First(n => n.Cost == Nodes.Select(n1 => n1.Cost).Min());

        private static int GetMinimumCost(IEnumerable<Node<Vector2Int>> nodes) =>
            nodes.Select(n => n.Cost).Min();
        
        private Node<Vector2Int> NeighbourWithMinimumCost(Node<Vector2Int> node)
        {
            var neighbours = Enumerable.Range(node.Element.x - 1, NeighbourDistance)
                .SelectMany(kk =>
                    Enumerable.Range(node.Element.y - 1, NeighbourDistance).Select(vv => new Vector2Int(kk, vv)))
                .Where(p => Nodes.Select(n => n.Element).Contains(p))
                .SelectMany(p => Nodes.ToList().Where(n => n.Element.Equals(p)));

            return neighbours.First(n => n.Cost == GetMinimumCost(neighbours));
        }

        private IEnumerable<Node<Vector2Int>> NonClosedNeighbours(Node<Vector2Int> node)
        {
            return Enumerable.Range(node.Element.x - 1, NeighbourDistance)
                .SelectMany(kk => 
                    Enumerable.Range(node.Element.y - 1, NeighbourDistance).Select(vv => new Vector2Int(kk, vv)))
                .Where(p => Nodes.Select(n => n.Element).Contains(p))
                .SelectMany(p => Nodes.ToList().Where(n => n.Element.Equals(p)))
                .Where(n => n.State != NodeState.Close);
        }

        private static bool TryUpdateCost(Node<Vector2Int> toBeUpdated, Node<Vector2Int> from)
        {
            var cost = from.Cost + (NodeUtil.IsDiagonalNeighbour(from, toBeUpdated)
                ? DiagonalCost
                : HorizontalAndVerticalCost);

            if (cost > toBeUpdated.Cost) return false;
            toBeUpdated.Cost = cost;
            return true;
        }
    }
}