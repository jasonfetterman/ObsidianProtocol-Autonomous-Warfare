using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Resources
{
    public sealed class ResourceProcessingRecipe
    {
        public string RecipeId { get; }
        public string InputResourceId { get; }
        public int InputAmount { get; }
        public string OutputResourceId { get; }
        public int OutputAmount { get; }

        public ResourceProcessingRecipe(
            string recipeId,
            string inputResourceId,
            int inputAmount,
            string outputResourceId,
            int outputAmount)
        {
            RecipeId =
                recipeId ?? string.Empty;

            InputResourceId =
                inputResourceId ?? string.Empty;

            InputAmount =
                Math.Max(0, inputAmount);

            OutputResourceId =
                outputResourceId ?? string.Empty;

            OutputAmount =
                Math.Max(0, outputAmount);
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(RecipeId) &&
            !string.IsNullOrWhiteSpace(InputResourceId) &&
            !string.IsNullOrWhiteSpace(OutputResourceId) &&
            InputAmount > 0 &&
            OutputAmount > 0;
    }

    public sealed class ResourceProcessingSystem
    {
        private readonly Dictionary<string, ResourceProcessingRecipe> recipes =
            new Dictionary<string, ResourceProcessingRecipe>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterRecipe(
            ResourceProcessingRecipe recipe)
        {
            if (recipe == null ||
                !recipe.Valid ||
                recipes.ContainsKey(recipe.RecipeId))
            {
                return false;
            }

            recipes.Add(
                recipe.RecipeId,
                recipe);

            return true;
        }

        public bool RemoveRecipe(
            string recipeId)
        {
            if (string.IsNullOrWhiteSpace(recipeId))
            {
                return false;
            }

            return recipes.Remove(recipeId);
        }

        public bool TryGetRecipe(
            string recipeId,
            out ResourceProcessingRecipe recipe)
        {
            return recipes.TryGetValue(
                recipeId,
                out recipe);
        }

        public bool CanProcess(
            string recipeId,
            ResourceInventory inventory)
        {
            if (inventory == null ||
                !recipes.TryGetValue(
                    recipeId,
                    out ResourceProcessingRecipe recipe))
            {
                return false;
            }

            return inventory.GetAmount(
                recipe.InputResourceId) >=
                recipe.InputAmount;
        }

        public bool TryProcess(
            string recipeId,
            ResourceInventory inventory)
        {
            if (inventory == null ||
                !recipes.TryGetValue(
                    recipeId,
                    out ResourceProcessingRecipe recipe))
            {
                return false;
            }

            if (!CanProcess(
                    recipeId,
                    inventory))
            {
                return false;
            }

            if (!inventory.TrySpend(
                    recipe.InputResourceId,
                    recipe.InputAmount))
            {
                return false;
            }

            inventory.Add(
                recipe.OutputResourceId,
                recipe.OutputAmount);

            return true;
        }

        public IReadOnlyCollection<ResourceProcessingRecipe>
            GetRecipes()
        {
            return recipes.Values;
        }
    }
}
