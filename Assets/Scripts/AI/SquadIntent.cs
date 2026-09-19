using UnityEngine;

public class SquadIntent : MonoBehaviour
{
    [SerializeField]
    private IntentType currentIntent = IntentType.Hold;

    private IntentType lastIntent;

    public IntentType CurrentIntent => currentIntent;

    private void Start()
    {
        lastIntent = currentIntent;
    }

    private void Update()
    {
        if (lastIntent == currentIntent)
            return;

        lastIntent = currentIntent;

        Debug.Log($"{gameObject.name} Intent = {currentIntent}");
    }
}
