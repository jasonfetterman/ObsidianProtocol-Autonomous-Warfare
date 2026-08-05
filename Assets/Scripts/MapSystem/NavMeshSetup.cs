using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSetup : MonoBehaviour
{
    public NavMeshSurface surface;

    void Start()
    {
        if (surface != null)
            surface.BuildNavMesh();
    }
}
