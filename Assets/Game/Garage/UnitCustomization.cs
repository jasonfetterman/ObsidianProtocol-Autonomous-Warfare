using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public sealed class UnitCustomization
    {
        private readonly Dictionary<string, string> values =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

        public string OwnershipId { get; }

        public bool Locked { get; private set; }

        public UnitCustomization(
            string ownershipId)
        {
            OwnershipId =
                ownershipId ?? string.Empty;

            Locked = false;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                OwnershipId);

        public bool Set(
            string customizationId,
            string value)
        {
            if (Locked ||
                string.IsNullOrWhiteSpace(
                    customizationId))
            {
                return false;
            }

            values[customizationId] =
                value ?? string.Empty;

            return true;
        }

        public bool TryGet(
            string customizationId,
            out string value)
        {
            return values.TryGetValue(
                customizationId,
                out value);
        }

        public bool Remove(
            string customizationId)
        {
            if (Locked ||
                string.IsNullOrWhiteSpace(
                    customizationId))
            {
                return false;
            }

            return values.Remove(
                customizationId);
        }

        public void Lock()
        {
            Locked = true;
        }

        public void Unlock()
        {
            Locked = false;
        }

        public IReadOnlyDictionary<
            string,
            string>
            GetValues()
        {
            return values;
        }

        public void Clear()
        {
            if (!Locked)
                values.Clear();
        }
    }

    public sealed class UnitCustomizationRegistry
    {
        private readonly Dictionary<
            string,
            UnitCustomization> customizations =
            new Dictionary<
                string,
                UnitCustomization>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            UnitCustomization customization)
        {
            if (customization == null ||
                !customization.Valid ||
                customizations.ContainsKey(
                    customization.OwnershipId))
            {
                return false;
            }

            customizations.Add(
                customization.OwnershipId,
                customization);

            return true;
        }

        public bool Remove(
            string ownershipId)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId))
            {
                return false;
            }

            return customizations.Remove(
                ownershipId);
        }

        public bool TryGet(
            string ownershipId,
            out UnitCustomization customization)
        {
            return customizations.TryGetValue(
                ownershipId,
                out customization);
        }

        public IReadOnlyCollection<
            UnitCustomization>
            GetCustomizations()
        {
            return customizations.Values;
        }

        public void Clear()
        {
            customizations.Clear();
        }
    }
}
