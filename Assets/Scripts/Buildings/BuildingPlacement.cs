using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    public GameObject ghostPrefab;
    public GameObject buildingPrefab;

    BuildingGhost ghost;
    bool placing = false;

    void Update()
    {
        if (!placing) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ghost.transform.position = hit.point;

            bool valid = hit.collider.CompareTag("Ground");
            ghost.SetValid(valid);

            if (valid && Input.GetMouseButtonDown(0))
            {
                PlaceBuilding(hit.point);
            }
        }
    }

    public void StartPlacement()
    {
        placing = true;
        ghost = Instantiate(ghostPrefab).GetComponent<BuildingGhost>();
    }

    void PlaceBuilding(Vector3 pos)
    {
        placing = false;
        Destroy(ghost.gameObject);

        GameObject b = Instantiate(buildingPrefab, pos, Quaternion.identity);
        b.AddComponent<BuildingConstruction>();
    }
}
