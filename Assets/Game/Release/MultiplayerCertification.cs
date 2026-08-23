using System;

namespace ObsidianProtocol.Game.Release
{
    public sealed class MultiplayerCertification
    {
        public bool Initialized { get; private set; }

        public bool Certified { get; private set; }

        public bool ConnectivityVerified { get; private set; }

        public bool SynchronizationVerified { get; private set; }

        public bool PersistenceVerified { get; private set; }

        public bool RecoveryVerified { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            Certified = false;
            ConnectivityVerified = false;
            SynchronizationVerified = false;
            PersistenceVerified = false;
            RecoveryVerified = false;

            Initialized = true;

            return true;
        }

        public bool SubmitResults(
            bool connectivityVerified,
            bool synchronizationVerified,
            bool persistenceVerified,
            bool recoveryVerified)
        {
            if (!Initialized)
            {
                return false;
            }

            ConnectivityVerified =
                connectivityVerified;

            SynchronizationVerified =
                synchronizationVerified;

            PersistenceVerified =
                persistenceVerified;

            RecoveryVerified =
                recoveryVerified;

            Certified =
                ConnectivityVerified &&
                SynchronizationVerified &&
                PersistenceVerified &&
                RecoveryVerified;

            return true;
        }

        public bool IsCertified()
        {
            return Initialized &&
                   Certified;
        }

        public void Reset()
        {
            Initialized = false;
            Certified = false;
            ConnectivityVerified = false;
            SynchronizationVerified = false;
            PersistenceVerified = false;
            RecoveryVerified = false;
        }
    }
}
