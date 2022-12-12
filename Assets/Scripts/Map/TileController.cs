using JetBrains.Annotations;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// Represents a controller attached to the characters that manage the collision
    /// with the collider of a single tile.
    /// </summary>
    public sealed class TileController : MonoBehaviour
    {
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        public Color color = Color.green;
        
        [CanBeNull] private Renderer _previousTile;
        
        private void OnTriggerEnter(Collider other)
        {
            // Change tile's color when the character steps on it.
            if (other.name != "Cube") return;
            if (_previousTile is not null) _previousTile.material.SetColor(EmissionColor, Color.black);
            var renderer = other.gameObject.GetComponent<Renderer>();
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor(EmissionColor, color);
            _previousTile = renderer;
        }
    }
}
