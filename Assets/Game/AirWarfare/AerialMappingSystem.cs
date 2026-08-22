using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public sealed class AerialMapCell
    {
        public int X { get; }
        public int Y { get; }

        public float Elevation { get; private set; }
        public float Confidence { get; private set; }

        public bool Explored { get; private set; }

        public AerialMapCell(
            int x,
            int y)
        {
            X = x;
            Y = y;

            Elevation = 0f;
            Confidence = 0f;
            Explored = false;
        }

        public void Update(
            float elevation,
            float confidence)
        {
            Elevation = elevation;

            Confidence =
                Math.Clamp(
                    confidence,
                    0f,
                    1f);

            Explored = true;
        }

        public void Clear()
        {
            Elevation = 0f;
            Confidence = 0f;
            Explored = false;
        }
    }

    public sealed class AerialMappingSystem
    {
        private readonly Dictionary<string, AerialMapCell> cells =
            new Dictionary<string, AerialMapCell>(
                StringComparer.Ordinal);

        public void UpdateCell(
            int x,
            int y,
            float elevation,
            float confidence)
        {
            string key =
                CreateKey(x, y);

            if (!cells.TryGetValue(
                    key,
                    out AerialMapCell cell))
            {
                cell =
                    new AerialMapCell(x, y);

                cells.Add(key, cell);
            }

            cell.Update(
                elevation,
                confidence);
        }

        public bool TryGetCell(
            int x,
            int y,
            out AerialMapCell cell)
        {
            return cells.TryGetValue(
                CreateKey(x, y),
                out cell);
        }

        public bool IsMapped(
            int x,
            int y)
        {
            return cells.TryGetValue(
                       CreateKey(x, y),
                       out AerialMapCell cell) &&
                   cell.Explored;
        }

        public void ClearCell(
            int x,
            int y)
        {
            cells.Remove(
                CreateKey(x, y));
        }

        public void Clear()
        {
            cells.Clear();
        }

        private string CreateKey(
            int x,
            int y)
        {
            return x + ":" + y;
        }
    }
}
