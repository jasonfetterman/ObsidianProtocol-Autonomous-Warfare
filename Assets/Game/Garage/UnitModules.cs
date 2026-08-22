using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public sealed class UnitModule
    {
        public string ModuleId { get; }
        public string ModuleType { get; }

        public int Level { get; private set; }

        public bool Installed { get; private set; }
        public bool Enabled { get; private set; }

        public UnitModule(
            string moduleId,
            string moduleType,
            int level)
        {
            ModuleId =
                moduleId ?? string.Empty;

            ModuleType =
                moduleType ?? string.Empty;

            Level =
                Math.Max(1, level);

            Installed = false;
            Enabled = false;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(ModuleId);

        public void Install()
        {
            Installed = true;
            Enabled = true;
        }

        public void Uninstall()
        {
            Installed = false;
            Enabled = false;
        }

        public void Enable()
        {
            if (Installed)
                Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void SetLevel(int level)
        {
            Level = Math.Max(1, level);
        }
    }

    public sealed class UnitModuleRegistry
    {
        private readonly Dictionary<
            string,
            List<UnitModule>> modules =
            new Dictionary<
                string,
                List<UnitModule>>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterUnit(
            string ownershipId)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId) ||
                modules.ContainsKey(ownershipId))
            {
                return false;
            }

            modules.Add(
                ownershipId,
                new List<UnitModule>());

            return true;
        }

        public bool RemoveUnit(
            string ownershipId)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId))
            {
                return false;
            }

            return modules.Remove(ownershipId);
        }

        public bool AddModule(
            string ownershipId,
            UnitModule module)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId) ||
                module == null ||
                !module.Valid ||
                !modules.TryGetValue(
                    ownershipId,
                    out List<UnitModule> unitModules))
            {
                return false;
            }

            foreach (UnitModule existing in unitModules)
            {
                if (string.Equals(
                        existing.ModuleId,
                        module.ModuleId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            unitModules.Add(module);
            return true;
        }

        public bool RemoveModule(
            string ownershipId,
            string moduleId)
        {
            if (!modules.TryGetValue(
                    ownershipId,
                    out List<UnitModule> unitModules) ||
                string.IsNullOrWhiteSpace(moduleId))
            {
                return false;
            }

            for (int i = 0; i < unitModules.Count; i++)
            {
                if (string.Equals(
                        unitModules[i].ModuleId,
                        moduleId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    unitModules.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public IReadOnlyList<UnitModule>
            GetModules(string ownershipId)
        {
            if (!modules.TryGetValue(
                    ownershipId,
                    out List<UnitModule> unitModules))
            {
                return Array.Empty<UnitModule>();
            }

            return unitModules;
        }

        public void Clear()
        {
            modules.Clear();
        }
    }
}
