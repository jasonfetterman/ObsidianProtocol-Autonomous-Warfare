using UnityEngine;

namespace ObsidianProtocol.Game.Units
{
    public sealed class UnitInstance : Unit
    {
        [SerializeField] private UnitDefinition definition;

        public UnitDefinition Definition => definition;
    }
}
