using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Camera_Controller : MonoBehaviour
{
    [Header("Mouse Magnify Inputs")]
    [SerializeField]private float Magnify_speed = 100f;
    [SerializeField]private float max_magnify = 0.2f;
    [SerializeField]private float min_magnify = -10f;

    [Header("Mouse Movement Inputs")]
    [SerializeField]private float MouseMoveSpeed = 100f;
    [SerializeField]private float MouseInputTemp = 0f;

    [Header("Mouse Boundary Restrictions")]
    [SerializeField]private float up_boundary = 50f;
    [SerializeField]private float down_boundary = -50f;
    [SerializeField]private float left_boundary = -50f;
    [SerializeField]private float right_boundary = 50f;

    [Header("Camera Rotations")]
    [SerializeField]private Vector3 Initial_Cam_Rotation;
    [SerializeField]private KeyCode left_movement = KeyCode.Q;
    [SerializeField]private KeyCode right_movement = KeyCode.E;
    [SerializeField]private float Rotation_Speed = 360f;

    [Header("Keyboard  Inputs")]
    [SerializeField] private float KeyboardMoveSpeed = 100f;

    private void Awake()
    {
        Initial_Cam_Rotation = transform.rotation.eulerAngles;
        //Debug.Log("Initial cam rotation: "+Initial_Cam_Rotation);
    }

    private void LateUpdate()
    {
        MoveWithKeyboard();
        MoveWithMouse();
        Magnify();
        RotateCamera();
    }

    private void RotateCamera() // Rotate the camera with given input - Q and E are default values //
    {
        //Vector3 cached_value = new(Initial_Cam_Rotation.x,(Initial_Cam_Rotation.y * Time.unscaledDeltaTime * Rotation_Speed), Initial_Cam_Rotation.z);

        if (Input.GetKey(left_movement))
        {
            transform.RotateAround(transform.position,-(Vector3.up),(Rotation_Speed * Time.deltaTime));
        }
        else if(Input.GetKey(right_movement))
        {
            transform.RotateAround(transform.position,Vector3.up,(Rotation_Speed * Time.deltaTime));
        }
    }
    private void MoveWithMouse()
    {
        Vector3 mousepos = Input.mousePosition;

        float mouse_x = mousepos.x;
        float mouse_y = mousepos.y;

        float width = Screen.width;
        float height = Screen.height;

        Vector3 pos = transform.position;
        float pos_x = pos.x;
        float pos_z = pos.z;

        if(mouse_x <= (0 + MouseInputTemp) && pos_x >= left_boundary) // Move left //
        {
            transform.Translate(-1f * MouseMoveSpeed * Time.unscaledDeltaTime * Vector3.right);
        }
        else if(mouse_x >= (width - MouseInputTemp) && pos_x <= right_boundary) // Move Right //
        {
                transform.Translate(MouseMoveSpeed * Time.unscaledDeltaTime * Vector3.right); 
        }

        if (mouse_y >= (height - MouseInputTemp) && pos_z <= up_boundary) // Move up //
        {
                transform.Translate(MouseMoveSpeed * Time.unscaledDeltaTime * (0.5f * (Vector3.up + Vector3.forward))); // 
        }
        else if (mouse_y <= (0 + MouseInputTemp) && pos_z >= down_boundary) // Move Down //
        {
                transform.Translate(-1f * MouseMoveSpeed * Time.unscaledDeltaTime * (0.5f * (Vector3.up + Vector3.forward)));
        }
;    }
    private void MoveWithKeyboard() // the script that moves the camera through keyboard input //
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (x == 0 && y == 0) { return; }
        Vector3 moveBy = (Vector3.right * x) + ((Vector3.up+Vector3.forward) * 0.5f * y);
        //Debug.DrawRay(Camera.main.transform.position, Vector3.forward * 100f,Color.black);
        transform.Translate(KeyboardMoveSpeed * Time.unscaledDeltaTime * moveBy.normalized);
    }

    private void Magnify()
    {
        float MouseWheelInput = Input.GetAxisRaw("Mouse ScrollWheel");

        // Constraints - START// 
        if (MouseWheelInput == 0f) { return; } // if mouse 3 not engaged  do not execute//
        float x_pos = transform.position.x;
        float y_pos = transform.position.y;
        float z_pos = transform.position.z;
        if (z_pos < min_magnify && MouseWheelInput < 0f) { transform.Translate(x_pos, y_pos, min_magnify); Debug.Log("minimum magnification reached"); return; }
        if (z_pos > max_magnify && MouseWheelInput > 0f) { transform.Translate(x_pos, y_pos, max_magnify); Debug.Log("maximum magnification reached"); return; }
        // Constraints - END // 

        transform.Translate(Magnify_speed * MouseWheelInput * Time.unscaledDeltaTime * Vector3.forward);
    }
}