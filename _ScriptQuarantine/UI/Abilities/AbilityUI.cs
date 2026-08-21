using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    public AbilityUser abilityUser;

    public void UseAbility0()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
            abilityUser.UseAbility(0, hit.point, hit.collider.gameObject);
    }

    public void UseAbility1()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
            abilityUser.UseAbility(1, hit.point, hit.collider.gameObject);
    }
}
