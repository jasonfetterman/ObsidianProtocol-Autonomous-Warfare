using UnityEngine;

public class BuildingSelector : MonoBehaviour
{
    Builder builder;

    void Awake()
    {
        builder = GetComponent<Builder>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // right-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                BuildingConstruction bc = hit.collider.GetComponentInParent<BuildingConstruction>();
                if (bc != null)
                    builder.SetTarget(bc);
            }
        }
    }
}
