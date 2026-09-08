using UnityEngine;

[CreateAssetMenu()]
public class GizmoSettingsSO : ScriptableObject
{
    [Range(0f, 4f)]public float radius;
    public Color color = new Color(255f, 255f, 255f, 1f);

    public bool showInInspector;
    public bool showInGame;
}
