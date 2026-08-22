using UnityEngine;

namespace ObsidianProtocol.Game.Core
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
