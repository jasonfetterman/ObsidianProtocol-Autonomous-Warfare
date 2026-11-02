using UnityEngine;
using TMPro;
using ObsidianProtocol.Game.Research;
using ObsidianProtocol.Game.Technology;

namespace ObsidianProtocol.UI.Research
{
    public sealed class ResearchMaterialsController : MonoBehaviour
    {
        private const string MaterialsKey = "OPAW_RESEARCH_MATERIALS";
        private const float StartingMaterials = 72f;
        private const float MaterialsPerResearch = 10f;

        [SerializeField] private TMP_Text materialsText;

        private float materials;
        private ResearchManager subscribedManager;

        private void Awake()
        {
            materials = PlayerPrefs.GetFloat(
                MaterialsKey, StartingMaterials);

            if (materialsText == null)
            {
                GameObject target = GameObject.Find("TEXT MATERIALS");
                if (target != null)
                    materialsText = target.GetComponent<TMP_Text>();
            }

            UpdateDisplay();
        }

        private void OnEnable()
        {
            SubscribeToManager();
        }

        private void OnDisable()
        {
            if (subscribedManager != null)
                subscribedManager.ResearchCompleted -= OnResearchCompleted;

            subscribedManager = null;
        }

        private void SubscribeToManager()
        {
            ResearchManager manager = ResearchManager.Instance;
            if (manager == null || manager == subscribedManager)
                return;

            if (subscribedManager != null)
                subscribedManager.ResearchCompleted -= OnResearchCompleted;

            subscribedManager = manager;
            subscribedManager.ResearchCompleted += OnResearchCompleted;
        }

        private void Update()
        {
            if (subscribedManager == null)
                SubscribeToManager();
        }

        private void OnResearchCompleted(TechnologyDefinition technology)
        {
            materials = Mathf.Max(0f, materials - MaterialsPerResearch);

            PlayerPrefs.SetFloat(MaterialsKey, materials);
            PlayerPrefs.Save();

            UpdateDisplay();

            Debug.Log(
                $"[RESEARCH MATERIALS] Research completed: {technology.DisplayName}. Materials remaining: {materials:0}%");
        }

        private void UpdateDisplay()
        {
            if (materialsText != null)
                materialsText.text = $"MATERIALS {Mathf.RoundToInt(materials)}%";
        }
    }
}
