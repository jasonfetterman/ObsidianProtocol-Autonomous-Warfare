using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniRTS.EditorTools
{
    /// <summary>
    /// Creates the one-object bootstrap scene. This method is also suitable for
    /// -executeMethod MiniRTS.EditorTools.SceneGenerator.Generate.
    /// </summary>
    public static class SceneGenerator
    {
        public const string MainScenePath = "Assets/Scenes/Main.unity";
        private const string TemplateScenePath = "Assets/Scenes/SampleScene.unity";
        private const string TextureRoot = "Assets/Textures";

        [MenuItem("MiniRTS/Regenerate Main Scene")]
        public static void Generate()
        {
            EnsureFolder("Assets", "Scenes");

            Scene scene = EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene,
                NewSceneMode.Single);
            GameObject game = new GameObject("Game");
            GameBootstrap bootstrap = game.AddComponent<GameBootstrap>();
            bootstrap.ConfigureMaterialTextures(
                LoadTexture("TerrainGrass.png"),
                LoadTexture("TerrainDirt.png"),
                LoadTexture("Rock.png"),
                LoadTexture("BlueCrystal.png"),
                LoadTexture("MetalBuildingPanel.png"));
            EditorSceneManager.MarkSceneDirty(scene);

            if (!EditorSceneManager.SaveScene(scene, MainScenePath))
            {
                throw new InvalidOperationException(
                    $"Failed to save MiniRTS bootstrap scene at {MainScenePath}.");
            }

            RemoveTemplateScene();
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(MainScenePath, true)
            };
            AssetDatabase.SaveAssets();
            Selection.activeGameObject = game;
            Debug.Log($"Generated {MainScenePath} and registered it in Build Settings.");
        }

        private static void RemoveTemplateScene()
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(TemplateScenePath) != null &&
                !AssetDatabase.DeleteAsset(TemplateScenePath))
            {
                Debug.LogWarning(
                    $"Could not remove the unused template scene at {TemplateScenePath}.");
            }
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = $"{parent}/{child}";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, child);
            }
        }

        private static Texture2D LoadTexture(string fileName)
        {
            string path = $"{TextureRoot}/{fileName}";
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                throw new InvalidOperationException(
                    $"Required MiniRTS texture is missing at {path}.");
            }

            return texture;
        }
    }
}
