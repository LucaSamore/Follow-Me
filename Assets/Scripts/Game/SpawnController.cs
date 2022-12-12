using UnityEngine;

namespace Game
{
    /// <summary>
    /// Represents the controller that handles characters respawning.
    /// </summary>
    public sealed class SpawnController : MonoBehaviour
    {
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        
        [SerializeField] private Transform character;
        [SerializeField] private Transform spawnPosition;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }
        }
        
        private void Respawn()
        {
            ResetColors("Player Zone");
            ResetColors("Opponent Zone");
            
            character.transform.position = spawnPosition.transform.position;
            Physics.SyncTransforms();
        }

        private static void ResetColors(string zone)
        {
            var playerZone = GameObject.Find(zone).transform;

            foreach (Transform tile in playerZone)
            {
                var renderer = tile.GetComponent<Renderer>();
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor(EmissionColor, Color.black);
            }
        }
    }
}
