using UnityEngine;

public class AcheronContinentSelector : MonoBehaviour
{
    public AcheronPlanetaryCameraController cameraController;
    public Transform continentTarget;
    public float zoomDistance = 85f;

    private void OnMouseDown()
    {
        if (cameraController == null || continentTarget == null)
        {
            Debug.LogWarning("Blackspire navigation references missing.");
            return;
        }

        cameraController.ZoomToContinent(continentTarget, zoomDistance);
        Debug.Log("CONTINENT SELECTED: BLACKSPIRE");
    }
}