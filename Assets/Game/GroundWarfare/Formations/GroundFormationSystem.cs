using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public enum GroundFormationType
    {
        Line,
        Column,
        Wedge,
        Vee,
        Box,
        Convoy,
        Screen,
        DefensiveRing
    }

    public sealed class GroundFormationMember
    {
        public string UnitId { get; }

        public float OffsetX { get; private set; }
        public float OffsetY { get; private set; }
        public float OffsetZ { get; private set; }

        public GroundFormationMember(string unitId)
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

    public sealed class GroundFormation
    {
        public string FormationId { get; }
        public GroundFormationType Type { get; private set; }

        private readonly Dictionary<string, GroundFormationMember> members =
            new Dictionary<string, GroundFormationMember>(
                StringComparer.OrdinalIgnoreCase);

        public GroundFormation(
            string formationId,
            GroundFormationType type)
        {
            FormationId = formationId ?? string.Empty;
            Type = type;
        }

        public void SetType(
            GroundFormationType type)
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

            GroundFormationMember member =
                new GroundFormationMember(unitId);

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
            out GroundFormationMember member)
        {
            return members.TryGetValue(
                unitId,
                out member);
        }

        public IReadOnlyCollection<GroundFormationMember> GetMembers()
        {
            return members.Values;
        }
    }

    public sealed class GroundFormationSystem
    {
        private readonly Dictionary<string, GroundFormation> formations =
            new Dictionary<string, GroundFormation>(
                StringComparer.OrdinalIgnoreCase);

        public void CreateFormation(
            string formationId,
            GroundFormationType type)
        {
            if (string.IsNullOrWhiteSpace(formationId))
            {
                return;
            }

            formations[formationId] =
                new GroundFormation(
                    formationId,
                    type);
        }

        public void AddVehicle(
            string formationId,
            string unitId,
            float offsetX,
            float offsetY,
            float offsetZ)
        {
            if (!formations.TryGetValue(
                    formationId,
                    out GroundFormation formation))
            {
                return;
            }

            formation.AddMember(
                unitId,
                offsetX,
                offsetY,
                offsetZ);
        }

        public void RemoveVehicle(
            string formationId,
            string unitId)
        {
            if (formations.TryGetValue(
                    formationId,
                    out GroundFormation formation))
            {
                formation.RemoveMember(unitId);
            }
        }

        public bool TryGetFormation(
            string formationId,
            out GroundFormation formation)
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
