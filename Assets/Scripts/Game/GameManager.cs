using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.AI;
using Characters.AI.Algorithms;
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
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject opponent;

        private PlayerController _playerController;
        private OpponentController _opponentController;

        public IDictionary<Vector3, Vector2> Tilemap { get; private set; }
        public IDictionary<Vector3, Vector2> PlayerMap { get; private set; }
        public IDictionary<Vector3, Vector2> OpponentMap { get; private set; }

        private void Awake()
        {
            _playerController = player.GetComponent<PlayerController>();
            _opponentController = opponent.GetComponent<OpponentController>();
        }

        private void Start()
        {
            Tilemap = MapUtil.Merge(MapUtil.Map(playerZone, playerZoneCenter.transform.position), 
                MapUtil.Map(opponentZone, opponentZoneCenter.transform.position));
            PlayerMap = MapUtil.Map(playerZone, playerZoneCenter.transform.position);
            OpponentMap = MapUtil.Map(opponentZone, opponentZoneCenter.transform.position);

            _opponentController.PathBuilder = new PathBuilder(OpponentMap, new NeighbourAlgorithm());
        }

        private void Update()
        {
            //_opponentController.Test();
        }
    }
}
