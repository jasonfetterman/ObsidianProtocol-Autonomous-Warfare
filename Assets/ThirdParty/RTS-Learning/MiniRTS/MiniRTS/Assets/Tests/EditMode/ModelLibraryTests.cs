using System.Collections.Generic;
using NUnit.Framework;

namespace MiniRTS.Tests.EditMode
{
    public sealed class ModelLibraryTests
    {
        [TestCase(UnitType.Worker, "Models/SpaceKit/astronautB")]
        [TestCase(UnitType.Marine, "Models/SpaceKit/astronautA")]
        [TestCase(UnitType.Tank, "Models/SpaceKit/craft_speederA")]
        public void UnitModels_MapToExpectedResourcePaths(
            UnitType type,
            string expectedPath)
        {
            Assert.That(
                ModelLibrary.Get(type).ResourcePath,
                Is.EqualTo(expectedPath));
        }

        [TestCase(
            BuildingType.Headquarters,
            "Models/SpaceKit/hangar_largeA")]
        [TestCase(
            BuildingType.SupplyDepot,
            "Models/SpaceKit/machine_generatorLarge")]
        [TestCase(
            BuildingType.Barracks,
            "Models/SpaceKit/hangar_roundA")]
        [TestCase(
            BuildingType.Factory,
            "Models/SpaceKit/hangar_largeB")]
        [TestCase(
            BuildingType.Refinery,
            "Models/SpaceKit/machine_barrelLarge")]
        public void BuildingModels_MapToExpectedResourcePaths(
            BuildingType type,
            string expectedPath)
        {
            Assert.That(
                ModelLibrary.Get(type).ResourcePath,
                Is.EqualTo(expectedPath));
        }

        [Test]
        public void AllDefinitions_HaveValidUniquePathsAndCorrections()
        {
            HashSet<string> paths = new HashSet<string>();
            foreach (ModelDefinition definition in
                     ModelLibrary.AllDefinitions)
            {
                Assert.That(
                    ModelLibrary.IsValidResourcePath(
                        definition.ResourcePath),
                    Is.True,
                    definition.ResourcePath);
                Assert.That(
                    paths.Add(definition.ResourcePath),
                    Is.True,
                    definition.ResourcePath);
                Assert.That(definition.Scale.x, Is.GreaterThan(0f));
                Assert.That(definition.Scale.y, Is.GreaterThan(0f));
                Assert.That(definition.Scale.z, Is.GreaterThan(0f));
                Assert.That(
                    definition.SelectionRingScale,
                    Is.GreaterThan(0f));
            }
        }
    }
}
