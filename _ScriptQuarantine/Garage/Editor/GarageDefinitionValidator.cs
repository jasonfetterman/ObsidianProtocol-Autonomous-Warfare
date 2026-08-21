using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Garage.Editor
{
    public static class GarageDefinitionValidator
    {
        [MenuItem("Obsidian Protocol/Garage/Validate Unit Definitions")]
        public static void Validate()
        {
            string[] guids = AssetDatabase.FindAssets(
                "t:UnitDefinition",
                new[]
                {
                    "Assets/Scripts/Garage/Definitions"
                });

            int valid = 0;
            int invalid = 0;

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                UnitDefinition unit =
                    AssetDatabase.LoadAssetAtPath<UnitDefinition>(path);

                if (unit == null || unit.identity == null)
                {
                    Debug.LogError(
                        $"INVALID UNIT DEFINITION: {path}"
                    );

                    invalid++;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(unit.identity.unitId))
                {
                    Debug.LogError(
                        $"MISSING UNIT ID: {path}"
                    );

                    invalid++;
                    continue;
                }

                valid++;
            }

            Debug.Log(
                $"GARAGE VALIDATION COMPLETE — " +
                $"Valid: {valid} | Invalid: {invalid} | Total: {guids.Length}"
            );
        }
    }
}
