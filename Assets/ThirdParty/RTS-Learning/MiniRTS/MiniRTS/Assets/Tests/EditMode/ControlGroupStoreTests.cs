using System.Collections.Generic;
using NUnit.Framework;

namespace MiniRTS.Tests.EditMode
{
    public sealed class ControlGroupStoreTests
    {
        [Test]
        public void Assign_CopiesCurrentSelection()
        {
            ControlGroupStore<string> groups = new ControlGroupStore<string>(5);
            List<string> selection = new List<string> { "Marine 1", "Marine 2" };

            groups.Assign(1, selection);
            selection.Clear();

            Assert.That(
                groups.Recall(1),
                Is.EqualTo(new[] { "Marine 1", "Marine 2" }));
        }

        [Test]
        public void Assign_ReplacesPreviousGroupContents()
        {
            ControlGroupStore<string> groups = new ControlGroupStore<string>(5);
            groups.Assign(3, new[] { "Worker" });

            groups.Assign(3, new[] { "Tank" });

            Assert.That(groups.Recall(3), Is.EqualTo(new[] { "Tank" }));
        }

        [Test]
        public void Clear_OnlyClearsRequestedGroup()
        {
            ControlGroupStore<string> groups = new ControlGroupStore<string>(5);
            groups.Assign(1, new[] { "Marine" });
            groups.Assign(2, new[] { "Tank" });

            groups.Clear(1);

            Assert.That(groups.Recall(1), Is.Empty);
            Assert.That(groups.Recall(2), Is.EqualTo(new[] { "Tank" }));
        }
    }
}
