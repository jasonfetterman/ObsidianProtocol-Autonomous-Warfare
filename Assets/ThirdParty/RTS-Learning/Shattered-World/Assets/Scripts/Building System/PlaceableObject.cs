using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed {get; private set;}
    public Vector3Int Size {get; private set;}
    private Vector3[] Vertices;
    private void GetColliderVertexPositionsLocal()
    {
        Vector3[] vertices = new Vector3[4];
        BoxCollider collider = GetComponent<BoxCollider>();
        vertices[0] = collider.center + new Vector3(-collider.size.x, -collider.size.y, -collider.size.z) * 0.5f;
        vertices[1] = collider.center + new Vector3(collider.size.x, -collider.size.y, -collider.size.z) * 0.5f;
        vertices[2] = collider.center + new Vector3(collider.size.x, -collider.size.y, collider.size.z) * 0.5f;
        vertices[3] = collider.center + new Vector3(-collider.size.x, -collider.size.y, collider.size.z) * 0.5f;
    }
    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];
        for(int i = 0; i < Vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            vertices[i] = BuildingSystem.current.grid.WorldToCell(worldPos);
        }
        Size = new Vector3Int(
            Mathf.Abs(vertices[0].x - vertices[1].x),
            Mathf.Abs(vertices[0].y - vertices[2].y),
            Mathf.Abs(vertices[0].z - vertices[3].z)
        );

    }
    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }
    private void Start() 
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();
    }
    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);
        Placed = true;
    }
}
