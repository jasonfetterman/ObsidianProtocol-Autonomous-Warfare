using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.FormationMovement
{
    public sealed class FormationMovementService : MonoBehaviour
    {
        [SerializeField] private float spacing = 4f;
        [SerializeField] private int columns = 3;

        public float Spacing => Mathf.Max(0.1f, spacing);
        public int Columns => Mathf.Max(1, columns);

        public List<Vector3> BuildFormationPositions(
            Vector3 center,
            Vector3 forward,
            int unitCount)
        {
            var positions = new List<Vector3>();

            if (unitCount <= 0)
            {
                return positions;
            }

            forward.y = 0f;

            if (forward.sqrMagnitude <= 0.0001f)
            {
                forward = Vector3.forward;
            }

            forward.Normalize();

            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

            int actualColumns = Mathf.Max(
                1,
                Mathf.Min(Columns, unitCount));

            int rows = Mathf.CeilToInt(
                unitCount / (float)actualColumns);

            for (int row = 0; row < rows; row++)
            {
                int unitsThisRow = Mathf.Min(
                    actualColumns,
                    unitCount - row * actualColumns);

                float rowOffset =
                    (unitsThisRow - 1) * 0.5f;

                for (int column = 0;
                     column < unitsThisRow;
                     column++)
                {
                    float lateralOffset =
                        (column - rowOffset) * Spacing;

                    float forwardOffset =
                        -row * Spacing;

                    Vector3 position =
                        center +
                        right * lateralOffset +
                        forward * forwardOffset;

                    positions.Add(position);
                }
            }

            return positions;
        }
    }
}
