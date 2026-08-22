using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public enum AerialFormationType
    {
        Line,
        Column,
        Wedge,
        Vee,
        Diamond,
        Screen,
        Escort,
        Reconnaissance
    }

    public sealed class AerialFormationMember
    {
        public string UnitId { get; }
        public float OffsetX { get; private set; }
        public float OffsetY { get; private set; }
        public float OffsetZ { get; private set; }

        public AerialFormationMember(string unitId)
        {
            UnitId = unitId ?? string.Empty;
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

    public sealed class AerialFormation
    {
        public string FormationId { get; }
        public AerialFormationType Type { get; private set; }

        private readonly Dictionary<string, AerialFormationMember> members =
            new Dictionary<string, AerialFormationMember>(
                StringComparer.OrdinalIgnoreCase);

        public AerialFormation(
            string formationId,
            AerialFormationType type)
        {
            FormationId = formationId ?? string.Empty;
            Type = type;
        }

        public void SetType(
            AerialFormationType type)
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

            AerialFormationMember member =
                new AerialFormationMember(unitId);

            member.SetOffset(
                offsetX,
                offsetY,
                offsetZ);

            members[unitId] = member;
        }

        public void RemoveMember(string unitId)
        {
            members.Remove(unitId);
        }

        public bool TryGetMember(
            string unitId,
            out AerialFormationMember member)
        {
            return members.TryGetValue(
                unitId,
                out member);
        }

        public IReadOnlyCollection<AerialFormationMember> GetMembers()
        {
            return members.Values;
        }
    }

    public sealed class AerialFormationSystem
    {
        private readonly Dictionary<string, AerialFormation> formations =
            new Dictionary<string, AerialFormation>(
                StringComparer.OrdinalIgnoreCase);

        public void CreateFormation(
            string formationId,
            AerialFormationType type)
        {
            if (string.IsNullOrWhiteSpace(formationId))
            {
                return;
            }

            formations[formationId] =
                new AerialFormation(
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
                    out AerialFormation formation))
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
                    out AerialFormation formation))
            {
                formation.RemoveMember(unitId);
            }
        }

        public bool TryGetFormation(
            string formationId,
            out AerialFormation formation)
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
