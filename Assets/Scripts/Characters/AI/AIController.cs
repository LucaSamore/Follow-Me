using System.Collections.Generic;
using Characters.AI.Algorithms._2D;
using Characters.AI.CustomAgent;
using Characters.HealthBar;
using UnityEngine;
using JetBrains.Annotations;
using Map.CustomNavMesh;

namespace Characters.AI
{
    public sealed class AIController : MonoBehaviour
    {
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        [SerializeField] private Transform navigable;
        [SerializeField] private Transform startingPosition;
        [SerializeField] private bool isMap2D;
        [SerializeField] private int maxHp;
        [SerializeField] private int currentHp;
        [SerializeField] private HealthBarController healthBar;
        [SerializeField] private GameObject agentObject;
        [SerializeField] private int howMany;
        
        [CanBeNull] private Renderer _previousTile;
        private INavMeshFactory _navMeshFactory;
        private IList<IAgent> _agents;
        private AgentMovement _agentMovement;
        
        public void AgentWalk() => _agents[0].Walk();

        public void AgentJump() => _agents[0].Jump();

        private void Awake()
        {
            _agentMovement = agentObject.GetComponent<AgentMovement>();
            _navMeshFactory = new NavMeshFactory();
        }

        private void Start()
        {
            _agents = new List<IAgent>();
            _navMeshFactory = new NavMeshFactory();
            _agents.Add(isMap2D ?
                new Agent<Vector2Int>(transform, _agentMovement, _navMeshFactory.BakeMesh2D(navigable, startingPosition.position), 
                    new TweakedDijkstra2D(), 
                    new Vector2Int(0,0)) : 
                new Agent<Vector3Int>(transform, _agentMovement, _navMeshFactory.BakeMesh3D(navigable, startingPosition.position), 
                    null, 
                    new Vector3Int(0,0,0)));
            
            healthBar.SetMaxHealth(maxHp);
            CreateMany();
        }
    
        private void Update()
        {
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.name != "Cube") return;
            //if (_previousTile is not null) _previousTile.material.SetColor(EmissionColor, Color.black);
            var renderer = other.gameObject.GetComponent<Renderer>();
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor(EmissionColor, Color.cyan);
            //_previousTile = renderer;
        }
        
        private void CreateMany()
        {
            if(howMany <= 0) return;

            for (var i = 0; i < howMany; i++)
            {
                var newAgent = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                newAgent.name = "AGENT 007";
                newAgent.transform.parent = agentObject.transform;
                newAgent.AddComponent<SphereCollider>();
                newAgent.AddComponent<Rigidbody>();
                newAgent.GetComponent<Rigidbody>().isKinematic = true;
                newAgent.GetComponent<Renderer>().material = Resources.Load("Opponent Material", typeof(Material)) as Material;
                newAgent.AddComponent<HealthBarController>();
                newAgent.transform.position = new Vector3(6.5f, 1f, .5f);
                //_agents.Add(newAgent);
            }
        }
    }
}
