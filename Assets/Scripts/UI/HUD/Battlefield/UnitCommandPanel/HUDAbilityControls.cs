using UnityEngine;

public class HUDAbilityControls : MonoBehaviour
{
    public void ExecuteAbility(int abilityIndex)
    {
        Debug.Log($"[HUD] Ability {abilityIndex} command issued.");
    }
}
