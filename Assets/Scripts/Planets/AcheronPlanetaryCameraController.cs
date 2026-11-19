using UnityEngine;

public class AcheronPlanetaryCameraController : MonoBehaviour
{
    public Transform planet;
    public float orbitDistance = 219f;
    public float continentDistance = 125f;
    public float surfaceDistance = 55f;
    public float zoomSpeed = 35f;

    private Vector3 direction;
    private Vector3 lookTarget;
    private float targetDistance;

    void Start()
    {
        if (planet == null) return;

        direction = (transform.position - planet.position).normalized;
        lookTarget = planet.position;
        targetDistance = orbitDistance;
    }

    public void ZoomToDistance(float distance)
    {
        targetDistance = distance;
        lookTarget = planet.position;
    }

    public void ZoomToContinent(Transform continent, float distance)
    {
        if (continent == null) return;

        lookTarget = continent.position;
        targetDistance = distance;
    }

    void Update()
    {
        if (planet == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ZoomToDistance(orbitDistance);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ZoomToDistance(continentDistance);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            ZoomToDistance(surfaceDistance);

        float currentDistance = Vector3.Distance(
            transform.position, lookTarget
        );

        float newDistance = Mathf.MoveTowards(
            currentDistance,
            targetDistance,
            zoomSpeed * Time.deltaTime
        );

        transform.position = lookTarget + direction * newDistance;
        transform.LookAt(lookTarget);
    }
}