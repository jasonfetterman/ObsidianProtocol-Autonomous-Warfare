using UnityEngine;
using UnityEngine.UI;

public class UnitPanelController : MonoBehaviour
{
    public Image unitBlueprintImage;

    public void ShowUnit(UnitData data)
    {
        unitBlueprintImage.sprite = data.Portrait;
        unitBlueprintImage.enabled = true;
    }

    public void HideUnit()
    {
        unitBlueprintImage.enabled = false;
    }
}
