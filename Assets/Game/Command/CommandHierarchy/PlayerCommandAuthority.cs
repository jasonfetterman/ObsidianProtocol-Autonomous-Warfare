using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command.Authority
{
    public sealed class PlayerCommandAuthority
    {
        private readonly HashSet<int> authorizedEntities =
            new HashSet<int>();

        public void GrantAuthority(int entityId)
        {
            authorizedEntities.Add(entityId);
        }

        public void RevokeAuthority(int entityId)
        {
            authorizedEntities.Remove(entityId);
        }

        public bool HasAuthority(int entityId)
        {
            return authorizedEntities.Contains(entityId);
        }

        public bool TryAuthorizeCommand(int entityId)
        {
            return HasAuthority(entityId);
        }

        public void Clear()
        {
            authorizedEntities.Clear();
        }
    }
}
