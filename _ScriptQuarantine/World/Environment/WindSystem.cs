using UnityEngine;

public class WindSystem : MonoBehaviour
{
    public Vector3 windForce = new(2f, 0f, 1f);

    private void Update()
    {
        BallisticProjectile[] projectiles =
            Object.FindObjectsByType<BallisticProjectile>(FindObjectsInactive.Exclude);

        foreach (var p in projectiles)
        {
            p.transform.position += windForce * Time.deltaTime;
        }
    }
}
