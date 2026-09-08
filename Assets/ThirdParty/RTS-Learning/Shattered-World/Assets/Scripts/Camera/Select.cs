using UnityEngine;
public class Select : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] public GameObject CurrentSelectedEntity = null;
    [SerializeField] private LayerMask layer_mask;
    [SerializeField] public GameObject Inventory_Canvas; // encapsulation is set to public because other classes use this reference to access it's functions //
    private void Awake()
    {
        cam = GetComponent<Camera>();
        Inventory_Canvas = GameObject.Find("Inventory_Canvas");
        Inventory_Canvas.SetActive(false);
    }
    [System.Obsolete]
    private void Update()
    {
        Select_Object();
        MoveTarget();
    }
    private void MoveTarget()
    {
        if (!Input.GetMouseButtonDown(1)) { return; }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (CurrentSelectedEntity == null) { return; }
        if (!CurrentSelectedEntity.CompareTag("Unit")) { return; }
        if (!CurrentSelectedEntity.TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var navmesh_temp)) { return; }

        Unit_Controller unit_temp = navmesh_temp.GetComponent<Unit_Controller>();

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            unit_temp.ChangeDirection(hit.point);
            //Debug.Log(hit.transform.position);
        }
    }

    [System.Obsolete] // because of GetAllChildren() this function is obsolete //
    private void Select_Object() // Select a playable object by raycasting the target should be on the corresponding layer mask //
    {
        if (!Input.GetMouseButtonDown(0)) { return; }

        //Debug.Log("It works");
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer_mask))
        {
            if (CurrentSelectedEntity != null) { DisposeUI(); } // if the currently selected entity is not null dispose of the UI //
            else if(CurrentSelectedEntity != hit.transform.gameObject) { Inventory_Canvas.SetActive(false); Canvas_Controller.DeleteAllChildren(); } // 

            CurrentSelectedEntity = hit.transform.gameObject;

            if (CurrentSelectedEntity == null) {  return; } // if nothing hit the raycast return //

            if (CurrentSelectedEntity.GetComponent<Inventory_Manager>() == null) { return; } // if the object does not have an inventory script exit execution //
            Inventory_Canvas.SetActive(true); // Activate Inventory Canvas for display //
            CurrentSelectedEntity.GetComponent<Inventory_Manager>().DisplayInventory();
        }
        else
        {
            DisposeUI();
        }
    }
    [System.Obsolete]
    private void DisposeUI()
    {
        Inventory_Canvas.SetActive(false); // If nothing is selected don't dislpay the Inventory Canvas //

        if (CurrentSelectedEntity == null) { Debug.Log("Nothing preferable is selected"); }

        else
        {
            CurrentSelectedEntity = null; // if the raycast did not hit a playable object drop the current selected object //
            Canvas_Controller.DeleteAllChildren();
            //Debug.Log("Selected object dropped");
        }
    }
}