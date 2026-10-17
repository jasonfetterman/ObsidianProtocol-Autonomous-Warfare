using UnityEngine;

namespace ObsidianProtocol.Game.PersistentMilitary
{
    public sealed class MilitaryUnitIdentity : MonoBehaviour
    {
        public string ServiceRecordId { get; private set; }
        public string UnitId { get; private set; }

        public void Initialize(string serviceRecordId, string unitId)
        {
            ServiceRecordId = serviceRecordId ?? string.Empty;
            UnitId = unitId ?? string.Empty;
        }
    }
}

