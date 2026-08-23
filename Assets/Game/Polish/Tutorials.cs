using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class TutorialStep
    {
        public string StepId { get; }

        public string Description { get; }

        public bool Completed { get; private set; }

        public TutorialStep(
            string stepId,
            string description)
        {
            StepId =
                stepId ?? string.Empty;

            Description =
                description ?? string.Empty;

            Completed = false;
        }

        public bool Complete()
        {
            if (Completed)
            {
                return false;
            }

            Completed = true;

            return true;
        }
    }

    public sealed class Tutorials
    {
        private readonly Dictionary<
            string,
            TutorialStep> steps =
            new Dictionary<
                string,
                TutorialStep>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int StepCount =>
            steps.Count;

        public int CompletedStepCount
        {
            get
            {
                int count = 0;

                foreach (TutorialStep step
                         in steps.Values)
                {
                    if (step.Completed)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            steps.Clear();
            Initialized = true;

            return true;
        }

        public bool AddStep(
            string stepId,
            string description)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(stepId) ||
                string.IsNullOrWhiteSpace(description))
            {
                return false;
            }

            string id =
                stepId.Trim();

            if (steps.ContainsKey(id))
            {
                return false;
            }

            steps.Add(
                id,
                new TutorialStep(
                    id,
                    description.Trim()));

            return true;
        }

        public bool CompleteStep(
            string stepId)
        {
            TutorialStep step =
                GetStep(stepId);

            return step != null &&
                   step.Complete();
        }

        public TutorialStep GetStep(
            string stepId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(stepId))
            {
                return null;
            }

            steps.TryGetValue(
                stepId.Trim(),
                out TutorialStep step);

            return step;
        }

        public IReadOnlyCollection<
            TutorialStep>
            GetSteps()
        {
            return steps.Values;
        }

        public void Reset()
        {
            steps.Clear();
            Initialized = false;
        }
    }
}
