using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MiniRTS.EditorTools
{
    /// <summary>
    /// Keeps embedded Kenney FBX materials deterministic and URP-compatible.
    /// The version override reimports affected model assets when this conversion changes.
    /// </summary>
    public sealed class KenneyUrpModelPostprocessor : AssetPostprocessor
    {
        private const string ModelFolder = "Assets/Resources/Models";
        private const string ModelPrefix = ModelFolder + "/";
        private const uint PostprocessorVersion = 1;

        public override uint GetVersion()
        {
            return PostprocessorVersion;
        }

        private void OnPostprocessMaterial(Material material)
        {
            if (IsKenneyModel(assetPath))
            {
                ConvertToUrpLit(material);
            }
        }

        private void OnPostprocessModel(GameObject root)
        {
            if (!IsKenneyModel(assetPath))
            {
                return;
            }

            Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);
            HashSet<Material> converted = new HashSet<Material>();
            for (int rendererIndex = 0;
                 rendererIndex < renderers.Length;
                 rendererIndex++)
            {
                Material[] materials = renderers[rendererIndex].sharedMaterials;
                for (int materialIndex = 0;
                     materialIndex < materials.Length;
                     materialIndex++)
                {
                    Material material = materials[materialIndex];
                    if (material != null && converted.Add(material))
                    {
                        ConvertToUrpLit(material);
                    }
                }
            }
        }

        [MenuItem("Tools/MiniRTS/Reimport Kenney Models for URP")]
        public static void ReimportKenneyModelsForUrp()
        {
            string[] guids = AssetDatabase.FindAssets(
                "t:Model",
                new[] { ModelFolder });
            Array.Sort(guids, StringComparer.Ordinal);

            AssetDatabase.StartAssetEditing();
            try
            {
                for (int i = 0; i < guids.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                    if (IsKenneyModel(path))
                    {
                        AssetDatabase.ImportAsset(
                            path,
                            ImportAssetOptions.ForceUpdate);
                    }
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            AssetDatabase.SaveAssets();
        }

        private static bool IsKenneyModel(string path)
        {
            return path.StartsWith(ModelPrefix, StringComparison.Ordinal) &&
                   path.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase);
        }

        private static void ConvertToUrpLit(Material material)
        {
            Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
            if (material == null || urpLit == null)
            {
                return;
            }

            Color baseColor = ReadColor(material);
            Texture baseTexture = ReadTexture(material);
            Vector2 textureScale = ReadTextureScale(material);
            Vector2 textureOffset = ReadTextureOffset(material);
            float metallic = material.HasProperty("_Metallic")
                ? material.GetFloat("_Metallic")
                : 0f;
            float smoothness = material.HasProperty("_Smoothness")
                ? material.GetFloat("_Smoothness")
                : material.HasProperty("_Glossiness")
                    ? material.GetFloat("_Glossiness")
                    : 0.25f;

            material.shader = urpLit;
            material.SetColor("_BaseColor", baseColor);
            material.SetFloat("_Metallic", metallic);
            material.SetFloat("_Smoothness", smoothness);
            if (baseTexture != null)
            {
                material.SetTexture("_BaseMap", baseTexture);
                material.SetTextureScale("_BaseMap", textureScale);
                material.SetTextureOffset("_BaseMap", textureOffset);
            }
        }

        private static Color ReadColor(Material material)
        {
            if (material.HasProperty("_BaseColor"))
            {
                return material.GetColor("_BaseColor");
            }

            return material.HasProperty("_Color")
                ? material.GetColor("_Color")
                : Color.white;
        }

        private static Texture ReadTexture(Material material)
        {
            if (material.HasProperty("_BaseMap"))
            {
                Texture texture = material.GetTexture("_BaseMap");
                if (texture != null)
                {
                    return texture;
                }
            }

            return material.HasProperty("_MainTex")
                ? material.GetTexture("_MainTex")
                : null;
        }

        private static Vector2 ReadTextureScale(Material material)
        {
            if (material.HasProperty("_BaseMap"))
            {
                return material.GetTextureScale("_BaseMap");
            }

            return material.HasProperty("_MainTex")
                ? material.GetTextureScale("_MainTex")
                : Vector2.one;
        }

        private static Vector2 ReadTextureOffset(Material material)
        {
            if (material.HasProperty("_BaseMap"))
            {
                return material.GetTextureOffset("_BaseMap");
            }

            return material.HasProperty("_MainTex")
                ? material.GetTextureOffset("_MainTex")
                : Vector2.zero;
        }
    }
}
