using System;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public enum AllianceState
    {
        Neutral,
        Allied,
        Hostile
    }

    public sealed class PlayerAllianceSystem
    {
        public bool Initialized { get; private set; }

        public AllianceState State { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            State = AllianceState.Neutral;
            Initialized = true;

            return true;
        }

        public bool SetAllied()
        {
            if (!Initialized)
            {
                return false;
            }

            State = AllianceState.Allied;
            return true;
        }

        public bool SetHostile()
        {
            if (!Initialized)
            {
                return false;
            }

            State = AllianceState.Hostile;
            return true;
        }

        public bool SetNeutral()
        {
            if (!Initialized)
            {
                return false;
            }

            State = AllianceState.Neutral;
            return true;
        }

        public bool AreAllied()
        {
            return Initialized &&
                   State == AllianceState.Allied;
        }

        public bool AreHostile()
        {
            return Initialized &&
                   State == AllianceState.Hostile;
        }

        public void Reset()
        {
            State = AllianceState.Neutral;
            Initialized = false;
        }
    }
}
