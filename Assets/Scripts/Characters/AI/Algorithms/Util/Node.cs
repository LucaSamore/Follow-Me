using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Algorithms.Util
{
    public struct Node
    {
        public Vector3 Position { get; }
        public Vector2Int PositionFromCenter { get; }
        public int Cost { get; set; }

        public Node(Vector3 position, Vector2Int positionFromCenter)
        {
            Position = position;
            PositionFromCenter = positionFromCenter;
            Cost = int.MaxValue;
        }

        public override int GetHashCode() => HashCode.Combine(Position, PositionFromCenter);

        public override bool Equals(object obj) => obj is Node node && 
            EqualityComparer<Vector3>.Default.Equals(Position, node.Position) &&
            EqualityComparer<Vector2Int>.Default.Equals(PositionFromCenter, node.PositionFromCenter) &&
            EqualityComparer<int>.Default.Equals(Cost, node.Cost);

        public override string ToString() => 
            $"Position: {Position} | PositionFromCenter: {PositionFromCenter} | Cost: {Cost}";
    }
}