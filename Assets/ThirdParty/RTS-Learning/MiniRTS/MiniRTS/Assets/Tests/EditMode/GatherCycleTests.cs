using NUnit.Framework;

namespace MiniRTS.Tests.EditMode
{
    public sealed class GatherCycleTests
    {
        [Test]
        public void CompletedTrip_DepositsCargoAndRepeats()
        {
            GatherCycle cycle = new GatherCycle(2f, 5);

            cycle.Begin(ResourceType.Minerals);
            Assert.That(cycle.Phase, Is.EqualTo(GatherPhase.MovingToResource));
            Assert.That(cycle.ArriveAtResource(), Is.True);
            Assert.That(cycle.AdvanceHarvest(1.5f), Is.False);
            Assert.That(cycle.AdvanceHarvest(0.5f), Is.True);
            Assert.That(cycle.CompleteHarvest(20), Is.EqualTo(5));
            Assert.That(cycle.Phase, Is.EqualTo(GatherPhase.ReturningToDropoff));
            Assert.That(cycle.CarriedAmount, Is.EqualTo(5));

            GatherDelivery delivery = cycle.ArriveAtDropoff();
            Assert.That(delivery.Type, Is.EqualTo(ResourceType.Minerals));
            Assert.That(delivery.Amount, Is.EqualTo(5));
            Assert.That(cycle.CarriedAmount, Is.Zero);
            Assert.That(cycle.Phase, Is.EqualTo(GatherPhase.MovingToResource));
        }

        [Test]
        public void InterruptedTrip_ResumesTowardTheCorrectEndpoint()
        {
            GatherCycle cycle = new GatherCycle(2f, 5);
            cycle.Begin(ResourceType.Gas);
            cycle.ArriveAtResource();
            cycle.AdvanceHarvest(2f);
            cycle.CompleteHarvest(5);

            Assert.That(cycle.Interrupt(), Is.True);
            Assert.That(cycle.Phase, Is.EqualTo(GatherPhase.Interrupted));
            Assert.That(cycle.Resume(), Is.True);
            Assert.That(cycle.Phase, Is.EqualTo(GatherPhase.ReturningToDropoff));

            GatherDelivery delivery = cycle.ArriveAtDropoff();
            Assert.That(delivery.Type, Is.EqualTo(ResourceType.Gas));
            Assert.That(cycle.Interrupt(), Is.True);
            Assert.That(cycle.Resume(), Is.True);
            Assert.That(cycle.Phase, Is.EqualTo(GatherPhase.MovingToResource));
        }
    }
}
