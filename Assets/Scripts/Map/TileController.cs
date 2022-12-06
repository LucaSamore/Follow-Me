using JetBrains.Annotations;
using UnityEngine;

namespace Map
{
    public sealed class TileController : MonoBehaviour
    {
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        
        [CanBeNull] private Renderer _previousTile;
        private void OnTriggerEnter(Collider other)
        {
            if (other.name != "Cube") return;
            if (_previousTile is not null) _previousTile.material.SetColor(EmissionColor, Color.black);
            var renderer = other.gameObject.GetComponent<Renderer>();
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor(EmissionColor, Color.green);
            _previousTile = renderer;
        }
    }
}
