using UnityEngine;

public class LOSDebugger : MonoBehaviour
{
    public VisionProvider vp;

    void OnDrawGizmos()
    {
        if (vp == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(vp.transform.position, vp.visionRange);
    }
}

