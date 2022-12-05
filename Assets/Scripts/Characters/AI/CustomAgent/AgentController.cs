using Characters.AI.Algorithms._2D;
using Characters.AI.Algorithms._3D;
using Map;
using Map.CustomNavMesh;
using UnityEngine;

namespace Characters.AI.CustomAgent
{
    public sealed class AgentController : MonoBehaviour
    {
        [SerializeField] private Transform navigable;
        [SerializeField] private Transform startingPosition;
        [SerializeField] private bool isMap2D;
        [SerializeField] private GameObject agentSource;
        [SerializeField] private GameObject aiSource;

        private INavMeshFactory _navMeshFactory;
        private IAgent _agent;
        private GameObject _spawned;
        private AgentMovement _agentMovement;
        
        public void Walk() => _agent.Walk();

        public void Jump() => _agent.Jump();
        
        private void Awake()
        {
            _navMeshFactory = new NavMeshFactory();
            _agentMovement = agentSource.GetComponent<AgentMovement>();
        }
        
        private void Start()
        {
            Spawn();
            
            _agent = isMap2D ? new Agent<Vector2Int>(
                    _spawned.transform, 
                    _agentMovement,
                    _navMeshFactory.BakeMesh2D(navigable, startingPosition.position),
                    new NeighbourAlgorithm2D(),
                    new Vector2Int(0,0)) : 
                new Agent<Vector3Int>(
                    _spawned.transform, 
                    _agentMovement, 
                    _navMeshFactory.BakeMesh3D(navigable, startingPosition.position), 
                    new TweakedDijkstra3D(), 
                    new Vector3Int(0,0,0));
        }
        
        private void Spawn()
        {
            var newAgent = Instantiate(aiSource);
            Destroy(newAgent.GetComponent<AIController>());
            newAgent.AddComponent<TileController>();
            _spawned = newAgent;
        }
    }
}
