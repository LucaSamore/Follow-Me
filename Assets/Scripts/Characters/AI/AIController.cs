using System.Collections.Generic;
using Characters.AI.Algorithms._2D;
using Characters.AI.Algorithms._3D;
using Characters.AI.CustomAgent;
using Characters.HealthBar;
using Map.CustomNavMesh;
using UnityEngine;

namespace Characters.AI
{
    /// <summary>
    /// Represents a controller for the AI.
    /// A custom agent is attached to the AI and it handles its behaviour and movement.
    /// The AI can also control multiple custom agents.
    /// </summary>
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
                    new NeighbourAlgorithm2D(),
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
                
                //Tell all generated agents to walk.
                foreach (var a in _generatedAgents)
                {
                    a.Walk();
                }
                
                TakeDamage(5);
            }
            
            if (Input.GetKeyDown(KeyCode.C)) CreateMany();
            if (Input.GetKeyDown(KeyCode.X)) DestroyAll();
        }

        /// <summary>
        /// Update current hp upon taking damage.
        /// </summary>
        /// <param name="damage">Amount of hp lost.</param>
        private void TakeDamage(int damage)
        {
            currentHp -= damage;
            healthBar.SetHealth(currentHp);
        }

        /// <summary>
        /// Creates many custom agents by cloning the agent attached to the AI.
        /// All generated agents are controlled by the AI.
        /// </summary>
        private void CreateMany()
        {
            if(howMany <= 0) return;

            for (var i = 0; i < howMany; i++)
            {
                var newAgent = _aiAgent.Clone();
                _generatedAgents.Add(newAgent);
            }
        }

        /// <summary>
        /// Kills all the agents, except for the one attached to the AI.
        /// </summary>
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
