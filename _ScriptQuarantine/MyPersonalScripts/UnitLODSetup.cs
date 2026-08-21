using UnityEngine;

[DisallowMultipleComponent]
public class UnitLODSetup : MonoBehaviour
{
    [Header("LOD Models")]
    [Tooltip("Highest-detail model. Used when the unit is close.")]
    public GameObject lod0;

    [Tooltip("Reduced-detail model.")]
    public GameObject lod1;

    [Tooltip("Low-detail model.")]
    public GameObject lod2;

    [Tooltip("Lowest-detail model. Used when the unit is far away.")]
    public GameObject lod3;

    [Header("LOD Screen Percentages")]
    [Range(0.01f, 1f)]
    public float lod0Transition = 0.60f;

    [Range(0.01f, 1f)]
    public float lod1Transition = 0.30f;

    [Range(0.01f, 1f)]
    public float lod2Transition = 0.10f;

    [Range(0.01f, 1f)]
    public float lod3Transition = 0.03f;

    private LODGroup lodGroup;

    private void Awake()
    {
        SetupLOD();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            SetupLOD();
        }
    }
#endif

    public void SetupLOD()
    {
        lodGroup = GetComponent<LODGroup>();

        if (lodGroup == null)
            lodGroup = gameObject.AddComponent<LODGroup>();

        LOD[] lods = new LOD[4];

        lods[0] = CreateLOD(lod0, lod0Transition);
        lods[1] = CreateLOD(lod1, lod1Transition);
        lods[2] = CreateLOD(lod2, lod2Transition);
        lods[3] = CreateLOD(lod3, lod3Transition);

        lodGroup.SetLODs(lods);
        lodGroup.RecalculateBounds();
    }

    private LOD CreateLOD(GameObject lodObject, float transitionHeight)
    {
        if (lodObject == null)
        {
            Debug.LogWarning(
                $"LOD object missing on {gameObject.name}",
                this
            );

            return new LOD(
                transitionHeight,
                new Renderer[0]
            );
        }

        Renderer[] renderers =
            lodObject.GetComponentsInChildren<Renderer>(true);

        return new LOD(
            transitionHeight,
            renderers
        );
    }
}