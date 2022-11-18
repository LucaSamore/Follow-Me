using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Algorithms
{
    public interface IPathStrategy
    {
        IList<Tuple<Vector3,Vector2Int>> CreatePath(IDictionary<Vector3,Vector2Int> map, Vector2Int startingPosition, int steps);
    }
}