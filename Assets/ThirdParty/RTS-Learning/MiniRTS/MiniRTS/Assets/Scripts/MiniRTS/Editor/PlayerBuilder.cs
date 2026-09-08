using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace MiniRTS.EditorTools
{
    /// Batch-mode entry point for `unity build --execute-method
    /// MiniRTS.EditorTools.PlayerBuilder.PerformBuild`.
    public static class PlayerBuilder
    {
        // Shaders resolved via Shader.Find at runtime; without a serialized
        // reference they are stripped from player builds.
        static readonly string[] RuntimeShaders =
        {
            "Standard",
            "Universal Render Pipeline/Lit",
            "Universal Render Pipeline/Unlit",
            "Unlit/Color",
            "Unlit/Transparent"
        };

        public static void EnsureAlwaysIncludedShaders()
        {
            var graphicsSettings = AssetDatabase.LoadAllAssetsAtPath(
                "ProjectSettings/GraphicsSettings.asset")[0];
            var serialized = new SerializedObject(graphicsSettings);
            SerializedProperty list =
                serialized.FindProperty("m_AlwaysIncludedShaders");

            foreach (string shaderName in RuntimeShaders)
            {
                Shader shader = Shader.Find(shaderName);
                if (shader == null)
                {
                    continue;
                }

                bool present = false;
                for (int i = 0; i < list.arraySize; i++)
                {
                    if (list.GetArrayElementAtIndex(i).objectReferenceValue == shader)
                    {
                        present = true;
                        break;
                    }
                }

                if (!present)
                {
                    list.InsertArrayElementAtIndex(list.arraySize);
                    list.GetArrayElementAtIndex(list.arraySize - 1)
                        .objectReferenceValue = shader;
                }
            }

            serialized.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
        }

        public static void PerformBuild()
        {
            EnsureAlwaysIncludedShaders();
            string output = null;
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == "-buildOutput")
                {
                    output = args[i + 1];
                }
            }

            if (string.IsNullOrEmpty(output))
            {
                output = Path.Combine("Builds", "MiniRTS.app");
            }

            var options = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/Scenes/Main.unity" },
                locationPathName = output,
                target = BuildTarget.StandaloneOSX,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new Exception(
                    $"Build failed: {report.summary.result}, errors: {report.summary.totalErrors}");
            }

            Console.WriteLine($"BUILD_OK {report.summary.outputPath} " +
                $"({report.summary.totalSize / (1024 * 1024)} MB)");
        }
    }
}
