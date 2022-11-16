using UnityEngine;

namespace Characters.HealthBar
{
    public sealed class Billboard : MonoBehaviour
    {
        [SerializeField] private Transform _camera;

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camera.forward);
        }
    }
}
