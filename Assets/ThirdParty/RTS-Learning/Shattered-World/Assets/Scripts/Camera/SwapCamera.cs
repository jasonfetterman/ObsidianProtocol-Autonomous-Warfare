using UnityEngine;
public class SwapCamera : MonoBehaviour
{
    [SerializeField]private Select sel;

    [SerializeField]private Vector3 free_cam_position = Vector3.zero;
    [SerializeField]private Vector3 t1 = Vector3.zero, t2 = Vector3.zero;

    private GameObject selected_unit = null;

    private bool is_lerping = false;

    private void Awake()
    {
        sel = GetComponent<Select>();
    }
    private void Update()
    {
        UpdateCurrentlySelectedEntity();
    }

    private void LateUpdate()
    {
        LerpControl();
    }

    private void UpdateCurrentlySelectedEntity()
    {
        selected_unit = sel.CurrentSelectedEntity;
    }

    private void LerpControl()
    {
        if (!Input.GetKeyDown(KeyCode.P)) { return; }

        else if (Input.GetKeyDown(KeyCode.P)) 
        {
            if (selected_unit == null) { return; }
            if (selected_unit.GetComponentInChildren<CameraSocket>() == null) { return; }

            is_lerping = true;
        }

        LerpIntoFirstPerson();
        

        if(t1.Equals(t2))
        {
            is_lerping = false;
        }

        if(is_lerping)
        {
            transform.Translate(Vector3.Lerp(t1, t2, Time.unscaledDeltaTime));
        }
        
    }

    private void LerpIntoThirdPerson()
    {
        //transform.parent = null;
        t1 = transform.position;
        t2 = free_cam_position;
    }

    private void LerpIntoFirstPerson()
    {
        free_cam_position = transform.position;
        t1 = transform.position;
        t2 = selected_unit.GetComponentInChildren<CameraSocket>().transform.position;
        //transform.parent = selected_unit.transform;
    }
}
