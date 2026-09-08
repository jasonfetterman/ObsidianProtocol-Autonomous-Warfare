using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MiniRTS
{
    /// <summary>
    /// StarCraft-style selection and right-click move/gather context commands.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SelectionController : MonoBehaviour
    {
        private readonly List<Unit> selectedUnits = new List<Unit>();
        private readonly ControlGroupStore<Unit> unitControlGroups =
            new ControlGroupStore<Unit>(BalanceConfig.ControlGroupCount);
        private readonly ControlGroupStore<Building> buildingControlGroups =
            new ControlGroupStore<Building>(BalanceConfig.ControlGroupCount);

        private Camera worldCamera;
        private WalkGrid grid;
        private BuildingPlacementController placementController;
        private Building selectedBuilding;
        private Vector2 dragStart;
        private Vector2 dragCurrent;
        private bool dragging;
        private bool awaitingAttackMoveTarget;

        public IReadOnlyList<Unit> SelectedUnits => selectedUnits;
        public Building SelectedBuilding => selectedBuilding;
        public bool HasSelectedUnits
        {
            get
            {
                RemoveMissingUnits();
                return selectedUnits.Count > 0;
            }
        }
        public bool IsAwaitingAttackMoveTarget => awaitingAttackMoveTarget;
        public bool HasSelectedWorker
        {
            get
            {
                RemoveMissingUnits();
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    if (selectedUnits[i].IsAlive &&
                        selectedUnits[i].Type == UnitType.Worker)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void Initialize(
            Camera targetCamera,
            WalkGrid walkGrid,
            BuildingPlacementController buildingPlacement = null)
        {
            worldCamera = targetCamera;
            grid = walkGrid;
            placementController = buildingPlacement;
        }

        private void Update()
        {
            if (worldCamera == null || grid == null)
            {
                return;
            }

            HandleHotkeys();

            Mouse mouse = Mouse.current;
            if (mouse == null)
            {
                return;
            }

            if (placementController != null && placementController.CapturesPointer)
            {
                dragging = false;
                return;
            }

            dragCurrent = mouse.position.ReadValue();
            if (awaitingAttackMoveTarget)
            {
                if (mouse.rightButton.wasPressedThisFrame)
                {
                    awaitingAttackMoveTarget = false;
                    dragging = false;
                    return;
                }

                if (mouse.leftButton.wasPressedThisFrame)
                {
                    if (!IsPointerOverUi())
                    {
                        IssueAttackMoveCommand(dragCurrent);
                        awaitingAttackMoveTarget = false;
                    }

                    dragging = false;
                    return;
                }
            }

            if (mouse.leftButton.wasPressedThisFrame)
            {
                if (!IsPointerOverUi())
                {
                    dragStart = dragCurrent;
                    dragging = true;
                }
            }

            if (dragging && mouse.leftButton.wasReleasedThisFrame)
            {
                if (IsPointerOverUi())
                {
                    dragging = false;
                    return;
                }

                bool additive = IsAdditiveSelectionPressed();
                if ((dragCurrent - dragStart).magnitude >= BalanceConfig.SelectionDragThreshold)
                {
                    SelectInBox(GetScreenRect(dragStart, dragCurrent), additive);
                }
                else
                {
                    SelectAtPointer(dragCurrent, additive);
                }

                dragging = false;
            }

            if (mouse.rightButton.wasPressedThisFrame)
            {
                if (!IsPointerOverUi())
                {
                    awaitingAttackMoveTarget = false;
                    IssueContextCommand(dragCurrent);
                }
            }
        }

        private void OnDisable()
        {
            ClearSelection();
            dragging = false;
            awaitingAttackMoveTarget = false;
        }

        private void OnGUI()
        {
            if (!dragging ||
                (dragCurrent - dragStart).magnitude < BalanceConfig.SelectionDragThreshold)
            {
                return;
            }

            Rect screenRect = GetScreenRect(dragStart, dragCurrent);
            Rect guiRect = new Rect(
                screenRect.xMin,
                Screen.height - screenRect.yMax,
                screenRect.width,
                screenRect.height);

            Color previousColor = GUI.color;
            GUI.color = new Color(0.18f, 0.7f, 1f, 0.18f);
            GUI.DrawTexture(guiRect, Texture2D.whiteTexture);
            GUI.color = new Color(0.2f, 0.85f, 1f, 0.9f);
            DrawBorder(guiRect, 2f);
            GUI.color = previousColor;
        }

        private void SelectAtPointer(Vector2 pointer, bool additive)
        {
            Ray ray = worldCamera.ScreenPointToRay(pointer);
            RaycastHit[] hits = Physics.RaycastAll(ray, 500f);
            Unit hitUnit = null;
            Building hitBuilding = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < hits.Length; i++)
            {
                Unit unit = hits[i].collider.GetComponentInParent<Unit>();
                if (unit != null && unit.IsAlive && unit.IsPlayerControlled &&
                    hits[i].distance < closestDistance)
                {
                    hitUnit = unit;
                    hitBuilding = null;
                    closestDistance = hits[i].distance;
                }

                Building building = hits[i].collider.GetComponentInParent<Building>();
                if (building != null && building.IsAlive &&
                    building.IsPlayerControlled &&
                    hits[i].distance < closestDistance)
                {
                    hitUnit = null;
                    hitBuilding = building;
                    closestDistance = hits[i].distance;
                }
            }

            if (!additive || hitBuilding != null)
            {
                ClearSelection();
            }

            if (hitUnit != null)
            {
                ClearSelectedBuilding();
                if (additive && hitUnit.IsSelected)
                {
                    RemoveFromSelection(hitUnit);
                }
                else
                {
                    AddToSelection(hitUnit);
                }
            }
            else if (hitBuilding != null)
            {
                selectedBuilding = hitBuilding;
                selectedBuilding.SetSelected(true);
            }
        }

        private void SelectInBox(Rect selectionRect, bool additive)
        {
            if (!additive)
            {
                ClearSelection();
            }
            else
            {
                ClearSelectedBuilding();
            }

            IReadOnlyList<Unit> units = Unit.ActiveUnits;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit == null || !unit.IsAlive || !unit.IsPlayerControlled)
                {
                    continue;
                }

                Vector3 screenPoint = worldCamera.WorldToScreenPoint(unit.transform.position);
                if (screenPoint.z > 0f &&
                    selectionRect.Contains(new Vector2(screenPoint.x, screenPoint.y)))
                {
                    AddToSelection(unit);
                }
            }
        }

        private void IssueContextCommand(Vector2 pointer)
        {
            RemoveMissingUnits();
            Ray ray = worldCamera.ScreenPointToRay(pointer);
            ProductionQueue selectedProduction =
                selectedBuilding != null
                    ? selectedBuilding.GetComponent<ProductionQueue>()
                    : null;
            if (selectedUnits.Count == 0 &&
                selectedProduction != null &&
                selectedBuilding.IsConstructed)
            {
                Plane rallyPlane = new Plane(Vector3.up, Vector3.zero);
                if (rallyPlane.Raycast(ray, out float rallyDistance))
                {
                    selectedProduction.SetRallyPoint(ray.GetPoint(rallyDistance));
                }

                return;
            }

            if (selectedUnits.Count == 0)
            {
                return;
            }

            ICombatTarget enemyTarget = FindClosestEnemyTarget(
                ray,
                BalanceConfig.PlayerOwnerId);
            if (enemyTarget != null)
            {
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    Unit selectedUnit = selectedUnits[i];
                    PrepareForCombatCommand(selectedUnit);
                    Combatant combatant = selectedUnit.Combatant;
                    if (combatant != null)
                    {
                        combatant.IssueAttackTarget(enemyTarget);
                    }
                }

                return;
            }

            ResourceNode resourceNode = FindClosestResource(ray);
            if (resourceNode != null)
            {
                bool issuedGatherCommand = false;
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    WorkerGatherer gatherer =
                        selectedUnits[i].GetComponent<WorkerGatherer>();
                    if (gatherer != null && gatherer.BeginGather(resourceNode))
                    {
                        issuedGatherCommand = true;
                    }
                }

                if (issuedGatherCommand)
                {
                    return;
                }
            }

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (!groundPlane.Raycast(ray, out float distance))
            {
                return;
            }

            IssueFormationMove(ray.GetPoint(distance), false);
        }

        private void IssueAttackMoveCommand(Vector2 pointer)
        {
            RemoveMissingUnits();
            if (selectedUnits.Count == 0)
            {
                return;
            }

            Ray ray = worldCamera.ScreenPointToRay(pointer);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(ray, out float distance))
            {
                IssueFormationMove(ray.GetPoint(distance), true);
            }
        }

        private void IssueFormationMove(Vector3 center, bool attackMove)
        {
            int columns = Mathf.CeilToInt(Mathf.Sqrt(selectedUnits.Count));
            int rows = Mathf.CeilToInt((float)selectedUnits.Count / columns);

            for (int i = 0; i < selectedUnits.Count; i++)
            {
                int column = i % columns;
                int row = i / columns;
                Vector3 offset = new Vector3(
                    (column - (columns - 1) * 0.5f) * BalanceConfig.FormationSpacing,
                    0f,
                    (row - (rows - 1) * 0.5f) * BalanceConfig.FormationSpacing);
                Vector3 desired = grid.ClampWorldPosition(center + offset);
                Vector2Int desiredCell = grid.WorldToCell(desired);

                if (grid.TryFindNearestWalkable(
                        desiredCell,
                        Mathf.Max(grid.Width, grid.Height),
                        out Vector2Int goal))
                {
                    Unit selectedUnit = selectedUnits[i];
                    Combatant combatant = selectedUnit.Combatant;
                    if (combatant != null)
                    {
                        if (attackMove)
                        {
                            PrepareForCombatCommand(selectedUnit);
                            combatant.IssueAttackMove(grid.CellToWorld(goal));
                        }
                        else
                        {
                            PrepareForMoveCommand(selectedUnit);
                            combatant.IssueMove(grid.CellToWorld(goal));
                        }
                    }
                }
            }
        }

        private void AddToSelection(Unit unit)
        {
            if (unit == null || !unit.IsAlive || selectedUnits.Contains(unit))
            {
                return;
            }

            selectedUnits.Add(unit);
            unit.SetSelected(true);
        }

        private void RemoveFromSelection(Unit unit)
        {
            selectedUnits.Remove(unit);
            if (unit != null)
            {
                unit.SetSelected(false);
            }
        }

        private void ClearSelection()
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                if (selectedUnits[i] != null)
                {
                    selectedUnits[i].SetSelected(false);
                }
            }

            selectedUnits.Clear();
            ClearSelectedBuilding();
        }

        private void RemoveMissingUnits()
        {
            for (int i = selectedUnits.Count - 1; i >= 0; i--)
            {
                if (selectedUnits[i] == null || !selectedUnits[i].IsAlive)
                {
                    selectedUnits.RemoveAt(i);
                }
            }
        }

        private void ClearSelectedBuilding()
        {
            if (selectedBuilding != null)
            {
                selectedBuilding.SetSelected(false);
            }

            selectedBuilding = null;
        }

        public bool BeginBuildingPlacement(BuildingType type)
        {
            bool beganPlacement =
                placementController != null &&
                placementController.TryBeginPlacement(type);
            if (beganPlacement)
            {
                awaitingAttackMoveTarget = false;
            }

            return beganPlacement;
        }

        public bool QueueSelectedUnit(UnitType type)
        {
            if (selectedBuilding == null ||
                !selectedBuilding.IsPlayerControlled ||
                !selectedBuilding.IsConstructed)
            {
                return false;
            }

            ProductionQueue queue =
                selectedBuilding.GetComponent<ProductionQueue>();
            return queue != null && queue.TryQueue(type);
        }

        public bool BeginAttackMoveTargeting()
        {
            if (!HasSelectedUnits)
            {
                return false;
            }

            if (placementController != null)
            {
                placementController.CancelPlacement();
            }

            awaitingAttackMoveTarget = true;
            dragging = false;
            return true;
        }

        public void StopSelectedUnits()
        {
            RemoveMissingUnits();
            awaitingAttackMoveTarget = false;
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                Unit selectedUnit = selectedUnits[i];
                PrepareForCombatCommand(selectedUnit);
                if (selectedUnit.Combatant != null)
                {
                    selectedUnit.Combatant.Stop();
                }
            }
        }

        private void HandleHotkeys()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            int controlGroupNumber = GetPressedControlGroupNumber(keyboard);
            if (controlGroupNumber > 0)
            {
                bool assigning =
                    keyboard.leftCtrlKey.isPressed ||
                    keyboard.rightCtrlKey.isPressed;
                if (assigning)
                {
                    AssignControlGroup(controlGroupNumber);
                }
                else
                {
                    RecallControlGroup(controlGroupNumber);
                }
            }

            if (HasSelectedWorker && placementController != null)
            {
                if (keyboard.dKey.wasPressedThisFrame)
                {
                    BeginBuildingPlacement(BuildingType.SupplyDepot);
                }
                else if (keyboard.bKey.wasPressedThisFrame)
                {
                    BeginBuildingPlacement(BuildingType.Barracks);
                }
                else if (keyboard.fKey.wasPressedThisFrame)
                {
                    BeginBuildingPlacement(BuildingType.Factory);
                }
                else if (keyboard.rKey.wasPressedThisFrame)
                {
                    BeginBuildingPlacement(BuildingType.Refinery);
                }
            }

            if (keyboard.aKey.wasPressedThisFrame)
            {
                BeginAttackMoveTargeting();
            }
            else if (keyboard.hKey.wasPressedThisFrame && HasSelectedUnits)
            {
                StopSelectedUnits();
            }

            if (keyboard.sKey.wasPressedThisFrame)
            {
                QueueSelectedUnit(UnitType.Worker);
            }
            else if (keyboard.mKey.wasPressedThisFrame)
            {
                QueueSelectedUnit(UnitType.Marine);
            }
            else if (keyboard.tKey.wasPressedThisFrame)
            {
                QueueSelectedUnit(UnitType.Tank);
            }
        }

        private void AssignControlGroup(int groupNumber)
        {
            RemoveMissingUnits();
            if (selectedUnits.Count > 0)
            {
                unitControlGroups.Assign(groupNumber, selectedUnits);
                buildingControlGroups.Clear(groupNumber);
                return;
            }

            unitControlGroups.Clear(groupNumber);
            if (selectedBuilding != null &&
                selectedBuilding.IsAlive &&
                selectedBuilding.IsPlayerControlled)
            {
                List<Building> buildingSelection = new List<Building>
                {
                    selectedBuilding
                };
                buildingControlGroups.Assign(groupNumber, buildingSelection);
            }
            else
            {
                buildingControlGroups.Clear(groupNumber);
            }
        }

        private void RecallControlGroup(int groupNumber)
        {
            IReadOnlyList<Unit> recalledUnits =
                unitControlGroups.Recall(groupNumber);
            ClearSelection();
            for (int i = 0; i < recalledUnits.Count; i++)
            {
                Unit unit = recalledUnits[i];
                if (unit != null && unit.IsAlive && unit.IsPlayerControlled)
                {
                    AddToSelection(unit);
                }
            }

            if (selectedUnits.Count > 0)
            {
                return;
            }

            IReadOnlyList<Building> recalledBuildings =
                buildingControlGroups.Recall(groupNumber);
            for (int i = 0; i < recalledBuildings.Count; i++)
            {
                Building building = recalledBuildings[i];
                if (building == null ||
                    !building.IsAlive ||
                    !building.IsPlayerControlled)
                {
                    continue;
                }

                selectedBuilding = building;
                selectedBuilding.SetSelected(true);
                break;
            }
        }

        private static int GetPressedControlGroupNumber(Keyboard keyboard)
        {
            if (keyboard.digit1Key.wasPressedThisFrame)
            {
                return 1;
            }

            if (keyboard.digit2Key.wasPressedThisFrame)
            {
                return 2;
            }

            if (keyboard.digit3Key.wasPressedThisFrame)
            {
                return 3;
            }

            if (keyboard.digit4Key.wasPressedThisFrame)
            {
                return 4;
            }

            return keyboard.digit5Key.wasPressedThisFrame ? 5 : 0;
        }

        private static void PrepareForMoveCommand(Unit unit)
        {
            WorkerGatherer gatherer = unit.GetComponent<WorkerGatherer>();
            if (gatherer != null)
            {
                gatherer.InterruptForMove();
            }

            WorkerBuilder builder = unit.GetComponent<WorkerBuilder>();
            if (builder != null)
            {
                builder.CancelConstruction();
            }
        }

        private static void PrepareForCombatCommand(Unit unit)
        {
            WorkerGatherer gatherer = unit.GetComponent<WorkerGatherer>();
            if (gatherer != null)
            {
                gatherer.StopGathering();
            }

            WorkerBuilder builder = unit.GetComponent<WorkerBuilder>();
            if (builder != null)
            {
                builder.CancelConstruction();
            }
        }

        private static ResourceNode FindClosestResource(Ray ray)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, 500f);
            ResourceNode closest = null;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < hits.Length; i++)
            {
                ResourceNode candidate =
                    hits[i].collider.GetComponentInParent<ResourceNode>();
                if (candidate != null && hits[i].distance < closestDistance)
                {
                    closest = candidate;
                    closestDistance = hits[i].distance;
                }
            }

            return closest;
        }

        private static ICombatTarget FindClosestEnemyTarget(
            Ray ray,
            int ownerId)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, 500f);
            ICombatTarget closest = null;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < hits.Length; i++)
            {
                ICombatTarget candidate =
                    hits[i].collider.GetComponentInParent<Unit>();
                if (candidate == null)
                {
                    candidate =
                        hits[i].collider.GetComponentInParent<Building>();
                }

                if (candidate != null &&
                    candidate.IsAlive &&
                    candidate.OwnerId != ownerId &&
                    FogOfWar.CanOwnerSeeTarget(ownerId, candidate) &&
                    hits[i].distance < closestDistance)
                {
                    closest = candidate;
                    closestDistance = hits[i].distance;
                }
            }

            return closest;
        }

        private static bool IsAdditiveSelectionPressed()
        {
            Keyboard keyboard = Keyboard.current;
            return keyboard != null &&
                   (keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed);
        }

        private static bool IsPointerOverUi()
        {
            return EventSystem.current != null &&
                   EventSystem.current.IsPointerOverGameObject();
        }

        private static Rect GetScreenRect(Vector2 first, Vector2 second)
        {
            float xMin = Mathf.Min(first.x, second.x);
            float yMin = Mathf.Min(first.y, second.y);
            return new Rect(
                xMin,
                yMin,
                Mathf.Abs(first.x - second.x),
                Mathf.Abs(first.y - second.y));
        }

        private static void DrawBorder(Rect rect, float thickness)
        {
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, thickness),
                Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness),
                Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, thickness, rect.height),
                Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height),
                Texture2D.whiteTexture);
        }
    }
}
