using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public enum DestructionState
    {
        Intact,
        Damaged,
        Critical,
        Destroyed
    }

    public sealed class DestructibleObject
    {
        public string ObjectId { get; }

        public float Integrity { get; private set; }

        public DestructionState State { get; private set; }

        public DestructibleObject(
            string objectId,
            float maximumIntegrity)
        {
            ObjectId =
                objectId ?? string.Empty;

            Integrity =
                Math.Max(0f, maximumIntegrity);

            State =
                Integrity > 0f
                    ? DestructionState.Intact
                    : DestructionState.Destroyed;
        }

        public bool ApplyDamage(
            float damage)
        {
            if (damage < 0f ||
                State == DestructionState.Destroyed)
            {
                return false;
            }

            Integrity =
                Math.Max(
                    0f,
                    Integrity - damage);

            UpdateState();

            return true;
        }

        public bool Repair(
            float amount,
            float maximumIntegrity)
        {
            if (amount < 0f ||
                maximumIntegrity <= 0f ||
                State == DestructionState.Destroyed)
            {
                return false;
            }

            Integrity =
                Math.Min(
                    maximumIntegrity,
                    Integrity + amount);

            UpdateState();

            return true;
        }

        private void UpdateState()
        {
            if (Integrity <= 0f)
            {
                State =
                    DestructionState.Destroyed;
            }
            else if (Integrity <= 25f)
            {
                State =
                    DestructionState.Critical;
            }
            else if (Integrity < 100f)
            {
                State =
                    DestructionState.Damaged;
            }
            else
            {
                State =
                    DestructionState.Intact;
            }
        }
    }

    public sealed class DestructionPropagation
    {
        private readonly Dictionary<
            string,
            DestructibleObject> objects =
            new Dictionary<
                string,
                DestructibleObject>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<
            string,
            HashSet<string>> propagationLinks =
            new Dictionary<
                string,
                HashSet<string>>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ObjectCount =>
            objects.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            objects.Clear();
            propagationLinks.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterObject(
            string objectId,
            float maximumIntegrity)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectId) ||
                maximumIntegrity <= 0f)
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
                new DestructibleObject(
                    id,
                    maximumIntegrity));

            propagationLinks.Add(
                id,
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase));

            return true;
        }

        public bool LinkPropagation(
            string sourceObjectId,
            string targetObjectId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(sourceObjectId) ||
                string.IsNullOrWhiteSpace(targetObjectId))
            {
                return false;
            }

            if (!objects.ContainsKey(sourceObjectId.Trim()) ||
                !objects.ContainsKey(targetObjectId.Trim()))
            {
                return false;
            }

            if (string.Equals(
                    sourceObjectId.Trim(),
                    targetObjectId.Trim(),
                    StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return propagationLinks[
                sourceObjectId.Trim()].Add(
                    targetObjectId.Trim());
        }

        public bool ApplyDamage(
            string objectId,
            float damage)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectId))
            {
                return false;
            }

            DestructibleObject source =
                GetObject(objectId);

            if (source == null)
            {
                return false;
            }

            return source.ApplyDamage(damage);
        }

        public bool PropagateDamage(
            string objectId,
            float damagePerTarget)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectId) ||
                damagePerTarget < 0f)
            {
                return false;
            }

            string sourceId =
                objectId.Trim();

            if (!propagationLinks.TryGetValue(
                    sourceId,
                    out HashSet<string> targets))
            {
                return false;
            }

            bool changed = false;

            foreach (string targetId in targets)
            {
                DestructibleObject target =
                    GetObject(targetId);

                if (target != null &&
                    target.ApplyDamage(
                        damagePerTarget))
                {
                    changed = true;
                }
            }

            return changed;
        }

        public DestructibleObject GetObject(
            string objectId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectId))
            {
                return null;
            }

            objects.TryGetValue(
                objectId.Trim(),
                out DestructibleObject destructibleObject);

            return destructibleObject;
        }

        public IReadOnlyCollection<
            DestructibleObject>
            GetObjects()
        {
            return objects.Values;
        }

        public void Reset()
        {
            objects.Clear();
            propagationLinks.Clear();

            Initialized = false;
        }
    }
}
