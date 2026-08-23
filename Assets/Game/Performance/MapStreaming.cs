using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public enum MapStreamingState
    {
        Unloaded,
        Loading,
        Loaded,
        Unloading
    }

    public sealed class MapStreamChunk
    {
        public string ChunkId { get; }

        public int Priority { get; private set; }

        public float LoadDistance { get; private set; }

        public float UnloadDistance { get; private set; }

        public float CurrentDistance { get; private set; }

        public MapStreamingState State { get; private set; }

        public MapStreamChunk(
            string chunkId,
            int priority,
            float loadDistance,
            float unloadDistance)
        {
            ChunkId =
                chunkId ?? string.Empty;

            Priority =
                priority;

            LoadDistance =
                Math.Max(0f, loadDistance);

            UnloadDistance =
                Math.Max(
                    LoadDistance,
                    unloadDistance);

            CurrentDistance =
                float.MaxValue;

            State =
                MapStreamingState.Unloaded;
        }

        public bool SetPriority(
            int priority)
        {
            Priority =
                priority;

            return true;
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
                MapStreamingState.Unloaded &&
                CurrentDistance <= LoadDistance)
            {
                State =
                    MapStreamingState.Loading;

                return;
            }

            if (State ==
                MapStreamingState.Loaded &&
                CurrentDistance >= UnloadDistance)
            {
                State =
                    MapStreamingState.Unloading;
            }
        }

        public bool CompleteLoad()
        {
            if (State !=
                MapStreamingState.Loading)
            {
                return false;
            }

            State =
                MapStreamingState.Loaded;

            return true;
        }

        public bool CompleteUnload()
        {
            if (State !=
                MapStreamingState.Unloading)
            {
                return false;
            }

            State =
                MapStreamingState.Unloaded;

            return true;
        }
    }

    public sealed class MapStreaming
    {
        private readonly Dictionary<
            string,
            MapStreamChunk> chunks =
            new Dictionary<
                string,
                MapStreamChunk>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ChunkCount =>
            chunks.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            chunks.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterChunk(
            string chunkId,
            int priority,
            float loadDistance,
            float unloadDistance)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(chunkId) ||
                loadDistance < 0f ||
                unloadDistance < loadDistance)
            {
                return false;
            }

            string id =
                chunkId.Trim();

            if (chunks.ContainsKey(id))
            {
                return false;
            }

            chunks.Add(
                id,
                new MapStreamChunk(
                    id,
                    priority,
                    loadDistance,
                    unloadDistance));

            return true;
        }

        public bool RemoveChunk(
            string chunkId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(chunkId))
            {
                return false;
            }

            return chunks.Remove(
                chunkId.Trim());
        }

        public bool SetPriority(
            string chunkId,
            int priority)
        {
            MapStreamChunk chunk =
                GetChunk(chunkId);

            return chunk != null &&
                   chunk.SetPriority(priority);
        }

        public bool UpdateChunkDistance(
            string chunkId,
            float distance)
        {
            MapStreamChunk chunk =
                GetChunk(chunkId);

            if (chunk == null)
            {
                return false;
            }

            chunk.UpdateDistance(distance);

            return true;
        }

        public bool CompleteLoad(
            string chunkId)
        {
            MapStreamChunk chunk =
                GetChunk(chunkId);

            return chunk != null &&
                   chunk.CompleteLoad();
        }

        public bool CompleteUnload(
            string chunkId)
        {
            MapStreamChunk chunk =
                GetChunk(chunkId);

            return chunk != null &&
                   chunk.CompleteUnload();
        }

        public MapStreamChunk GetChunk(
            string chunkId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(chunkId))
            {
                return null;
            }

            chunks.TryGetValue(
                chunkId.Trim(),
                out MapStreamChunk chunk);

            return chunk;
        }

        public IReadOnlyCollection<
            MapStreamChunk>
            GetChunks()
        {
            return chunks.Values;
        }

        public void Reset()
        {
            chunks.Clear();

            Initialized = false;
        }
    }
}
