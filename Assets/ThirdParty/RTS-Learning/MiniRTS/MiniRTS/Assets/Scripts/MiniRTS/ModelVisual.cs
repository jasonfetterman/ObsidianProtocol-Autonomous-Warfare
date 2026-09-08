using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MiniRTS
{
    /// <summary>
    /// Runtime presentation wrapper for an instantiated imported model.
    /// Gameplay remains on the parent object and never depends on model geometry.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ModelVisual : MonoBehaviour
    {
        private const string FactionAccentMaterialName = "metalRed";

        private readonly List<Material> ownedMaterials =
            new List<Material>();

        private Renderer[] modelRenderers;
        private Material[][] modelMaterials;
        private MaterialPropertyBlock tintBlock;
        private bool hasFactionAccent;
        private bool isTransparent;

        public IReadOnlyList<Renderer> Renderers =>
            modelRenderers ?? Array.Empty<Renderer>();

        internal void Initialize()
        {
            modelRenderers = GetComponentsInChildren<Renderer>(true);
            modelMaterials = new Material[modelRenderers.Length][];
            for (int i = 0; i < modelRenderers.Length; i++)
            {
                modelMaterials[i] = modelRenderers[i].sharedMaterials;
            }

            tintBlock = new MaterialPropertyBlock();
            hasFactionAccent = FindFactionAccent();
        }

        public void ApplyFactionTint(Color factionColor)
        {
            EnsureInitialized();
            for (int rendererIndex = 0;
                 rendererIndex < modelRenderers.Length;
                 rendererIndex++)
            {
                Renderer modelRenderer = modelRenderers[rendererIndex];
                if (modelRenderer == null)
                {
                    continue;
                }

                Material[] materials = modelMaterials[rendererIndex];
                for (int materialIndex = 0;
                     materialIndex < materials.Length;
                     materialIndex++)
                {
                    Material material = materials[materialIndex];
                    bool tintMaterial = hasFactionAccent
                        ? IsFactionAccent(material)
                        : materialIndex == 0;
                    tintBlock.Clear();
                    if (tintMaterial && material != null)
                    {
                        Color baseColor = GetMaterialColor(material);
                        float value = Mathf.Max(
                            0.55f,
                            Mathf.Max(
                                baseColor.r,
                                Mathf.Max(baseColor.g, baseColor.b)));
                        Color targetColor = new Color(
                            factionColor.r * value,
                            factionColor.g * value,
                            factionColor.b * value,
                            baseColor.a);
                        Color tintedColor = Color.Lerp(
                            baseColor,
                            targetColor,
                            0.9f);
                        tintBlock.SetColor("_BaseColor", tintedColor);
                        tintBlock.SetColor("_Color", tintedColor);
                    }

                    modelRenderer.SetPropertyBlock(
                        tintBlock,
                        materialIndex);
                }
            }
        }

        public void ApplyTransparentTint(Color tint)
        {
            EnsureInitialized();
            if (!isTransparent)
            {
                CreateTransparentMaterialCopies();
            }

            for (int i = 0; i < ownedMaterials.Count; i++)
            {
                SetMaterialColor(ownedMaterials[i], tint);
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < ownedMaterials.Count; i++)
            {
                if (ownedMaterials[i] != null)
                {
                    Destroy(ownedMaterials[i]);
                }
            }

            ownedMaterials.Clear();
        }

        private void EnsureInitialized()
        {
            if (modelRenderers == null)
            {
                Initialize();
            }
        }

        private bool FindFactionAccent()
        {
            for (int rendererIndex = 0;
                 rendererIndex < modelRenderers.Length;
                 rendererIndex++)
            {
                Material[] materials = modelMaterials[rendererIndex];
                for (int materialIndex = 0;
                     materialIndex < materials.Length;
                     materialIndex++)
                {
                    if (IsFactionAccent(materials[materialIndex]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsFactionAccent(Material material)
        {
            return material != null &&
                   material.name.IndexOf(
                       FactionAccentMaterialName,
                       StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void CreateTransparentMaterialCopies()
        {
            for (int rendererIndex = 0;
                 rendererIndex < modelRenderers.Length;
                 rendererIndex++)
            {
                Renderer modelRenderer = modelRenderers[rendererIndex];
                Material[] sourceMaterials = modelMaterials[rendererIndex];
                Material[] transparentMaterials =
                    new Material[sourceMaterials.Length];
                for (int materialIndex = 0;
                     materialIndex < sourceMaterials.Length;
                     materialIndex++)
                {
                    Material source = sourceMaterials[materialIndex];
                    if (source == null)
                    {
                        continue;
                    }

                    Material copy = new Material(source)
                    {
                        name = $"{source.name}_RuntimeGhost",
                        hideFlags = HideFlags.DontSave,
                        renderQueue = (int)RenderQueue.Transparent
                    };
                    ConfigureTransparentMaterial(copy);
                    transparentMaterials[materialIndex] = copy;
                    ownedMaterials.Add(copy);
                }

                modelRenderer.sharedMaterials = transparentMaterials;
                modelMaterials[rendererIndex] = transparentMaterials;
                modelRenderer.shadowCastingMode = ShadowCastingMode.Off;
                modelRenderer.receiveShadows = false;
            }

            isTransparent = true;
        }

        private static Color GetMaterialColor(Material material)
        {
            if (material.HasProperty("_BaseColor"))
            {
                return material.GetColor("_BaseColor");
            }

            return material.HasProperty("_Color")
                ? material.GetColor("_Color")
                : Color.white;
        }

        private static void SetMaterialColor(Material material, Color color)
        {
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }

            material.color = color;
        }

        private static void ConfigureTransparentMaterial(Material material)
        {
            material.SetOverrideTag("RenderType", "Transparent");
            if (material.HasProperty("_Surface"))
            {
                material.SetFloat("_Surface", 1f);
            }

            if (material.HasProperty("_Mode"))
            {
                material.SetFloat("_Mode", 3f);
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

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHATEST_ON");
        }
    }

    /// <summary>
    /// Loads model prefabs and applies their library corrections consistently.
    /// </summary>
    public static class ModelVisualFactory
    {
        public static ModelVisual Create(
            ModelDefinition definition,
            Transform parent,
            string objectName = "ModelVisual")
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            GameObject prefab =
                Resources.Load<GameObject>(definition.ResourcePath);
            if (prefab == null)
            {
                Debug.LogError(
                    $"Could not load model at Resources/{definition.ResourcePath}.",
                    parent);
                return null;
            }

            GameObject instance = UnityEngine.Object.Instantiate(
                prefab,
                parent,
                false);
            instance.name = objectName;
            instance.transform.localPosition =
                Vector3.up * definition.VerticalOffset;
            instance.transform.localRotation =
                Quaternion.Euler(definition.EulerRotation);
            instance.transform.localScale = definition.Scale;

            Collider[] importedColliders =
                instance.GetComponentsInChildren<Collider>(true);
            for (int i = 0; i < importedColliders.Length; i++)
            {
                importedColliders[i].enabled = false;
                UnityEngine.Object.Destroy(importedColliders[i]);
            }

            ModelVisual visual = instance.GetComponent<ModelVisual>();
            if (visual == null)
            {
                visual = instance.AddComponent<ModelVisual>();
            }

            visual.Initialize();
            return visual;
        }
    }
}
