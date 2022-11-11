using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    private static readonly float Speed = 7.5f;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private int _maxHP;
    [SerializeField] private int _currentHP;
    [SerializeField] private HealthBarController _healthBar;
    
    private float _gravity;
    private float _rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _gravity = -9.81f * Time.deltaTime;
        _rotationSpeed = 720f;
        _maxHP = 100;
        _currentHP = _maxHP;
        _healthBar.SetMaxHealth(_maxHP);
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }
    
    private void MovePlayer()
    {
        var x = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        var z = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        var movement = new Vector3(x, _gravity, z);
        
        _characterController.Move(movement);
        
        if (movement == Vector3.zero) return;
        
        movement.y = 0f;
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            Quaternion.LookRotation(movement, Vector3.up),
            _rotationSpeed * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.name == "Cube")
        {
            var renderer = hit.gameObject.GetComponent<Renderer>();
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor(EmissionColor, Color.green);
        }
    }

    private void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _healthBar.SetHealth(_currentHP);
    }
}
