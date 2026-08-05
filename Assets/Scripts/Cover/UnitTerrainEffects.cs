using UnityEngine;

[RequireComponent(typeof(UnitMover))]
[RequireComponent(typeof(Health))]
public class UnitTerrainEffects : MonoBehaviour
{
    public float dangerDamagePerSecond = 5f;

    private UnitMover mover;
    private Health health;
    private TerrainMetadata terrain;

    private float baseSpeed;

    private void Awake()
    {
        mover = GetComponent<UnitMover>();
        health = GetComponent<Health>();
        terrain = Object.FindAnyObjectByType<TerrainMetadata>();

        if (mover != null && mover.agent != null)
            baseSpeed = mover.agent.speed;
    }

    private void Update()
    {
        if (terrain == null || mover == null || mover.agent == null)
            return;

        Vector3 pos = transform.position;

        // Slow terrain
        if (terrain.IsSlow(pos))
            mover.agent.speed = baseSpeed * terrain.slowMultiplier;
        else
            mover.agent.speed = baseSpeed;

        // Danger terrain
        if (terrain.IsDanger(pos))
            health.TakeDamage(dangerDamagePerSecond * Time.deltaTime, DamageClass.Fire, null);
    }
}
