using UnityEngine;
using UnityEngine.Rendering;

namespace MiniRTS
{
    /// <summary>
    /// Lightweight runtime-created combat feedback. No prefab or authored asset is used.
    /// </summary>
    public static class CombatEffects
    {
        public static void PlayTracer(
            Vector3 start,
            Vector3 end,
            Color factionColor,
            float weaponScale = 1f)
        {
            PlayMuzzleFlash(start, end, factionColor, weaponScale);

            GameObject tracerObject = new GameObject("AttackTracer");
            LineRenderer tracer = tracerObject.AddComponent<LineRenderer>();
            Material material = CreateEffectMaterial(
                Color.Lerp(factionColor, Color.white, 0.65f));
            tracer.sharedMaterial = material;
            tracer.positionCount = 2;
            tracer.useWorldSpace = true;
            tracer.SetPosition(0, start);
            tracer.SetPosition(1, end);
            tracer.startWidth = 0.085f;
            tracer.endWidth = 0.025f;
            tracer.startColor = material.color;
            tracer.endColor = new Color(
                material.color.r,
                material.color.g,
                material.color.b,
                0.35f);
            tracer.shadowCastingMode = ShadowCastingMode.Off;
            tracer.receiveShadows = false;
            tracerObject.AddComponent<TimedCombatEffect>().Initialize(
                BalanceConfig.TracerDuration,
                tracerObject.transform.localScale,
                tracerObject.transform.localScale,
                material);
        }

        private static void PlayMuzzleFlash(
            Vector3 start,
            Vector3 end,
            Color factionColor,
            float weaponScale)
        {
            Vector3 direction = end - start;
            if (direction.sqrMagnitude <= 0.0001f)
            {
                direction = Vector3.forward;
            }
            else
            {
                direction.Normalize();
            }

            float size = Mathf.Clamp(weaponScale * 0.22f, 0.12f, 0.42f);
            GameObject flash = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            flash.name = "MuzzleFlash";
            flash.transform.position =
                start + direction * Mathf.Clamp(weaponScale * 0.32f, 0.16f, 0.58f);
            flash.transform.rotation =
                Quaternion.LookRotation(direction, Vector3.up);
            Vector3 startScale = new Vector3(
                size * 1.35f,
                size * 1.35f,
                size * 2.1f);
            flash.transform.localScale = startScale;
            RemoveCollider(flash);

            Material material = CreateEffectMaterial(
                Color.Lerp(factionColor, Color.white, 0.82f));
            Renderer effectRenderer = flash.GetComponent<Renderer>();
            effectRenderer.sharedMaterial = material;
            effectRenderer.shadowCastingMode = ShadowCastingMode.Off;
            effectRenderer.receiveShadows = false;
            flash.AddComponent<TimedCombatEffect>().Initialize(
                BalanceConfig.MuzzleFlashDuration,
                startScale,
                Vector3.zero,
                material);
        }

        public static void PlayUnitDeath(
            Vector3 worldPosition,
            Vector3 unitScale,
            Color factionColor)
        {
            GameObject flash = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            flash.name = "UnitDeathFlash";
            flash.transform.position = worldPosition;
            float radius = Mathf.Max(
                0.45f,
                Mathf.Max(unitScale.x, Mathf.Max(unitScale.y, unitScale.z)) * 0.65f);
            Vector3 startScale = Vector3.one * radius;
            flash.transform.localScale = startScale;
            RemoveCollider(flash);

            Material material = CreateEffectMaterial(
                Color.Lerp(factionColor, Color.white, 0.4f));
            Renderer effectRenderer = flash.GetComponent<Renderer>();
            effectRenderer.sharedMaterial = material;
            effectRenderer.shadowCastingMode = ShadowCastingMode.Off;
            effectRenderer.receiveShadows = false;
            flash.AddComponent<TimedCombatEffect>().Initialize(
                BalanceConfig.UnitDeathEffectSeconds,
                startScale,
                Vector3.zero,
                material);
        }

        public static void PlayBuildingRubble(
            Vector3 buildingPosition,
            Vector2Int footprint,
            float cellSize,
            Color factionColor)
        {
            GameObject rubble = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rubble.name = "BuildingRubble";
            Vector3 startScale = new Vector3(
                footprint.x * cellSize * 0.9f,
                0.24f,
                footprint.y * cellSize * 0.9f);
            rubble.transform.position = new Vector3(
                buildingPosition.x,
                startScale.y * 0.5f,
                buildingPosition.z);
            rubble.transform.localScale = startScale;
            RemoveCollider(rubble);

            Color rubbleColor = Color.Lerp(factionColor, new Color(0.1f, 0.09f, 0.08f), 0.72f);
            Material material = CreateEffectMaterial(rubbleColor);
            Renderer effectRenderer = rubble.GetComponent<Renderer>();
            effectRenderer.sharedMaterial = material;
            effectRenderer.shadowCastingMode = ShadowCastingMode.Off;
            rubble.AddComponent<TimedCombatEffect>().Initialize(
                BalanceConfig.BuildingRubbleSeconds,
                startScale,
                new Vector3(startScale.x * 0.7f, 0.025f, startScale.z * 0.7f),
                material);
        }

        private static Material CreateEffectMaterial(Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null)
            {
                shader = Shader.Find("Unlit/Color");
            }

            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            return new Material(shader)
            {
                name = "CombatEffectMaterial",
                color = color
            };
        }

        private static void RemoveCollider(GameObject target)
        {
            Collider collider = target.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
                Object.Destroy(collider);
            }
        }
    }

    /// <summary>
    /// Scales and removes a temporary combat visual and its runtime material.
    /// </summary>
    public sealed class TimedCombatEffect : MonoBehaviour
    {
        private float duration;
        private float elapsed;
        private Vector3 startScale;
        private Vector3 endScale;
        private Material ownedMaterial;

        public void Initialize(
            float lifetime,
            Vector3 initialScale,
            Vector3 finalScale,
            Material material)
        {
            duration = Mathf.Max(0.01f, lifetime);
            startScale = initialScale;
            endScale = finalScale;
            ownedMaterial = material;
        }

        private void Update()
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            transform.localScale = Vector3.Lerp(startScale, endScale, progress);
            if (progress >= 1f)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (ownedMaterial != null)
            {
                Destroy(ownedMaterial);
            }
        }
    }
}
