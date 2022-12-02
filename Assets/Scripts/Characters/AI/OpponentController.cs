using System;
using System.Collections.Generic;
using System.Linq;
using Characters.HealthBar;
using UnityEngine;
using UnityEngine.AI;
using JetBrains.Annotations;

namespace Characters.AI
{
    public sealed class OpponentController : MonoBehaviour
    {
        private static readonly float Speed = 3f;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        
        [SerializeField] private CharacterController characterController;
        [SerializeField] private int maxHp;
        [SerializeField] private int currentHp;
        [SerializeField] private HealthBarController healthBar;
        [CanBeNull] private Renderer _previousTile;
        private float _gravity;
        private NavMeshAgent _agent;
        private Vector3 _desVelocity;
        private Transform _randomDestination;
        
        public PathBuilder<Vector2Int> PathBuilder2D { get; set; }
        public IList<Tuple<Vector3,Vector2Int>> Path { get; set; }

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _gravity = -9.81f * Time.deltaTime;
            healthBar.SetMaxHealth(maxHp);
            SetNewDestination();
        }
    
        private void Update()
        {
            characterController.Move(new Vector3(0f, _gravity, 0f));

            var position = _randomDestination.position;
            _agent.destination = position;
            _desVelocity = _agent.desiredVelocity;
            
            _agent.updatePosition = false;
            _agent.updateRotation = false;
            
            var lookPos = position - transform.position;
            lookPos.y = 0f;
            var targetRot = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * Speed);

            characterController.Move(_desVelocity.normalized * (2f * Time.deltaTime));
            _agent.velocity = characterController.velocity;

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

        public void Test()
        {
            Path = PathBuilder2D.BuildPath(new Vector2Int(0, 0), 3);

            var step = 1;

            foreach (var p in Path)
            {
                Debug.Log($"Step: {step}, Position: ({p.Item1},{p.Item2})");
                step++;
            }
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

        private void SetNewDestination()
        {
            var children = GameObject.Find("Opponent Zone").transform;
            _randomDestination = children.GetChild(new System.Random().Next(0, children.childCount - 1));
        }
    }
}
