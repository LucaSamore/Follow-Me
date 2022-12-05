using System.Collections.Generic;
using Characters.AI.CustomAgent;
using Characters.HealthBar;
using UnityEngine;
using JetBrains.Annotations;

namespace Characters.AI
{
    public sealed class AIController : MonoBehaviour
    {
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        
        [SerializeField] private int maxHp;
        [SerializeField] private int currentHp;
        [SerializeField] private HealthBarController healthBar;
        [SerializeField] private GameObject agent;
        [SerializeField] private int howMany;
        
        [CanBeNull] private Renderer _previousTile;
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
                // Si clona l’oggetto Agent
                _agents.Add(Instantiate(agent));
            }
        }
    }
}
