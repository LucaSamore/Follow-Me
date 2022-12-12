using System.Collections.Generic;
using Characters.AI.Algorithms.Util;
using UnityEngine;

namespace Characters.AI.Algorithms._3D
{
    /// <summary>
    /// Gives an implementation for all abstract method from <see cref="NeighbourAlgorithm{T}"/>
    /// </summary>
    public sealed class TweakedDijkstra3D : TweakedDijkstra<Vector3Int>
    {
        /// <inheritdoc cref="TweakedDijkstra{T}.SetIterations"/>
        protected override int SetIterations(Node<Vector3Int> destination)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="TweakedDijkstra{T}.FindShortestPath"/>
        protected override IList<Node<Vector3Int>> FindShortestPath(Node<Vector3Int> destination)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="TweakedDijkstra{T}.NeighbourWithMinimumCost"/>
        protected override Node<Vector3Int> NeighbourWithMinimumCost(Node<Vector3Int> node)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="TweakedDijkstra{T}.NonClosedNeighbours"/>
        protected override IList<Node<Vector3Int>> NonClosedNeighbours(Node<Vector3Int> node)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="TweakedDijkstra{T}.TryUpdateCost"/>
        protected override bool TryUpdateCost(Node<Vector3Int> toBeUpdated, Node<Vector3Int> from)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="TweakedDijkstra{T}.ResetMap"/>
        protected override void ResetMap(Vector3Int startingPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}