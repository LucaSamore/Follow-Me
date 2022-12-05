using System;
using System.Collections.Generic;
using Characters.AI.Algorithms.Util;
using UnityEngine;
using System.Linq;
using Map;

namespace Characters.AI.Algorithms
{
    public abstract class TweakedDijkstra<T> : IPathStrategy<T> where T : struct
    {
        protected static readonly int NeighbourDistance = 3;
        protected static readonly int HorizontalAndVerticalCost = 10;
        protected static readonly int DiagonalCost = 14;
        
        protected int _algorithmIterations;
        protected readonly ISet<IList<Node<T>>> _generatedPaths = new HashSet<IList<Node<T>>>();
        protected List<Node<T>> Nodes { get; set; }
        
        public IList<Tuple<Vector3,T>> CreatePath(IDictionary<Vector3,T> map, T startingPosition, int depth)
        {
            Nodes = NodeUtil.MapToNodes(map).ToList();
            var source = new Node<T>(startingPosition) { Cost = 0, State = NodeState.Open };
            var destination = ChooseDestination(depth);
            _algorithmIterations = SetIterations(destination);
            Debug.Log($"Destination: {destination.Element}");

            for (var i = 0; i < _algorithmIterations; i++)
            {
                Nodes.Add(source);
                var path = FindShortestPath(destination);
                _generatedPaths.Add(path);
                ResetMap();
                RemovePathFromMap(path);
            }

            var finalPath = ChooseRandomPath();

            return finalPath.Select(n =>
                    new Tuple<Vector3,T>(MapUtil.GetKeyFromValue(map, n.Element), n.Element))
                .ToList();
        }

        protected abstract int SetIterations(Node<T> destination);
        
        protected abstract IList<Node<T>> FindShortestPath(Node<T> destination);
        
        protected abstract Node<T> NeighbourWithMinimumCost(Node<T> node);

        protected abstract IList<Node<T>> NonClosedNeighbours(Node<T> node);

        protected abstract bool TryUpdateCost(Node<T> toBeUpdated, Node<T> from);

        protected abstract void ResetMap();
        
        protected virtual Node<T> ChooseDestination(int depth)
        {
            // var rng = new Random();
            // var items = Nodes
            //     .Where(n => Math.Abs(n.Element.x) >= depth || 
            //              Math.Abs(n.Element.y) >= depth)
            //     .ToList();
            //
            // return items.OrderBy(i => rng.Next()).First();
            
            return Nodes[6];
        }
        
        protected IList<Node<T>> OpenNodesWithMinimumCost() =>
            Nodes
                .Where(n => n.State == NodeState.Open)
                .Where(on => on.Cost == NodeWithMinimumCost().Cost)
                .ToList();
        
        protected Node<T> NodeWithMinimumCost() =>
            Nodes.First(n => n.Cost == Nodes
                .Where(n1 => n1.State == NodeState.Open)
                .Select(n1 => n1.Cost).Min());
        
        protected int GetMinimumCost(IEnumerable<Node<T>> nodes) =>
            nodes.Select(n => n.Cost).Min();

        protected void RemovePathFromMap(ICollection<Node<T>> path)
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
        
        protected IList<Node<T>> ChooseRandomPath() => _generatedPaths.Count > 0 ? 
            _generatedPaths.ElementAt(new System.Random().Next(0, _generatedPaths.Count - 1)) : new List<Node<T>>();
    }
}