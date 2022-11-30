using System;
using System.Collections.Generic;
using System.Linq;
using Characters.HealthBar;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.AI
{
    public sealed class OpponentController : MonoBehaviour
    {
        private static readonly float Speed = 3f;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private int maxHp;
        [SerializeField] private int currentHp;
        [SerializeField] private HealthBarController healthBar;
        [SerializeField] private Transform dest;
        
        private float _gravity;
        private NavMeshAgent _agent;
        private Vector3 _desVelocity;
        
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
        }
    
        private void Update()
        {
            characterController.Move(new Vector3(0f, _gravity, 0f));

            _agent.destination = dest.position;
            _desVelocity = _agent.desiredVelocity;
            
            _agent.updatePosition = false;
            _agent.updateRotation = false;
            
            var lookPos = dest.position - transform.position;
            lookPos.y = 0f;
            var targetRot = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * Speed);

            characterController.Move(_desVelocity.normalized * (2 * Time.deltaTime));
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

        public void Walk()
        {
            // foreach (var position in Path)
            // {
            //     var x = position.Item1.x * Speed * Time.deltaTime;
            //     var z = position.Item1.z * Speed * Time.deltaTime;
            //     characterController.Move(new Vector3(x, _gravity, z));
            // }
        }
    }
}
