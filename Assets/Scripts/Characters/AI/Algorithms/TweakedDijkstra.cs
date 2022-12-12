using System;
using System.Collections.Generic;
using Characters.AI.Algorithms.Util;
using UnityEngine;
using System.Linq;
using Map;

namespace Characters.AI.Algorithms
{
    /// <summary>
    /// Represents a template for a slightly modified version of the Dijkstra's Algorithm.
    /// After randomly select a destination, the algorithm runs the Dijkstra's Algorithm n times, where n is previously
    /// calculated by the <see cref="TweakedDijkstra{T}.SetIterations"/> method, and stores the path found. Ultimately,
    /// the algorithm chooses a random path from a list containing all the generated paths.
    /// </summary>
    /// <typeparam name="T">Any struct that represents a position in a custom coordinate system.</typeparam>
    public abstract class TweakedDijkstra<T> : IPathStrategy<T> where T : struct
    {
        protected static readonly int NeighbourDistance = 3;
        protected static readonly int HorizontalAndVerticalCost = 10;
        protected static readonly int DiagonalCost = 14;
        
        protected int _algorithmIterations;
        protected readonly ISet<IList<Node<T>>> _generatedPaths = new HashSet<IList<Node<T>>>();
        protected List<Node<T>> Nodes { get; private set; }
        
        /// <inheritdoc cref="IPathStrategy{T}.CreatePath"/>
        public IList<Tuple<Vector3,T>> CreatePath(IDictionary<Vector3,T> map, T startingPosition, int depth)
        {
            Nodes = NodeUtil.MapToNodes(map).ToList(); // Create the graph from map
            var source = new Node<T>(startingPosition) { Cost = 0, State = NodeState.Open }; // Create source node
            var destination = ChooseDestination(); // Choose a random destination
            _algorithmIterations = SetIterations(destination); // Sets how many iterations the algorithm has to do

            for (var i = 0; i < _algorithmIterations; i++)
            {
                // Find shortest path, store it and update the graph map
                Nodes.Add(source);
                var path = FindShortestPath(destination);
                _generatedPaths.Add(path);
                ResetMap(startingPosition);
                RemovePathFromMap(path);
            }

            // Return a random path
            var finalPath = ChooseRandomPath();
            return finalPath.Select(n =>
                    new Tuple<Vector3,T>(MapUtil.GetKeyFromValue(map, n.Element), n.Element))
                .ToList();
        }

        /// <summary>
        /// Compute how many iterations the algorithm has to do by counting how many entry points the selected
        /// destination has.
        /// </summary>
        /// <param name="destination">The destination node previously selected.</param>
        /// <returns>The number of algorithm's iterations.</returns>
        protected abstract int SetIterations(Node<T> destination);
        
        /// <summary>
        /// Runs the Dijkstra's Algorithm to find the shortest path.
        /// </summary>
        /// <param name="destination">The node to reach.</param>
        /// <returns>The shortest path between source and destination.</returns>
        protected abstract IList<Node<T>> FindShortestPath(Node<T> destination);
        
        /// <summary>
        /// Given a node, returns the neighbour with the minimum cost.
        /// </summary>
        /// <param name="node">The target node.</param>
        /// <returns>His neighbour with minimum cost.</returns>
        protected abstract Node<T> NeighbourWithMinimumCost(Node<T> node);

        /// <summary>
        /// Given a node, returns all his neighbours with State != Close.
        /// </summary>
        /// <param name="node">The target node.</param>
        /// <returns>All neighbours with State != Close</returns>
        protected abstract IList<Node<T>> NonClosedNeighbours(Node<T> node);

        /// <summary>
        /// Tries to update a target node's cost if the other node's cost plus the movement cost is less than the current
        /// target node cost.
        /// </summary>
        /// <param name="toBeUpdated">The node with the cost to be updated.</param>
        /// <param name="from">The node from which we are coming.</param>
        /// <returns>True if the cost has been updated, false otherwise.</returns>
        protected abstract bool TryUpdateCost(Node<T> toBeUpdated, Node<T> from);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startingPosition"></param>
        protected abstract void ResetMap(T startingPosition);
        
        /// <summary>
        /// Chooses a random destination.
        /// </summary>
        /// <returns>The new destination node.</returns>
        protected virtual Node<T> ChooseDestination()
        {
            var rng = new System.Random();
            var n = rng.Next(Nodes.Count - 1);
            Debug.Log($"Generated Number {n}");
            return Nodes[n];
        }
        
        /// <summary>
        /// Returns all nodes with State = Open and minimum cost.
        /// </summary>
        /// <returns>A <c>IList</c> of nodes.</returns>
        protected IList<Node<T>> OpenNodesWithMinimumCost() =>
            Nodes
                .Where(n => n.State == NodeState.Open)
                .Where(on => on.Cost == NodeWithMinimumCost().Cost)
                .ToList();
        
        /// <summary>
        /// Returns the node with minimum cost.
        /// </summary>
        /// <returns>The node with cost minimum.</returns>
        protected Node<T> NodeWithMinimumCost() =>
            Nodes.First(n => n.Cost == Nodes
                .Where(n1 => n1.State == NodeState.Open)
                .Select(n1 => n1.Cost).Min());
        
        /// <summary>
        /// Returns the minimum cost over the entire graph.
        /// </summary>
        /// <param name="nodes">the graph</param>
        /// <returns>An integer describing the minimum cost.</returns>
        protected int GetMinimumCost(IEnumerable<Node<T>> nodes) =>
            nodes.Select(n => n.Cost).Min();

        /// <summary>
        /// Removes a generated path from the graph by setting each node to State = Used.
        /// </summary>
        /// <param name="path">The path generated by Dijkstra's Algorithm.</param>
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
        
        /// <summary>
        /// Chooses a random path from all the generated path by Dijkstra's Algorithm.
        /// </summary>
        /// <returns>The selected path.</returns>
        protected IList<Node<T>> ChooseRandomPath() => _generatedPaths.Count > 0 ? 
            _generatedPaths.ElementAt(new System.Random().Next(0, _generatedPaths.Count - 1)) : new List<Node<T>>();
    }
}