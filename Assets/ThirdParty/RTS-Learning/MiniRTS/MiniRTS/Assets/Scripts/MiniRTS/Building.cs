using System.Collections.Generic;
using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Shared building identity, durability, selection, and grid occupancy.
    /// </summary>
    [DisallowMultipleComponent]
    public class Building : MonoBehaviour, ICombatTarget
    {
        private static readonly List<Building> Buildings = new List<Building>();

        private Color factionColor;
        private float constructionElapsed;
        private float constructionSeconds;
        private Vector3 completedPosition;
        private Vector3 completedScale;
        private GameObject selectionMarker;
        private ModelVisual modelVisual;
        private bool initialized;
        private bool isDead;
        private bool footprintReleased;

        public static IReadOnlyList<Building> ActiveBuildings => Buildings;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetForPlaySession()
        {
            ResetStaticState();
        }

        internal static void ResetStaticState()
        {
            Buildings.Clear();
        }

        public int OwnerId { get; private set; }
        public int MaxHitPoints { get; private set; }
        public int HitPoints { get; private set; }
        public Vector2Int Footprint { get; private set; }
        public BuildingType Type { get; private set; }
        public string DisplayName => BuildingDefinition.Get(Type).DisplayName;
        public bool IsSelected { get; private set; }
        public bool IsAlive => initialized && !isDead;
        public bool IsDamaged => IsConstructed && HitPoints < MaxHitPoints;
        public bool IsConstructed { get; private set; }
        public float ConstructionProgress =>
            IsConstructed
                ? 1f
                : constructionSeconds <= 0f
                    ? 0f
                    : Mathf.Clamp01(constructionElapsed / constructionSeconds);
        public bool IsPlayerControlled => OwnerId == BalanceConfig.PlayerOwnerId;
        public Color FactionColor => factionColor;
        public virtual bool IsResourceDropoff => false;
        public Vector3 CombatTargetPosition
        {
            get
            {
                Collider buildingCollider = GetComponent<Collider>();
                return buildingCollider != null
                    ? buildingCollider.bounds.center
                    : transform.position;
            }
        }
        public Vector3 HealthBarWorldPosition
        {
            get
            {
                Collider buildingCollider = GetComponent<Collider>();
                float height = buildingCollider != null
                    ? buildingCollider.bounds.max.y
                    : transform.position.y + transform.localScale.y * 0.5f;
                return new Vector3(
                    transform.position.x,
                    height + 0.45f,
                    transform.position.z);
            }
        }

        public WalkGrid Grid { get; private set; }
        protected PlayerEconomy Economy { get; private set; }

        protected virtual void Awake()
        {
            CreateSelectionMarker();
        }

        protected virtual void OnEnable()
        {
            if (!Buildings.Contains(this))
            {
                Buildings.Add(this);
            }
        }

        protected virtual void OnDisable()
        {
            Buildings.Remove(this);
        }

        protected virtual void OnDestroy()
        {
            ReleaseFootprint();
        }

        public virtual void Initialize(
            WalkGrid walkGrid,
            PlayerEconomy factionEconomy,
            int ownerId,
            Color factionColor,
            BuildingType buildingType,
            Vector2Int footprint,
            int maxHitPoints,
            bool startsConstructed = true,
            float buildSeconds = 0f)
        {
            if (walkGrid == null)
            {
                throw new System.ArgumentNullException(nameof(walkGrid));
            }

            if (factionEconomy == null)
            {
                throw new System.ArgumentNullException(nameof(factionEconomy));
            }

            if (maxHitPoints <= 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(maxHitPoints));
            }

            if (!startsConstructed && buildSeconds <= 0f)
            {
                throw new System.ArgumentOutOfRangeException(nameof(buildSeconds));
            }

            Grid = walkGrid;
            Economy = factionEconomy;
            OwnerId = ownerId;
            Type = buildingType;
            Footprint = footprint;
            MaxHitPoints = maxHitPoints;
            this.factionColor = factionColor;
            constructionSeconds = buildSeconds;
            constructionElapsed = startsConstructed ? buildSeconds : 0f;
            IsConstructed = startsConstructed;
            HitPoints = startsConstructed ? maxHitPoints : 1;
            transform.position = BuildingFootprint.SnapToGrid(
                Grid,
                transform.position,
                footprint);
            completedPosition = transform.position;
            completedScale = transform.localScale;
            modelVisual = GetComponentInChildren<ModelVisual>(true);

            if (!BuildingFootprint.SetWalkable(Grid, transform.position, footprint, false))
            {
                Debug.LogWarning($"{name}'s footprint extends beyond the walk grid.", this);
            }

            initialized = true;
            ApplyConstructionVisual();
            PositionSelectionMarker();
            SetSelected(false);
            WorldHealthBar healthBar = gameObject.AddComponent<WorldHealthBar>();
            healthBar.Initialize(this);
            if (startsConstructed)
            {
                OnConstructionCompleted();
            }
        }

        public bool AdvanceConstruction(float deltaTime)
        {
            if (deltaTime < 0f)
            {
                throw new System.ArgumentOutOfRangeException(nameof(deltaTime));
            }

            if (!initialized || isDead || IsConstructed)
            {
                return IsConstructed;
            }

            constructionElapsed = Mathf.Min(
                constructionSeconds,
                constructionElapsed + deltaTime);
            HitPoints = Mathf.Max(
                1,
                Mathf.RoundToInt(MaxHitPoints * ConstructionProgress));
            ApplyConstructionVisual();

            if (constructionElapsed < constructionSeconds)
            {
                return false;
            }

            IsConstructed = true;
            HitPoints = MaxHitPoints;
            ApplyConstructionVisual();
            OnConstructionCompleted();
            return true;
        }

        public void SetSelected(bool selected)
        {
            IsSelected = selected;
            if (selectionMarker != null)
            {
                selectionMarker.SetActive(selected);
            }
        }

        public float DistanceToFootprint(Vector3 worldPosition)
        {
            float halfWidth = Footprint.x * Grid.CellSize * 0.5f;
            float halfDepth = Footprint.y * Grid.CellSize * 0.5f;
            float deltaX = Mathf.Max(
                0f,
                Mathf.Abs(worldPosition.x - transform.position.x) - halfWidth);
            float deltaZ = Mathf.Max(
                0f,
                Mathf.Abs(worldPosition.z - transform.position.z) - halfDepth);
            return Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
        }

        public float DistanceTo(Vector3 worldPosition)
        {
            return DistanceToFootprint(worldPosition);
        }

        public void TakeDamage(int damage)
        {
            if (!IsAlive || damage <= 0)
            {
                return;
            }

            HitPoints = CombatMath.ApplyDamage(HitPoints, damage);
            if (HitPoints > 0)
            {
                return;
            }

            isDead = true;
            IsSelected = false;
            Buildings.Remove(this);
            ReleaseFootprint();
            CombatEffects.PlayBuildingRubble(
                transform.position,
                Footprint,
                Grid != null ? Grid.CellSize : BalanceConfig.CellSize,
                factionColor);
            Destroy(gameObject);
        }

        public static Building FindNearestDropoff(int ownerId, Vector3 worldPosition)
        {
            Building nearest = null;
            float nearestDistanceSquared = float.MaxValue;

            for (int i = 0; i < Buildings.Count; i++)
            {
                Building candidate = Buildings[i];
                if (candidate == null ||
                    !candidate.initialized ||
                    !candidate.IsConstructed ||
                    candidate.OwnerId != ownerId ||
                    !candidate.IsResourceDropoff)
                {
                    continue;
                }

                Vector3 difference = candidate.transform.position - worldPosition;
                difference.y = 0f;
                float distanceSquared = difference.sqrMagnitude;
                if (distanceSquared < nearestDistanceSquared)
                {
                    nearest = candidate;
                    nearestDistanceSquared = distanceSquared;
                }
            }

            return nearest;
        }

        protected virtual void OnConstructionCompleted()
        {
        }

        private void ApplyConstructionVisual()
        {
            if (!initialized)
            {
                return;
            }

            float heightRatio = IsConstructed
                ? 1f
                : Mathf.Lerp(0.1f, 1f, ConstructionProgress);
            Vector3 scale = completedScale;
            scale.y = completedScale.y * heightRatio;
            transform.localScale = scale;
            transform.position = new Vector3(
                completedPosition.x,
                completedPosition.y - completedScale.y * 0.5f + scale.y * 0.5f,
                completedPosition.z);

            if (modelVisual != null)
            {
                modelVisual.ApplyFactionTint(IsConstructed
                    ? factionColor
                    : Color.Lerp(
                        new Color(0.18f, 0.2f, 0.22f),
                        factionColor,
                        0.35f + ConstructionProgress * 0.45f));
            }
        }

        private void ReleaseFootprint()
        {
            if (footprintReleased || !initialized || Grid == null)
            {
                return;
            }

            BuildingFootprint.SetWalkable(
                Grid,
                transform.position,
                Footprint,
                true);
            footprintReleased = true;
        }

        private void CreateSelectionMarker()
        {
            selectionMarker = SelectionRing.Create(
                transform,
                Vector3.zero,
                true);
        }

        private void PositionSelectionMarker()
        {
            if (selectionMarker == null)
            {
                return;
            }

            float verticalScale = Mathf.Max(0.01f, completedScale.y);
            selectionMarker.transform.localPosition =
                new Vector3(0f, -0.5f + 0.025f / verticalScale, 0f);
            float ringScale = ModelLibrary.Get(Type).SelectionRingScale;
            selectionMarker.transform.localScale =
                new Vector3(ringScale, 1f, ringScale);
        }
    }
}
