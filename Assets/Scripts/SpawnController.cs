using UnityEngine;

public sealed class SpawnController : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private Transform spawnPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        character.transform.position = spawnPosition.transform.position;
        Physics.SyncTransforms();
    }
}
