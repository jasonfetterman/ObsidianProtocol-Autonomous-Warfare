using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace MiniRTS
{
    /// <summary>
    /// Small runtime-created billboard health bar shared by units and buildings.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class WorldHealthBar : MonoBehaviour
    {
        private ICombatTarget target;
        private Component targetComponent;
        private GameObject barRoot;
        private Transform fillTransform;
        private Renderer fillRenderer;
        private Material backMaterial;
        private Material fillMaterial;

        public void Initialize(ICombatTarget combatTarget)
        {
            target = combatTarget ??
                throw new ArgumentNullException(nameof(combatTarget));
            targetComponent = combatTarget as Component;
            if (targetComponent == null)
            {
                throw new ArgumentException(
                    "A world health bar target must be a Component.",
                    nameof(combatTarget));
            }

            BuildBar();
            Refresh();
        }

        private void LateUpdate()
        {
            Refresh();
        }

        private void OnDestroy()
        {
            if (barRoot != null)
            {
                Destroy(barRoot);
            }

            if (backMaterial != null)
            {
                Destroy(backMaterial);
            }

            if (fillMaterial != null)
            {
                Destroy(fillMaterial);
            }
        }

        private void BuildBar()
        {
            barRoot = new GameObject($"{name}_HealthBar");

            GameObject background = GameObject.CreatePrimitive(PrimitiveType.Cube);
            background.name = "Background";
            background.transform.SetParent(barRoot.transform, false);
            background.transform.localScale = new Vector3(
                BalanceConfig.HealthBarWidth + 0.08f,
                BalanceConfig.HealthBarHeight + 0.06f,
                0.055f);
            RemoveCollider(background);
            backMaterial = CreateMaterial(new Color(0.025f, 0.025f, 0.025f));
            Renderer backgroundRenderer = background.GetComponent<Renderer>();
            backgroundRenderer.sharedMaterial = backMaterial;
            ConfigureRenderer(backgroundRenderer);

            GameObject fill = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fill.name = "Fill";
            fill.transform.SetParent(barRoot.transform, false);
            fillTransform = fill.transform;
            fillTransform.localPosition = new Vector3(0f, 0f, -0.04f);
            RemoveCollider(fill);
            fillMaterial = CreateMaterial(BalanceConfig.SelectionColor);
            fillRenderer = fill.GetComponent<Renderer>();
            fillRenderer.sharedMaterial = fillMaterial;
            ConfigureRenderer(fillRenderer);
        }

        private void Refresh()
        {
            if (barRoot == null || targetComponent == null ||
                target == null || !target.IsAlive)
            {
                if (barRoot != null)
                {
                    barRoot.SetActive(false);
                }

                return;
            }

            bool visible =
                FogOfWar.IsVisibleToPlayer(target) &&
                (target.IsSelected || target.IsDamaged);
            barRoot.SetActive(visible);
            if (!visible)
            {
                return;
            }

            float fraction = CombatMath.HitPointFraction(
                target.HitPoints,
                target.MaxHitPoints);
            float width = BalanceConfig.HealthBarWidth * fraction;
            fillTransform.localScale = new Vector3(
                width,
                BalanceConfig.HealthBarHeight,
                0.04f);
            fillTransform.localPosition = new Vector3(
                -(BalanceConfig.HealthBarWidth - width) * 0.5f,
                0f,
                -0.04f);
            fillMaterial.color = HealthColor(fraction);

            barRoot.transform.position = target.HealthBarWorldPosition;
            Camera worldCamera = Camera.main;
            if (worldCamera != null)
            {
                barRoot.transform.rotation = worldCamera.transform.rotation;
            }
        }

        private static Color HealthColor(float fraction)
        {
            return fraction < 0.5f
                ? Color.Lerp(new Color(0.9f, 0.08f, 0.05f), Color.yellow, fraction * 2f)
                : Color.Lerp(Color.yellow, new Color(0.1f, 0.9f, 0.18f),
                    (fraction - 0.5f) * 2f);
        }

        private static Material CreateMaterial(Color color)
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
                name = "WorldHealthBarMaterial",
                color = color
            };
        }

        private static void ConfigureRenderer(Renderer targetRenderer)
        {
            targetRenderer.shadowCastingMode = ShadowCastingMode.Off;
            targetRenderer.receiveShadows = false;
        }

        private static void RemoveCollider(GameObject targetObject)
        {
            Collider collider = targetObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
                Destroy(collider);
            }
        }
    }
}
