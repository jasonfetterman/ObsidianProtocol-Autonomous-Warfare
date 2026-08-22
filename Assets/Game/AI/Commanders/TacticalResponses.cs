using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum TacticalResponseType
    {
        None,
        ConcentrateForce,
        Attack,
        Flank,
        Suppress,
        Breach,
        Advance,
        HoldPosition,
        Screen,
        Regroup,
        Withdraw,
        Reinforce,
        Recon,
        Pursue,
        EstablishFiringPosition
    }

    public sealed class TacticalResponse
    {
        public TacticalResponseType Type
        {
            get;
            private set;
        }

        public AICommanderPriority Priority
        {
            get;
            private set;
        }

        public float Confidence
        {
            get;
            private set;
        }

        public string IntentId
        {
            get;
            private set;
        }

        public bool Valid =>
            Type != TacticalResponseType.None &&
            Confidence >= 0.0f &&
            !string.IsNullOrWhiteSpace(
                IntentId);

        public TacticalResponse()
        {
            Clear();
        }

        public void Set(
            TacticalResponseType type,
            AICommanderPriority priority,
            float confidence,
            string intentId)
        {
            Type = type;
            Priority = priority;

            Confidence =
                Clamp01(confidence);

            IntentId =
                intentId ?? string.Empty;
        }

        public void Clear()
        {
            Type =
                TacticalResponseType.None;

            Priority =
                AICommanderPriority.TacticalAdvantage;

            Confidence = 0.0f;
            IntentId = string.Empty;
        }

        private static float Clamp01(
            float value)
        {
            return Math.Max(
                0.0f,
                Math.Min(
                    1.0f,
                    value));
        }
    }

    public sealed class TacticalResponseSelector
    {
        public TacticalResponse Select(
            BattlefieldEvaluation battlefield,
            ForceEvaluation forces,
            StrategicObjective objective)
        {
            if (battlefield == null ||
                !battlefield.Valid ||
                forces == null ||
                !forces.Valid ||
                objective == null ||
                !objective.Valid)
            {
                return null;
            }

            TacticalResponse response =
                new TacticalResponse();

            if (battlefield.ThreatLevel ==
                BattlefieldThreatLevel.Critical)
            {
                response.Set(
                    TacticalResponseType.Withdraw,
                    AICommanderPriority.Survival,
                    1.0f,
                    "TACTICAL_WITHDRAW");

                return response;
            }

            switch (objective.Type)
            {
                case AICommanderObjective.Attack:
                    return SelectAttackResponse(
                        battlefield,
                        forces);

                case AICommanderObjective.Advance:
                    response.Set(
                        TacticalResponseType.Advance,
                        AICommanderPriority.StrategicObjective,
                        0.75f,
                        "TACTICAL_ADVANCE");

                    return response;

                case AICommanderObjective.Defend:
                case AICommanderObjective.Hold:
                    response.Set(
                        TacticalResponseType.HoldPosition,
                        AICommanderPriority.StrategicObjective,
                        0.75f,
                        "TACTICAL_HOLD");

                    return response;

                case AICommanderObjective.Regroup:
                    response.Set(
                        TacticalResponseType.Regroup,
                        AICommanderPriority.ForcePreservation,
                        0.90f,
                        "TACTICAL_REGROUP");

                    return response;

                case AICommanderObjective.Reinforce:
                    response.Set(
                        TacticalResponseType.Reinforce,
                        AICommanderPriority.Logistics,
                        0.85f,
                        "TACTICAL_REINFORCE");

                    return response;

                case AICommanderObjective.Recon:
                    response.Set(
                        TacticalResponseType.Recon,
                        AICommanderPriority.Reconnaissance,
                        0.85f,
                        "TACTICAL_RECON");

                    return response;

                case AICommanderObjective.Withdraw:
                    response.Set(
                        TacticalResponseType.Withdraw,
                        AICommanderPriority.Survival,
                        0.95f,
                        "TACTICAL_WITHDRAW");

                    return response;

                default:
                    response.Set(
                        TacticalResponseType.Screen,
                        AICommanderPriority.TacticalAdvantage,
                        0.50f,
                        "TACTICAL_SCREEN");

                    return response;
            }
        }

        private TacticalResponse SelectAttackResponse(
            BattlefieldEvaluation battlefield,
            ForceEvaluation forces)
        {
            TacticalResponse response =
                new TacticalResponse();

            if (forces.ForceAdvantage >= 0.40f &&
                battlefield.ReconAdvantage >= 0.10f)
            {
                response.Set(
                    TacticalResponseType.ConcentrateForce,
                    AICommanderPriority.TacticalAdvantage,
                    0.90f,
                    "TACTICAL_CONCENTRATE_FORCE");

                return response;
            }

            if (forces.ForceAdvantage >= 0.20f &&
                battlefield.ReconAdvantage < 0.0f)
            {
                response.Set(
                    TacticalResponseType.Recon,
                    AICommanderPriority.Reconnaissance,
                    0.80f,
                    "TACTICAL_RECON_BEFORE_ATTACK");

                return response;
            }

            if (forces.ForceAdvantage >= 0.10f)
            {
                response.Set(
                    TacticalResponseType.Flank,
                    AICommanderPriority.TacticalAdvantage,
                    0.75f,
                    "TACTICAL_FLANK");

                return response;
            }

            if (forces.ForceAdvantage > -0.10f)
            {
                response.Set(
                    TacticalResponseType.Suppress,
                    AICommanderPriority.TacticalAdvantage,
                    0.70f,
                    "TACTICAL_SUPPRESS");

                return response;
            }

            response.Set(
                TacticalResponseType.Regroup,
                AICommanderPriority.ForcePreservation,
                0.80f,
                "TACTICAL_ATTACK_REGROUP");

            return response;
        }
    }

    public sealed class TacticalResponseRegistry
    {
        private readonly List<
            TacticalResponse> responses =
            new List<
                TacticalResponse>();

        public void Add(
            TacticalResponse response)
        {
            if (response == null ||
                !response.Valid)
            {
                return;
            }

            responses.Add(response);
        }

        public TacticalResponse GetHighestConfidence()
        {
            TacticalResponse best = null;

            foreach (TacticalResponse response
                in responses)
            {
                if (best == null ||
                    response.Confidence >
                    best.Confidence)
                {
                    best = response;
                }
            }

            return best;
        }

        public IReadOnlyCollection<
            TacticalResponse>
            GetResponses()
        {
            return responses;
        }

        public void Clear()
        {
            responses.Clear();
        }
    }
}
