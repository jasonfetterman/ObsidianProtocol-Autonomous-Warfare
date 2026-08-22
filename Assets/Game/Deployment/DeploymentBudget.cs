namespace ObsidianProtocol.Game.Deployment
{
    public sealed class DeploymentBudget
    {
        public int MaximumPoints { get; }
        public int UsedPoints { get; private set; }
        public int RemainingPoints => MaximumPoints - UsedPoints;

        public DeploymentBudget(int maximumPoints)
        {
            MaximumPoints = maximumPoints < 0 ? 0 : maximumPoints;
        }

        public bool CanSpend(int points)
        {
            return points > 0 && UsedPoints + points <= MaximumPoints;
        }

        public bool TrySpend(int points)
        {
            if (!CanSpend(points))
            {
                return false;
            }

            UsedPoints += points;
            return true;
        }

        public bool Refund(int points)
        {
            if (points <= 0 || points > UsedPoints)
            {
                return false;
            }

            UsedPoints -= points;
            return true;
        }

        public void Reset()
        {
            UsedPoints = 0;
        }
    }
}
