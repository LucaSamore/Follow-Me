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
        private IList<IAgent> _agents;
        
        public void AgentWalk() => _agents[0].Walk();

        public void AgentJump() => _agents[0].Jump();

        private void Awake()
        {
            _agents = new List<IAgent>();
            _navMeshFactory = new NavMeshFactory();
            _agents.Add(isMap2D ?
                new Agent<Vector2Int>(characterController,_navMeshFactory.BakeMesh2D(navigable, startingPosition.position), 
                    new TweakedDijkstra2Dv2(), 
                    new Vector2Int(0,0)) : 
                new Agent<Vector3Int>(characterController,_navMeshFactory.BakeMesh3D(navigable, startingPosition.position), 
                    null, 
                    new Vector3Int(0,0,0)));
        }

        private void Start()
        {
            _gravity = -9.81f * Time.deltaTime;
            healthBar.SetMaxHealth(maxHp);
        }
    
        private void Update()
        {
            //characterController.Move(new Vector3(0f, _gravity, 0f));
            AgentWalk();

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
