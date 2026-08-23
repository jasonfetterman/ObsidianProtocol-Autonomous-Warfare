using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public enum StreamingState
    {
        Unloaded,
        Loading,
        Loaded,
        Unloading
    }

    public sealed class WorldStreamRegion
    {
        public string RegionId { get; }

        public float LoadDistance { get; private set; }

        public float UnloadDistance { get; private set; }

        public float CurrentDistance { get; private set; }

        public StreamingState State { get; private set; }

        public WorldStreamRegion(
            string regionId,
            float loadDistance,
            float unloadDistance)
        {
            RegionId =
                regionId ?? string.Empty;

            LoadDistance =
                Math.Max(0f, loadDistance);

            UnloadDistance =
                Math.Max(
                    LoadDistance,
                    unloadDistance);

            CurrentDistance = float.MaxValue;

            State =
                StreamingState.Unloaded;
        }

        public bool SetDistances(
            float loadDistance,
            float unloadDistance)
        {
            if (loadDistance < 0f ||
                unloadDistance < loadDistance)
            {
                return false;
            }

            LoadDistance =
                loadDistance;

            UnloadDistance =
                unloadDistance;

            return true;
        }

        public void UpdateDistance(
            float distance)
        {
            CurrentDistance =
                Math.Max(
                    0f,
                    distance);

            if (State ==
                StreamingState.Unloaded &&
                CurrentDistance <= LoadDistance)
            {
                State =
                    StreamingState.Loading;

                return;
            }

            if (State ==
                StreamingState.Loaded &&
                CurrentDistance >= UnloadDistance)
            {
                State =
                    StreamingState.Unloading;
            }
        }

        public bool CompleteLoad()
        {
            if (State != StreamingState.Loading)
            {
                return false;
            }

            State =
                StreamingState.Loaded;

            return true;
        }

        public bool CompleteUnload()
        {
            if (State != StreamingState.Unloading)
            {
                return false;
            }

            State =
                StreamingState.Unloaded;

            return true;
        }
    }

    public sealed class WorldStreaming
    {
        private readonly Dictionary<
            string,
            WorldStreamRegion> regions =
            new Dictionary<
                string,
                WorldStreamRegion>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int RegionCount =>
            regions.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            regions.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterRegion(
            string regionId,
            float loadDistance,
            float unloadDistance)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId) ||
                loadDistance < 0f ||
                unloadDistance < loadDistance)
            {
                return false;
            }

            string id =
                regionId.Trim();

            if (regions.ContainsKey(id))
            {
                return false;
            }

            regions.Add(
                id,
                new WorldStreamRegion(
                    id,
                    loadDistance,
                    unloadDistance));

            return true;
        }

        public bool RemoveRegion(
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return false;
            }

            return regions.Remove(
                regionId.Trim());
        }

        public bool UpdateRegionDistance(
            string regionId,
            float distance)
        {
            WorldStreamRegion region =
                GetRegion(regionId);

            if (region == null)
            {
                return false;
            }

            region.UpdateDistance(distance);

            return true;
        }

        public bool CompleteLoad(
            string regionId)
        {
            WorldStreamRegion region =
                GetRegion(regionId);

            return region != null &&
                   region.CompleteLoad();
        }

        public bool CompleteUnload(
            string regionId)
        {
            WorldStreamRegion region =
                GetRegion(regionId);

            return region != null &&
                   region.CompleteUnload();
        }

        public WorldStreamRegion GetRegion(
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return null;
            }

            regions.TryGetValue(
                regionId.Trim(),
                out WorldStreamRegion region);

            return region;
        }

        public IReadOnlyCollection<
            WorldStreamRegion>
            GetRegions()
        {
            return regions.Values;
        }

        public void Reset()
        {
            regions.Clear();

            Initialized = false;
        }
    }
}
