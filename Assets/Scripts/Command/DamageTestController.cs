using UnityEngine;
using UnityEngine.InputSystem;

public class DamageTestController : MonoBehaviour
{
    [SerializeField] private UnitSelectionManager selectionManager;
    [SerializeField] private float testDamageAmount = 10f;

    private void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            foreach (SelectableUnit unit in selectionManager.SelectedUnits)
            {
                UnitStatsComponent stats = unit.GetComponent<UnitStatsComponent>();
                if (stats != null)
                {
                    stats.TakeDamage(testDamageAmount);
                }
            }
        }
    }
}