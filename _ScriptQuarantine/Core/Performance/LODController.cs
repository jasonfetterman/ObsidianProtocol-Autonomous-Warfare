using UnityEngine;

public class LODController : MonoBehaviour
{
    public GameObject highDetail;
    public GameObject lowDetail;

    public float switchDistance = 25f;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        float dist = Vector3.Distance(cam.transform.position, transform.position);

        if (dist > switchDistance)
        {
            highDetail.SetActive(false);
            lowDetail.SetActive(true);
        }
        else
        {
            highDetail.SetActive(true);
            lowDetail.SetActive(false);
        }
    }
}
