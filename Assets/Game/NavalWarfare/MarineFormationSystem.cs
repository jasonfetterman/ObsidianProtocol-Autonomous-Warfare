using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public enum MarineFormationType
    {
        Line,
        Column,
        Wedge,
        Screen,
        Patrol,
        Convoy,
        DefensiveRing
    }

    public sealed class MarineFormationMember
    {
        public string UnitId { get; }

        public float OffsetX { get; private set; }
        public float OffsetY { get; private set; }
        public float OffsetZ { get; private set; }

        public MarineFormationMember(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;
        }

        public void SetOffset(
            float x,
            float y,
            float z)
        {
            OffsetX = x;
            OffsetY = y;
            OffsetZ = z;
        }
    }

    public sealed class MarineFormation
    {
        public string FormationId { get; }
        public MarineFormationType Type { get; private set; }

        private readonly Dictionary<string, MarineFormationMember> members =
            new Dictionary<string, MarineFormationMember>(
                StringComparer.OrdinalIgnoreCase);

        public MarineFormation(
            string formationId,
            MarineFormationType type)
        {
            FormationId =
                formationId ?? string.Empty;

            Type =
                type;
        }

        public void SetType(
            MarineFormationType type)
        {
            Type = type;
        }

        public void AddMember(
            string unitId,
            float offsetX,
            float offsetY,
            float offsetZ)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            MarineFormationMember member =
                new MarineFormationMember(unitId);

            member.SetOffset(
                offsetX,
                offsetY,
                offsetZ);

            members[unitId] = member;
        }

        public void RemoveMember(
            string unitId)
        {
            members.Remove(unitId);
        }

        public bool TryGetMember(
            string unitId,
            out MarineFormationMember member)
        {
            return members.TryGetValue(
                unitId,
                out member);
        }

        public IReadOnlyCollection<MarineFormationMember> GetMembers()
        {
            return members.Values;
        }
    }

    public sealed class MarineFormationSystem
    {
        private readonly Dictionary<string, MarineFormation> formations =
            new Dictionary<string, MarineFormation>(
                StringComparer.OrdinalIgnoreCase);

        public void CreateFormation(
            string formationId,
            MarineFormationType type)
        {
            if (string.IsNullOrWhiteSpace(formationId))
            {
                return;
            }

            formations[formationId] =
                new MarineFormation(
                    formationId,
                    type);
        }

        public void AddUnit(
            string formationId,
            string unitId,
            float offsetX,
            float offsetY,
            float offsetZ)
        {
            if (!formations.TryGetValue(
                    formationId,
                    out MarineFormation formation))
            {
                return;
            }

            formation.AddMember(
                unitId,
                offsetX,
                offsetY,
                offsetZ);
        }

        public void RemoveUnit(
            string formationId,
            string unitId)
        {
            if (formations.TryGetValue(
                    formationId,
                    out MarineFormation formation))
            {
                formation.RemoveMember(unitId);
            }
        }

        public bool TryGetFormation(
            string formationId,
            out MarineFormation formation)
        {
            return formations.TryGetValue(
                formationId,
                out formation);
        }

        public void RemoveFormation(
            string formationId)
        {
            formations.Remove(formationId);
        }

        public void Clear()
        {
            formations.Clear();
        }
    }
}
