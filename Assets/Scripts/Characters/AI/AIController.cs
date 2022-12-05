using System.Collections.Generic;
using Characters.AI.CustomAgent;
using Characters.HealthBar;
using UnityEngine;

namespace Characters.AI
{
    public sealed class AIController : MonoBehaviour
    {
        [SerializeField] private int maxHp;
        [SerializeField] private int currentHp;
        [SerializeField] private HealthBarController healthBar;
        [SerializeField] private GameObject agent;
        [SerializeField] private int howMany;
        
        private IList<GameObject> _agents;

        private void Awake()
        {
            _agents = new List<GameObject>();
        }

        private void Start()
        {
            healthBar.SetMaxHealth(maxHp);
            CreateMany();
        }
    
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (var a in _agents)
                {
                    a.GetComponent<AgentController>().Walk();
                }
                
                TakeDamage(5);
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
                _agents.Add(Instantiate(agent));
            }
        }
    }
}
