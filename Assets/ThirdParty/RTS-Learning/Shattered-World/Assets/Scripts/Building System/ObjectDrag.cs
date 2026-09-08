using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offSet;

    private void OnMouseDown()
    {
        offSet = transform.position - BuildingSystem.GetMouseWorldPosition();
    }
    private void OnMouseDrag() 
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offSet;
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
