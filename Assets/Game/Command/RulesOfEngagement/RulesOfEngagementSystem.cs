using System;

namespace ObsidianProtocol.Game.Command.RulesOfEngagement
{
    public enum EngagementPermission
    {
        HoldFire,
        DefensiveFire,
        ReturnFire,
        EngageOnDetection,
        EngageOnCommand
    }

    [Serializable]
    public sealed class RulesOfEngagement
    {
        public EngagementPermission Permission =
            EngagementPermission.ReturnFire;

        public bool AllowPursuit;
        public bool AllowTargetChange = true;
        public bool AllowRetreat = true;
    }

    public sealed class RulesOfEngagementSystem
    {
        public RulesOfEngagement CurrentRules { get; private set; }

        public RulesOfEngagementSystem()
        {
            CurrentRules = new RulesOfEngagement();
        }

        public void SetRules(RulesOfEngagement rules)
        {
            if (rules == null)
            {
                return;
            }

            CurrentRules = rules;
        }

        public bool CanEngageWithoutCommand()
        {
            return CurrentRules.Permission ==
                       EngagementPermission.EngageOnDetection ||
                   CurrentRules.Permission ==
                       EngagementPermission.DefensiveFire ||
                   CurrentRules.Permission ==
                       EngagementPermission.ReturnFire;
        }

        public bool CanPursue()
        {
            return CurrentRules.AllowPursuit;
        }

        public bool CanChangeTarget()
        {
            return CurrentRules.AllowTargetChange;
        }

        public bool CanRetreat()
        {
            return CurrentRules.AllowRetreat;
        }

        public void Reset()
        {
            CurrentRules = new RulesOfEngagement();
        }
    }
}
