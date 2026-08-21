using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ObsidianProtocol.Garage
{
    /// <summary>
    /// ============================================================
    /// OBSIDIAN PROTOCOL
    /// AUTHORITATIVE UNIT PHYSICAL SPECIFICATION
    /// ============================================================
    ///
    /// AXES:
    /// X = LENGTH
    /// Y = HEIGHT
    /// Z = WIDTH
    ///
    /// ALL AUTHORITATIVE DIMENSIONS ARE STORED IN METERS.
    ///
    /// The system calculates derived physical information from the
    /// primary unit dimensions and category-specific quantities.
    ///
    /// GROUND:
    ///     Enter wheelCount.
    ///     Wheel dimensions, wheelbase, track width, clearance,
    ///     turning radius, etc. are automatically calculated.
    ///
    /// AIR:
    ///     Enter rotorCount.
    ///     Rotor diameter, radius, blade thickness, spacing and
    ///     rotor envelope are automatically calculated.
    ///
    /// SEA:
    ///     Enter propellerCount.
    ///     Propeller diameter, draft, waterline and marine
    ///     operating envelope are automatically calculated.
    ///
    /// ALL:
    ///     Payload, weapon, sensor, operating and collision
    ///     envelopes are automatically calculated.
    ///
    /// MODEL PLACEMENT:
    ///     The model can automatically be placed on the ground
    ///     after sizing.
    ///
    /// AUTOMATIC DROP:
    ///     In Play Mode, the unit can begin above the ground and
    ///     smoothly descend into its final position.
    ///
    /// ============================================================
    /// </summary>

    [ExecuteAlways]
    public class UnitPhysicalDimensions : MonoBehaviour
    {
        // ============================================================
        // IDENTITY
        // ============================================================

        [Header("UNIT IDENTITY")]

        public string unitName;

        public UnitPhysicalCategory category;


        // ============================================================
        // SOURCE / IMPORT
        // ============================================================

        [Header("SOURCE / IMPORT")]

        public UnitAuthoringSoftware authoringSoftware =
            UnitAuthoringSoftware.Other;

        public UnitSourceUnits sourceUnits =
            UnitSourceUnits.Meters;


        // ============================================================
        // IMPORT ORIENTATION
        // ============================================================

        [Header("IMPORT ORIENTATION")]

        [Tooltip(
            "Rotation used by the source/import pipeline."
        )]
        public Vector3 sourceRotationEuler =
            Vector3.zero;

        [Tooltip(
            "Compensate for source orientation when measuring."
        )]
        public bool compensateSourceOrientation =
            true;


        // ============================================================
        // PRIMARY BODY DIMENSIONS
        // ============================================================

        [Header("PRIMARY BODY DIMENSIONS — METERS")]

        [Min(0.001f)]
        public float length = 1f;

        [Min(0.001f)]
        public float height = 1f;

        [Min(0.001f)]
        public float width = 1f;


        // ============================================================
        // MASS
        // ============================================================

        [Header("MASS")]

        [Min(0f)]
        public float massKg = 100f;


        // ============================================================
        // UNITY MODEL
        // ============================================================

        [Header("UNITY MODEL")]

        [Tooltip(
            "Assign the Meshy Bridge imported model root."
        )]
        public Transform model;


        // ============================================================
        // GROUND VEHICLE
        // ============================================================

        [Header("GROUND VEHICLE")]

        public GroundDriveType groundDriveType =
            GroundDriveType.Wheeled;

        [Tooltip(
            "Enter the total number of wheels. " +
            "All wheel specifications are then calculated."
        )]
        [Min(0)]
        public int wheelCount;

        [Header("CALCULATED WHEEL SPECIFICATION")]

        [Min(0f)]
        public float wheelDiameter;

        [Min(0f)]
        public float wheelWidth;

        [Min(0f)]
        public float wheelbase;

        [Min(0f)]
        public float trackWidth;

        [Min(0f)]
        public float groundClearance;

        [Min(0f)]
        public float turningRadius;

        [Min(0f)]
        public float wheelRadius;

        [Min(0f)]
        public float wheelCircumference;

        [Min(0f)]
        public float wheelContactLength;

        [Min(0f)]
        public float wheelAxleSpacing;

        [Min(0f)]
        public float wheelSideClearance;


        // ============================================================
        // AIR VEHICLE
        // ============================================================

        [Header("AIR VEHICLE")]

        [Tooltip(
            "Enter rotor quantity. Rotor dimensions are automatically calculated."
        )]
        [Min(0)]
        public int rotorCount;

        [Header("CALCULATED ROTOR SPECIFICATION")]

        [Min(0f)]
        public float rotorDiameter;

        [Min(0f)]
        public float rotorBladeThickness;

        [Min(0f)]
        public float rotorRadius;

        [Min(0f)]
        public float rotorCircumference;

        [Min(0f)]
        public float rotorSpacing;

        [Min(0f)]
        public float rotorClearance;

        [Min(0f)]
        public float rotorEnvelopeLength;

        [Min(0f)]
        public float rotorEnvelopeWidth;

        [Min(0f)]
        public float rotorEnvelopeHeight;


        // ============================================================
        // SEA VEHICLE
        // ============================================================

        [Header("SEA VEHICLE")]

        [Tooltip(
            "Enter propeller quantity. Marine specifications are automatically calculated."
        )]
        [Min(0)]
        public int propellerCount;

        [Header("CALCULATED MARINE SPECIFICATION")]

        [Min(0f)]
        public float draft;

        [Min(0f)]
        public float propellerDiameter;

        [Min(0f)]
        public float propellerRadius;

        [Min(0f)]
        public float propellerClearance;

        [Min(0f)]
        public float waterlineHeight;

        [Min(0f)]
        public float hullClearance;

        [Min(0f)]
        public float marineOperatingLength;

        [Min(0f)]
        public float marineOperatingWidth;

        [Min(0f)]
        public float marineOperatingDepth;


        // ============================================================
        // ENVELOPES
        // ============================================================

        [Header("PAYLOAD ENVELOPE — METERS")]

        public Vector3 payloadEnvelope;

        [Header("WEAPON ENVELOPE — METERS")]

        public Vector3 weaponEnvelope;

        [Header("SENSOR ENVELOPE — METERS")]

        public Vector3 sensorEnvelope;


        // ============================================================
        // OPERATING FOOTPRINT
        // ============================================================

        [Header("OPERATING FOOTPRINT — METERS")]

        public Vector3 operatingFootprint;


        // ============================================================
        // COLLISION ENVELOPE
        // ============================================================

        [Header("PRIMARY COLLISION ENVELOPE — METERS")]

        public Vector3 collisionEnvelope;


        // ============================================================
        // CENTER OF MASS
        // ============================================================

        [Header("CENTER OF MASS")]

        [Range(0f, 100f)]
        public float centerOfMassXPercent = 50f;

        [Range(0f, 100f)]
        public float centerOfMassYPercent = 40f;

        public float centerOfMassZOffset = 0f;


        // ============================================================
        // MEASURED MODEL SIZE
        // ============================================================

        [Header("MEASURED MODEL SIZE — READ ONLY")]

        [SerializeField]
        private Vector3 measuredModelSize;

        public Vector3 MeasuredModelSize =>
            measuredModelSize;

        public Vector3 TargetDimensions =>
            new Vector3(
                length,
                height,
                width
            );


        // ============================================================
        // SCALE TRACKING
        // ============================================================

        [Header("SCALE INFORMATION — READ ONLY")]

        [SerializeField]
        private Vector3 appliedScale =
            Vector3.one;

        [SerializeField]
        private Vector3 lastUniformFitMultiplier =
            Vector3.one;

        [SerializeField]
        private Vector3 lastExactFitMultiplier =
            Vector3.one;

        public Vector3 AppliedScale =>
            appliedScale;

        public Vector3 LastUniformFitMultiplier =>
            lastUniformFitMultiplier;

        public Vector3 LastExactFitMultiplier =>
            lastExactFitMultiplier;


        // ============================================================
        // AUTOMATIC CALCULATION SETTINGS
        // ============================================================

        [Header("AUTOMATIC CALCULATION")]

        [Tooltip(
            "Automatically calculate category-specific physical specifications."
        )]
        public bool automaticPhysicalCalculation = true;

        [Tooltip(
            "Automatically calculate envelopes."
        )]
        public bool automaticEnvelopeCalculation = true;

        [Tooltip(
            "Automatically calculate operating footprint."
        )]
        public bool automaticOperatingFootprint = true;

        [Tooltip(
            "Automatically calculate collision envelope."
        )]
        public bool automaticCollisionEnvelope = true;


        // ============================================================
        // AUTOMATIC GROUND PLACEMENT
        // ============================================================

        [Header("AUTOMATIC GROUND PLACEMENT")]

        [Tooltip(
            "Automatically place the model onto the detected ground."
        )]
        public bool autoPlaceOnGround = true;

        [Tooltip(
            "Ground layers that can receive the unit."
        )]
        public LayerMask groundLayerMask = ~0;

        [Tooltip(
            "Extra distance above the ground."
        )]
        [Min(0f)]
        public float groundOffset = 0f;

        [Tooltip(
            "Maximum distance used to find ground."
        )]
        [Min(0.1f)]
        public float groundDetectionDistance = 1000f;

        [Tooltip(
            "Place the model after physical dimensions are calculated."
        )]
        public bool placeAfterPhysicalSizing = true;

        [Tooltip(
            "Automatically place the model when this component is enabled."
        )]
        public bool placeOnEnable = false;

        [Tooltip(
            "For sea units, place the hull at the calculated waterline."
        )]
        public bool useWaterlineForSeaUnits = false;


        // ============================================================
        // AUTOMATIC DROP / ARRIVAL
        // ============================================================

        [Header("AUTOMATIC DROP / ARRIVAL")]

        [Tooltip(
            "When enabled, the unit will descend smoothly onto the ground " +
            "instead of instantly snapping into position."
        )]
        public bool automaticGroundDrop = true;

        [Tooltip(
            "Height above the detected ground where the unit begins its descent."
        )]
        [Min(0f)]
        public float dropStartHeight = 25f;

        [Tooltip(
            "How fast the unit descends toward the ground, in meters per second."
        )]
        [Min(0.01f)]
        public float dropSpeed = 25f;

        [Tooltip(
            "When enabled, the unit rotates to a stable upright orientation before landing."
        )]
        public bool stabilizeRotationOnDrop = true;

        [Tooltip(
            "Rotation used when the unit finishes its descent."
        )]
        public Vector3 landingRotationEuler = Vector3.zero;

        [Tooltip(
            "Automatically begin the drop after physical sizing."
        )]
        public bool dropAfterPhysicalSizing = true;

        [Tooltip(
            "Distance from the ground at which the unit is considered landed."
        )]
        [Min(0.001f)]
        public float landingTolerance = 0.01f;


        // ============================================================
        // DROP STATE
        // ============================================================

        [Header("DROP STATE — READ ONLY")]

        [SerializeField]
        private bool isDropping;

        [SerializeField]
        private bool hasLanded;

        [SerializeField]
        private Vector3 dropTargetPosition;

        [SerializeField]
        private float detectedGroundHeight;

        public bool IsDropping =>
            isDropping;

        public bool HasLanded =>
            hasLanded;


        // ============================================================
        // GROUND CONTACT SAFETY
        // ============================================================

        [Header("GROUND CONTACT SAFETY")]

        public bool protectAgainstRigidbodyFall = true;

        public bool autoFindRigidbody = true;

        public bool makeRigidbodyKinematicDuringPlacement = true;

        [Min(0f)]
        public float groundSafetyOffset = 0.02f;

        [SerializeField]
        private Rigidbody controlledRigidbody;

        private bool originalRigidbodyKinematic;
        private bool originalRigidbodyUseGravity;
        private bool rigidbodyStateCaptured;


        // ============================================================
        // RIGIDBODY GROUND PROTECTION
        // ============================================================

        private Rigidbody FindControlledRigidbody()
        {
            if (controlledRigidbody != null)
                return controlledRigidbody;

            if (model != null)
            {
                controlledRigidbody =
                    model.GetComponent<Rigidbody>();

                if (controlledRigidbody == null)
                    controlledRigidbody =
                        model.GetComponentInChildren<Rigidbody>(true);

                if (controlledRigidbody == null)
                    controlledRigidbody =
                        model.GetComponentInParent<Rigidbody>();
            }

            if (controlledRigidbody == null)
                controlledRigidbody =
                    GetComponent<Rigidbody>();

            return controlledRigidbody;
        }


        private void BeginGroundPhysicsProtection()
        {
            if (!protectAgainstRigidbodyFall)
                return;

            Rigidbody rb =
                FindControlledRigidbody();

            if (rb == null)
                return;

            if (!rigidbodyStateCaptured)
            {
                originalRigidbodyKinematic =
                    rb.isKinematic;

                originalRigidbodyUseGravity =
                    rb.useGravity;

                rigidbodyStateCaptured = true;
            }

            if (makeRigidbodyKinematicDuringPlacement)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }


        private void EndGroundPhysicsProtection()
        {
            if (!protectAgainstRigidbodyFall)
                return;

            Rigidbody rb =
                FindControlledRigidbody();

            if (rb == null)
                return;

            if (!rigidbodyStateCaptured)
                return;

            rb.isKinematic =
                originalRigidbodyKinematic;

            rb.useGravity =
                originalRigidbodyUseGravity;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rigidbodyStateCaptured = false;
        }


        // ============================================================
        // UNIT CONVERSION
        // ============================================================

        private float GetSourceToMeterMultiplier()
        {
            switch (sourceUnits)
            {
                case UnitSourceUnits.Meters:
                    return 1f;

                case UnitSourceUnits.Centimeters:
                    return 0.01f;

                case UnitSourceUnits.Millimeters:
                    return 0.001f;

                case UnitSourceUnits.Inches:
                    return 0.0254f;

                case UnitSourceUnits.Feet:
                    return 0.3048f;

                default:
                    return 1f;
            }
        }


        // ============================================================
        // SOURCE ROTATION
        // ============================================================

        private Quaternion GetSourceRotation()
        {
            return Quaternion.Euler(
                sourceRotationEuler
            );
        }


        // ============================================================
        // ENABLE
        // ============================================================

        private void OnEnable()
        {
            isDropping = false;
            hasLanded = false;

            if (!placeOnEnable)
                return;

            if (model == null)
                return;

            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                EditorApplication.delayCall += DelayedPlaceOnGround;
#endif
            }
            else
            {
                if (automaticGroundDrop &&
                    dropAfterPhysicalSizing)
                {
                    // FIX 1 — ensure sizing happens before drop
                    CalculateAllPhysicalDimensions();
                    StartAutomaticGroundDrop();
                }
                else
                {
                    PlaceModelOnGround();
                }
            }
        }


#if UNITY_EDITOR

        private void DelayedPlaceOnGround()
        {
            if (this == null)
                return;

            if (model == null)
                return;

            if (!Application.isPlaying)
            {
                PlaceModelOnGround();
            }
        }

#endif


        // ============================================================
        // AUTOMATIC DROP UPDATE
        // ============================================================

        private void Update()
        {
            if (!Application.isPlaying)
                return;

            if (!isDropping)
                return;

            if (model == null)
            {
                isDropping = false;
                return;
            }

            PerformGroundDropStep();
        }


        // ============================================================
        // COMBINED RENDERER BOUNDS
        // ============================================================

        private Bounds GetCombinedRendererBounds(
            Renderer[] renderers)
        {
            Bounds bounds =
                renderers[0].bounds;

            for (int i = 1;
                i < renderers.Length;
                i++)
            {
                if (renderers[i] == null)
                    continue;

                bounds.Encapsulate(
                    renderers[i].bounds
                );
            }

            return bounds;
        }


        // ============================================================
        // MEASURE MODEL
        // ============================================================

        [ContextMenu("Measure Model")]
        public void MeasureModel()
        {
            if (model == null)
            {
                Debug.LogError(
                    $"[{unitName}] No model assigned.",
                    this
                );

                return;
            }

            Renderer[] renderers =
                model.GetComponentsInChildren<Renderer>(true);

            if (renderers.Length == 0)
            {
                Debug.LogError(
                    $"[{unitName}] No Renderer components found.",
                    this
                );

                return;
            }

            Bounds bounds =
                renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(
                    renderers[i].bounds
                );
            }

            Vector3 worldSize =
                bounds.size;

            Vector3 correctedSize =
                worldSize;

            if (compensateSourceOrientation)
            {
                Quaternion inverseRotation =
                    Quaternion.Inverse(
                        GetSourceRotation()
                    );

                correctedSize =
                    GetRotatedBoundsSize(
                        worldSize,
                        inverseRotation
                    );
            }

            correctedSize *=
                GetSourceToMeterMultiplier();

            measuredModelSize =
                correctedSize;

            Debug.Log(
                $"[{unitName}] " +
                $"Current: " +
                $"({measuredModelSize.x:F2}, " +
                $"{measuredModelSize.y:F2}, " +
                $"{measuredModelSize.z:F2}) | " +
                $"Target: " +
                $"({length:F2}, " +
                $"{height:F2}, " +
                $"{width:F2})",
                this
            );
        }


        // ============================================================
        // ROTATED BOUNDS
        // ============================================================

        private Vector3 GetRotatedBoundsSize(
    Vector3 size,
    Quaternion rotation)
        {
            Matrix4x4 matrix =
                Matrix4x4.Rotate(rotation);

            Vector3 x =
                new Vector3(
                    Mathf.Abs(matrix.m00),
                    Mathf.Abs(matrix.m10),
                    Mathf.Abs(matrix.m20)
                ) * size.x;

            Vector3 y =
                new Vector3(
                    Mathf.Abs(matrix.m01),
                    Mathf.Abs(matrix.m11),
                    Mathf.Abs(matrix.m21)
                ) * size.y;

            Vector3 z =
                new Vector3(
                    Mathf.Abs(matrix.m02),
                    Mathf.Abs(matrix.m12),
                    Mathf.Abs(matrix.m22)
                ) * size.z;

            return x + y + z;
        }


        // ============================================================
        // ACCEPT MEASURED SIZE
        // ============================================================

        [ContextMenu(
            "ACCEPT MEASURED SIZE AS AUTHORITATIVE"
        )]
        public void AcceptMeasuredSize()
        {
            if (model == null)
            {
                Debug.LogError(
                    $"[{unitName}] No model assigned.",
                    this
                );

                return;
            }

            MeasureModel();

            if (measuredModelSize.x <= 0f ||
                measuredModelSize.y <= 0f ||
                measuredModelSize.z <= 0f)
            {
                Debug.LogError(
                    $"[{unitName}] Invalid measured size.",
                    this
                );

                return;
            }

            length =
                measuredModelSize.x;

            height =
                measuredModelSize.y;

            width =
                measuredModelSize.z;

            CalculateAllPhysicalDimensions();
        }


        // ============================================================
        // UNIFORM REAL WORLD SIZE
        // ============================================================

        [ContextMenu(
            "SET MODEL TO REAL WORLD SIZE (UNIFORM)"
        )]
        public void SetRealWorldSizeUniform()
        {
            if (model == null)
            {
                Debug.LogError(
                    $"[{unitName}] Assign the model first.",
                    this
                );

                return;
            }

            MeasureModel();

            if (measuredModelSize.x <= 0f ||
                measuredModelSize.y <= 0f ||
                measuredModelSize.z <= 0f)
            {
                Debug.LogError(
                    $"[{unitName}] Invalid measured model size.",
                    this
                );

                return;
            }

            Vector3 target =
                TargetDimensions;

            float scaleX =
                target.x /
                measuredModelSize.x;

            float scaleY =
                target.y /
                measuredModelSize.y;

            float scaleZ =
                target.z /
                measuredModelSize.z;

            float uniformScale =
                Mathf.Min(
                    scaleX,
                    scaleY,
                    scaleZ
                );

            if (uniformScale <= 0f)
            {
                Debug.LogError(
                    $"[{unitName}] Invalid uniform scale.",
                    this
                );

                return;
            }

            lastUniformFitMultiplier =
                Vector3.one *
                uniformScale;

            model.localScale *=
                uniformScale;

            appliedScale =
                model.localScale;

            MeasureModel();

            CalculateAllPhysicalDimensions();
        }


        // ============================================================
        // EXACT X/Y/Z FIT
        // ============================================================

        [ContextMenu(
            "FIT EXACT PHYSICAL DIMENSIONS (X/Y/Z)"
        )]
        public void FitExactPhysicalDimensions()
        {
            if (model == null)
            {
                Debug.LogError(
                    $"[{unitName}] Assign the model first.",
                    this
                );

                return;
            }

            MeasureModel();

            if (measuredModelSize.x <= 0f ||
                measuredModelSize.y <= 0f ||
                measuredModelSize.z <= 0f)
            {
                Debug.LogError(
                    $"[{unitName}] Invalid measured model size.",
                    this
                );

                return;
            }

            Vector3 target =
                TargetDimensions;

            float scaleX =
                target.x /
                measuredModelSize.x;

            float scaleY =
                target.y /
                measuredModelSize.y;

            float scaleZ =
                target.z /
                measuredModelSize.z;

            lastExactFitMultiplier =
                new Vector3(
                    scaleX,
                    scaleY,
                    scaleZ
                );

            model.localScale =
                Vector3.Scale(
                    model.localScale,
                    lastExactFitMultiplier
                );

            appliedScale =
                model.localScale;

            MeasureModel();

            CalculateAllPhysicalDimensions();
        }


        // ============================================================
        // BACKWARD COMPATIBILITY
        // ============================================================

        [ContextMenu("SET MODEL TO REAL WORLD SIZE")]
        public void SetRealWorldSize()
        {
            SetRealWorldSizeUniform();
        }


        // ============================================================
        // AUTOMATIC GROUND PLACEMENT
        // ============================================================

        [ContextMenu("PLACE MODEL ON GROUND")]
        public void PlaceModelOnGround()
        {
            // FIX 2 — protect rigidbody before placement
            BeginGroundPhysicsProtection();

            if (!autoPlaceOnGround)
            {
                Debug.Log(
                    $"[{unitName}] Automatic ground placement disabled.",
                    this
                );

                return;
            }

            if (model == null)
            {
                Debug.LogError(
                    $"[{unitName}] No model assigned.",
                    this
                );

                return;
            }

            Renderer[] renderers =
                model.GetComponentsInChildren<Renderer>(true);

            if (renderers.Length == 0)
            {
                Debug.LogError(
                    $"[{unitName}] No Renderer components found.",
                    this
                );

                return;
            }

            Bounds bounds =
                renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(
                    renderers[i].bounds
                );
            }


            // --------------------------------------------------------
            // SEA WATERLINE
            // --------------------------------------------------------

            if (category == UnitPhysicalCategory.Sea &&
                useWaterlineForSeaUnits)
            {
                float targetWaterline =
                    waterlineHeight;

                float currentWaterline =
                    bounds.max.y;

                float verticalDifference =
                    targetWaterline -
                    currentWaterline;

                model.position +=
                    Vector3.up *
                    verticalDifference;

                hasLanded = true;
                isDropping = false;

                EndGroundPhysicsProtection(); // FIX 4 applies here too

                Debug.Log(
                    $"[{unitName}] " +
                    $"Placed at calculated marine waterline.",
                    this
                );

                return;
            }


            // --------------------------------------------------------
            // NORMAL GROUND PLACEMENT
            // --------------------------------------------------------

            float modelBottom =
                bounds.min.y;

            Vector3 rayOrigin =
                new Vector3(
                    bounds.center.x,
                    bounds.max.y +
                    groundDetectionDistance * 0.5f,
                    bounds.center.z
                );

            Ray ray =
                new Ray(
                    rayOrigin,
                    Vector3.down
                );

            if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                groundDetectionDistance,
                groundLayerMask,
                QueryTriggerInteraction.Ignore))
            {
                float difference =
                    hit.point.y -
                    modelBottom;

                model.position +=
                    Vector3.up *
                    (difference + groundOffset);

                isDropping = false;
                hasLanded = true;

                EndGroundPhysicsProtection(); // FIX 4

                Debug.Log(
                    $"[{unitName}] " +
                    $"MODEL PLACED ON GROUND.\n" +
                    $"Ground: {hit.point.y:F2}m\n" +
                    $"Previous Bottom: {modelBottom:F2}m\n" +
                    $"Offset: {groundOffset:F2}m",
                    this
                );

                return;
            }

            Debug.LogWarning(
                $"[{unitName}] " +
                $"No ground detected within " +
                $"{groundDetectionDistance:F1}m.",
                this
            );
        }


        // ============================================================
        // BEGIN AUTOMATIC GROUND DROP
        // ============================================================

        [ContextMenu("START AUTOMATIC GROUND DROP")]
        public void StartAutomaticGroundDrop()
        {
            // FIX 3 — protect rigidbody before drop
            BeginGroundPhysicsProtection();

            if (!Application.isPlaying)
            {
                Debug.LogWarning(
                    $"[{unitName}] " +
                    $"Automatic ground drop requires Play Mode.",
                    this
                );

                return;
            }

            if (!automaticGroundDrop)
            {
                Debug.Log(
                    $"[{unitName}] " +
                    $"Automatic ground drop disabled.",
                    this
                );

                return;
            }

            if (model == null)
            {
                Debug.LogError(
                    $"[{unitName}] No model assigned.",
                    this
                );

                return;
            }

            Renderer[] renderers =
                model.GetComponentsInChildren<Renderer>(true);

            if (renderers.Length == 0)
            {
                Debug.LogError(
                    $"[{unitName}] No renderers found.",
                    this
                );

                return;
            }


            // --------------------------------------------------------
            // GET CURRENT MODEL BOUNDS
            // --------------------------------------------------------

            Bounds bounds =
                renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(
                    renderers[i].bounds
                );
            }


            // --------------------------------------------------------
            // FIND GROUND
            // --------------------------------------------------------

            Vector3 rayOrigin =
                new Vector3(
                    bounds.center.x,
                    bounds.max.y +
                    dropStartHeight,
                    bounds.center.z
                );

            Ray ray =
                new Ray(
                    rayOrigin,
                    Vector3.down
                );

            if (!Physics.Raycast(
                ray,
                out RaycastHit hit,
                groundDetectionDistance +
                dropStartHeight,
                groundLayerMask,
                QueryTriggerInteraction.Ignore))
            {
                Debug.LogWarning(
                    $"[{unitName}] " +
                    $"Could not find ground for automatic drop.",
                    this
                );

                return;
            }

            detectedGroundHeight =
                hit.point.y;

            BeginGroundPhysicsProtection();

            // --------------------------------------------------------
            // CALCULATE MODEL BOTTOM TO PIVOT
            // --------------------------------------------------------

            float modelBottom =
                bounds.min.y;

            float bottomToPivot =
                model.position.y -
                modelBottom;


            // --------------------------------------------------------
            // CALCULATE FINAL MODEL POSITION
            // --------------------------------------------------------

            float finalY =
                hit.point.y +
                bottomToPivot +
                groundOffset;

            dropTargetPosition =
                new Vector3(
                    model.position.x,
                    finalY,
                    model.position.z
                );


            // --------------------------------------------------------
            // MOVE MODEL ABOVE GROUND
            // --------------------------------------------------------

            Vector3 startPosition =
                model.position;

            startPosition.y =
                finalY +
                dropStartHeight;

            model.position =
                startPosition;


            // --------------------------------------------------------
            // LANDING ROTATION
            // --------------------------------------------------------

            if (stabilizeRotationOnDrop)
            {
                model.rotation =
                    Quaternion.Euler(
                        landingRotationEuler
                    );

                renderers =
                    model.GetComponentsInChildren<Renderer>(true);

                if (renderers.Length > 0)
                {
                    Bounds rotatedBounds =
                        renderers[0].bounds;

                    for (int i = 1; i < renderers.Length; i++)
                    {
                        rotatedBounds.Encapsulate(
                            renderers[i].bounds
                        );
                    }

                    float rotatedBottomToPivot =
                        model.position.y -
                        rotatedBounds.min.y;

                    finalY =
                        hit.point.y +
                        rotatedBottomToPivot +
                        groundOffset;

                    dropTargetPosition =
                        new Vector3(
                            model.position.x,
                            finalY,
                            model.position.z
                        );

                    startPosition =
                        model.position;

                    startPosition.y =
                        finalY +
                        dropStartHeight;

                    model.position =
                        startPosition;
                }
            }


            // --------------------------------------------------------
            // STATE
            // --------------------------------------------------------

            isDropping = true;
            hasLanded = false;

            Debug.Log(
                $"[{unitName}] " +
                $"AUTOMATIC GROUND DROP STARTED.\n" +

                $"Ground Height: " +
                $"{detectedGroundHeight:F2}m\n" +

                $"Drop Start: " +
                $"{model.position.y:F2}m\n" +

                $"Landing Height: " +
                $"{dropTargetPosition.y:F2}m\n" +

                $"Drop Distance: " +
                $"{dropStartHeight:F2}m",
                this
            );
        }


        // ============================================================
        // PERFORM GROUND DROP STEP
        // ============================================================

        private void PerformGroundDropStep()
        {
            if (model == null)
            {
                isDropping = false;
                return;
            }


            // --------------------------------------------------------
            // MOVE DOWN
            // --------------------------------------------------------

            // --------------------------------------------------------
            // MOVE DOWN
            // --------------------------------------------------------

            float currentDropSpeed =
                dropSpeed *
                Mathf.Clamp(
                    Vector3.Distance(
                        model.position,
                        dropTargetPosition
                    ) / 5f,
                    1f,
                    4f
                );

            Vector3 nextPosition =
    Vector3.MoveTowards(
        model.position,
        dropTargetPosition,
        currentDropSpeed *
        Time.deltaTime
    );

            Rigidbody rb =
                model.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.MovePosition(nextPosition);
            }
            else
            {
                model.position = nextPosition;
            }


            // --------------------------------------------------------
            // ROTATION
            // --------------------------------------------------------

            if (stabilizeRotationOnDrop)
            {
                Quaternion targetRotation =
                    Quaternion.Euler(
                        landingRotationEuler
                    );

                model.rotation =
                    Quaternion.RotateTowards(
                        model.rotation,
                        targetRotation,
                        180f *
                        Time.deltaTime
                    );
            }


            // --------------------------------------------------------
            // CHECK LANDING
            // --------------------------------------------------------

            bool reachedTarget =
                Vector3.Distance(
                    model.position,
                    dropTargetPosition
                ) <= landingTolerance;

            if (!reachedTarget)
                return;


            // --------------------------------------------------------
            // FORCE EXACT LANDING POSITION
            // --------------------------------------------------------

            // --------------------------------------------------------
            // FORCE EXACT LANDING POSITION
            // --------------------------------------------------------

            model.position =
                dropTargetPosition;

            if (stabilizeRotationOnDrop)
            {
                model.rotation =
                    Quaternion.Euler(
                        landingRotationEuler
                    );
            }

            // --------------------------------------------------------
            // FINAL GROUND CONTACT CORRECTION
            // --------------------------------------------------------

            Renderer[] landingRenderers =
                model.GetComponentsInChildren<Renderer>(true);

            if (landingRenderers.Length > 0)
            {
                Bounds landingBounds =
                    landingRenderers[0].bounds;

                for (int i = 1; i < landingRenderers.Length; i++)
                {
                    landingBounds.Encapsulate(
                        landingRenderers[i].bounds
                    );
                }

                float penetration =
                    detectedGroundHeight -
                    landingBounds.min.y;

                if (penetration > 0f)
                {
                    model.position +=
                        Vector3.up *
                        penetration;
                }
            }

            isDropping = false;
            hasLanded = true;

            // FIX 4 — restore rigidbody when landing
            EndGroundPhysicsProtection();

            Debug.Log(
                $"[{unitName}] " +
                $"UNIT LANDED ON GROUND.",
                this
            );
        }


        // ============================================================
        // STOP GROUND DROP
        // ============================================================

        [ContextMenu("STOP GROUND DROP")]
        public void StopAutomaticGroundDrop()
        {
            isDropping = false;

            // FIX 5 — restore rigidbody when manually stopped
            EndGroundPhysicsProtection();

            Debug.Log(
                $"[{unitName}] " +
                $"Automatic ground drop stopped.",
                this
            );
        }

        // ============================================================
        // GROUND VEHICLE CALCULATIONS
        // ============================================================

        [ContextMenu("Calculate Wheel Specification")]
        public void CalculateWheelSpecification()
        {
            if (category != UnitPhysicalCategory.Ground)
            {
                Debug.Log(
                    $"[{unitName}] Not a ground unit.",
                    this
                );

                return;
            }

            if (groundDriveType != GroundDriveType.Wheeled)
            {
                CalculateTrackedGroundSpecification();
                return;
            }

            if (wheelCount <= 0)
            {
                Debug.LogWarning(
                    $"[{unitName}] Wheel count is 0.",
                    this
                );

                ClearWheelSpecification();
                return;
            }


            // --------------------------------------------------------
            // WHEEL DIAMETER
            // --------------------------------------------------------

            float wheelCountFactor =
                Mathf.Clamp(
                    Mathf.Sqrt(
                        8f /
                        Mathf.Max(
                            2f,
                            wheelCount
                        )
                    ),
                    0.55f,
                    1.35f
                );

            wheelDiameter =
                Mathf.Clamp(
                    height *
                    0.34f *
                    wheelCountFactor,
                    0.18f,
                    height * 0.55f
                );


            // --------------------------------------------------------
            // WHEEL WIDTH
            // --------------------------------------------------------

            wheelWidth =
                Mathf.Clamp(
                    width *
                    0.12f,
                    0.08f,
                    width * 0.20f
                );


            // --------------------------------------------------------
            // WHEEL RADIUS
            // --------------------------------------------------------

            wheelRadius =
                wheelDiameter *
                0.5f;


            // --------------------------------------------------------
            // WHEEL CIRCUMFERENCE
            // --------------------------------------------------------

            wheelCircumference =
                Mathf.PI *
                wheelDiameter;


            // --------------------------------------------------------
            // AXLE COUNT
            // --------------------------------------------------------

            int axleCount =
                Mathf.Max(
                    1,
                    Mathf.CeilToInt(
                        wheelCount /
                        2f
                    )
                );


            // --------------------------------------------------------
            // WHEELBASE
            // --------------------------------------------------------

            if (axleCount <= 1)
            {
                wheelbase = 0f;
            }
            else
            {
                wheelbase =
                    length /
                    (axleCount + 0.25f);

                wheelbase =
                    Mathf.Clamp(
                        wheelbase,
                        length * 0.25f,
                        length * 0.80f
                    );
            }


            // --------------------------------------------------------
            // TRACK WIDTH
            // --------------------------------------------------------

            trackWidth =
                Mathf.Clamp(
                    width *
                    0.70f,
                    width * 0.50f,
                    width * 0.90f
                );


            // --------------------------------------------------------
            // GROUND CLEARANCE
            // --------------------------------------------------------

            groundClearance =
                Mathf.Clamp(
                    wheelDiameter *
                    0.28f,
                    0.08f,
                    height * 0.25f
                );


            // --------------------------------------------------------
            // TURNING RADIUS
            // --------------------------------------------------------

            turningRadius =
                Mathf.Max(
                    0.1f,
                    wheelbase /
                    Mathf.Tan(
                        35f *
                        Mathf.Deg2Rad
                    )
                );


            // --------------------------------------------------------
            // WHEEL CONTACT LENGTH
            // --------------------------------------------------------

            wheelContactLength =
                Mathf.Clamp(
                    wheelDiameter *
                    0.35f,
                    0.05f,
                    wheelDiameter
                );


            // --------------------------------------------------------
            // AXLE SPACING
            // --------------------------------------------------------

            if (axleCount > 1)
            {
                wheelAxleSpacing =
                    wheelbase /
                    (axleCount - 1);
            }
            else
            {
                wheelAxleSpacing = 0f;
            }


            // --------------------------------------------------------
            // SIDE CLEARANCE
            // --------------------------------------------------------

            wheelSideClearance =
                Mathf.Max(
                    0.02f,
                    (
                        width -
                        trackWidth -
                        wheelWidth
                    ) * 0.5f
                );

            Debug.Log(
                $"[{unitName}] " +
                $"WHEEL SPECIFICATION CALCULATED.\n" +

                $"Wheel Count: {wheelCount}\n" +
                $"Wheel Diameter: {wheelDiameter:F3}m\n" +
                $"Wheel Radius: {wheelRadius:F3}m\n" +
                $"Wheel Width: {wheelWidth:F3}m\n" +
                $"Wheelbase: {wheelbase:F3}m\n" +
                $"Track Width: {trackWidth:F3}m\n" +
                $"Ground Clearance: {groundClearance:F3}m\n" +
                $"Turning Radius: {turningRadius:F3}m\n" +
                $"Wheel Circumference: {wheelCircumference:F3}m\n" +
                $"Wheel Contact Length: {wheelContactLength:F3}m\n" +
                $"Axle Spacing: {wheelAxleSpacing:F3}m\n" +
                $"Side Clearance: {wheelSideClearance:F3}m",
                this
            );
        }


        // ============================================================
        // TRACKED GROUND SPECIFICATION
        // ============================================================

        private void CalculateTrackedGroundSpecification()
        {
            if (groundDriveType != GroundDriveType.Tracked)
                return;

            wheelDiameter =
                Mathf.Clamp(
                    height * 0.30f,
                    0.20f,
                    height * 0.50f
                );

            wheelRadius =
                wheelDiameter * 0.5f;

            wheelWidth =
                Mathf.Clamp(
                    width * 0.16f,
                    0.10f,
                    width * 0.25f
                );

            wheelbase =
                Mathf.Clamp(
                    length * 0.55f,
                    0.5f,
                    length * 0.80f
                );

            trackWidth =
                Mathf.Clamp(
                    width * 0.80f,
                    width * 0.55f,
                    width * 0.95f
                );

            groundClearance =
                Mathf.Clamp(
                    height * 0.18f,
                    0.10f,
                    height * 0.25f
                );

            turningRadius =
                Mathf.Max(
                    0.1f,
                    width * 0.25f
                );

            wheelCircumference =
                Mathf.PI *
                wheelDiameter;

            wheelContactLength =
                length *
                0.65f;

            wheelAxleSpacing =
                wheelbase /
                4f;

            wheelSideClearance =
                Mathf.Max(
                    0.02f,
                    (
                        width -
                        trackWidth
                    ) * 0.5f
                );
        }


        // ============================================================
        // CLEAR WHEEL DATA
        // ============================================================

        private void ClearWheelSpecification()
        {
            wheelDiameter = 0f;
            wheelWidth = 0f;
            wheelbase = 0f;
            trackWidth = 0f;
            groundClearance = 0f;
            turningRadius = 0f;
            wheelRadius = 0f;
            wheelCircumference = 0f;
            wheelContactLength = 0f;
            wheelAxleSpacing = 0f;
            wheelSideClearance = 0f;
        }


        // ============================================================
        // AIR ROTOR CALCULATIONS
        // ============================================================

        [ContextMenu("Calculate Rotor Specification")]
        public void CalculateRotorSpecification()
        {
            if (category != UnitPhysicalCategory.Air)
            {
                Debug.Log(
                    $"[{unitName}] Not an air unit.",
                    this
                );

                return;
            }

            if (rotorCount <= 0)
            {
                ClearRotorSpecification();

                Debug.LogWarning(
                    $"[{unitName}] Rotor count is 0.",
                    this
                );

                return;
            }


            // --------------------------------------------------------
            // ROTOR DIAMETER
            // --------------------------------------------------------

            float countFactor =
                Mathf.Clamp(
                    Mathf.Sqrt(
                        4f /
                        Mathf.Max(
                            1f,
                            rotorCount
                        )
                    ),
                    0.45f,
                    1.50f
                );

            rotorDiameter =
                Mathf.Clamp(
                    Mathf.Min(
                        length,
                        width
                    ) *
                    0.42f *
                    countFactor,
                    0.20f,
                    Mathf.Min(
                        length,
                        width
                    ) * 0.85f
                );


            // --------------------------------------------------------
            // ROTOR RADIUS
            // --------------------------------------------------------

            rotorRadius =
                rotorDiameter *
                0.5f;


            // --------------------------------------------------------
            // BLADE THICKNESS
            // --------------------------------------------------------

            rotorBladeThickness =
                Mathf.Clamp(
                    rotorDiameter *
                    0.018f,
                    0.003f,
                    0.08f
                );


            // --------------------------------------------------------
            // ROTOR CIRCUMFERENCE
            // --------------------------------------------------------

            rotorCircumference =
                Mathf.PI *
                rotorDiameter;


            // --------------------------------------------------------
            // ROTOR SPACING
            // --------------------------------------------------------

            rotorSpacing =
                Mathf.Clamp(
                    rotorDiameter *
                    0.20f,
                    0.05f,
                    rotorDiameter
                );


            // --------------------------------------------------------
            // ROTOR CLEARANCE
            // --------------------------------------------------------

            rotorClearance =
                rotorDiameter *
                0.50f;


            // --------------------------------------------------------
            // ROTOR ENVELOPE
            // --------------------------------------------------------

            rotorEnvelopeLength =
                length +
                rotorClearance;

            rotorEnvelopeWidth =
                width +
                rotorClearance;

            rotorEnvelopeHeight =
                height +
                rotorDiameter *
                0.15f;

            Debug.Log(
                $"[{unitName}] " +
                $"ROTOR SPECIFICATION CALCULATED.\n" +

                $"Rotor Count: {rotorCount}\n" +
                $"Rotor Diameter: {rotorDiameter:F3}m\n" +
                $"Rotor Radius: {rotorRadius:F3}m\n" +
                $"Blade Thickness: {rotorBladeThickness:F3}m\n" +
                $"Rotor Circumference: {rotorCircumference:F3}m\n" +
                $"Rotor Spacing: {rotorSpacing:F3}m\n" +
                $"Rotor Clearance: {rotorClearance:F3}m\n" +
                $"Rotor Envelope: " +
                $"{rotorEnvelopeLength:F2} × " +
                $"{rotorEnvelopeHeight:F2} × " +
                $"{rotorEnvelopeWidth:F2}m",
                this
            );
        }


        // ============================================================
        // CLEAR ROTOR DATA
        // ============================================================

        private void ClearRotorSpecification()
        {
            rotorDiameter = 0f;
            rotorBladeThickness = 0f;
            rotorRadius = 0f;
            rotorCircumference = 0f;
            rotorSpacing = 0f;
            rotorClearance = 0f;
            rotorEnvelopeLength = 0f;
            rotorEnvelopeWidth = 0f;
            rotorEnvelopeHeight = 0f;
        }


        // ============================================================
        // SEA / MARINE CALCULATIONS
        // ============================================================

        [ContextMenu("Calculate Marine Specification")]
        public void CalculateMarineSpecification()
        {
            if (category != UnitPhysicalCategory.Sea)
            {
                Debug.Log(
                    $"[{unitName}] Not a sea unit.",
                    this
                );

                return;
            }

            if (propellerCount <= 0)
            {
                propellerCount = 1;
            }


            // --------------------------------------------------------
            // DRAFT
            // --------------------------------------------------------

            draft =
                Mathf.Clamp(
                    height *
                    0.45f,
                    0.25f,
                    height * 0.80f
                );


            // --------------------------------------------------------
            // PROPELLER DIAMETER
            // --------------------------------------------------------

            float propellerCountFactor =
                Mathf.Clamp(
                    Mathf.Sqrt(
                        1f /
                        Mathf.Max(
                            1f,
                            propellerCount
                        )
                    ),
                    0.45f,
                    1f
                );

            propellerDiameter =
                Mathf.Clamp(
                    height *
                    0.30f *
                    propellerCountFactor,
                    0.15f,
                    height * 0.60f
                );


            // --------------------------------------------------------
            // PROPELLER RADIUS
            // --------------------------------------------------------

            propellerRadius =
                propellerDiameter *
                0.5f;


            // --------------------------------------------------------
            // PROPELLER CLEARANCE
            // --------------------------------------------------------

            propellerClearance =
                propellerDiameter *
                0.50f;


            // --------------------------------------------------------
            // WATERLINE
            // --------------------------------------------------------

            waterlineHeight =
                draft;


            // --------------------------------------------------------
            // HULL CLEARANCE
            // --------------------------------------------------------

            hullClearance =
                Mathf.Max(
                    0.05f,
                    draft *
                    0.10f
                );


            // --------------------------------------------------------
            // MARINE OPERATING ENVELOPE
            // --------------------------------------------------------

            marineOperatingLength =
                length +
                Mathf.Max(
                    1f,
                    length * 0.20f
                );

            marineOperatingWidth =
                width +
                Mathf.Max(
                    0.5f,
                    width * 0.30f
                );

            marineOperatingDepth =
                draft +
                propellerDiameter +
                propellerClearance;

            Debug.Log(
                $"[{unitName}] " +
                $"MARINE SPECIFICATION CALCULATED.\n" +

                $"Propeller Count: {propellerCount}\n" +
                $"Draft: {draft:F3}m\n" +
                $"Propeller Diameter: {propellerDiameter:F3}m\n" +
                $"Propeller Radius: {propellerRadius:F3}m\n" +
                $"Propeller Clearance: {propellerClearance:F3}m\n" +
                $"Waterline Height: {waterlineHeight:F3}m\n" +
                $"Hull Clearance: {hullClearance:F3}m\n" +
                $"Marine Envelope: " +
                $"{marineOperatingLength:F2} × " +
                $"{marineOperatingWidth:F2} × " +
                $"{marineOperatingDepth:F2}m",
                this
            );
        }


        // ============================================================
        // PAYLOAD ENVELOPE
        // ============================================================

        [ContextMenu("Calculate Payload Envelope")]
        public void CalculatePayloadEnvelope()
        {
            float payloadLength =
                length *
                0.55f;

            float payloadHeight =
                height *
                0.35f;

            float payloadWidth =
                width *
                0.65f;

            payloadEnvelope =
                new Vector3(
                    Mathf.Max(
                        0.05f,
                        payloadLength
                    ),
                    Mathf.Max(
                        0.05f,
                        payloadHeight
                    ),
                    Mathf.Max(
                        0.05f,
                        payloadWidth
                    )
                );
        }


        // ============================================================
        // WEAPON ENVELOPE
        // ============================================================

        [ContextMenu("Calculate Weapon Envelope")]
        public void CalculateWeaponEnvelope()
        {
            float weaponLength =
                length *
                0.60f;

            float weaponHeight =
                height *
                0.30f;

            float weaponWidth =
                width *
                0.45f;

            weaponEnvelope =
                new Vector3(
                    Mathf.Max(
                        0.05f,
                        weaponLength
                    ),
                    Mathf.Max(
                        0.05f,
                        weaponHeight
                    ),
                    Mathf.Max(
                        0.05f,
                        weaponWidth
                    )
                );
        }


        // ============================================================
        // SENSOR ENVELOPE
        // ============================================================

        [ContextMenu("Calculate Sensor Envelope")]
        public void CalculateSensorEnvelope()
        {
            float sensorLength =
                length *
                0.40f;

            float sensorHeight =
                height *
                0.40f;

            float sensorWidth =
                width *
                0.50f;

            sensorEnvelope =
                new Vector3(
                    Mathf.Max(
                        0.05f,
                        sensorLength
                    ),
                    Mathf.Max(
                        0.05f,
                        sensorHeight
                    ),
                    Mathf.Max(
                        0.05f,
                        sensorWidth
                    )
                );
        }


        // ============================================================
        // OPERATING FOOTPRINT
        // ============================================================

        [ContextMenu("Calculate Operating Footprint")]
        public void CalculateOperatingFootprint()
        {
            switch (category)
            {
                case UnitPhysicalCategory.Air:
                    {
                        float rotorClearanceAmount =
                            rotorCount > 0
                                ? rotorClearance
                                : length * 0.10f;

                        operatingFootprint =
                            new Vector3(
                                length +
                                rotorClearanceAmount,

                                height,

                                width +
                                rotorClearanceAmount
                            );

                        break;
                    }

                case UnitPhysicalCategory.Ground:
                    {
                        float lengthMargin;

                        float widthMargin;

                        if (groundDriveType ==
                            GroundDriveType.Wheeled &&
                            wheelCount > 0)
                        {
                            lengthMargin =
                                Mathf.Max(
                                    1f,
                                    wheelDiameter *
                                    0.50f
                                );

                            widthMargin =
                                Mathf.Max(
                                    0.25f,
                                    wheelWidth
                                );
                        }
                        else
                        {
                            lengthMargin =
                                Mathf.Max(
                                    2.0f,
                                    length * 0.30f
                                );

                            widthMargin =
                                Mathf.Max(
                                    1.5f,
                                    width * 0.50f
                                );
                        }

                        operatingFootprint =
                            new Vector3(
                                length +
                                lengthMargin,

                                height,

                                width +
                                widthMargin
                            );

                        break;
                    }

                case UnitPhysicalCategory.Sea:
                    {
                        float lengthMargin =
                            Mathf.Max(
                                5.0f,
                                length * 0.30f
                            );

                        float widthMargin =
                            Mathf.Max(
                                2.0f,
                                width * 0.50f
                            );

                        operatingFootprint =
                            new Vector3(
                                marineOperatingLength > 0f
                                    ? marineOperatingLength
                                    : length +
                                      lengthMargin,

                                height,

                                marineOperatingWidth > 0f
                                    ? marineOperatingWidth
                                    : width +
                                      widthMargin
                            );

                        break;
                    }

                case UnitPhysicalCategory.Command:
                case UnitPhysicalCategory.Experimental:
                    {
                        float lengthMargin =
                            Mathf.Max(
                                2.0f,
                                length * 0.30f
                            );

                        float widthMargin =
                            Mathf.Max(
                                1.5f,
                                width * 0.50f
                            );

                        operatingFootprint =
                            new Vector3(
                                length +
                                lengthMargin,

                                height,

                                width +
                                widthMargin
                            );

                        break;
                    }
            }

            Debug.Log(
        $"[{unitName}] " +
        $"Operating Footprint: " +
        $"{operatingFootprint.x:F2}m × " +
        $"{operatingFootprint.y:F2}m × " +
        $"{operatingFootprint.z:F2}m",
        this
    );
        }


        // ============================================================
        // COLLISION ENVELOPE
        // ============================================================

        [ContextMenu("Calculate Collision Envelope")]
        public void CalculateCollisionEnvelope()
        {
            float xMargin =
                Mathf.Clamp(
                    length * 0.02f,
                    0.05f,
                    0.25f
                );

            float yMargin =
                Mathf.Clamp(
                    height * 0.02f,
                    0.05f,
                    0.20f
                );

            float zMargin =
                Mathf.Clamp(
                    width * 0.02f,
                    0.05f,
                    0.25f
                );

            collisionEnvelope =
                new Vector3(
                    Mathf.Max(
                        0.001f,
                        length -
                        xMargin
                    ),

                    Mathf.Max(
                        0.001f,
                        height -
                        yMargin
                    ),

                    Mathf.Max(
                        0.001f,
                        width -
                        zMargin
                    )
                );

            Debug.Log(
                $"[{unitName}] " +
                $"Collision Envelope: " +
                $"{collisionEnvelope.x:F2}m × " +
                $"{collisionEnvelope.y:F2}m × " +
                $"{collisionEnvelope.z:F2}m",
                this
            );
        }


        // ============================================================
        // CALCULATE EVERYTHING
        // ============================================================

        [ContextMenu(
            "CALCULATE ALL PHYSICAL DIMENSIONS"
        )]
        public void CalculateAllPhysicalDimensions()
        {
            // --------------------------------------------------------
            // CATEGORY-SPECIFIC CALCULATIONS
            // --------------------------------------------------------

            if (automaticPhysicalCalculation)
            {
                switch (category)
                {
                    case UnitPhysicalCategory.Ground:
                        CalculateWheelSpecification();
                        break;

                    case UnitPhysicalCategory.Air:
                        CalculateRotorSpecification();
                        break;

                    case UnitPhysicalCategory.Sea:
                        CalculateMarineSpecification();
                        break;

                    case UnitPhysicalCategory.Command:
                        break;

                    case UnitPhysicalCategory.Experimental:
                        break;
                }
            }


            // --------------------------------------------------------
            // ENVELOPES
            // --------------------------------------------------------

            if (automaticEnvelopeCalculation)
            {
                CalculatePayloadEnvelope();

                CalculateWeaponEnvelope();

                CalculateSensorEnvelope();
            }


            // --------------------------------------------------------
            // FOOTPRINT
            // --------------------------------------------------------

            if (automaticOperatingFootprint)
            {
                CalculateOperatingFootprint();
            }


            // --------------------------------------------------------
            // COLLISION
            // --------------------------------------------------------

            if (automaticCollisionEnvelope)
            {
                CalculateCollisionEnvelope();
            }


            // --------------------------------------------------------
            // GROUND PLACEMENT / DROP
            // --------------------------------------------------------

            if (placeAfterPhysicalSizing)
            {
                if (dropAfterPhysicalSizing &&
                    automaticGroundDrop &&
                    Application.isPlaying)
                {
                    StartAutomaticGroundDrop();
                }
                else
                {
                    PlaceModelOnGround();
                }
            }


            Debug.Log(
                $"[{unitName}] " +
                $"ALL PHYSICAL DIMENSIONS CALCULATED.",
                this
            );
        }


        // ============================================================
        // AIR ROTOR SPECIFICATION
        // ============================================================

        [ContextMenu(
            "Print Air Rotor Specification"
        )]
        public void PrintAirRotorSpecification()
        {
            if (category !=
                UnitPhysicalCategory.Air)
            {
                Debug.Log(
                    $"[{unitName}] Not an air unit.",
                    this
                );

                return;
            }

            Debug.Log(
                $"[{unitName}] AIR ROTOR SPEC\n" +

                $"Diameter: " +
                $"{rotorDiameter:F3}m\n" +

                $"Radius: " +
                $"{rotorRadius:F3}m\n" +

                $"Count: " +
                $"{rotorCount}\n" +

                $"Blade Thickness: " +
                $"{rotorBladeThickness:F3}m\n" +

                $"Spacing: " +
                $"{rotorSpacing:F3}m\n" +

                $"Clearance: " +
                $"{rotorClearance:F3}m\n" +

                $"Envelope: " +
                $"{rotorEnvelopeLength:F2} × " +
                $"{rotorEnvelopeHeight:F2} × " +
                $"{rotorEnvelopeWidth:F2}m",
                this
            );
        }


        // ============================================================
        // WHEEL SPECIFICATION
        // ============================================================

        [ContextMenu(
            "Print Ground Wheel Specification"
        )]
        public void PrintGroundWheelSpecification()
        {
            if (category !=
                UnitPhysicalCategory.Ground)
            {
                Debug.Log(
                    $"[{unitName}] Not a ground unit.",
                    this
                );

                return;
            }

            Debug.Log(
                $"[{unitName}] GROUND WHEEL SPEC\n" +

                $"Drive Type: " +
                $"{groundDriveType}\n" +

                $"Wheel Count: " +
                $"{wheelCount}\n" +

                $"Wheel Diameter: " +
                $"{wheelDiameter:F3}m\n" +

                $"Wheel Radius: " +
                $"{wheelRadius:F3}m\n" +

                $"Wheel Width: " +
                $"{wheelWidth:F3}m\n" +

                $"Wheelbase: " +
                $"{wheelbase:F3}m\n" +

                $"Track Width: " +
                $"{trackWidth:F3}m\n" +

                $"Ground Clearance: " +
                $"{groundClearance:F3}m\n" +

                $"Turning Radius: " +
                $"{turningRadius:F3}m\n" +

                $"Circumference: " +
                $"{wheelCircumference:F3}m\n" +

                $"Contact Length: " +
                $"{wheelContactLength:F3}m\n" +

                $"Axle Spacing: " +
                $"{wheelAxleSpacing:F3}m\n" +

                $"Side Clearance: " +
                $"{wheelSideClearance:F3}m",
                this
            );
        }


        // ============================================================
        // MARINE SPECIFICATION
        // ============================================================

        [ContextMenu(
            "Print Marine Specification"
        )]
        public void PrintMarineSpecification()
        {
            if (category !=
                UnitPhysicalCategory.Sea)
            {
                Debug.Log(
                    $"[{unitName}] Not a sea unit.",
                    this
                );

                return;
            }

            Debug.Log(
                $"[{unitName}] MARINE SPEC\n" +

                $"Propeller Count: " +
                $"{propellerCount}\n" +

                $"Draft: " +
                $"{draft:F3}m\n" +

                $"Propeller Diameter: " +
                $"{propellerDiameter:F3}m\n" +

                $"Propeller Radius: " +
                $"{propellerRadius:F3}m\n" +

                $"Propeller Clearance: " +
                $"{propellerClearance:F3}m\n" +

                $"Waterline Height: " +
                $"{waterlineHeight:F3}m\n" +

                $"Hull Clearance: " +
                $"{hullClearance:F3}m\n" +

                $"Operating Length: " +
                $"{marineOperatingLength:F3}m\n" +

                $"Operating Width: " +
                $"{marineOperatingWidth:F3}m\n" +

                $"Operating Depth: " +
                $"{marineOperatingDepth:F3}m",
                this
            );
        }


        // ============================================================
        // CENTER OF MASS
        // ============================================================

        public Vector3 GetCenterOfMassLocalPosition()
        {
            float x =
                length *
                (centerOfMassXPercent / 100f);

            float y =
                height *
                (centerOfMassYPercent / 100f);

            float z =
                centerOfMassZOffset;

            return new Vector3(
                x,
                y,
                z
            );
        }


        // ============================================================
        // COMPLETE SPECIFICATION
        // ============================================================

        [ContextMenu(
            "Print Complete Physical Specification"
        )]
        public void PrintCompletePhysicalSpecification()
        {
            Vector3 com =
                GetCenterOfMassLocalPosition();

            string categoryData =
                string.Empty;

            if (category ==
                UnitPhysicalCategory.Ground)
            {
                categoryData =
                    "\n" +
                    $"GROUND DRIVE: {groundDriveType}\n" +
                    $"WHEEL COUNT: {wheelCount}\n" +
                    $"WHEEL DIAMETER: {wheelDiameter:F3}m\n" +
                    $"WHEEL WIDTH: {wheelWidth:F3}m\n" +
                    $"WHEELBASE: {wheelbase:F3}m\n" +
                    $"TRACK WIDTH: {trackWidth:F3}m\n" +
                    $"GROUND CLEARANCE: {groundClearance:F3}m\n" +
                    $"TURNING RADIUS: {turningRadius:F3}m\n";
            }

            if (category ==
                UnitPhysicalCategory.Air)
            {
                categoryData =
                    "\n" +
                    $"ROTOR COUNT: {rotorCount}\n" +
                    $"ROTOR DIAMETER: {rotorDiameter:F3}m\n" +
                    $"ROTOR RADIUS: {rotorRadius:F3}m\n" +
                    $"BLADE THICKNESS: {rotorBladeThickness:F3}m\n" +
                    $"ROTOR SPACING: {rotorSpacing:F3}m\n" +
                    $"ROTOR CLEARANCE: {rotorClearance:F3}m\n";
            }

            if (category ==
                UnitPhysicalCategory.Sea)
            {
                categoryData =
                    "\n" +
                    $"PROPELLER COUNT: {propellerCount}\n" +
                    $"DRAFT: {draft:F3}m\n" +
                    $"PROPELLER DIAMETER: {propellerDiameter:F3}m\n" +
                    $"PROPELLER RADIUS: {propellerRadius:F3}m\n" +
                    $"WATERLINE: {waterlineHeight:F3}m\n" +
                    $"MARINE OPERATING DEPTH: {marineOperatingDepth:F3}m\n";
            }

            Debug.Log(
                $"========== {unitName} ==========\n" +

                $"CATEGORY: {category}\n" +

                $"BODY: " +
                $"{length:F2} × " +
                $"{height:F2} × " +
                $"{width:F2}m\n" +

                $"MASS: " +
                $"{massKg:F1}kg\n" +

                categoryData +

                $"OPERATING FOOTPRINT: " +
                $"{operatingFootprint.x:F2} × " +
                $"{operatingFootprint.y:F2} × " +
                $"{operatingFootprint.z:F2}m\n" +

                $"COLLISION ENVELOPE: " +
                $"{collisionEnvelope.x:F2} × " +
                $"{collisionEnvelope.y:F2} × " +
                $"{collisionEnvelope.z:F2}m\n" +

                $"PAYLOAD ENVELOPE: " +
                $"{payloadEnvelope.x:F2} × " +
                $"{payloadEnvelope.y:F2} × " +
                $"{payloadEnvelope.z:F2}m\n" +

                $"WEAPON ENVELOPE: " +
                $"{weaponEnvelope.x:F2} × " +
                $"{weaponEnvelope.y:F2} × " +
                $"{weaponEnvelope.z:F2}m\n" +

                $"SENSOR ENVELOPE: " +
                $"{sensorEnvelope.x:F2} × " +
                $"{sensorEnvelope.y:F2} × " +
                $"{sensorEnvelope.z:F2}m\n" +

                $"CENTER OF MASS: " +
                $"{com.x:F2} × " +
                $"{com.y:F2} × " +
                $"{com.z:F2}m\n" +

                $"MODEL SCALE: " +
                $"{model?.localScale}\n" +

                $"DROP STATE: " +
                $"{(isDropping ? "DROPPING" : "STATIONARY")}\n" +

                $"LANDED: " +
                $"{hasLanded}\n" +

                $"DROP TARGET: " +
                $"{dropTargetPosition}\n" +

                $"DETECTED GROUND HEIGHT: " +
                $"{detectedGroundHeight:F2}m\n" +

                $"================================",
                this
            );
        }


        // ============================================================
        // VALIDATE
        // ============================================================

        [ContextMenu("Validate Physical Size")]
        public void ValidatePhysicalSize()
        {
            if (model == null)
            {
                Debug.LogError(
                    $"[{unitName}] No model assigned.",
                    this
                );

                return;
            }

            MeasureModel();

            Vector3 target =
                TargetDimensions;

            Vector3 difference =
                measuredModelSize -
                target;

            bool valid =
                Mathf.Abs(difference.x) <= 0.02f &&
                Mathf.Abs(difference.y) <= 0.02f &&
                Mathf.Abs(difference.z) <= 0.02f;

            Debug.Log(
                $"[{unitName}] " +

                $"Current: " +
                $"({measuredModelSize.x:F2}, " +
                $"{measuredModelSize.y:F2}, " +
                $"{measuredModelSize.z:F2}) | " +

                $"Target: " +
                $"({target.x:F2}, " +
                $"{target.y:F2}, " +
                $"{target.z:F2}) | " +

                $"Scale: " +
                $"{model.localScale} | " +

                $"VALID: {valid}",
                this
            );
        }


        // ============================================================
        // RESET MODEL SCALE
        // ============================================================

        [ContextMenu("Reset Model Scale")]
        public void ResetModelScale()
        {
            if (model == null)
                return;

            model.localScale =
                Vector3.one;

            appliedScale =
                Vector3.one;

            lastUniformFitMultiplier =
                Vector3.one;

            lastExactFitMultiplier =
                Vector3.one;

            Debug.Log(
                $"[{unitName}] " +
                $"Model scale reset to 1,1,1.",
                this
            );
        }


        // ============================================================
        // FREEZE / APPLY TRANSFORMATIONS
        // ============================================================

        [ContextMenu(
            "FREEZE / APPLY MODEL TRANSFORMATIONS"
        )]
        public void FreezeApplyModelTransformations()
        {
            if (model == null)
            {
                Debug.LogError(
                    $"[{unitName}] No model assigned.",
                    this
                );

                return;
            }

            Vector3 currentScale =
                model.localScale;

            if (ApproximatelyOne(currentScale))
            {
                Debug.Log(
                    $"[{unitName}] " +
                    $"Model scale is already 1,1,1.",
                    this
                );

                return;
            }

#if UNITY_EDITOR

            if (!Application.isPlaying)
            {
                FreezeMeshesInEditor(
                    currentScale
                );
            }
            else
            {
                FreezeMeshesRuntime(
                    currentScale
                );
            }

#else

            FreezeMeshesRuntime(
                currentScale
            );

#endif

            model.localScale =
                Vector3.one;

            appliedScale =
                Vector3.one;

            MeasureModel();

            Debug.Log(
                $"[{unitName}] " +
                $"MODEL TRANSFORMATIONS FROZEN.\n" +

                $"Previous Scale: " +
                $"{currentScale}\n" +

                $"New Scale: " +
                $"{model.localScale}\n" +

                $"Final Physical Size: " +

                $"{measuredModelSize.x:F2}m × " +
                $"{measuredModelSize.y:F2}m × " +
                $"{measuredModelSize.z:F2}m",
                this
            );
        }


        // ============================================================
        // CHECK SCALE
        // ============================================================

        private bool ApproximatelyOne(
            Vector3 value)
        {
            return
                Mathf.Abs(value.x - 1f) < 0.0001f &&
                Mathf.Abs(value.y - 1f) < 0.0001f &&
                Mathf.Abs(value.z - 1f) < 0.0001f;
        }


        // ============================================================
        // FREEZE MESHES — RUNTIME
        // ============================================================

        private void FreezeMeshesRuntime(
            Vector3 rootScale)
        {
            MeshFilter[] meshFilters =
                model.GetComponentsInChildren<MeshFilter>(
                    true
                );

            foreach (
                MeshFilter filter
                in meshFilters)
            {
                if (filter.sharedMesh == null)
                    continue;

                Mesh original =
                    filter.sharedMesh;

                Mesh copy =
                    Instantiate(original);

                copy.name =
                    original.name +
                    "_Frozen";

                BakeScaleIntoMesh(
                    copy,
                    rootScale
                );

                filter.sharedMesh =
                    copy;
            }

            SkinnedMeshRenderer[] skinnedMeshes =
                model.GetComponentsInChildren<
                    SkinnedMeshRenderer
                >(true);

            foreach (
                SkinnedMeshRenderer renderer
                in skinnedMeshes)
            {
                if (renderer.sharedMesh == null)
                    continue;

                Mesh original =
                    renderer.sharedMesh;

                Mesh copy =
                    Instantiate(original);

                copy.name =
                    original.name +
                    "_Frozen";

                BakeScaleIntoMesh(
                    copy,
                    rootScale
                );

                renderer.sharedMesh =
                    copy;
            }
        }


        // ============================================================
        // FREEZE MESHES — EDITOR
        // ============================================================

