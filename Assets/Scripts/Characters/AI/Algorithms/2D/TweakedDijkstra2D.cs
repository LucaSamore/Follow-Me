﻿using System.Collections.Generic;
using Characters.AI.Algorithms.Util;
using UnityEngine;
using System.Linq;

namespace Characters.AI.Algorithms._2D
{
    /// <summary>
    /// Gives an implementation for all abstract method from <see cref="TweakedDijkstra{T}"/>
    /// </summary>
    public sealed class TweakedDijkstra2D : TweakedDijkstra<Vector2Int>
    {
        /// <inheritdoc cref="TweakedDijkstra{T}.FindShortestPath"/>
        protected override IList<Node<Vector2Int>> FindShortestPath(Node<Vector2Int> destination)
        {
            var path = new List<Node<Vector2Int>>();
            var closedNodes = new List<Node<Vector2Int>>();
            var openNodes = OpenNodesWithMinimumCost();
            
            while (destination.Parent is null)
            {
                foreach (var node in openNodes)
                {
                    node.State = NodeState.Close;
                    var nonClosedNeighbours = NonClosedNeighbours(node);
                    nonClosedNeighbours.ToList().ForEach(n => n.State = NodeState.Open);
                    nonClosedNeighbours.ToList().ForEach(n =>
                    {
                        var neighbourWithMinCost = NeighbourWithMinimumCost(n);
                        if (TryUpdateCost(n, neighbourWithMinCost)) n.Parent = neighbourWithMinCost;
                    });
                    closedNodes.Add(node);
                }
                if (destination.Parent is null) openNodes = OpenNodesWithMinimumCost();
            }
            
            var current = destination;
            path.Add(current);
            while (current.Parent is not null)
            {
                path.Add(current.Parent);
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }
        
        /// <inheritdoc cref="TweakedDijkstra{T}.SetIterations"/>
        protected override int SetIterations(Node<Vector2Int> destination) =>
            Enumerable
                .Range(destination.Element.x - 1, NeighbourDistance)
                .SelectMany(kk => Enumerable.Range(destination.Element.y - 1, NeighbourDistance)
                    .Select(vv => new Vector2Int(kk, vv)))
                .Where(p => Nodes.Select(n => n.Element).Contains(p))
                .SelectMany(p => Nodes.ToList().Where(n => n.Element.Equals(p)))
                .Count(n => n.State != NodeState.Used) - 1;
        
        /// <inheritdoc cref="TweakedDijkstra{T}.NonClosedNeighbours"/>
        protected override IList<Node<Vector2Int>> NonClosedNeighbours(Node<Vector2Int> node) =>
            Enumerable.Range(node.Element.x - 1, NeighbourDistance)
                .SelectMany(kk =>
                    Enumerable.Range(node.Element.y - 1, NeighbourDistance).Select(vv => new Vector2Int(kk, vv)))
                .Where(p => Nodes.Select(n => n.Element).Contains(p))
                .SelectMany(p => Nodes.ToList().Where(n => n.Element.Equals(p)))
                .Where(n => n.State != NodeState.Close && n.State != NodeState.Used)
                .ToList();

        /// <inheritdoc cref="TweakedDijkstra{T}.NeighbourWithMinimumCost"/>
        protected override Node<Vector2Int> NeighbourWithMinimumCost(Node<Vector2Int> node)
        {
            var neighbours = Enumerable.Range(node.Element.x - 1, NeighbourDistance)
                .SelectMany(kk =>
                    Enumerable.Range(node.Element.y - 1, NeighbourDistance).Select(vv => new Vector2Int(kk, vv)))
                .Where(p => Nodes.Select(n => n.Element).Contains(p))
                .SelectMany(p => Nodes.ToList().Where(n => n.Element.Equals(p)))
                .Where(n => n.State != NodeState.Used);

            var enumerable = neighbours as Node<Vector2Int>[] ?? neighbours.ToArray();
            return enumerable.First(n => n.Cost == GetMinimumCost(enumerable));
        }

        /// <inheritdoc cref="TweakedDijkstra{T}.TryUpdateCost"/>
        protected override bool TryUpdateCost(Node<Vector2Int> toBeUpdated, Node<Vector2Int> from)
        {
            var cost = from.Cost + (NodeUtil.IsDiagonalNeighbour(from, toBeUpdated)
                ? DiagonalCost
                : HorizontalAndVerticalCost);

            if (cost > toBeUpdated.Cost) return false;
            toBeUpdated.Cost = cost;
            return true;
        }

        /// <inheritdoc cref="TweakedDijkstra{T}.ResetMap"/>
        protected override void ResetMap(Vector2Int startingPosition)
        {
            Nodes
                .Where(n => !n.Element.Equals(startingPosition))
                .Where(n => n.State != NodeState.Used)
                .ToList()
                .ForEach(n =>
                {
                    n.Cost = int.MaxValue;
                    n.Parent = null;
                    n.State = NodeState.None;
                });

            Nodes.First(n => n.Element.Equals(startingPosition)).State = NodeState.Open;
        }
    }
}