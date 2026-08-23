using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public enum CullingState
    {
        Visible,
        Culled
    }

    public sealed class CullingObject
    {
        public string ObjectId { get; }

        public float CullingDistance { get; private set; }

        public float CurrentDistance { get; private set; }

        public CullingState State { get; private set; }

        public bool IsVisible =>
            State == CullingState.Visible;

        public CullingObject(
            string objectId,
            float cullingDistance)
        {
            ObjectId =
                objectId ?? string.Empty;

            CullingDistance =
                Math.Max(
                    0f,
                    cullingDistance);

            CurrentDistance = 0f;

            State =
                CullingState.Visible;
        }

        public bool SetCullingDistance(
            float distance)
        {
            if (distance < 0f)
            {
                return false;
            }

            CullingDistance =
                distance;

            Update(CurrentDistance);

            return true;
        }

        public void Update(
            float distance)
        {
            CurrentDistance =
                Math.Max(
                    0f,
                    distance);

            State =
                CurrentDistance <= CullingDistance
                    ? CullingState.Visible
                    : CullingState.Culled;
        }
    }

    public sealed class Culling
    {
        private readonly Dictionary<
            string,
            CullingObject> objects =
            new Dictionary<
                string,
                CullingObject>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ObjectCount =>
            objects.Count;

        public int VisibleObjectCount
        {
            get
            {
                int count = 0;

                foreach (CullingObject objectData
                         in objects.Values)
                {
                    if (objectData.IsVisible)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            objects.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterObject(
            string objectId,
            float cullingDistance)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectId) ||
                cullingDistance < 0f)
            {
                return false;
            }

            string id =
                objectId.Trim();

            if (objects.ContainsKey(id))
            {
                return false;
            }

            objects.Add(
                id,
                new CullingObject(
                    id,
                    cullingDistance));

            return true;
        }

        public bool RemoveObject(
            string objectId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectId))
            {
                return false;
            }

            return objects.Remove(
                objectId.Trim());
        }

        public bool SetCullingDistance(
            string objectId,
            float distance)
        {
            CullingObject objectData =
                GetObject(objectId);

            return objectData != null &&
                   objectData.SetCullingDistance(
                       distance);
        }

        public bool UpdateObjectDistance(
            string objectId,
            float distance)
        {
            CullingObject objectData =
                GetObject(objectId);

            if (objectData == null)
            {
                return false;
            }

            objectData.Update(distance);

            return true;
        }

        public CullingObject GetObject(
            string objectId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectId))
            {
                return null;
            }

            objects.TryGetValue(
                objectId.Trim(),
                out CullingObject objectData);

            return objectData;
        }

        public IReadOnlyCollection<CullingObject>
            GetObjects()
        {
            return objects.Values;
        }

        public void Reset()
        {
            objects.Clear();

            Initialized = false;
        }
    }
}
