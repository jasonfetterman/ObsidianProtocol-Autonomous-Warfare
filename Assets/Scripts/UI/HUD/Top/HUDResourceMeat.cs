using UnityEngine;
using UnityEngine.UI;

public class HUDResourceMeat : MonoBehaviour
{
    [SerializeField] private Text amountText;

    public int Amount { get; private set; }

    public void SetAmount(int amount)
    {
        Amount = Mathf.Max(0, amount);

        if (amountText != null)
            amountText.text = Amount.ToString();
    }

    public void AddAmount(int amount)
    {
        SetAmount(Amount + amount);
    }

    public void RemoveAmount(int amount)
    {
        SetAmount(Amount - amount);
    }
}