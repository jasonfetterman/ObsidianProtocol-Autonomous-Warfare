using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Resource path and transform corrections for imported Kenney models.
    /// Scale and vertical offset are local to the unchanged gameplay body.
    /// </summary>
    public readonly struct ModelDefinition
    {
        public string ResourcePath { get; }
        public Vector3 Scale { get; }
        public Vector3 EulerRotation { get; }
        public float VerticalOffset { get; }
        public float SelectionRingScale { get; }

        public ModelDefinition(
            string resourcePath,
            Vector3 scale,
            Vector3 eulerRotation,
            float verticalOffset,
            float selectionRingScale = 1f)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                throw new ArgumentException(
                    "A model resource path is required.",
                    nameof(resourcePath));
            }

            if (scale.x <= 0f || scale.y <= 0f || scale.z <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(scale),
                    "Every model scale component must be positive.");
            }

            if (selectionRingScale <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(selectionRingScale));
            }

            ResourcePath = resourcePath;
            Scale = scale;
            EulerRotation = eulerRotation;
            VerticalOffset = verticalOffset;
            SelectionRingScale = selectionRingScale;
        }
    }

    /// <summary>
    /// Single source of truth for runtime entity-to-model presentation.
    /// Paths are relative to Assets/Resources and omit the file extension.
    /// </summary>
    public static class ModelLibrary
    {
        private const string SpaceKit = "Models/SpaceKit/";
        private const string TowerDefense = "Models/TowerDefense/";

        private static readonly ModelDefinition WorkerModel =
            new ModelDefinition(
                SpaceKit + "astronautB",
                Vector3.one * 0.9f,
                Vector3.zero,
                -0.65f,
                1.05f);

        private static readonly ModelDefinition MarineModel =
            new ModelDefinition(
                SpaceKit + "astronautA",
                Vector3.one * 0.9f,
                Vector3.zero,
                -0.65f,
                1.1f);

        private static readonly ModelDefinition TankModel =
            new ModelDefinition(
                SpaceKit + "craft_speederA",
                new Vector3(0.72f, 1.1f, 0.55f),
                Vector3.zero,
                -0.5f,
                1.15f);

        private static readonly ModelDefinition HeadquartersModel =
            new ModelDefinition(
                SpaceKit + "hangar_largeA",
                new Vector3(0.45f, 1f, 0.3f),
                Vector3.zero,
                -0.5f);

        private static readonly ModelDefinition SupplyDepotModel =
            new ModelDefinition(
                SpaceKit + "machine_generatorLarge",
                new Vector3(0.85f, 1.32f, 0.7f),
                Vector3.zero,
                -0.5f);

        private static readonly ModelDefinition BarracksModel =
            new ModelDefinition(
                SpaceKit + "hangar_roundA",
                new Vector3(0.29f, 0.67f, 0.32f),
                Vector3.zero,
                -0.5f);

        private static readonly ModelDefinition FactoryModel =
            new ModelDefinition(
                SpaceKit + "hangar_largeB",
                new Vector3(0.41f, 0.91f, 0.3f),
                Vector3.zero,
                -0.5f);

        private static readonly ModelDefinition RefineryModel =
            new ModelDefinition(
                SpaceKit + "machine_barrelLarge",
                new Vector3(1.15f, 1.5f, 1.15f),
                Vector3.zero,
                -0.5f);

        private static readonly ModelDefinition[] MineralModels =
        {
            new ModelDefinition(
                TowerDefense + "detail-crystal",
                Vector3.one * 3f,
                Vector3.zero,
                0f),
            new ModelDefinition(
                TowerDefense + "detail-crystal-large",
                Vector3.one * 1.55f,
                new Vector3(0f, 30f, 0f),
                0f),
            new ModelDefinition(
                SpaceKit + "rock_crystals",
                Vector3.one * 2.2f,
                new Vector3(0f, 55f, 0f),
                0f),
            new ModelDefinition(
                SpaceKit + "rock_crystalsLargeA",
                Vector3.one * 1.7f,
                new Vector3(0f, 95f, 0f),
                0f),
            new ModelDefinition(
                SpaceKit + "rock_crystalsLargeB",
                Vector3.one * 1.7f,
                new Vector3(0f, 140f, 0f),
                0f)
        };

        private static readonly ModelDefinition[] GeyserModels =
        {
            new ModelDefinition(
                SpaceKit + "machine_wireless",
                new Vector3(3f, 2.5f, 3.5f),
                Vector3.zero,
                0f),
            new ModelDefinition(
                SpaceKit + "meteor_half",
                new Vector3(2.4f, 3.5f, 2.7f),
                new Vector3(0f, 35f, 0f),
                0f)
        };

        private static readonly ModelDefinition[] RockModels =
        {
            new ModelDefinition(
                SpaceKit + "rock_largeA",
                new Vector3(1f, 1.85f, 1.05f),
                Vector3.zero,
                -0.5f),
            new ModelDefinition(
                SpaceKit + "rock_largeB",
                new Vector3(1f, 1.85f, 1f),
                Vector3.zero,
                -0.5f),
            new ModelDefinition(
                TowerDefense + "detail-rocks-large",
                new Vector3(1f, 2.15f, 1f),
                Vector3.zero,
                -0.5f)
        };

        private static readonly ModelDefinition[] Definitions =
        {
            WorkerModel,
            MarineModel,
            TankModel,
            HeadquartersModel,
            SupplyDepotModel,
            BarracksModel,
            FactoryModel,
            RefineryModel,
            MineralModels[0],
            MineralModels[1],
            MineralModels[2],
            MineralModels[3],
            MineralModels[4],
            GeyserModels[0],
            GeyserModels[1],
            RockModels[0],
            RockModels[1],
            RockModels[2]
        };

        public static IReadOnlyList<ModelDefinition> AllDefinitions =>
            Definitions;

        public static ModelDefinition Get(UnitType type)
        {
            switch (type)
            {
                case UnitType.Worker:
                    return WorkerModel;
                case UnitType.Marine:
                    return MarineModel;
                case UnitType.Tank:
                    return TankModel;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        public static ModelDefinition Get(BuildingType type)
        {
            switch (type)
            {
                case BuildingType.Headquarters:
                    return HeadquartersModel;
                case BuildingType.SupplyDepot:
                    return SupplyDepotModel;
                case BuildingType.Barracks:
                    return BarracksModel;
                case BuildingType.Factory:
                    return FactoryModel;
                case BuildingType.Refinery:
                    return RefineryModel;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        public static ModelDefinition GetMineral(int variant)
        {
            return GetVariant(MineralModels, variant);
        }

        public static ModelDefinition GetGeyser(int variant)
        {
            return GetVariant(GeyserModels, variant);
        }

        public static ModelDefinition GetRock(int variant)
        {
            return GetVariant(RockModels, variant);
        }

        public static bool IsValidResourcePath(string resourcePath)
        {
            return !string.IsNullOrWhiteSpace(resourcePath) &&
                   resourcePath.StartsWith("Models/", StringComparison.Ordinal) &&
                   resourcePath.IndexOf('\\') < 0 &&
                   !resourcePath.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase);
        }

        private static ModelDefinition GetVariant(
            ModelDefinition[] models,
            int variant)
        {
            int index = variant % models.Length;
            if (index < 0)
            {
                index += models.Length;
            }

            return models[index];
        }
    }
}
