using UnityEngine;
using ObsidianProtocol.Game.Command;

public class IntentButton : MonoBehaviour
{
    [SerializeField]
    private IntentType intentType;

    [SerializeField]
    private CommandIntentManager intentManager;

    public void OnClick()
    {
        if (intentManager == null)
        {
            Debug.LogWarning($"IntentButton: CommandIntentManager is not assigned on {gameObject.name}.");
            return;
        }

        intentManager.IssueIntent(intentType);
    }
}
