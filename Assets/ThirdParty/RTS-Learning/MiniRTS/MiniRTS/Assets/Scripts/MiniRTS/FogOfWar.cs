using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MiniRTS
{
    /// <summary>
    /// Player-faction fog runtime: vision stamping, overlay texture, enemy hiding,
    /// and last-known enemy-building markers.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class FogOfWar : MonoBehaviour
    {
        private static readonly Color32 UndiscoveredColor =
            new Color32(0, 0, 0, 245);
        private static readonly Color32 DiscoveredColor =
            new Color32(0, 0, 0, 150);
        private static readonly Color32 VisibleColor =
            new Color32(0, 0, 0, 0);

        private readonly Dictionary<int, Renderer[]> rendererCache =
            new Dictionary<int, Renderer[]>();
        private readonly Dictionary<int, EnemyBuildingMemory> buildingMemories =
            new Dictionary<int, EnemyBuildingMemory>();
        private readonly HashSet<int> activeEnemyBuildingIds =
            new HashSet<int>();
        private readonly HashSet<int> activeEnemyEntityIds =
            new HashSet<int>();
        private readonly List<int> memoryKeys = new List<int>();
        private readonly List<int> rendererCacheKeys = new List<int>();

        private WalkGrid walkGrid;
        private Texture2D overlayTexture;
        private Color32[] overlayPixels;
        private Material overlayMaterial;
        private Mesh overlayMesh;
        private float nextVisibilityUpdate;

        public static FogOfWar Current { get; private set; }
        public FogGrid VisibilityGrid { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetForPlaySession()
        {
            ResetStaticState();
        }

        internal static void ResetStaticState()
        {
            Current = null;
        }

        public void Initialize(WalkGrid grid)
        {
            walkGrid = grid ??
                throw new System.ArgumentNullException(nameof(grid));
            VisibilityGrid = new FogGrid(grid);
            Current = this;
            BuildOverlay();
            RefreshVisibilityNow();
        }

        /// <summary>
        /// Player observers use the fog grid. The final red AI is intentionally
        /// omniscient, while Combatant still limits acquisition to normal aggro range.
        /// </summary>
        public static bool CanOwnerSeeTarget(
            int observerOwnerId,
            ICombatTarget target)
        {
            if (target == null)
            {
                return false;
            }

            if (Current == null || Current.VisibilityGrid == null)
            {
                return true;
            }

            Building building = target as Building;
            FogState state = building != null
                ? Current.GetFootprintState(
                    building.transform.position,
                    building.Footprint)
                : Current.VisibilityGrid.GetState(
                    target.CombatTargetPosition);
            bool omniscient =
                observerOwnerId != BalanceConfig.PlayerOwnerId;
            return FogVisibilityRules.CanSeeEntity(
                observerOwnerId,
                target.OwnerId,
                state,
                omniscient);
        }

        public static bool IsVisibleToPlayer(ICombatTarget target)
        {
            return CanOwnerSeeTarget(BalanceConfig.PlayerOwnerId, target);
        }

        public void RefreshVisibilityNow()
        {
            if (VisibilityGrid == null)
            {
                return;
            }

            VisibilityGrid.BeginVisibilityUpdate();
            StampPlayerVision();
            RefreshOverlayTexture();
            RefreshEnemyVisibility();
            nextVisibilityUpdate =
                Time.unscaledTime + BalanceConfig.FogUpdateInterval;
        }

        private void Update()
        {
            if (Time.unscaledTime >= nextVisibilityUpdate)
            {
                RefreshVisibilityNow();
            }
        }

        private void OnDestroy()
        {
            if (Current == this)
            {
                Current = null;
            }

            if (overlayTexture != null)
            {
                Destroy(overlayTexture);
            }

            if (overlayMaterial != null)
            {
                Destroy(overlayMaterial);
            }

            if (overlayMesh != null)
            {
                Destroy(overlayMesh);
            }
        }

        private void StampPlayerVision()
        {
            IReadOnlyList<Unit> units = Unit.ActiveUnits;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit == null ||
                    !unit.IsAlive ||
                    unit.OwnerId != BalanceConfig.PlayerOwnerId)
                {
                    continue;
                }

                float radius =
                    BalanceConfig.GetVisionRadius(unit.Type) *
                    walkGrid.CellSize;
                VisibilityGrid.StampCircle(unit.transform.position, radius);
            }

            IReadOnlyList<Building> buildings = Building.ActiveBuildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                Building building = buildings[i];
                if (building == null ||
                    !building.IsAlive ||
                    building.OwnerId != BalanceConfig.PlayerOwnerId)
                {
                    continue;
                }

                float radius =
                    BalanceConfig.GetVisionRadius(building.Type) *
                    walkGrid.CellSize;
                VisibilityGrid.StampCircle(building.transform.position, radius);
            }
        }

        private void BuildOverlay()
        {
            overlayTexture = new Texture2D(
                walkGrid.Width,
                walkGrid.Height,
                TextureFormat.RGBA32,
                false,
                true)
            {
                name = "FogOfWarTexture",
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };
            overlayPixels = new Color32[walkGrid.Width * walkGrid.Height];

            Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null)
            {
                shader = Shader.Find("Unlit/Transparent");
            }

            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            overlayMaterial = new Material(shader)
            {
                name = "FogOfWarOverlayMaterial",
                color = Color.white,
                mainTexture = overlayTexture,
                renderQueue = (int)RenderQueue.Transparent
            };
            ConfigureTransparentMaterial(overlayMaterial);
            if (overlayMaterial.HasProperty("_ZTest"))
            {
                overlayMaterial.SetFloat(
                    "_ZTest",
                    (float)CompareFunction.Always);
            }

            GameObject overlayObject = new GameObject(
                "FogOfWarOverlay",
                typeof(MeshFilter),
                typeof(MeshRenderer));
            overlayObject.transform.SetParent(transform, false);
            overlayObject.transform.position = new Vector3(
                walkGrid.Origin.x + walkGrid.Width * walkGrid.CellSize * 0.5f,
                BalanceConfig.FogOverlayHeight,
                walkGrid.Origin.z + walkGrid.Height * walkGrid.CellSize * 0.5f);

            float halfWidth = walkGrid.Width * walkGrid.CellSize * 0.5f;
            float halfHeight = walkGrid.Height * walkGrid.CellSize * 0.5f;
            overlayMesh = new Mesh
            {
                name = "FogOfWarOverlayMesh",
                vertices = new[]
                {
                    new Vector3(-halfWidth, 0f, -halfHeight),
                    new Vector3(-halfWidth, 0f, halfHeight),
                    new Vector3(halfWidth, 0f, halfHeight),
                    new Vector3(halfWidth, 0f, -halfHeight)
                },
                uv = new[]
                {
                    new Vector2(0f, 0f),
                    new Vector2(0f, 1f),
                    new Vector2(1f, 1f),
                    new Vector2(1f, 0f)
                },
                triangles = new[] { 0, 1, 2, 0, 2, 3 }
            };
            overlayMesh.RecalculateNormals();
            overlayMesh.RecalculateBounds();
            overlayObject.GetComponent<MeshFilter>().sharedMesh = overlayMesh;

            MeshRenderer overlayRenderer =
                overlayObject.GetComponent<MeshRenderer>();
            overlayRenderer.sharedMaterial = overlayMaterial;
            overlayRenderer.shadowCastingMode = ShadowCastingMode.Off;
            overlayRenderer.receiveShadows = false;
            overlayRenderer.sortingOrder = 100;
        }

        private void RefreshOverlayTexture()
        {
            for (int y = 0; y < walkGrid.Height; y++)
            {
                for (int x = 0; x < walkGrid.Width; x++)
                {
                    Color32 pixel;
                    switch (VisibilityGrid.GetState(x, y))
                    {
                        case FogState.Visible:
                            pixel = VisibleColor;
                            break;
                        case FogState.Discovered:
                            pixel = DiscoveredColor;
                            break;
                        default:
                            pixel = UndiscoveredColor;
                            break;
                    }

                    overlayPixels[y * walkGrid.Width + x] = pixel;
                }
            }

            overlayTexture.SetPixels32(overlayPixels);
            overlayTexture.Apply(false, false);
        }

        private void RefreshEnemyVisibility()
        {
            activeEnemyEntityIds.Clear();
            IReadOnlyList<Unit> units = Unit.ActiveUnits;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit == null ||
                    !unit.IsAlive ||
                    unit.OwnerId == BalanceConfig.PlayerOwnerId)
                {
                    continue;
                }

                activeEnemyEntityIds.Add(unit.GetInstanceID());
                SetTargetRenderersVisible(
                    unit,
                    IsVisibleToPlayer(unit));
            }

            activeEnemyBuildingIds.Clear();
            IReadOnlyList<Building> buildings = Building.ActiveBuildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                Building building = buildings[i];
                if (building == null ||
                    !building.IsAlive ||
                    building.OwnerId == BalanceConfig.PlayerOwnerId)
                {
                    continue;
                }

                int instanceId = building.GetInstanceID();
                activeEnemyBuildingIds.Add(instanceId);
                activeEnemyEntityIds.Add(instanceId);
                FogState state = GetFootprintState(
                    building.transform.position,
                    building.Footprint);
                bool visible = IsVisibleToPlayer(building);
                SetTargetRenderersVisible(building, visible);

                if (visible)
                {
                    EnemyBuildingMemory memory =
                        GetOrCreateMemory(instanceId, building);
                    memory.UpdateFrom(building, walkGrid.CellSize);
                    memory.Ghost.SetActive(false);
                }
                else if (buildingMemories.TryGetValue(
                             instanceId,
                             out EnemyBuildingMemory memory))
                {
                    memory.Ghost.SetActive(
                        FogVisibilityRules.ShouldShowLastKnownBuilding(
                            BalanceConfig.PlayerOwnerId,
                            building.OwnerId,
                            true,
                            state));
                }
            }

            memoryKeys.Clear();
            foreach (int key in buildingMemories.Keys)
            {
                memoryKeys.Add(key);
            }

            for (int i = 0; i < memoryKeys.Count; i++)
            {
                int key = memoryKeys[i];
                if (activeEnemyBuildingIds.Contains(key))
                {
                    continue;
                }

                EnemyBuildingMemory memory = buildingMemories[key];
                FogState state = GetFootprintState(
                    memory.WorldPosition,
                    memory.Footprint);
                if (state == FogState.Visible)
                {
                    Destroy(memory.Ghost);
                    buildingMemories.Remove(key);
                    rendererCache.Remove(key);
                }
                else
                {
                    memory.Ghost.SetActive(
                        state == FogState.Discovered);
                }
            }

            rendererCacheKeys.Clear();
            foreach (int key in rendererCache.Keys)
            {
                rendererCacheKeys.Add(key);
            }

            for (int i = 0; i < rendererCacheKeys.Count; i++)
            {
                int key = rendererCacheKeys[i];
                if (!activeEnemyEntityIds.Contains(key))
                {
                    rendererCache.Remove(key);
                }
            }
        }

        private void SetTargetRenderersVisible(
            ICombatTarget target,
            bool visible)
        {
            Component component = target as Component;
            if (component == null)
            {
                return;
            }

            int instanceId = component.GetInstanceID();
            if (!rendererCache.TryGetValue(
                    instanceId,
                    out Renderer[] renderers))
            {
                renderers =
                    component.GetComponentsInChildren<Renderer>(true);
                rendererCache.Add(instanceId, renderers);
            }

            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                {
                    renderers[i].enabled = visible;
                }
            }
        }

        private EnemyBuildingMemory GetOrCreateMemory(
            int instanceId,
            Building building)
        {
            if (buildingMemories.TryGetValue(
                    instanceId,
                    out EnemyBuildingMemory memory))
            {
                return memory;
            }

            GameObject ghost = new GameObject(
                $"LastKnown_{building.DisplayName}_{instanceId}");
            ghost.transform.SetParent(transform, false);
            ModelVisual ghostVisual = ModelVisualFactory.Create(
                ModelLibrary.Get(building.Type),
                ghost.transform);
            if (ghostVisual != null)
            {
                ghostVisual.ApplyTransparentTint(
                    new Color(0.46f, 0.5f, 0.56f, 0.32f));
            }

            ghost.SetActive(false);

            memory = new EnemyBuildingMemory(ghost);
            memory.UpdateFrom(building, walkGrid.CellSize);
            buildingMemories.Add(instanceId, memory);
            return memory;
        }

        private FogState GetFootprintState(
            Vector3 worldCenter,
            Vector2Int footprint)
        {
            Vector2Int minimum = BuildingFootprint.GetMinimumCell(
                walkGrid,
                worldCenter,
                footprint);
            FogState aggregate = FogState.Undiscovered;
            for (int x = 0; x < footprint.x; x++)
            {
                for (int y = 0; y < footprint.y; y++)
                {
                    FogState state = VisibilityGrid.GetState(
                        minimum.x + x,
                        minimum.y + y);
                    if (state == FogState.Visible)
                    {
                        return FogState.Visible;
                    }

                    if (state == FogState.Discovered)
                    {
                        aggregate = FogState.Discovered;
                    }
                }
            }

            return aggregate;
        }

        private static void ConfigureTransparentMaterial(Material material)
        {
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", material.color);
            }

            if (material.HasProperty("_Surface"))
            {
                material.SetFloat("_Surface", 1f);
            }

            if (material.HasProperty("_SrcBlend"))
            {
                material.SetFloat(
                    "_SrcBlend",
                    (float)BlendMode.SrcAlpha);
            }

            if (material.HasProperty("_DstBlend"))
            {
                material.SetFloat(
                    "_DstBlend",
                    (float)BlendMode.OneMinusSrcAlpha);
            }

            if (material.HasProperty("_ZWrite"))
            {
                material.SetFloat("_ZWrite", 0f);
            }

            if (material.HasProperty("_Cull"))
            {
                material.SetFloat("_Cull", (float)CullMode.Off);
            }

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHATEST_ON");
        }

        private sealed class EnemyBuildingMemory
        {
            public GameObject Ghost { get; }
            public Vector3 WorldPosition { get; private set; }
            public Vector2Int Footprint { get; private set; }

            public EnemyBuildingMemory(GameObject ghost)
            {
                Ghost = ghost;
            }

            public void UpdateFrom(Building building, float cellSize)
            {
                BuildingDefinition definition =
                    BuildingDefinition.Get(building.Type);
                WorldPosition = building.transform.position;
                Footprint = building.Footprint;
                Ghost.transform.position = new Vector3(
                    WorldPosition.x,
                    definition.Height * 0.5f,
                    WorldPosition.z);
                Ghost.transform.localScale = new Vector3(
                    building.Footprint.x * cellSize,
                    definition.Height,
                    building.Footprint.y * cellSize);
            }
        }
    }
}
