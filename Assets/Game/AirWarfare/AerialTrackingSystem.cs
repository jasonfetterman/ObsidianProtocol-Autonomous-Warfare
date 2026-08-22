using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public sealed class AerialTrack
    {
        public string TrackId { get; }

        public float PositionX { get; private set; }
        public float PositionY { get; private set; }
        public float PositionZ { get; private set; }

        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }
        public float VelocityZ { get; private set; }

        public float Confidence { get; private set; }

        public bool Active { get; private set; }

        public AerialTrack(string trackId)
        {
            TrackId = trackId ?? string.Empty;

            Confidence = 0f;
            Active = true;
        }

        public void Update(
            float positionX,
            float positionY,
            float positionZ,
            float velocityX,
            float velocityY,
            float velocityZ,
            float confidence)
        {
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;

            VelocityX = velocityX;
            VelocityY = velocityY;
            VelocityZ = velocityZ;

            Confidence =
                Math.Clamp(
                    confidence,
                    0f,
                    1f);

            Active = true;
        }

        public void ReduceConfidence(
            float amount)
        {
            Confidence =
                Math.Clamp(
                    Confidence - Math.Max(0f, amount),
                    0f,
                    1f);

            if (Confidence <= 0f)
            {
                Active = false;
            }
        }

        public void Deactivate()
        {
            Active = false;
        }
    }

    public sealed class AerialTrackingSystem
    {
        private readonly Dictionary<string, AerialTrack> tracks =
            new Dictionary<string, AerialTrack>(
                StringComparer.OrdinalIgnoreCase);

        public void CreateTrack(string trackId)
        {
            if (string.IsNullOrWhiteSpace(trackId))
            {
                return;
            }

            if (!tracks.ContainsKey(trackId))
            {
                tracks.Add(
                    trackId,
                    new AerialTrack(trackId));
            }
        }

        public void UpdateTrack(
            string trackId,
            float positionX,
            float positionY,
            float positionZ,
            float velocityX,
            float velocityY,
            float velocityZ,
            float confidence)
        {
            CreateTrack(trackId);

            tracks[trackId].Update(
                positionX,
                positionY,
                positionZ,
                velocityX,
                velocityY,
                velocityZ,
                confidence);
        }

        public void ReduceConfidence(
            string trackId,
            float amount)
        {
            if (tracks.TryGetValue(
                    trackId,
                    out AerialTrack track))
            {
                track.ReduceConfidence(amount);
            }
        }

        public bool TryGetTrack(
            string trackId,
            out AerialTrack track)
        {
            return tracks.TryGetValue(
                trackId,
                out track);
        }

        public void RemoveTrack(string trackId)
        {
            tracks.Remove(trackId);
        }

        public void Clear()
        {
            tracks.Clear();
        }
    }
}
