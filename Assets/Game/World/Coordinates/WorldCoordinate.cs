using System;
using UnityEngine;

namespace ObsidianProtocol.Game.World.Coordinates
{
    [Serializable]
    public readonly struct WorldCoordinate
    {
        public Vector3 Position { get; }

        public WorldCoordinate(Vector3 position)
        {
            Position = position;
        }

        public float X => Position.x;
        public float Y => Position.y;
        public float Z => Position.z;

        public static implicit operator Vector3(WorldCoordinate coordinate)
        {
            return coordinate.Position;
        }

        public static implicit operator WorldCoordinate(Vector3 position)
        {
            return new WorldCoordinate(position);
        }
    }
}
