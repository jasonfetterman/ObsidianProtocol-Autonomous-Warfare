using UnityEngine;
using TMPro;
using ObsidianProtocol.Game.Research;

namespace ObsidianProtocol.UI.Research
{
    public sealed class ResearchMaterialsController : MonoBehaviour
    {
        private const string MaterialsKey = "OPAW_RESEARCH_MATERIALS";
        private const float StartingMaterials = 72f;
        private const float MaterialsPerResearch = 10f;

        [SerializeField] private TMP_Text materialsText;

        private float materials;
        private bool wasResearching;

        private void Awake()
        {
            materials = PlayerPrefs.GetFloat(
                MaterialsKey,
                StartingMaterials);

            if (materialsText == null)
            {
                GameObject target = GameObject.Find("TEXT MATERIALS");

                if (target != null)
                    materialsText = target.GetComponent<TMP_Text>();
            }

            UpdateDisplay();
        }

        private void Update()
        {
            ResearchManager manager = ResearchManager.Instance;

            if (manager == null)
                return;

            if (manager.IsResearching)
            {
                wasResearching = true;
            }
            else if (wasResearching)
            {
                wasResearching = false;

                materials = Mathf.Max(
                    0f,
                    materials - MaterialsPerResearch);

                PlayerPrefs.SetFloat(
                    MaterialsKey,
                    materials);

                PlayerPrefs.Save();

                UpdateDisplay();

                Debug.Log(
                    $"[RESEARCH MATERIALS] Materials remaining: {materials:0}%");
            }
        }

        private void UpdateDisplay()
        {
            if (materialsText != null)
                materialsText.text =
                    $"MATERIALS {Mathf.RoundToInt(materials)}%";
        }
    }
}
