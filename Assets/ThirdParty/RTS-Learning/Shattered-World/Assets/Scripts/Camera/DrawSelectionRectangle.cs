using System.Collections.Generic;
using UnityEngine;
public class DrawSelectionRectangle : MonoBehaviour
{
    Vector3 mouse_pos;

    Rect rectangle;

    [Header("Rectangle Dimensions")]
    [SerializeField] private Vector2 _box_end_pos;
    [SerializeField] private Vector2 _box_start_pos;

    [Header("Texture of the Rectangle Drawing")]
    [SerializeField] private Texture2D texture;

    [Header("Color of the Texture")]
    [SerializeField] private UnityEngine.Color color;

    [SerializeField] private List<Unit_Controller> units;

    private void Start()
    {
        if (texture == null) { texture = Assigntexture(); }
    }
    private void Update()
    {
        GetPositions();
    }
    private void GetPositions() // get positions for rectangle //
    {
        mouse_pos = Input.mousePosition;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _box_start_pos = mouse_pos;
            }

            else
            {
                _box_end_pos = mouse_pos;
            }
        }
        else
        {
            if (_box_end_pos != Vector2.zero && _box_start_pos != Vector2.zero) // Handle unit selection right after selection //
            {
                HandleUnitSelection();
            }
            _box_end_pos = _box_start_pos = Vector2.zero;
        }
    }
    private void HandleUnitSelection()
    {
        //Debug.Log(texture.alphaIsTransparency);
        //Debug.Log(color.a);
    }
    private Texture2D Assigntexture()
    {
        Texture2D tex = new(1, 1, TextureFormat.RGBA32, false)
        {
            alphaIsTransparency = true
        };

        /* 
        color.a = 1;
        color = UnityEngine.Color.white;
        tex.SetPixel(0, 1, color);
        tex.Apply();
        */
        return tex;
    }
    private void OnGUI() // Draw rectangle on Graphical User Interface using the data provided from GetPositions()  function //
    {
        float screen_height = Screen.height;

        if (_box_start_pos != Vector2.zero && _box_end_pos != Vector2.zero)
        {
            rectangle = new(_box_start_pos.x, (screen_height - _box_start_pos.y), (_box_end_pos.x - _box_start_pos.x), (-1 * (_box_end_pos.y - _box_start_pos.y)));
            GUI.DrawTexture(rectangle, texture);
        }
    }
}