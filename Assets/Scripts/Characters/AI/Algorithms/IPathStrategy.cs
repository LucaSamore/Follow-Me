using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Algorithms
{
    public interface IPathStrategy
    {
        IDictionary<Vector3,Vector2> CreatePath(IDictionary<Vector3,Vector2> map, Vector2 startingPosition, int steps);
    }
}