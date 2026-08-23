using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public enum SmokeState
    {
        Clear,
        Active,
        Dissipating,
        Dispersed
    }

    public sealed class BattlefieldSmoke
    {
        public string SmokeId { get; }

        public SmokeState State { get; private set; }

        public float Density { get; private set; }

        public float Radius { get; private set; }

        public float Duration { get; private set; }

        public BattlefieldSmoke(
            string smokeId,
            float density,
            float radius,
            float duration)
        {
            SmokeId =
                smokeId ?? string.Empty;

            Density =
                Math.Max(0f, density);

            Radius =
                Math.Max(0f, radius);

            Duration =
                Math.Max(0f, duration);

            State =
                SmokeState.Clear;
        }

        public bool Deploy()
        {
            if (State != SmokeState.Clear ||
                Density <= 0f ||
                Radius <= 0f ||
                Duration <= 0f)
            {
                return false;
            }

            State =
                SmokeState.Active;

            return true;
        }

        public bool BeginDissipation()
        {
            if (State != SmokeState.Active)
            {
                return false;
            }

            State =
                SmokeState.Dissipating;

            return true;
        }

        public bool Disperse()
        {
            if (State == SmokeState.Dispersed ||
                State == SmokeState.Clear)
            {
                return false;
            }

            Density = 0f;
            Radius = 0f;
            Duration = 0f;

            State =
                SmokeState.Dispersed;

            return true;
        }

        public void Update(
            float deltaTime)
        {
            if (deltaTime <= 0f ||
                State == SmokeState.Clear ||
                State == SmokeState.Dispersed)
            {
                return;
            }

            Duration =
                Math.Max(
                    0f,
                    Duration - deltaTime);

            if (Duration <= 0f)
            {
                Disperse();
            }
            else if (State == SmokeState.Dissipating)
            {
                Density =
                    Math.Max(
                        0f,
                        Density - deltaTime);
            }
        }
    }

    public sealed class SmokeSystems
    {
        private readonly Dictionary<
            string,
            BattlefieldSmoke> smoke =
            new Dictionary<
                string,
                BattlefieldSmoke>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int SmokeCount =>
            smoke.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            smoke.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterSmoke(
            string smokeId,
            float density,
            float radius,
            float duration)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(smokeId) ||
                density <= 0f ||
                radius <= 0f ||
                duration <= 0f)
            {
                return false;
            }

            string id =
                smokeId.Trim();

            if (smoke.ContainsKey(id))
            {
                return false;
            }

            smoke.Add(
                id,
                new BattlefieldSmoke(
                    id,
                    density,
                    radius,
                    duration));

            return true;
        }

        public bool DeploySmoke(
            string smokeId)
        {
            BattlefieldSmoke effect =
                GetSmoke(smokeId);

            return effect != null &&
                   effect.Deploy();
        }

        public bool BeginDissipation(
            string smokeId)
        {
            BattlefieldSmoke effect =
                GetSmoke(smokeId);

            return effect != null &&
                   effect.BeginDissipation();
        }

        public bool DisperseSmoke(
            string smokeId)
        {
            BattlefieldSmoke effect =
                GetSmoke(smokeId);

            return effect != null &&
                   effect.Disperse();
        }

        public void Update(
            float deltaTime)
        {
            if (!Initialized)
            {
                return;
            }

            foreach (BattlefieldSmoke effect
                     in smoke.Values)
            {
                effect.Update(deltaTime);
            }
        }

        public BattlefieldSmoke GetSmoke(
            string smokeId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(smokeId))
            {
                return null;
            }

            smoke.TryGetValue(
                smokeId.Trim(),
                out BattlefieldSmoke effect);

            return effect;
        }

        public IReadOnlyCollection<BattlefieldSmoke>
            GetSmokeEffects()
        {
            return smoke.Values;
        }

        public void Reset()
        {
            smoke.Clear();

            Initialized = false;
        }
    }
}
