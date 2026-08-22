namespace ObsidianProtocol.Game.Resources
{
    public sealed class ResourceExtractionSystem
    {
        private readonly ResourceNodeSystem nodeSystem;

        public ResourceExtractionSystem(
            ResourceNodeSystem nodeSystem)
        {
            this.nodeSystem = nodeSystem;
        }

        public bool TryExtract(
            string nodeId,
            ResourceInventory destination,
            int amount)
        {
            if (destination == null ||
                nodeSystem == null ||
                amount <= 0)
            {
                return false;
            }

            if (!nodeSystem.TryGetNode(
                    nodeId,
                    out ResourceNode node))
            {
                return false;
            }

            if (!node.Available ||
                amount > node.RemainingAmount)
            {
                return false;
            }

            int extractedAmount = amount;

            if (!node.TryExtract(
                    extractedAmount))
            {
                return false;
            }

            destination.Add(
                node.ResourceId,
                extractedAmount);

            return true;
        }
    }
}
