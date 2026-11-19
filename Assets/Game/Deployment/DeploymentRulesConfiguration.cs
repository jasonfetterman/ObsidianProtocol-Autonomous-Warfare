using UnityEngine;

namespace ObsidianProtocol.Game.Deployment
{
    public enum DeploymentMode
    {
        Unconfigured,
        Offline,
        Multiplayer
    }

    [CreateAssetMenu(
        fileName = "DeploymentRulesConfiguration",
        menuName = "Obsidian Protocol/Deployment/Rules Configuration")]
    public sealed class DeploymentRulesConfiguration : ScriptableObject
    {
        [Header("DEPLOYMENT MODE")]
        [SerializeField] private DeploymentMode mode =
            DeploymentMode.Unconfigured;

        [Header("OFFLINE RULES")]
        [Min(0)] [SerializeField] private int offlineDeploymentBudget;
        [Min(0)] [SerializeField] private int offlineMaximumDeploymentPoints;
        [Min(0)] [SerializeField] private int offlineMaximumReinforcementPoints;
        [SerializeField] private bool offlineAllowReinforcements = true;

        [Header("MULTIPLAYER RULES")]
        [Min(0)] [SerializeField] private int multiplayerBattleBudget;
        [Min(0)] [SerializeField] private int multiplayerMaximumDeploymentPoints;
        [Min(0)] [SerializeField] private int multiplayerMaximumReinforcementPoints;
        [SerializeField] private bool multiplayerAllowReinforcements = true;

        public DeploymentMode Mode => mode;

        public int OfflineDeploymentBudget => offlineDeploymentBudget;
        public int OfflineMaximumDeploymentPoints => offlineMaximumDeploymentPoints;
        public int OfflineMaximumReinforcementPoints => offlineMaximumReinforcementPoints;
        public bool OfflineAllowReinforcements => offlineAllowReinforcements;

        public int MultiplayerBattleBudget => multiplayerBattleBudget;
        public int MultiplayerMaximumDeploymentPoints => multiplayerMaximumDeploymentPoints;
        public int MultiplayerMaximumReinforcementPoints => multiplayerMaximumReinforcementPoints;
        public bool MultiplayerAllowReinforcements => multiplayerAllowReinforcements;

        public bool IsConfigured
        {
            get
            {
                if (mode == DeploymentMode.Offline)
                    return offlineDeploymentBudget > 0 &&
                           offlineMaximumDeploymentPoints > 0;

                if (mode == DeploymentMode.Multiplayer)
                    return multiplayerBattleBudget > 0 &&
                           multiplayerMaximumDeploymentPoints > 0;

                return false;
            }
        }
    }
}

