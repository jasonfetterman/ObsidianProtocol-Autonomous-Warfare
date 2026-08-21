using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [CreateAssetMenu(
        fileName = "UnitDefinition",
        menuName = "Obsidian Protocol/Garage/Unit Definition"
    )]
    public class UnitDefinition : ScriptableObject
    {
        [Header("Identity")]
        public UnitIdentity identity;

        [Header("Complete Unit Stats")]
        public UnitStats stats;
    }
}
