using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Characters.AI.Algorithms.Util
{
    public sealed class Node<T> where T : struct
    {
        public T Element { get; }
        public int Cost { get; set; } = int.MaxValue;
        public NodeState State { get; set; } = NodeState.None;
        [CanBeNull] public Node<T> Parent { get; set; }

        public Node(T element) => Element = element;

        public override int GetHashCode() => HashCode.Combine(Element);

        public override bool Equals(object obj) => obj is Node<T> node &&
            EqualityComparer<T>.Default.Equals(Element, node.Element) &&
            EqualityComparer<int>.Default.Equals(Cost, node.Cost);

        public override string ToString() =>
            $"Element: {Element} | " +
            $"Cost: {Cost} | " +
            $"Parent: {Parent?.Element} | " +
            $"State: {State}";
    }
}