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
        private static readonly int NeighbourDistance = 3;
        private static readonly int HorizontalAndVerticalCost = 10;
        private static readonly int DiagonalCost = 14;
        private static readonly int AlgorithmIterations = 5;

        private IEnumerable<Node<Vector2Int>> Nodes { get; set; }
        private IList<Node<Vector2Int>> AlreadyVisited { get; set; } = new List<Node<Vector2Int>>();

        public IList<Tuple<Vector3,Vector2Int>> CreatePath(IDictionary<Vector3,Vector2Int> map, Vector2Int startingPosition, int depth)
        {
            Nodes = NodeUtil.MapToNodes(map);
            var source = new Node<Vector2Int>(startingPosition) { Cost = 0, State = NodeState.Open };
            var destination = ChooseDestination(source, depth);
            Nodes = Nodes.Append(source);
            var path = FindPath(source, destination);
            
            throw new NotImplementedException();
        }

        private Node<Vector2Int> ChooseDestination(Node<Vector2Int> source, int depth)
        {
            var items = Nodes.Where(
                n => Math.Abs(n.Element.x - source.Element.x) >= depth &&
                                       Math.Abs(n.Element.y - source.Element.y) >= depth).ToArray();

            return items[new Random().Next(0, items.Length - 1)];
        }
        
        private IEnumerable<Node<Vector2Int>> FindPath(Node<Vector2Int> source, Node<Vector2Int> destination)
        {
            var openNodes = NextNodes();

            foreach (var node in openNodes)
            {
                node.State = NodeState.Close;
                
            }


            throw new NotImplementedException();
        }
        
        private IEnumerable<Node<Vector2Int>> NextNodes()
        {
            return Nodes
                .Where(n => n.State == NodeState.Open)
                .Where(on => on.Cost == Nodes.Select(n => n.Cost).Min());
        }

        private IEnumerable<Node<Vector2Int>> Neighbours(Node<Vector2Int> node)
        {
            return Enumerable.Range(node.Element.x - 1, NeighbourDistance)
                .SelectMany(kk =>
                    Enumerable.Range(node.Element.y - 1, NeighbourDistance).Select(vv => new Vector2Int(kk, vv)))
                .Where(p => Nodes.Select(n => n.Element).Contains(p))
                .SelectMany(p => Nodes.ToList().Where(n => n.Element.Equals(p)));
        }

        private IEnumerable<Node<Vector2Int>> NonInfiniteNeighbours(Node<Vector2Int> node)
        {
            return Enumerable.Range(node.Element.x - 1, NeighbourDistance)
                .SelectMany(kk => 
                    Enumerable.Range(node.Element.y - 1, NeighbourDistance).Select(vv => new Vector2Int(kk, vv)))
                .Where(p => Nodes.Select(n => n.Element).Contains(p))
                .SelectMany(p => Nodes.ToList().Where(n => n.Element.Equals(p)))
                .Where(n => n.Cost < int.MaxValue);
        }

        private void UpdateCost(Node<Vector2Int> toBeUpdated, Node<Vector2Int> from)
        {
            toBeUpdated.Cost = IsDiagonalNeighbour(from, toBeUpdated)
                ? from.Cost + HorizontalAndVerticalCost
                : from.Cost + DiagonalCost;
        }
            

        private bool IsDiagonalNeighbour(Node<Vector2Int> node, Node<Vector2Int> neighbour) =>
            neighbour.Element.Equals(node.Element + new Vector2Int(1, 1)) ||
            neighbour.Element.Equals(node.Element + new Vector2Int(-1,-1)) ||
            neighbour.Element.Equals(node.Element + new Vector2Int(-1,1)) ||
            neighbour.Element.Equals(node.Element + new Vector2Int(1,-1));
    }
}