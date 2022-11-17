using System.Collections.Generic;
using System.Linq;
using Map;
using UnityEngine;

namespace Game
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private Transform playerZone;
        [SerializeField] private Transform opponentZone;
        [SerializeField] private Transform playerZoneCenter;
        [SerializeField] private Transform opponentZoneCenter;

        public IDictionary<Vector3, Vector2> Tilemap { get; private set; }
        private void Start()
        {
            Tilemap = MapUtil.Merge(MapUtil.Map(playerZone, playerZoneCenter.transform.position), 
                MapUtil.Map(opponentZone, opponentZoneCenter.transform.position));

            foreach (var kv in Tilemap)
            {
                Debug.Log(kv);
            }
        }
    }
}
