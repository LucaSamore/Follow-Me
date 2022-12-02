using System.Collections.Generic;
using UnityEngine;

namespace Map.CustomNavMesh
{
    public interface INavMeshFactory<T>
    {
        IDictionary<Vector3,T> Bake(Transform zone, Vector3 startingPosition);
    }
}