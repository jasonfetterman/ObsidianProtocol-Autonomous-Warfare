using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class GarageManager : MonoBehaviour
    {
        public static GarageManager Instance { get; private set; }

        [Header("Garage Identity")]
        public string garageID = "GARAGE_01";
        public string garageDisplayName = "OBSIDIAN PROTOCOL GARAGE";

        [Header("Deployment")]
        public int deploymentBudget = 10000;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void SetDeploymentBudget(int value)
        {
            deploymentBudget = Mathf.Max(0, value);
        }
    }
}
