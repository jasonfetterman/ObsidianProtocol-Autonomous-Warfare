using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    public Material validMat;
    public Material invalidMat;

    Renderer[] rends;

    void Awake()
    {
        rends = GetComponentsInChildren<Renderer>();
    }

    public void SetValid(bool valid)
    {
        foreach (var r in rends)
            r.material = valid ? validMat : invalidMat;
    }
}

