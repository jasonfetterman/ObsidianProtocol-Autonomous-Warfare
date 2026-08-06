using UnityEngine;
using Assets.Scripts.AI;   // SquadFormationController

public class SquadFormationDriver : MonoBehaviour
{
    private SquadFormationController controller;

    void Awake()
    {
        controller = ServiceLocator.Get<SquadFormationController>();
    }

    void Update()
    {
        // SquadFormationController has no Tick() method.
        // It is a pure utility class, so nothing runs per-frame.
    }
}
