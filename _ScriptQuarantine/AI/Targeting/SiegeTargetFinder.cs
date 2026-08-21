using UnityEngine;

public class SiegeTargetFinder : MonoBehaviour
{
    public float searchRadius = 50f;

    public GameObject FindBestTarget()
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");

        GameObject best = null;
        float bestScore = -Mathf.Infinity;

        foreach (var b in buildings)
        {
            float dist = Vector3.Distance(transform.position, b.transform.position);

            if (dist > searchRadius)
                continue;

            BuildingHealth bh = b.GetComponent<BuildingHealth>();

            if (bh == null)
                continue;

            float score = ScoreBuilding(b);

            if (score > bestScore)
            {
                bestScore = score;
                best = b;
            }
        }

        return best;
    }

    float ScoreBuilding(GameObject b)
    {
        if (b.GetComponent<WallSegment>() != null)
            return 100f; // walls highest priority

        if (b.GetComponent<BuildingSpawner>() != null)
            return 60f; // production buildings

        return 20f; // everything else
    }
}
