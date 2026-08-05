using System.Collections.Generic;
using UnityEngine;

public class AIBatchProcessor : MonoBehaviour
{
    [SerializeField] private float tickRate = 0.2f;
    public float TickRate
    {
        get => tickRate;
        set => tickRate = value;
    }

    private float nextTick;
    private readonly List<CombatAI> aiUnits = new();

    private void Start()
    {
        aiUnits.AddRange(FindObjectsByType<CombatAI>());
    }

    private void Update()
    {
        if (Time.time < nextTick) return;
        nextTick = Time.time + tickRate;

        foreach (var ai in aiUnits)
        {
            if (ai != null)
                ai.ManualTick();
        }
    }
}
