using System;
using System.Collections.Generic;
using Characters.AI.Algorithms._2D;
using Characters.AI.CustomAgent;
using Characters.HealthBar;
using UnityEngine;
using JetBrains.Annotations;
using Map.CustomNavMesh;

namespace Characters.AI
{
    public sealed class OpponentController : MonoBehaviour
    {
        private static readonly float Speed = 3f;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        [SerializeField] private Transform navigable;
        [SerializeField] private Transform startingPosition;
        [SerializeField] private bool isMap2D;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private int maxHp;
        [SerializeField] private int currentHp;
        [SerializeField] private HealthBarController healthBar;
        
        [CanBeNull] private Renderer _previousTile;
        private float _gravity;
        private INavMeshFactory _navMeshFactory;
        private IAgent _agent;
        
        public void AgentWalk() => _agent.Walk();

        public void AgentJump() => _agent.Jump();

        private void Awake()
        {
            _navMeshFactory = new NavMeshFactory();
            _agent = isMap2D ?
                new Agent<Vector2Int>(_navMeshFactory.BakeMesh2D(navigable, startingPosition.position), new TweakedDijkstra2D()) : 
                new Agent<Vector3Int>(_navMeshFactory.BakeMesh3D(navigable, startingPosition.position), null);
        }

        private void Start()
        {
            _gravity = -9.81f * Time.deltaTime;
            healthBar.SetMaxHealth(maxHp);
        }
    
        private void Update()
        {
            characterController.Move(new Vector3(0f, _gravity, 0f));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(5);
            }
        }

        private void TakeDamage(int damage)
        {
            currentHp -= damage;
            healthBar.SetHealth(currentHp);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.name != "Cube") return;
            if (_previousTile is not null) _previousTile.material.SetColor(EmissionColor, Color.black);
            var renderer = hit.gameObject.GetComponent<Renderer>();
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor(EmissionColor, Color.green);
            _previousTile = renderer;
        }
    }
}
