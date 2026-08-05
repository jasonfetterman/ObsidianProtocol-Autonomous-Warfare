using UnityEngine;

public partial class CombatAI : MonoBehaviour
{
    GameObject forcedTarget;

    public GameObject GetCurrentTarget()
    {
        return forcedTarget != null ? forcedTarget : currentTarget;
    }

    public void SetForcedTarget(GameObject t)
    {
        forcedTarget = t;
    }
}
