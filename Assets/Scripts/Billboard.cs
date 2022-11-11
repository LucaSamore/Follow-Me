using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Billboard : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    void LateUpdate()
    {
        transform.LookAt(transform.position + _camera.forward);
    }
}
