using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class GlobalSelection : MonoBehaviour
{
    SelectedDictionary selectedTable = new SelectedDictionary();
    RaycastHit hit;
    bool dragSelect;
    Vector3 p1, p2;
    MeshCollider selectionBox;
    Mesh selectedMesh;
    Vector2[] corners;
    Vector3[] verts;

    // Start is called before the first frame update
    void Start()
    {
        selectedTable = GetComponent<SelectedDictionary>();
        dragSelect = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if((p1 - Input.mousePosition).magnitude > 10)
            {
                dragSelect = true;
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            p1 = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(dragSelect == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(p1);
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.gameObject.GetComponent<SelectionComponent>() != null)
                    {
                        selectedTable.addSelected(hit.transform.gameObject);
                    }
                    else
                    {
                        selectedTable.clearSelected();
                        selectedTable.addSelected(hit.transform.gameObject);
                    }
                }
                else
                {
                    selectedTable.clearSelected();
                }
                
            }
        }
    }
    // private void OnGUI() 
    // {
    //     if(dragSelect == true)
    //     {
    //         var rect = Utils.GetScreenRect(p1, Input.mousePosition);
    //         Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
    //         Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
    //         p2 = Input.mousePosition;
    //         if(Input.GetMouseButtonUp(0))
    //         {
    //             dragSelect = false;
    //             corners = new Vector2[] {new Vector2(p1.x, p1.y), new Vector2(p1.x, p2.y), new Vector2(p2.x, p2.y), new Vector2(p2.x, p1.y)};
    //             verts = new Vector3[] {Camera.main.ScreenToWorldPoint(new Vector3(p1.x, p1.y, 0)), Camera.main.ScreenToWorldPoint(new Vector3(p1.x, p2.y, 0)), Camera.main.ScreenToWorldPoint(new Vector3(p2.x, p2.y, 0)), Camera.main.ScreenToWorldPoint(new Vector3(p2.x, p1.y, 0))};
    //             selectedMesh = new Mesh();
    //             selectedMesh.vertices = verts;
    //             selectedMesh.triangles = new int[] {0, 1, 2, 0, 2, 3};
    //             selectedMesh.RecalculateBounds();
    //             selectedMesh.RecalculateNormals();
    //             selectionBox = gameObject.AddComponent<MeshCollider>();
    //             selectionBox.sharedMesh = selectedMesh;
    //             foreach(GameObject go in FindObjectsOfType(typeof(GameObject)) as GameObject[])
    //             {
    //                 if(go.GetComponent<SelectionComponent>() != null)
    //                 {
    //                     if(selectionBox.bounds.Contains(go.transform.position))
    //                     {
    //                         selectedTable.addSelected(go);
    //                     }
    //                 }
    //             }
    //             Destroy(selectionBox);
    //         }
    //     }
    // }
}
