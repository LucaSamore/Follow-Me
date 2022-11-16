using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class TileController : MonoBehaviour
{
    [SerializeField] private Transform centerZone;
    [SerializeField] private Transform zone;
    
    public IEnumerable<Vector2> PositionsFromCenter { get; private set; }

    private void Start()
    {
        PositionsFromCenter = ComputePositions();
        PositionsFromCenter.ToList().ForEach(p => Debug.Log($"Position: {p}"));
    }

    private IEnumerable<Vector2> ComputePositions()
    {
        var position = centerZone.transform.position;

        return zone
            .Cast<Transform>()
            .Select(t => t.position)
            .Select(p => new Vector2(p.x - position.x, p.z - position.z));
    }
}
