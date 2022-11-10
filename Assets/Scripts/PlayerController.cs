using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly float SPEED = 5f;

    [SerializeField] 
    private CharacterController _characterController;

    private float _gravity;

    // Start is called before the first frame update
    void Start()
    {
        _gravity = -9.81f * Time.deltaTime;
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
        _characterController.Move(new Vector3(x, _gravity, z));
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.name == "Cube")
        {
            hit.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
