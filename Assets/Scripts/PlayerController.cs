using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly float SPEED = 7.5f;

    [SerializeField] 
    private CharacterController _characterController;

    private float _gravity;
    private float _rotationSpeed;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    // Start is called before the first frame update
    void Start()
    {
        _gravity = -9.81f * Time.deltaTime;
        _rotationSpeed = 720f;
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }
    
    private void MovePlayer()
    {
        var x = Input.GetAxis("Horizontal") * SPEED * Time.deltaTime;
        var z = Input.GetAxis("Vertical") * SPEED * Time.deltaTime;
        var movement = new Vector3(x, _gravity, z);
        _characterController.Move(movement);
        // TODO: Fix rotation
        if (movement == Vector3.zero) return;
        var toRotation = Quaternion.LookRotation(movement, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
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
}
