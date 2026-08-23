using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class Onboarding
    {
        private readonly List<string> steps =
            new List<string>();

        private int currentStep;

        public bool Initialized { get; private set; }

        public int StepCount =>
            steps.Count;

        public int CurrentStep =>
            currentStep;

        public bool Completed =>
            Initialized &&
            steps.Count > 0 &&
            currentStep >= steps.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            steps.Clear();
            currentStep = 0;

            AddDefaultStep("Welcome");
            AddDefaultStep("Command Basics");
            AddDefaultStep("Squad Intent");
            AddDefaultStep("Unit Deployment");
            AddDefaultStep("Combat Basics");
            AddDefaultStep("Garage");
            AddDefaultStep("Store");
            AddDefaultStep("VR Operator Mode");

            Initialized = true;

            return true;
        }

        public bool Advance()
        {
            if (!Initialized ||
                Completed)
            {
                return false;
            }

            currentStep++;

            return true;
        }

        public bool Skip()
        {
            if (!Initialized ||
                Completed)
            {
                return false;
            }

            currentStep =
                steps.Count;

            return true;
        }

        public string GetCurrentStep()
        {
            if (!Initialized ||
                Completed)
            {
                return null;
            }

            return steps[currentStep];
        }

        public IReadOnlyList<string>
            GetSteps()
        {
            return steps;
        }

        private void AddDefaultStep(
            string step)
        {
            steps.Add(step);
        }

        public void Reset()
        {
            steps.Clear();
            currentStep = 0;
            Initialized = false;
        }
    }
}
