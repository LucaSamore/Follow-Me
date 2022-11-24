using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Algorithms
{
    public interface IPathStrategy<T> where T : struct
    {
        IList<Tuple<Vector3,T>> CreatePath(IDictionary<Vector3,T> map, T startingPosition, int steps);
    }
}