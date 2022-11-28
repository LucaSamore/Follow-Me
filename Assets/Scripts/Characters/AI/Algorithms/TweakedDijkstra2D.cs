using System;
using System.Collections.Generic;
using System.Linq;
using Characters.AI.Algorithms.Util;
using Map;
using UnityEngine;
using Random = System.Random;

namespace Characters.AI.Algorithms
{
    public sealed class TweakedDijkstra2D : IPathStrategy<Vector2Int>
    {
        private static readonly int NeighbourDistance = 3;
        private static readonly int HorizontalAndVerticalCost = 10;
        private static readonly int DiagonalCost = 14;
        private static readonly int AlgorithmIterations = 6;

        private readonly ISet<IList<Node<Vector2Int>>> _generatedPaths = new HashSet<IList<Node<Vector2Int>>>();
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
                ResetMap();
                RemovePathFromMap(path);
            }

            var finalPath = ChooseRandomPath();

            foreach (var p in _generatedPaths)
            {
                Debug.Log("Path");
                p.ToList().ForEach(e => Debug.Log(e.Element));
            }

            return finalPath
                .Select(n => 
                    new Tuple<Vector3, Vector2Int>(MapUtil.GetKeyFromValue(map, n.Element), n.Element))
                .ToList();
        }

        private Node<Vector2Int> ChooseDestination(Node<Vector2Int> source, int depth)
        {
            var items = Nodes.Where(
                n => Math.Abs(n.Element.x - source.Element.x) >= depth &&
                                       Math.Abs(n.Element.y - source.Element.y) >= depth).ToArray();

            return items[new Random().Next(0, items.Length - 1)];
        }
        
        #region DIJKSTRA ALGORITHM
        
        private IList<Node<Vector2Int>> FindShortestPath(Node<Vector2Int> destination)
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
                    nonClosedNeighbours.ToList().ForEach(n => n.State = NodeState.Open);

                    nonClosedNeighbours.ToList().ForEach(n =>
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
        
        private IList<Node<Vector2Int>> OpenNodesWithMinimumCost() =>
            Nodes
                .Where(n => n.State == NodeState.Open)
                .Where(on => on.Cost == NodeWithMinimumCost().Cost)
                .ToList();
        
        private Node<Vector2Int> NodeWithMinimumCost() =>
            Nodes.First(n => n.Cost == Nodes
                .Where(n1 => n1.State == NodeState.Open)
                .Select(n1 => n1.Cost).Min());

        private int GetMinimumCost(IEnumerable<Node<Vector2Int>> nodes) =>
            nodes.Select(n => n.Cost).Min();
        
        private Node<Vector2Int> NeighbourWithMinimumCost(Node<Vector2Int> node)
        {
            var neighbours = Enumerable.Range(node.Element.x - 1, NeighbourDistance)
                .SelectMany(kk =>
                    Enumerable.Range(node.Element.y - 1, NeighbourDistance).Select(vv => new Vector2Int(kk, vv)))
                .Where(p => Nodes.Select(n => n.Element).Contains(p))
                .SelectMany(p => Nodes.ToList().Where(n => n.Element.Equals(p)))
                .Where(n => n.State != NodeState.Used);
            
            return neighbours.First(n => n.Cost == GetMinimumCost(neighbours));
        }

        private IList<Node<Vector2Int>> NonClosedNeighbours(Node<Vector2Int> node) =>
            Enumerable.Range(node.Element.x - 1, NeighbourDistance)
                .SelectMany(kk =>
                    Enumerable.Range(node.Element.y - 1, NeighbourDistance).Select(vv => new Vector2Int(kk, vv)))
                .Where(p => Nodes.Select(n => n.Element).Contains(p))
                .SelectMany(p => Nodes.ToList().Where(n => n.Element.Equals(p)))
                .Where(n => n.State != NodeState.Close && n.State != NodeState.Used)
                .ToList();

        private bool TryUpdateCost(Node<Vector2Int> toBeUpdated, Node<Vector2Int> from)
        {
            var cost = from.Cost + (NodeUtil.IsDiagonalNeighbour(from, toBeUpdated)
                ? DiagonalCost
                : HorizontalAndVerticalCost);

            if (cost > toBeUpdated.Cost) return false;
            toBeUpdated.Cost = cost;
            return true;
        }

        private void ResetMap()
        {
            Nodes
                .Where(n => !n.Element.Equals(Vector2Int.zero))
                .Where(n => n.State != NodeState.Used)
                .ToList()
                .ForEach(n =>
            {
                n.Cost = int.MaxValue;
                n.Parent = null;
                n.State = NodeState.None;
            });

            Nodes.First(n => n.Element.Equals(Vector2Int.zero)).State = NodeState.Open;
        }
        
        #endregion
        
        #region TWEAK
        
        private void RemovePathFromMap(ICollection<Node<Vector2Int>> path)
        {
            Nodes
                .Where(n => path
                    .ToList()
                    .GetRange(1, path.Count - 2)
                    .Select(x => x.Element)
                    .Contains(n.Element))
                .ToList()
                .ForEach(n => n.State = NodeState.Used);
        }

        private IList<Node<Vector2Int>> ChooseRandomPath() => 
            _generatedPaths.ElementAt(new Random().Next(0, _generatedPaths.Count - 1));
        
        #endregion
    }
}