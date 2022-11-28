using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Map
{
    public static class MapUtil
    {
        public static IDictionary<Vector3,Vector2Int> Map2D(Transform zone, Vector3 startingPosition) =>
            zone
                .Cast<Transform>()
                .Select(t => t.position)
                .ToDictionary(k => k, 
                    v => Vector2Int.RoundToInt(
                        new Vector2(v.x - startingPosition.x, v.z - startingPosition.z)));

        public static IDictionary<Vector3, Vector2Int> Map3D(Transform zone, Vector3 startingPosition) =>
            throw new NotImplementedException();

        public static Vector3 GetKeyFromValue<T>(IDictionary<Vector3,T> map, T v2) =>
            map.FirstOrDefault(kvp => kvp.Value.Equals(v2)).Key;
    }
}
