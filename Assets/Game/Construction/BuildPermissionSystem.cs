using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Construction
{
    public enum BuildPermissionState
    {
        Denied,
        Allowed
    }

    public sealed class BuildPermission
    {
        public string PermissionId { get; }

        public string FactionId { get; }

        public string StructureId { get; }

        public BuildPermissionState State { get; private set; }

        public BuildPermission(
            string permissionId,
            string factionId,
            string structureId,
            BuildPermissionState state)
        {
            PermissionId =
                permissionId ?? string.Empty;

            FactionId =
                factionId ?? string.Empty;

            StructureId =
                structureId ?? string.Empty;

            State =
                state;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(PermissionId) &&
            !string.IsNullOrWhiteSpace(FactionId) &&
            !string.IsNullOrWhiteSpace(StructureId);

        public bool Allowed =>
            State ==
            BuildPermissionState.Allowed;

        public void SetAllowed(
            bool allowed)
        {
            State =
                allowed
                    ? BuildPermissionState.Allowed
                    : BuildPermissionState.Denied;
        }
    }

    public sealed class BuildPermissionSystem
    {
        private readonly Dictionary<string, BuildPermission>
            permissions =
                new Dictionary<string, BuildPermission>(
                    StringComparer.OrdinalIgnoreCase);

        public bool RegisterPermission(
            BuildPermission permission)
        {
            if (permission == null ||
                !permission.Valid ||
                permissions.ContainsKey(
                    permission.PermissionId))
            {
                return false;
            }

            permissions.Add(
                permission.PermissionId,
                permission);

            return true;
        }

        public bool RemovePermission(
            string permissionId)
        {
            if (string.IsNullOrWhiteSpace(
                    permissionId))
            {
                return false;
            }

            return permissions.Remove(
                permissionId);
        }

        public bool TryGetPermission(
            string permissionId,
            out BuildPermission permission)
        {
            return permissions.TryGetValue(
                permissionId,
                out permission);
        }

        public bool SetPermission(
            string permissionId,
            bool allowed)
        {
            if (!permissions.TryGetValue(
                    permissionId,
                    out BuildPermission permission))
            {
                return false;
            }

            permission.SetAllowed(
                allowed);

            return true;
        }

        public bool CanBuild(
            string factionId,
            string structureId)
        {
            if (string.IsNullOrWhiteSpace(
                    factionId) ||
                string.IsNullOrWhiteSpace(
                    structureId))
            {
                return false;
            }

            foreach (
                BuildPermission permission
                in permissions.Values)
            {
                if (!permission.Allowed)
                {
                    continue;
                }

                if (string.Equals(
                        permission.FactionId,
                        factionId,
                        StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(
                        permission.StructureId,
                        structureId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public IReadOnlyCollection<BuildPermission>
            GetPermissions()
        {
            return permissions.Values;
        }

        public IReadOnlyCollection<BuildPermission>
            GetAllowedPermissions()
        {
            List<BuildPermission> allowed =
                new List<BuildPermission>();

            foreach (
                BuildPermission permission
                in permissions.Values)
            {
                if (permission.Allowed)
                {
                    allowed.Add(
                        permission);
                }
            }

            return allowed;
        }

        public void Clear()
        {
            permissions.Clear();
        }
    }
}
