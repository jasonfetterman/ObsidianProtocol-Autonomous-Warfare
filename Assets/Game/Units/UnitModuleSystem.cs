using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public enum UnitModuleType
    {
        Armor,
        Mobility,
        Sensor,
        Weapon,
        Communication,
        Power,
        Command,
        Logistics,
        Repair,
        Utility
    }

    public sealed class UnitModule
    {
        public string ModuleId { get; }
        public UnitModuleType Type { get; }
        public bool Installed { get; private set; }
        public bool Operational { get; private set; }

        public UnitModule(
            string moduleId,
            UnitModuleType type)
        {
            ModuleId = moduleId ?? string.Empty;
            Type = type;
            Installed = false;
            Operational = false;
        }

        public void Install()
        {
            Installed = true;
            Operational = true;
        }

        public void Uninstall()
        {
            Installed = false;
            Operational = false;
        }

        public void SetOperational(bool operational)
        {
            Operational =
                Installed && operational;
        }
    }

    public sealed class UnitModuleSystem
    {
        private readonly Dictionary<string, Dictionary<string, UnitModule>> modules =
            new Dictionary<string, Dictionary<string, UnitModule>>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!modules.ContainsKey(unitId))
            {
                modules.Add(
                    unitId,
                    new Dictionary<string, UnitModule>(
                        StringComparer.OrdinalIgnoreCase));
            }
        }

        public void InstallModule(
            string unitId,
            string moduleId,
            UnitModuleType type)
        {
            if (string.IsNullOrWhiteSpace(unitId) ||
                string.IsNullOrWhiteSpace(moduleId))
            {
                return;
            }

            RegisterUnit(unitId);

            UnitModule module =
                new UnitModule(
                    moduleId,
                    type);

            module.Install();

            modules[unitId][moduleId] = module;
        }

        public void UninstallModule(
            string unitId,
            string moduleId)
        {
            if (modules.TryGetValue(
                    unitId,
                    out Dictionary<string, UnitModule> unitModules))
            {
                unitModules.Remove(moduleId);
            }
        }

        public bool HasModule(
            string unitId,
            string moduleId)
        {
            return modules.TryGetValue(
                       unitId,
                       out Dictionary<string, UnitModule> unitModules) &&
                   unitModules.ContainsKey(moduleId);
        }

        public bool TryGetModule(
            string unitId,
            string moduleId,
            out UnitModule module)
        {
            module = null;

            return modules.TryGetValue(
                       unitId,
                       out Dictionary<string, UnitModule> unitModules) &&
                   unitModules.TryGetValue(
                       moduleId,
                       out module);
        }

        public void RemoveUnit(string unitId)
        {
            modules.Remove(unitId);
        }

        public void Clear()
        {
            modules.Clear();
        }
    }
}
