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

        private readonly ISet<List<Node<Vector2Int>>> _generatedPaths = new HashSet<List<Node<Vector2Int>>>();

        private readonly Func<IDictionary<Vector3,Vector2Int>, Vector2Int, Vector3> _getKeyFromValue = (map, v2) =>
            map.FirstOrDefault(x => x.Value.Equals(v2)).Key;

        private List<Node<Vector2Int>> Nodes { get; set; }

        public IList<Tuple<Vector3,Vector2Int>> CreatePath(IDictionary<Vector3,Vector2Int> map, Vector2Int startingPosition, int depth)
        {
            Nodes = NodeUtil.MapToNodes(map).ToList();
            var source = new Node<Vector2Int>(startingPosition) { Cost = 0, State = NodeState.Open };
            var destination = ChooseDestination(source, depth);
            Debug.Log($"Destination: {destination.Element}");
            
            for (var i = 0; i < AlgorithmIterations; i++)
            {
                Nodes.Add(source);
                var path = FindShortestPath(destination);
                _generatedPaths.Add(path);
                Nodes.Clear();
                Nodes.AddRange(NodeUtil.MapToNodes(map).ToList());
                RemovePathFromMap(path);
                //Nodes = NodeUtil.MapToNodes(map).ToList();
            }

            var finalPath = ChooseRandomPath();
            finalPath.ForEach(Debug.Log);

            // return path
            //     .Select(node => new Tuple<Vector3, Vector2Int>(_getKeyFromValue(map, node.Element), node.Element))
            //     .ToList();
            
            throw new NotImplementedException();
        }

        private Node<Vector2Int> ChooseDestination(Node<Vector2Int> source, int depth) // OK!
        {
            var items = Nodes.Where(
                n => Math.Abs(n.Element.x - source.Element.x) >= depth &&
                                       Math.Abs(n.Element.y - source.Element.y) >= depth).ToArray();

            return items[new Random().Next(0, items.Length - 1)];
        }
        
        private List<Node<Vector2Int>> FindShortestPath(Node<Vector2Int> destination)
        {
            var end = false;
            var path = new List<Node<Vector2Int>>();
            var closedNodes = new List<Node<Vector2Int>>();
            var openNodes = OpenNodesWithMinimumCost();

            while (!end)
            {
                foreach (var node in openNodes)
                {
                    node.State = NodeState.Close;
                    var nonClosedNeighbours = NonClosedNeighbours(node);
                    nonClosedNeighbours.ForEach(n => n.State = NodeState.Open);

                    nonClosedNeighbours.ForEach(n =>
                    {
                        var neighbourWithMinCost = NeighbourWithMinimumCost(n);

                        if (TryUpdateCost(n, neighbourWithMinCost))
                        {
                            n.Parent = neighbourWithMinCost;
                        }
                    });
                
                    closedNodes.Add(node);
                }
                end = closedNodes.Select(cn => cn.Element).Where(v2 => v2.Equals(destination.Element)).ToArray().Length == 1;
                openNodes = OpenNodesWithMinimumCost();
            }

            var current = closedNodes.Find(n => n.Element.Equals(destination.Element));
            path.Add(current);

            while (current.Parent is not null)
            {
                path.Add(current.Parent);
                current = current.Parent;
            }

            path.Reverse();

            return path;
        }
        
        private List<Node<Vector2Int>> OpenNodesWithMinimumCost() // OK!
        {
            return Nodes
                .Where(n => n.State == NodeState.Open)
                .Where(on => on.Cost == NodeWithMinimumCost().Cost)
                .ToList();
        }

        private Node<Vector2Int> NodeWithMinimumCost() => // OK!
            Nodes.First(n => n.Cost == Nodes
                .Where(n1 => n1.State == NodeState.Open)
                .Select(n1 => n1.Cost).Min());

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

        private List<Node<Vector2Int>> NonClosedNeighbours(Node<Vector2Int> node) // OK!
        {
             return Enumerable.Range(node.Element.x - 1, NeighbourDistance)
                .SelectMany(kk =>
                    Enumerable.Range(node.Element.y - 1, NeighbourDistance).Select(vv => new Vector2Int(kk, vv)))
                .Where(p => Nodes.Select(n => n.Element).Contains(p))
                .SelectMany(p => Nodes.ToList().Where(n => n.Element.Equals(p)))
                .Where(n => n.State != NodeState.Close)
                .ToList();
        }

        private static bool TryUpdateCost(Node<Vector2Int> toBeUpdated, Node<Vector2Int> from) // OK!
        {
            var cost = from.Cost + (NodeUtil.IsDiagonalNeighbour(from, toBeUpdated)
                ? DiagonalCost
                : HorizontalAndVerticalCost);

            if (cost > toBeUpdated.Cost) return false;
            toBeUpdated.Cost = cost;
            return true;
        }

        private void RemovePathFromMap(ICollection<Node<Vector2Int>> path)
        {
            Nodes = Nodes
                .Where(n => !n.Element.Equals(path.First().Element))
                .Where(n => !n.Element.Equals(path.Last().Element))
                .Where(n => !path.Select(p => p.Element).Contains(n.Element))
                .ToList();
        }

        private List<Node<Vector2Int>> ChooseRandomPath() => _generatedPaths.ElementAt(new Random().Next(0, _generatedPaths.Count - 1));
    }
}