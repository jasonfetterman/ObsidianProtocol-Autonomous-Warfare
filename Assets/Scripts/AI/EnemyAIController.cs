using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    public float thinkInterval = 2f;
    float nextThinkTime;

    public EnemyBuilder builder;
    public EnemyResourceManager resourceManager;
    public EnemyUnitManager unitManager;
    public EnemyAttackManager attackManager;

    void Awake()
    {
        builder = GetComponent<EnemyBuilder>();
        resourceManager = GetComponent<EnemyResourceManager>();
        unitManager = GetComponent<EnemyUnitManager>();
        attackManager = GetComponent<EnemyAttackManager>();
    }

    void Update()
    {
        if (Time.time >= nextThinkTime)
        {
            nextThinkTime = Time.time + thinkInterval;
            Think();
        }
    }

    void Think()
    {
        resourceManager.GatherTick();
        builder.BuildTick(resourceManager);
        unitManager.ProductionTick(resourceManager);
        attackManager.AttackTick(unitManager);
    }
}
