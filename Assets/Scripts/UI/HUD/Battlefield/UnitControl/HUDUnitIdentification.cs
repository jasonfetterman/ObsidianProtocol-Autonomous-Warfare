using UnityEngine;
using UnityEngine.UI;

public class HUDUnitIdentification : MonoBehaviour
{
    [SerializeField] private Text identificationText;

    public string UnitID { get; private set; }
    public string UnitName { get; private set; }

    public void SetIdentification(string id, string unitName)
    {
        UnitID = id;
        UnitName = unitName;

        Refresh();
    }

    private void Refresh()
    {
        if (identificationText == null)
            return;

        if (string.IsNullOrWhiteSpace(UnitName))
            identificationText.text = UnitID ?? "";
        else if (string.IsNullOrWhiteSpace(UnitID))
            identificationText.text = UnitName;
        else
            identificationText.text = $"{UnitID}  {UnitName}";
    }
}
