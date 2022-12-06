using System.Collections.Generic;
using Characters.AI.Algorithms._2D;
using Characters.AI.Algorithms._3D;
using Characters.AI.CustomAgent;
using Characters.HealthBar;
using Map.CustomNavMesh;
using UnityEngine;

namespace Characters.AI
{
    public sealed class AIController : MonoBehaviour
    {
        [SerializeField] private Transform navigable;
        [SerializeField] private Transform startingPosition;
        [SerializeField] private bool isMap2D;
        [SerializeField] private int maxHp;
        [SerializeField] private int currentHp;
        [SerializeField] private HealthBarController healthBar;
        [SerializeField] private GameObject agentSource;
        [SerializeField] public int howMany;
        
        private INavMeshFactory _navMeshFactory;
        private IAgent _aiAgent;
        private AgentMovement _agentMovement;
        private IList<IAgent> _generatedAgents;

        private void Awake()
        {
            _navMeshFactory = new NavMeshFactory();
            _agentMovement = agentSource.GetComponent<AgentMovement>();
            _generatedAgents = new List<IAgent>();
        }

        private void Start()
        {
            _aiAgent = isMap2D ? new Agent<Vector2Int>(
                    transform.gameObject, 
                    _agentMovement,
                    _navMeshFactory.BakeMesh2D(navigable, startingPosition.position),
                    new TweakedDijkstra2D(),
                    new Vector2Int(0,0)) : 
                new Agent<Vector3Int>(
                    transform.gameObject, 
                    _agentMovement, 
                    _navMeshFactory.BakeMesh3D(navigable, startingPosition.position), 
                    new TweakedDijkstra3D(), 
                    new Vector3Int(0,0,0));

            healthBar.SetMaxHealth(maxHp);
            CreateMany();
        }
    
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _aiAgent.Walk();
                
                foreach (var a in _generatedAgents)
                {
                    a.Walk();
                }

                TakeDamage(5);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                DestroyAll();
            }
        }

        private void TakeDamage(int damage)
        {
            currentHp -= damage;
            healthBar.SetHealth(currentHp);
        }

        private void CreateMany()
        {
            if(howMany <= 0) return;

            for (var i = 0; i < howMany; i++)
            {
                var newAgent = _aiAgent.Clone();
                _generatedAgents.Add(newAgent);
            }
        }

        private void DestroyAll()
        {
            foreach (var agent in _generatedAgents)
            {
                Destroy(agent.AgentObject);
            }
            
            _generatedAgents.Clear();
        }
    }
}
