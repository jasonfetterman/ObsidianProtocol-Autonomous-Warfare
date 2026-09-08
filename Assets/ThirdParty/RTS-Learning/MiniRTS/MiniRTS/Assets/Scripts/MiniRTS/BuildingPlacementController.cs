using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MiniRTS
{
    /// <summary>
    /// Owns worker building mode, snapped preview presentation, and site placement.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class BuildingPlacementController : MonoBehaviour
    {
        private Camera worldCamera;
        private WalkGrid grid;
        private PlayerEconomy economy;
        private SelectionController selection;
        private Func<BuildingType, Vector3, ResourceNode, Building> buildingFactory;
        private BuildingType placementType;
        private GameObject ghost;
        private ModelVisual ghostVisual;
        private ResourceNode previewGeyser;
        private Vector3 previewPosition;
        private bool previewValid;
        private bool awaitingLeftRelease;
        private int consumedInputFrame = -1;

        public bool IsPlacing { get; private set; }
        public bool CapturesPointer =>
            IsPlacing ||
            awaitingLeftRelease ||
            consumedInputFrame == Time.frameCount;
        public BuildingType PlacementType => placementType;
        public bool IsPreviewValid => previewValid;
        public Vector3 PreviewPosition => previewPosition;

        public void Initialize(
            Camera targetCamera,
            WalkGrid walkGrid,
            PlayerEconomy factionEconomy,
            Func<BuildingType, Vector3, ResourceNode, Building> createBuilding)
        {
            worldCamera = targetCamera ??
                throw new ArgumentNullException(nameof(targetCamera));
            grid = walkGrid ??
                throw new ArgumentNullException(nameof(walkGrid));
            economy = factionEconomy ??
                throw new ArgumentNullException(nameof(factionEconomy));
            buildingFactory = createBuilding ??
                throw new ArgumentNullException(nameof(createBuilding));
        }

        public void SetSelectionController(SelectionController controller)
        {
            selection = controller ??
                throw new ArgumentNullException(nameof(controller));
        }

        public bool TryBeginPlacement(BuildingType type)
        {
            if (selection == null || !selection.HasSelectedWorker)
            {
                return false;
            }

            BuildingDefinition definition = BuildingDefinition.Get(type);
            if (type == BuildingType.Headquarters ||
                !economy.CanAfford(definition.MineralCost, definition.GasCost))
            {
                return false;
            }

            CancelPlacement();
            placementType = type;
            IsPlacing = true;
            CreateGhost(definition);
            return true;
        }

        public void CancelPlacement()
        {
            IsPlacing = false;
            previewValid = false;
            previewGeyser = null;
            if (ghost != null)
            {
                Destroy(ghost);
                ghost = null;
                ghostVisual = null;
            }
        }

        private void Update()
        {
            Mouse mouse = Mouse.current;
            if (awaitingLeftRelease && (mouse == null || !mouse.leftButton.isPressed))
            {
                awaitingLeftRelease = false;
                consumedInputFrame = Time.frameCount;
            }

            if (!IsPlacing || mouse == null)
            {
                return;
            }

            if (mouse.rightButton.wasPressedThisFrame)
            {
                consumedInputFrame = Time.frameCount;
                if (!IsPointerOverUi())
                {
                    CancelPlacement();
                }

                return;
            }

            UpdatePreview(mouse.position.ReadValue());
            if (mouse.leftButton.wasPressedThisFrame)
            {
                consumedInputFrame = Time.frameCount;
                if (!IsPointerOverUi() && previewValid)
                {
                    PlaceConstructionSite();
                }
            }
        }

        private void OnDisable()
        {
            CancelPlacement();
            awaitingLeftRelease = false;
        }

        private void UpdatePreview(Vector2 pointer)
        {
            Ray ray = worldCamera.ScreenPointToRay(pointer);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (!groundPlane.Raycast(ray, out float distance))
            {
                previewValid = false;
                if (ghost != null)
                {
                    ghost.SetActive(false);
                }

                return;
            }

            BuildingDefinition definition = BuildingDefinition.Get(placementType);
            Vector3 desiredPosition = ray.GetPoint(distance);
            desiredPosition.y = 0f;
            previewGeyser = definition.RequiresGeyser
                ? FindNearestGeyser(desiredPosition)
                : null;
            if (previewGeyser != null)
            {
                desiredPosition = previewGeyser.transform.position;
            }

            previewPosition = BuildingFootprint.SnapToGrid(
                grid,
                desiredPosition,
                definition.Footprint);
            previewPosition.y = 0f;
            previewValid = definition.RequiresGeyser
                ? BuildingPlacementValidator.IsRefineryPlacementValid(
                    grid,
                    previewPosition,
                    definition.Footprint,
                    previewGeyser != null,
                    previewGeyser != null
                        ? previewGeyser.transform.position
                        : Vector3.zero,
                    Refinery.IsGeyserClaimed(previewGeyser))
                : BuildingPlacementValidator.IsFootprintAvailable(
                    grid,
                    previewPosition,
                    definition.Footprint);
            previewValid = previewValid &&
                selection != null &&
                selection.HasSelectedWorker &&
                economy.CanAfford(
                    definition.MineralCost,
                    definition.GasCost);

            ghost.SetActive(true);
            ghost.transform.position = new Vector3(
                previewPosition.x,
                definition.Height * 0.5f,
                previewPosition.z);
            if (ghostVisual != null)
            {
                ghostVisual.ApplyTransparentTint(previewValid
                    ? new Color(0.18f, 1f, 0.28f, 0.55f)
                    : new Color(1f, 0.12f, 0.1f, 0.55f));
            }
        }

        private void PlaceConstructionSite()
        {
            WorkerBuilder builder = FindClosestSelectedBuilder(previewPosition);
            if (builder == null)
            {
                return;
            }

            BuildingDefinition definition = BuildingDefinition.Get(placementType);
            if (!economy.TrySpend(definition.MineralCost, definition.GasCost))
            {
                previewValid = false;
                return;
            }

            Building site = buildingFactory(
                placementType,
                previewPosition,
                previewGeyser);
            if (site == null || !builder.BeginConstruction(site))
            {
                economy.Refund(definition.MineralCost, definition.GasCost);
                if (site != null)
                {
                    Destroy(site.gameObject);
                }

                return;
            }

            awaitingLeftRelease = true;
            CancelPlacement();
        }

        private WorkerBuilder FindClosestSelectedBuilder(Vector3 position)
        {
            WorkerBuilder closest = null;
            float closestDistanceSquared = float.MaxValue;
            System.Collections.Generic.IReadOnlyList<Unit> units =
                selection.SelectedUnits;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit == null || unit.Type != UnitType.Worker)
                {
                    continue;
                }

                WorkerBuilder candidate = unit.GetComponent<WorkerBuilder>();
                if (candidate == null)
                {
                    continue;
                }

                Vector3 difference = unit.transform.position - position;
                difference.y = 0f;
                float distanceSquared = difference.sqrMagnitude;
                if (distanceSquared < closestDistanceSquared)
                {
                    closest = candidate;
                    closestDistanceSquared = distanceSquared;
                }
            }

            return closest;
        }

        private static ResourceNode FindNearestGeyser(Vector3 position)
        {
            ResourceNode closest = null;
            float closestDistanceSquared =
                BalanceConfig.RefinerySnapRadius * BalanceConfig.RefinerySnapRadius;
            System.Collections.Generic.IReadOnlyList<ResourceNode> nodes =
                ResourceNode.ActiveNodes;
            for (int i = 0; i < nodes.Count; i++)
            {
                ResourceNode node = nodes[i];
                if (node == null ||
                    node.Type != ResourceType.Gas ||
                    !node.RequiresRefinery)
                {
                    continue;
                }

                Vector3 difference = node.transform.position - position;
                difference.y = 0f;
                float distanceSquared = difference.sqrMagnitude;
                if (distanceSquared <= closestDistanceSquared)
                {
                    closest = node;
                    closestDistanceSquared = distanceSquared;
                }
            }

            return closest;
        }

        private void CreateGhost(BuildingDefinition definition)
        {
            ghost = new GameObject();
            ghost.name = $"{definition.DisplayName}_PlacementGhost";
            ghost.transform.localScale = new Vector3(
                definition.Footprint.x,
                definition.Height,
                definition.Footprint.y);
            ghostVisual = ModelVisualFactory.Create(
                ModelLibrary.Get(placementType),
                ghost.transform);
            if (ghostVisual != null)
            {
                ghostVisual.ApplyTransparentTint(
                    new Color(0.18f, 1f, 0.28f, 0.55f));
            }
        }

        private static bool IsPointerOverUi()
        {
            return EventSystem.current != null &&
                   EventSystem.current.IsPointerOverGameObject();
        }
    }
}
