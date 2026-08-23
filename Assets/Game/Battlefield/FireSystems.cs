using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public enum FireState
    {
        Extinguished,
        Ignited,
        Burning,
        Spreading,
        ExtinguishedAfterBurn
    }

    public sealed class BattlefieldFire
    {
        private readonly HashSet<string> affectedObjects =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public string FireId { get; }

        public FireState State { get; private set; }

        public float Intensity { get; private set; }

        public float SpreadRate { get; private set; }

        public int AffectedObjectCount =>
            affectedObjects.Count;

        public BattlefieldFire(
            string fireId,
            float intensity,
            float spreadRate)
        {
            FireId =
                fireId ?? string.Empty;

            Intensity =
                Math.Max(0f, intensity);

            SpreadRate =
                Math.Max(0f, spreadRate);

            State =
                Intensity > 0f
                    ? FireState.Ignited
                    : FireState.Extinguished;
        }

        public bool Ignite()
        {
            if (Intensity <= 0f ||
                State != FireState.Extinguished)
            {
                return false;
            }

            State =
                FireState.Ignited;

            return true;
        }

        public bool StartBurning()
        {
            if (State != FireState.Ignited)
            {
                return false;
            }

            State =
                FireState.Burning;

            return true;
        }

        public bool StartSpreading()
        {
            if (State != FireState.Burning ||
                SpreadRate <= 0f)
            {
                return false;
            }

            State =
                FireState.Spreading;

            return true;
        }

        public bool Extinguish()
        {
            if (State == FireState.Extinguished ||
                State == FireState.ExtinguishedAfterBurn)
            {
                return false;
            }

            State =
                FireState.ExtinguishedAfterBurn;

            Intensity = 0f;

            return true;
        }

        public bool RegisterAffectedObject(
            string objectId)
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                return false;
            }

            return affectedObjects.Add(
                objectId.Trim());
        }

        public bool RemoveAffectedObject(
            string objectId)
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                return false;
            }

            return affectedObjects.Remove(
                objectId.Trim());
        }

        public IReadOnlyCollection<string>
            GetAffectedObjects()
        {
            return affectedObjects;
        }
    }

    public sealed class FireSystems
    {
        private readonly Dictionary<
            string,
            BattlefieldFire> fires =
            new Dictionary<
                string,
                BattlefieldFire>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int FireCount =>
            fires.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            fires.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterFire(
            string fireId,
            float intensity,
            float spreadRate)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(fireId) ||
                intensity < 0f ||
                spreadRate < 0f)
            {
                return false;
            }

            string id =
                fireId.Trim();

            if (fires.ContainsKey(id))
            {
                return false;
            }

            fires.Add(
                id,
                new BattlefieldFire(
                    id,
                    intensity,
                    spreadRate));

            return true;
        }

        public bool IgniteFire(
            string fireId)
        {
            BattlefieldFire fire =
                GetFire(fireId);

            return fire != null &&
                   fire.Ignite();
        }

        public bool StartBurning(
            string fireId)
        {
            BattlefieldFire fire =
                GetFire(fireId);

            return fire != null &&
                   fire.StartBurning();
        }

        public bool StartSpreading(
            string fireId)
        {
            BattlefieldFire fire =
                GetFire(fireId);

            return fire != null &&
                   fire.StartSpreading();
        }

        public bool ExtinguishFire(
            string fireId)
        {
            BattlefieldFire fire =
                GetFire(fireId);

            return fire != null &&
                   fire.Extinguish();
        }

        public bool RegisterAffectedObject(
            string fireId,
            string objectId)
        {
            BattlefieldFire fire =
                GetFire(fireId);

            return fire != null &&
                   fire.RegisterAffectedObject(
                       objectId);
        }

        public BattlefieldFire GetFire(
            string fireId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(fireId))
            {
                return null;
            }

            fires.TryGetValue(
                fireId.Trim(),
                out BattlefieldFire fire);

            return fire;
        }

        public IReadOnlyCollection<BattlefieldFire>
            GetFires()
        {
            return fires.Values;
        }

        public void Reset()
        {
            fires.Clear();

            Initialized = false;
        }
    }
}
