using UnityEngine;

namespace Characters.HealthBar
{
    /// <summary>
    /// Keeps the health bar following the camera.
    /// </summary>
    public sealed class Billboard : MonoBehaviour
    {
        [SerializeField] private Transform _camera;

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camera.forward);
        }
    }
}
