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
        [SerializeField] private int maxHp;
        [SerializeField] private int currentHp;
        [SerializeField] private HealthBarController healthBar;
        [SerializeField] private GameObject agent;

        private Rigidbody _rigidbody;
        [CanBeNull] private Renderer _previousTile;
        private float _gravity;
        private INavMeshFactory _navMeshFactory;
        private IList<IAgent> _agents;
        private AgentMovement _agentMovement;
        
        public void AgentWalk() => _agents[0].Walk();

        public void AgentJump() => _agents[0].Jump();

        private void Awake()
        {
            _agentMovement = agent.GetComponent<AgentMovement>();
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            _agents = new List<IAgent>();
            _navMeshFactory = new NavMeshFactory();
            _agents.Add(isMap2D ?
                new Agent<Vector2Int>(transform, _agentMovement, _navMeshFactory.BakeMesh2D(navigable, startingPosition.position), 
                    new TweakedDijkstra2D(), 
                    new Vector2Int(0,0)) : 
                new Agent<Vector3Int>(transform, _agentMovement, _navMeshFactory.BakeMesh3D(navigable, startingPosition.position), 
                    null, 
                    new Vector3Int(0,0,0)));
            
            _gravity = -9.81f * Time.deltaTime;
            healthBar.SetMaxHealth(maxHp);
            
        }
    
        private void Update()
        {
            //characterController.Move(new Vector3(0f, _gravity, 0f));
            //AgentWalk();
            
            // transform.position = Vector3.MoveTowards(transform.position, new Vector3(9.5f, 0f, 1.5f),
            //     Speed * Time.deltaTime);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AgentWalk();
                TakeDamage(5);
            }
        }

        private void TakeDamage(int damage)
        {
            currentHp -= damage;
            healthBar.SetHealth(currentHp);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.name != "Cube") return;
            //if (_previousTile is not null) _previousTile.material.SetColor(EmissionColor, Color.black);
            var renderer = collision.gameObject.GetComponent<Renderer>();
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor(EmissionColor, Color.green);
            //_previousTile = renderer;
        }
    }
}
