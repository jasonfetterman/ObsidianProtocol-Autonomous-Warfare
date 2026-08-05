using UnityEngine;

public class VisionProvider : MonoBehaviour
{
    public float visionRange = 15f;
    public LayerMask losBlockers;

    private FogOfWar fog;

    private void Awake()
    {
        fog = Object.FindAnyObjectByType<FogOfWar>();
    }

    private void Update()
    {
        if (fog == null) return;
        fog.RevealArea(transform.position, visionRange, losBlockers);
    }
}
