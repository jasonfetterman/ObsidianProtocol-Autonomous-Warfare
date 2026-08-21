using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [ExecuteAlways]
    public class UnitRealWorldSizer : MonoBehaviour
    {
        [Header("REAL WORLD SIZE — METERS")]
        public float length = 9f;
        public float width = 5f;
        public float height = 5f;

        [Header("MODEL")]
        public Transform model;

        [ContextMenu("SET REAL WORLD SIZE")]
        public void SetRealWorldSize()
        {
            if (model == null)
            {
                Debug.LogError("Assign the model first.", this);
                return;
            }

            Renderer[] renderers =
                model.GetComponentsInChildren<Renderer>(true);

            if (renderers.Length == 0)
            {
                Debug.LogError("No Renderer found on model.", this);
                return;
            }

            Bounds bounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            Vector3 currentSize = bounds.size;

            float x = length / currentSize.x;
            float y = height / currentSize.y;
            float z = width / currentSize.z;

            model.localScale = new Vector3(x, y, z);

            Debug.Log(
                $"Worldmap resized to {length}m L × {width}m W × {height}m H",
                this);
        }
    }
}