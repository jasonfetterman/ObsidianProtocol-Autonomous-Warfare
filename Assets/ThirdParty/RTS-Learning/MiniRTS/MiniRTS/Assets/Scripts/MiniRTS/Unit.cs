using System.Collections.Generic;
using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Selectable unit identity and selection presentation.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Unit : MonoBehaviour, ICombatTarget
    {
        private static readonly List<Unit> Units = new List<Unit>();

        private GameObject selectionMarker;
        private ModelVisual modelVisual;
        private UnitMover mover;
        private PlayerEconomy economy;
        private bool ownsSupplyReservation;
        private bool initialized;
        private bool isDead;

        public static IReadOnlyList<Unit> ActiveUnits => Units;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetForPlaySession()
        {
            ResetStaticState();
        }

        internal static void ResetStaticState()
        {
            Units.Clear();
        }

        public int OwnerId { get; private set; } = BalanceConfig.PlayerOwnerId;
        public UnitType Type { get; private set; } = UnitType.Worker;
        public string DisplayName => UnitDefinition.Get(Type).DisplayName;
        public int MaxHitPoints { get; private set; }
        public int HitPoints { get; private set; }
        public float MoveSpeed => UnitDefinition.Get(Type).MoveSpeed;
        public float GroundHeight => UnitDefinition.Get(Type).GroundHeight;
        public bool IsSelected { get; private set; }
        public bool IsAlive => initialized && !isDead;
        public bool IsDamaged => HitPoints < MaxHitPoints;
        public bool IsPlayerControlled => OwnerId == BalanceConfig.PlayerOwnerId;
        public Color FactionColor { get; private set; }
        public UnitMover Mover => mover != null ? mover : mover = GetComponent<UnitMover>();
        public Combatant Combatant { get; private set; }
        public Vector3 CombatTargetPosition
        {
            get
            {
                Collider bodyCollider = GetComponent<Collider>();
                return bodyCollider != null
                    ? bodyCollider.bounds.center
                    : transform.position;
            }
        }
        public Vector3 HealthBarWorldPosition
        {
            get
            {
                Collider bodyCollider = GetComponent<Collider>();
                float height = bodyCollider != null
                    ? bodyCollider.bounds.max.y
                    : transform.position.y + GroundHeight;
                return new Vector3(
                    transform.position.x,
                    height + 0.35f,
                    transform.position.z);
            }
        }

        private void Awake()
        {
            CreateSelectionMarker();
        }

        private void OnEnable()
        {
            if (!Units.Contains(this))
            {
                Units.Add(this);
            }
        }

        private void OnDisable()
        {
            Units.Remove(this);
        }

        private void OnDestroy()
        {
            if (ownsSupplyReservation && economy != null)
            {
                economy.ReleaseSupply(UnitDefinition.Get(Type).SupplyCost);
                ownsSupplyReservation = false;
            }
        }

        public void Initialize(
            int ownerId,
            Color factionColor,
            UnitType unitType = UnitType.Worker,
            PlayerEconomy factionEconomy = null,
            bool supplyAlreadyReserved = false)
        {
            OwnerId = ownerId;
            Type = unitType;
            FactionColor = factionColor;
            mover = GetComponent<UnitMover>();
            economy = factionEconomy;
            ownsSupplyReservation = supplyAlreadyReserved;
            UnitDefinition definition = UnitDefinition.Get(Type);
            MaxHitPoints = definition.HitPoints;
            HitPoints = MaxHitPoints;
            initialized = true;
            PositionSelectionMarker();

            modelVisual = GetComponentInChildren<ModelVisual>(true);
            if (modelVisual != null)
            {
                modelVisual.ApplyFactionTint(factionColor);
            }

            SetSelected(false);
            WorldHealthBar healthBar = gameObject.AddComponent<WorldHealthBar>();
            healthBar.Initialize(this);
        }

        public void SetCombatant(Combatant unitCombatant)
        {
            Combatant = unitCombatant;
        }

        public float DistanceTo(Vector3 worldPosition)
        {
            Vector3 difference = CombatTargetPosition - worldPosition;
            difference.y = 0f;
            return difference.magnitude;
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
            Units.Remove(this);
            if (mover != null)
            {
                mover.Stop();
            }

            CombatEffects.PlayUnitDeath(
                CombatTargetPosition,
                transform.localScale,
                FactionColor);
            Destroy(gameObject);
        }

        public void SetSelected(bool selected)
        {
            IsSelected = selected;
            if (selectionMarker != null)
            {
                selectionMarker.SetActive(selected);
            }
        }

        private void CreateSelectionMarker()
        {
            selectionMarker = SelectionRing.Create(
                transform,
                new Vector3(0f, -0.92f, 0f),
                false);
        }

        private void PositionSelectionMarker()
        {
            if (selectionMarker == null)
            {
                return;
            }

            float verticalScale = Mathf.Max(0.01f, transform.localScale.y);
            selectionMarker.transform.localPosition = new Vector3(
                0f,
                -GroundHeight / verticalScale + 0.025f / verticalScale,
                0f);
            float ringScale = ModelLibrary.Get(Type).SelectionRingScale;
            selectionMarker.transform.localScale =
                new Vector3(ringScale, 1f, ringScale);
        }
    }
}
