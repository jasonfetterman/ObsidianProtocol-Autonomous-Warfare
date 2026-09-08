using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A reminder: I set the camera rotation to 55-60, for top view go with 90
 */

public class CameraController : MonoBehaviour
{
    [SerializeField] float panSpeed = 20f;
    [SerializeField] float panBorderThickness = 10f;
    [SerializeField] Vector2 panLimit;

    [SerializeField] float mouseScrollSpeed = 20f;
    [SerializeField] float zoomMinDistance, zoomMaxDistance;
 
    void Update()
    {
        Vector3 newPosition = transform.position;

        //Keyboard and Mouse Input 
        if(Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panBorderThickness)
            newPosition.z += panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
            newPosition.z -= panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness)
            newPosition.x += panSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness)
            newPosition.x -= panSpeed * Time.deltaTime;

        float mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");
        newPosition.y -= mouseScrollInput * mouseScrollSpeed * 100f * Time.deltaTime;

        //boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, -panLimit.x, panLimit.x);
        newPosition.y = Mathf.Clamp(newPosition.y, zoomMinDistance, zoomMaxDistance);
        newPosition.z = Mathf.Clamp(newPosition.z, -panLimit.y, panLimit.y);

        transform.position = newPosition;
    }

    /*
     * float horizontalInput = Input.GetAxis("Horizontal");
     * float verticalInput = Input.GetAxis("VerticalInput);
     * 
     * Vector3 newPosition = transform.forward * horizontalInput + transform.forward * verticalInput;
     * 
     * transform.position += newPosition * panSpeed * Time.deltaTime;  
     */
}
