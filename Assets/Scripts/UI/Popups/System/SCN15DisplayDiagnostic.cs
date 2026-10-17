using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SCN15DisplayDiagnostic : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("========== SCN-15 DISPLAY DIAGNOSTIC ==========");

        Debug.Log("[SCN-15 DISPLAY] Screen = " +
                  Screen.width + " x " + Screen.height);

        Debug.Log("[SCN-15 DISPLAY] Mouse = " +
                  Input.mousePosition);

        Canvas canvas = GameObject.Find("EXIT CONFIRMATION HUD")?.GetComponent<Canvas>();

        if (canvas != null)
        {
            Debug.Log("[SCN-15 DISPLAY] Canvas RenderMode = " +
                      canvas.renderMode);

            Debug.Log("[SCN-15 DISPLAY] Canvas ScaleFactor = " +
                      canvas.scaleFactor);

            Debug.Log("[SCN-15 DISPLAY] Canvas PixelRect = " +
                      canvas.pixelRect);
        }

        PrintButton("CANCEL");
        PrintButton("CONFIRM EXIT");

        Debug.Log("========== END SCN-15 DISPLAY DIAGNOSTIC ==========");
    }

    private void PrintButton(string name)
    {
        GameObject go = GameObject.Find(name);

        if (go == null)
        {
            Debug.LogError("[SCN-15 DISPLAY] " + name + " NOT FOUND.");
            return;
        }

        RectTransform rect = go.GetComponent<RectTransform>();

        if (rect == null)
        {
            Debug.LogError("[SCN-15 DISPLAY] " + name + " HAS NO RECTTRANSFORM.");
            return;
        }

        Debug.Log(
            "[SCN-15 DISPLAY] " + name +
            " rect=" + rect.rect +
            " position=" + rect.position +
            " worldCorners=" +
            GetCorners(rect)
        );
    }

    private string GetCorners(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        return
            "BL=" + corners[0] +
            " TL=" + corners[1] +
            " TR=" + corners[2] +
            " BR=" + corners[3];
    }
}
