using System;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class BattleBudget
    {
        public int StartingBudget { get; private set; }
        public int CommittedPoints { get; private set; }

        public int RemainingBudget =>
            Math.Max(0, StartingBudget - CommittedPoints);

        public bool HasBudget =>
            RemainingBudget > 0;

        public BattleBudget(int startingBudget)
        {
            StartingBudget = Math.Max(0, startingBudget);
            CommittedPoints = 0;
        }

        public bool CanCommit(int points)
        {
            if (points <= 0)
                return false;

            return points <= RemainingBudget;
        }

        public bool TryCommit(int points)
        {
            if (!CanCommit(points))
                return false;

            CommittedPoints += points;
            return true;
        }

        public bool Release(int points)
        {
            if (points <= 0)
                return false;

            if (points > CommittedPoints)
                return false;

            CommittedPoints -= points;
            return true;
        }

        public void SetStartingBudget(int budget)
        {
            StartingBudget = Math.Max(0, budget);

            if (CommittedPoints > StartingBudget)
                CommittedPoints = StartingBudget;
        }

        public void Reset()
        {
            CommittedPoints = 0;
        }
    }
}
