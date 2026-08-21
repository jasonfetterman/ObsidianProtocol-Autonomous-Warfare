using UnityEngine;

namespace Assets.Scripts.AI
{
    public class SquadFormationController : MonoBehaviour
    {
        [Header("Formation Settings")]
        public float spacing = 2f;
        public int formationWidth = 3;

        [Header("Runtime")]
        public Vector3[] cachedPositions = new Vector3[0];

        public Vector3[] GetFormationPositions(Vector3 center, int count)
        {
            if (count <= 0)
                return new Vector3[0];

            cachedPositions = new Vector3[count];

            int rowLength = formationWidth;
            int rows = Mathf.CeilToInt(count / (float)rowLength);

            int index = 0;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < rowLength; c++)
                {
                    if (index >= count)
                        break;

                    float x = c * spacing - ((rowLength - 1) * spacing * 0.5f);
                    float z = r * spacing;

                    cachedPositions[index] = center + new Vector3(x, 0f, z);
                    index++;
                }
            }

            return cachedPositions;
        }
    }
}
