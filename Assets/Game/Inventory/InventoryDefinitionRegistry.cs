using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Inventory
{
    public static class InventoryDefinitionRegistry
    {
        private static readonly Dictionary<
            string,
            InventoryItemDefinition> definitions =
            new Dictionary<
                string,
                InventoryItemDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public static bool Register(
            InventoryItemDefinition definition)
        {
            if (definition == null ||
                !definition.IsValid)
            {
                return false;
            }

            definitions[
                definition.DefinitionId] =
                definition;

            return true;
        }

        public static bool Remove(
            string definitionId)
        {
            if (string.IsNullOrWhiteSpace(
                    definitionId))
            {
                return false;
            }

            return definitions.Remove(
                definitionId);
        }

        public static bool TryGet(
            string definitionId,
            out InventoryItemDefinition definition)
        {
            return definitions.TryGetValue(
                definitionId ?? "",
                out definition);
        }

        public static bool Contains(
            string definitionId)
        {
            return !string.IsNullOrWhiteSpace(
                       definitionId) &&
                   definitions.ContainsKey(
                       definitionId);
        }

        public static IReadOnlyCollection<
            InventoryItemDefinition>
            GetAll()
        {
            return definitions.Values;
        }

        public static void Clear()
        {
            definitions.Clear();
        }
    }
}
