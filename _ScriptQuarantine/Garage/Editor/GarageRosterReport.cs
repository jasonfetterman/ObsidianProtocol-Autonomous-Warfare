using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace ObsidianProtocol.Garage.Editor
{
    public static class GarageRosterReport
    {
        private const string OutputPath =
            "Assets/Scripts/Garage/Definitions/GarageUnitRoster.txt";

        [MenuItem("Obsidian Protocol/Garage/Generate Unit Roster")]
        public static void Generate()
        {
            string[] guids = AssetDatabase.FindAssets(
                "t:UnitDefinition",
                new[]
                {
                    "Assets/Scripts/Garage/Definitions"
                });

            List<UnitDefinition> units =
                new List<UnitDefinition>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                UnitDefinition unit =
                    AssetDatabase.LoadAssetAtPath<UnitDefinition>(path);

                if (unit != null)
                    units.Add(unit);
            }

            units.Sort((a, b) =>
            {
                string aId = a.identity != null
                    ? a.identity.unitId
                    : "";

                string bId = b.identity != null
                    ? b.identity.unitId
                    : "";

                return string.Compare(
                    aId,
                    bId,
                    System.StringComparison.OrdinalIgnoreCase);
            });

            using (StreamWriter writer =
                   new StreamWriter(OutputPath, false))
            {
                writer.WriteLine("OBSIDIAN PROTOCOL");
                writer.WriteLine("GARAGE MASTER UNIT ROSTER");
                writer.WriteLine("==========================");
                writer.WriteLine($"TOTAL UNITS: {units.Count}");
                writer.WriteLine("");

                foreach (UnitDefinition unit in units)
                {
                    if (unit.identity == null)
                    {
                        writer.WriteLine("[INVALID] " + unit.name);
                        continue;
                    }

                    writer.WriteLine(
                        $"{unit.identity.unitId} | " +
                        $"{unit.identity.displayName}");
                }
            }

            AssetDatabase.Refresh();

            Debug.Log(
                $"Garage roster generated successfully. " +
                $"Units: {units.Count}");
        }
    }
}