#if UNITY_EDITOR

        private void FreezeMeshesInEditor(
            Vector3 rootScale)
        {
            MeshFilter[] meshFilters =
                model.GetComponentsInChildren<MeshFilter>(
                    true
                );

            foreach (
                MeshFilter filter
                in meshFilters)
            {
                if (filter.sharedMesh == null)
                    continue;

                Mesh original =
                    filter.sharedMesh;

                Mesh copy =
                    Instantiate(original);

                copy.name =
                    original.name +
                    "_Frozen";

                BakeScaleIntoMesh(
                    copy,
                    rootScale
                );

                Undo.RecordObject(
                    filter,
                    "Freeze Mesh Transform"
                );

                filter.sharedMesh =
                    copy;

                copy.hideFlags =
                    HideFlags.DontSaveInBuild |
                    HideFlags.DontSaveInEditor;

                EditorUtility.SetDirty(
                    filter
                );
            }

            SkinnedMeshRenderer[] skinnedMeshes =
                model.GetComponentsInChildren<
                    SkinnedMeshRenderer
                >(true);

            foreach (
                SkinnedMeshRenderer renderer
                in skinnedMeshes)
            {
                if (renderer.sharedMesh == null)
                    continue;

                Mesh original =
                    renderer.sharedMesh;

                Mesh copy =
                    Instantiate(original);

                copy.name =
                    original.name +
                    "_Frozen";

                BakeScaleIntoMesh(
                    copy,
                    rootScale
                );

                Undo.RecordObject(
                    renderer,
                    "Freeze Scale"
                );

                renderer.sharedMesh = copy;
            }
        }

        // ============================================================
        // BAKE SCALE INTO MESH
        // ============================================================

        private void BakeScaleIntoMesh(
            Mesh mesh,
            Vector3 scale)
        {
            if (mesh == null)
                return;

            Vector3[] vertices =
                mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] =
                    Vector3.Scale(
                        vertices[i],
                        scale
                    );
            }

            mesh.vertices =
                vertices;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
    }
}
#endif