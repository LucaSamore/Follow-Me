using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Map
{
    public static class MapUtil
    {
        public static IDictionary<Vector3,Vector2Int> Map(Transform zone, Vector3 startingPosition)
        {
            return zone
                .Cast<Transform>()
                .Select(t => t.position)
                .ToDictionary(k => k, 
                              v => Vector2Int.RoundToInt(
                                  new Vector2(v.x - startingPosition.x, v.z - startingPosition.z)));
        }

        public static IDictionary<Vector3,Vector2Int> Merge(IDictionary<Vector3,Vector2Int> playerZone,
            IDictionary<Vector3,Vector2Int> opponentZone)
        {
            return playerZone
                .Concat(opponentZone)
                .ToLookup(k => k.Key, v => v.Value)
                .ToDictionary(x => x.Key, y => y.First());
        }
    }
}
