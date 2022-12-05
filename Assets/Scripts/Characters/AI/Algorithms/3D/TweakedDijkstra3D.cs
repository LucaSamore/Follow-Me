using System.Collections.Generic;
using Characters.AI.Algorithms.Util;
using UnityEngine;

namespace Characters.AI.Algorithms._3D
{
    public sealed class TweakedDijkstra3D : TweakedDijkstra<Vector3Int>
    {
        protected override int SetIterations(Node<Vector3Int> destination)
        {
            throw new System.NotImplementedException();
        }

        protected override IList<Node<Vector3Int>> FindShortestPath(Node<Vector3Int> destination)
        {
            throw new System.NotImplementedException();
        }

        protected override Node<Vector3Int> NeighbourWithMinimumCost(Node<Vector3Int> node)
        {
            throw new System.NotImplementedException();
        }

        protected override IList<Node<Vector3Int>> NonClosedNeighbours(Node<Vector3Int> node)
        {
            throw new System.NotImplementedException();
        }

        protected override bool TryUpdateCost(Node<Vector3Int> toBeUpdated, Node<Vector3Int> from)
        {
            throw new System.NotImplementedException();
        }

        protected override void ResetMap(Vector3Int startingPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}