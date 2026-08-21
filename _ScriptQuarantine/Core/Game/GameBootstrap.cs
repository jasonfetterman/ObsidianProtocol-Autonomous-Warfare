using Assets.Scripts.AI;      // SquadFormationController, CombatAI
using Assets.Scripts.Squad;   // SquadAI, SquadMemory, SquadController, SquadCommander, SquadIntent, SquadTactics
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    void Awake()
    {
        // ------------------------------------
        // CORE SYSTEMS
        // ------------------------------------
        ServiceLocator.Register(new SquadMemory());
        ServiceLocator.Register(new SquadAI());
        ServiceLocator.Register(new SquadController());
        ServiceLocator.Register(new SquadCommander());
        ServiceLocator.Register(new SquadIntent());

        // ------------------------------------
        // FORMATION SYSTEM
        // ------------------------------------
        SquadFormationInstaller.Install();
        ServiceLocator.Register(new SquadFormationController());

        // ------------------------------------
        // TACTICS SYSTEM
        // ------------------------------------
        ServiceLocator.Register(new SquadTactics());

        // ------------------------------------
        // COMBAT SYSTEM
        // ------------------------------------
        ServiceLocator.Register(new CombatAI());

        Debug.Log("GameBootstrap: All systems initialized.");
    }
}
