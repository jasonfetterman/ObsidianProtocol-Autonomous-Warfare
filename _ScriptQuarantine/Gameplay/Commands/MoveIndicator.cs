using UnityEngine;

public class MoveIndicator : MonoBehaviour
{
    public Color color = new Color(0f, 1f, 0.3f, 0.7f);
    public float size = 0.5f;
    public float duration = 1.2f;

    private float timer;

    void Start()
    {
        timer = duration;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
            Destroy(gameObject);
    }

    void OnRenderObject()
    {
        Material mat = GetLineMaterial();
        mat.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);
        GL.Color(color);

        float s = size;

        GL.Vertex3(-s, 0, -s);
        GL.Vertex3(s, 0, -s);

        GL.Vertex3(s, 0, -s);
        GL.Vertex3(s, 0, s);

        GL.Vertex3(s, 0, s);
        GL.Vertex3(-s, 0, s);

        GL.Vertex3(-s, 0, s);
        GL.Vertex3(-s, 0, -s);

        GL.End();
        GL.PopMatrix();
    }

    private static Material lineMat;
    private Material GetLineMaterial()
    {
        if (lineMat != null)
            return lineMat;

        Shader shader = Shader.Find("Hidden/Internal-Colored");
        lineMat = new Material(shader)
        {
            hideFlags = HideFlags.HideAndDontSave
        };
        lineMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        lineMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        lineMat.SetInt("_ZWrite", 0);

        return lineMat;
    }
}
