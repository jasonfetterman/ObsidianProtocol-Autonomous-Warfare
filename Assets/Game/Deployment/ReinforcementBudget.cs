using System;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class ReinforcementBudget
    {
        public int MaximumPoints { get; private set; }
        public int UsedPoints { get; private set; }

        public int RemainingPoints =>
            Math.Max(0, MaximumPoints - UsedPoints);

        public bool CanReinforce(int points)
        {
            if (points <= 0)
                return false;

            return points <= RemainingPoints;
        }

        public bool TrySpend(int points)
        {
            if (!CanReinforce(points))
                return false;

            UsedPoints += points;
            return true;
        }

        public bool Refund(int points)
        {
            if (points <= 0 ||
                points > UsedPoints)
            {
                return false;
            }

            UsedPoints -= points;
            return true;
        }

        public void SetMaximum(int points)
        {
            MaximumPoints = Math.Max(0, points);

            if (UsedPoints > MaximumPoints)
                UsedPoints = MaximumPoints;
        }

        public void Reset()
        {
            UsedPoints = 0;
        }
    }
}
