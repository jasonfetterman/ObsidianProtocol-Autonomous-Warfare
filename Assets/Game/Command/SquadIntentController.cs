using UnityEngine;
using ObsidianProtocol.Game.Squads;

namespace ObsidianProtocol.Game.Command
{
    public sealed class SquadIntentController : MonoBehaviour
    {
        [SerializeField] private Squad squad;

        private Intent currentIntent;

        public Squad Squad => squad;
        public Intent CurrentIntent => currentIntent;

        public void SetIntent(Intent intent)
        {
            currentIntent = intent;
        }

        public void ClearIntent()
        {
            currentIntent = null;
        }
    }
}
