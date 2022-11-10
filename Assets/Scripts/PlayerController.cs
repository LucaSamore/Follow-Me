using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly float SPEED = 5f;

    [SerializeField] private CharacterController _characterController;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal") * SPEED * Time.deltaTime;
        var z = Input.GetAxis("Vertical") * SPEED * Time.deltaTime;
        _characterController.Move(new Vector3(x, 0, z));
    }
}
