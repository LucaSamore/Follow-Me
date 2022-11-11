using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OpponentController : MonoBehaviour
{
    [SerializeField] private int _maxHP;
    [SerializeField] private int _currentHP;
    [SerializeField] private HealthBarController _healthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        _maxHP = 100;
        _currentHP = _maxHP;
        _healthBar.SetMaxHealth(_maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }
    
    private void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _healthBar.SetHealth(_currentHP);
    }
}
