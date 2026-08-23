using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VR
{
    public enum VRFacilityArea
    {
        Entrance,
        CommandCenter,
        MainHangar,
        AirOperations,
        GroundVehicleBay,
        NavalFacility,
        ExperimentalContainment,
        Maintenance,
        Customization,
        WeaponsEquipment,
        Upgrade,
        AICoreLab,
        Fabrication,
        Salvage,
        Storage,
        Deployment,
        Garage,
        Store
    }

    public sealed class VRFacilityNavigation
    {
        private readonly HashSet<VRFacilityArea>
            accessibleAreas =
            new HashSet<VRFacilityArea>();

        public bool Initialized { get; private set; }

        public VRFacilityArea CurrentArea { get; private set; }

        public int AccessibleAreaCount =>
            accessibleAreas.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            accessibleAreas.Clear();

            accessibleAreas.Add(
                VRFacilityArea.Entrance);

            CurrentArea =
                VRFacilityArea.Entrance;

            Initialized = true;

            return true;
        }

        public bool RegisterArea(
            VRFacilityArea area)
        {
            if (!Initialized)
            {
                return false;
            }

            return accessibleAreas.Add(area);
        }

        public bool RemoveArea(
            VRFacilityArea area)
        {
            if (!Initialized ||
                area == CurrentArea ||
                area == VRFacilityArea.Entrance)
            {
                return false;
            }

            return accessibleAreas.Remove(area);
        }

        public bool CanAccess(
            VRFacilityArea area)
        {
            return Initialized &&
                   accessibleAreas.Contains(area);
        }

        public bool NavigateTo(
            VRFacilityArea area)
        {
            if (!CanAccess(area))
            {
                return false;
            }

            CurrentArea = area;

            return true;
        }

        public bool ReturnToEntrance()
        {
            if (!Initialized ||
                !accessibleAreas.Contains(
                    VRFacilityArea.Entrance))
            {
                return false;
            }

            CurrentArea =
                VRFacilityArea.Entrance;

            return true;
        }

        public IReadOnlyCollection<VRFacilityArea>
            GetAccessibleAreas()
        {
            return accessibleAreas;
        }

        public void Reset()
        {
            accessibleAreas.Clear();

            Initialized = false;

            CurrentArea =
                VRFacilityArea.Entrance;
        }
    }
}
