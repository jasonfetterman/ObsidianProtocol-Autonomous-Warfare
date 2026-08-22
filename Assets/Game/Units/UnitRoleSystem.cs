using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public enum UnitRole
    {
        Combat,
        Reconnaissance,
        Surveillance,
        Support,
        Logistics,
        Transport,
        Engineering,
        Command,
        Coordination,
        Communication,
        SearchAndRescue,
        ElectronicWarfare,
        Prototype
    }

    public sealed class UnitRoleAssignment
    {
        public string UnitId { get; }
        public UnitRole PrimaryRole { get; private set; }

        private readonly HashSet<UnitRole> secondaryRoles =
            new HashSet<UnitRole>();

        public UnitRoleAssignment(
            string unitId,
            UnitRole primaryRole)
        {
            UnitId = unitId ?? string.Empty;
            PrimaryRole = primaryRole;
        }

        public void SetPrimaryRole(UnitRole role)
        {
            PrimaryRole = role;
            secondaryRoles.Remove(role);
        }

        public void AddSecondaryRole(UnitRole role)
        {
            if (role == PrimaryRole)
            {
                return;
            }

            secondaryRoles.Add(role);
        }

        public void RemoveSecondaryRole(UnitRole role)
        {
            secondaryRoles.Remove(role);
        }

        public bool HasRole(UnitRole role)
        {
            return PrimaryRole == role ||
                   secondaryRoles.Contains(role);
        }

        public IReadOnlyCollection<UnitRole> GetSecondaryRoles()
        {
            return secondaryRoles;
        }
    }

    public sealed class UnitRoleSystem
    {
        private readonly Dictionary<string, UnitRoleAssignment> assignments =
            new Dictionary<string, UnitRoleAssignment>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId,
            UnitRole primaryRole)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            assignments[unitId] =
                new UnitRoleAssignment(
                    unitId,
                    primaryRole);
        }

        public bool TryGetAssignment(
            string unitId,
            out UnitRoleAssignment assignment)
        {
            return assignments.TryGetValue(
                unitId,
                out assignment);
        }

        public bool HasRole(
            string unitId,
            UnitRole role)
        {
            return assignments.TryGetValue(
                       unitId,
                       out UnitRoleAssignment assignment) &&
                   assignment.HasRole(role);
        }

        public void RemoveUnit(string unitId)
        {
            assignments.Remove(unitId);
        }

        public void Clear()
        {
            assignments.Clear();
        }
    }
}
