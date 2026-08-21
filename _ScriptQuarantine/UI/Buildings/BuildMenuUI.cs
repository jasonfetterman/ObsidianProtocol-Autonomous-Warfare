using UnityEngine;

public class BuildMenuUI : MonoBehaviour
{
    public BuildingPlacement barracksPlacement;
    public BuildingSpawner barracksSpawner;

    public void OnBuildBarracks()
    {
        if (barracksPlacement != null)
            barracksPlacement.StartPlacement();
    }

    public void OnTrainInfantry()
    {
        if (barracksSpawner != null)
            barracksSpawner.SpawnUnit();
    }
}

