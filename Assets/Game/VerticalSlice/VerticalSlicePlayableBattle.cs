using System;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public enum VerticalSliceBattleState
    {
        Preparing,
        Deployment,
        Active,
        Victory,
        Defeat,
        Complete
    }

    public sealed class VerticalSlicePlayableBattle
    {
        public bool Initialized { get; private set; }

        public VerticalSliceBattleState State
        {
            get;
            private set;
        }

        public bool WardenReady { get; private set; }

        public bool EnemyReady { get; private set; }

        public bool BattlefieldReady { get; private set; }

        public bool CommandReady { get; private set; }

        public bool BattleComplete =>
            State == VerticalSliceBattleState.Complete;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            State =
                VerticalSliceBattleState.Preparing;

            WardenReady = false;
            EnemyReady = false;
            BattlefieldReady = false;
            CommandReady = false;

            Initialized = true;

            return true;
        }

        public bool SetWardenReady(
            bool ready)
        {
            if (!Initialized)
            {
                return false;
            }

            WardenReady = ready;

            return true;
        }

        public bool SetEnemyReady(
            bool ready)
        {
            if (!Initialized)
            {
                return false;
            }

            EnemyReady = ready;

            return true;
        }

        public bool SetBattlefieldReady(
            bool ready)
        {
            if (!Initialized)
            {
                return false;
            }

            BattlefieldReady = ready;

            return true;
        }

        public bool SetCommandReady(
            bool ready)
        {
            if (!Initialized)
            {
                return false;
            }

            CommandReady = ready;

            return true;
        }

        public bool BeginDeployment()
        {
            if (!Initialized ||
                State != VerticalSliceBattleState.Preparing ||
                !WardenReady ||
                !EnemyReady ||
                !BattlefieldReady ||
                !CommandReady)
            {
                return false;
            }

            State =
                VerticalSliceBattleState.Deployment;

            return true;
        }

        public bool BeginBattle()
        {
            if (!Initialized ||
                State != VerticalSliceBattleState.Deployment)
            {
                return false;
            }

            State =
                VerticalSliceBattleState.Active;

            return true;
        }

        public bool SetVictory()
        {
            if (!Initialized ||
                State != VerticalSliceBattleState.Active)
            {
                return false;
            }

            State =
                VerticalSliceBattleState.Victory;

            return true;
        }

        public bool SetDefeat()
        {
            if (!Initialized ||
                State != VerticalSliceBattleState.Active)
            {
                return false;
            }

            State =
                VerticalSliceBattleState.Defeat;

            return true;
        }

        public bool CompleteBattle()
        {
            if (!Initialized ||
                (State != VerticalSliceBattleState.Victory &&
                 State != VerticalSliceBattleState.Defeat))
            {
                return false;
            }

            State =
                VerticalSliceBattleState.Complete;

            return true;
        }

        public void Reset()
        {
            State =
                VerticalSliceBattleState.Preparing;

            WardenReady = false;
            EnemyReady = false;
            BattlefieldReady = false;
            CommandReady = false;

            Initialized = false;
        }
    }
}
