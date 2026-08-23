using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VR
{
    public enum VRGarageArea
    {
        Entrance,
        UnitDisplay,
        Customization,
        Maintenance,
        Upgrade,
        Deployment
    }

    public sealed class VRGarage
    {
        private readonly HashSet<string> displayedUnits =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public VRGarageArea CurrentArea { get; private set; }

        public int DisplayedUnitCount =>
            displayedUnits.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            displayedUnits.Clear();

            CurrentArea =
                VRGarageArea.Entrance;

            Initialized = true;

            return true;
        }

        public bool EnterArea(
            VRGarageArea area)
        {
            if (!Initialized)
            {
                return false;
            }

            CurrentArea = area;

            return true;
        }

        public bool RegisterDisplayedUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return displayedUnits.Add(
                unitId.Trim());
        }

        public bool RemoveDisplayedUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return displayedUnits.Remove(
                unitId.Trim());
        }

        public bool IsUnitDisplayed(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return displayedUnits.Contains(
                unitId.Trim());
        }

        public IReadOnlyCollection<string>
            GetDisplayedUnits()
        {
            return displayedUnits;
        }

        public void Reset()
        {
            displayedUnits.Clear();

            Initialized = false;
            CurrentArea =
                VRGarageArea.Entrance;
        }
    }
}
