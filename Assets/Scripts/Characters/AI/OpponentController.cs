using System;
using System.Collections.Generic;
using System.Linq;
using Characters.HealthBar;
using UnityEngine;

namespace Characters.AI
{
    public sealed class OpponentController : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private int maxHp;
        [SerializeField] private int currentHp;
        [SerializeField] private HealthBarController healthBar;
        
        private float _gravity;
        
        public PathBuilder PathBuilder { get; set; }
        public IList<Tuple<Vector3,Vector2Int>> Path { get; set; }

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

        public void Test()
        {
            Path = PathBuilder.BuildPath(new Vector2Int(0, 0), 5);

            var step = 1;
            
            foreach (var p in Path)
            {
                Debug.Log($"Step: {step}, Position: ({p.Item1},{p.Item2})");
                step++;
            }
        }
    }
}
