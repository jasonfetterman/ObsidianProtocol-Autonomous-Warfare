using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class OwnedUnit
    {
        [Header("Definition")]
        public UnitDefinition definition;

        [Header("Ownership")]
        public string instanceId;
        public string ownerId;

        [Header("Condition")]
        [Range(0f, 1f)]
        public float condition = 1f;

        [Range(0f, 1f)]
        public float readiness = 1f;

        [Header("Experience")]
        [Min(0)]
        public int experience;

        [Min(1)]
        public int level = 1;

        [Header("Operational State")]
        public bool deployed;
        public bool locked;
        public bool underMaintenance;

        [Header("Usage")]
        [Min(0)]
        public int missionsCompleted;

        [Min(0f)]
        public float operatingHours;

        [Min(0)]
        public int damageEvents;

        public string UnitId
        {
            get
            {
                if (definition == null || definition.identity == null)
                    return string.Empty;

                return definition.identity.unitId;
            }
        }

        public void Initialize(UnitDefinition unitDefinition, string newInstanceId)
        {
            definition = unitDefinition;
            instanceId = newInstanceId;

            condition = 1f;
            readiness = 1f;
            experience = 0;
            level = 1;

            deployed = false;
            locked = false;
            underMaintenance = false;

            missionsCompleted = 0;
            operatingHours = 0f;
            damageEvents = 0;
        }

        public void ApplyDamage(float amount)
        {
            condition = Mathf.Clamp01(condition - Mathf.Max(0f, amount));
        }

        public void RestoreCondition(float amount)
        {
            condition = Mathf.Clamp01(condition + Mathf.Max(0f, amount));
        }

        public void AddExperience(int amount)
        {
            experience += Mathf.Max(0, amount);

            while (experience >= LevelExperienceRequired(level))
                level++;
        }

        public static int LevelExperienceRequired(int currentLevel)
        {
            return Mathf.Max(100, currentLevel * 100);
        }
    }
}
