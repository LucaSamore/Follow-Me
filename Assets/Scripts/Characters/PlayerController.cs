using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    private static readonly float Speed = 7.5f;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    [SerializeField] private CharacterController characterController;
    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;
    [SerializeField] private HealthBarController healthBar;
    [CanBeNull] private Renderer _previousTile;
    private float _gravity;
    private float _rotationSpeed;
    
    private void Start()
    {
        _gravity = -9.81f * Time.deltaTime;
        _rotationSpeed = 720f;
        healthBar.SetMaxHealth(maxHp);
    }
    
    private void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(5);
        }
    }

    private void MovePlayer()
    {
        var x = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        var z = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        var movement = new Vector3(x, _gravity, z);
        characterController.Move(movement);
        movement.y = 0f;

        if (movement == Vector3.zero) return;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            Quaternion.LookRotation(movement, Vector3.up), 
            _rotationSpeed * Time.deltaTime);
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

    private void TakeDamage(int damage)
    {
        currentHp -= damage;
        healthBar.SetHealth(currentHp);
    }
}
