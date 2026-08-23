using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public sealed class MassiveBattleTest
    {
        public string TestId { get; }

        public int TotalUnits { get; private set; }

        public int ActiveUnits { get; private set; }

        public float DurationSeconds { get; private set; }

        public float AverageSimulationTimeMilliseconds
        {
            get;
            private set;
        }

        public float MaximumSimulationTimeMilliseconds
        {
            get;
            private set;
        }

        public bool Passed { get; private set; }

        public MassiveBattleTest(
            string testId,
            int totalUnits)
        {
            TestId =
                testId ?? string.Empty;

            TotalUnits =
                Math.Max(
                    0,
                    totalUnits);

            ActiveUnits = 0;

            DurationSeconds = 0f;

            AverageSimulationTimeMilliseconds = 0f;

            MaximumSimulationTimeMilliseconds = 0f;

            Passed = false;
        }

        public bool SetActiveUnits(
            int activeUnits)
        {
            if (activeUnits < 0 ||
                activeUnits > TotalUnits)
            {
                return false;
            }

            ActiveUnits =
                activeUnits;

            return true;
        }

        public bool Complete(
            float durationSeconds,
            float averageSimulationTimeMilliseconds,
            float maximumSimulationTimeMilliseconds,
            float maximumAllowedSimulationTimeMilliseconds)
        {
            if (durationSeconds < 0f ||
                averageSimulationTimeMilliseconds < 0f ||
                maximumSimulationTimeMilliseconds < 0f ||
                maximumAllowedSimulationTimeMilliseconds <= 0f)
            {
                return false;
            }

            DurationSeconds =
                durationSeconds;

            AverageSimulationTimeMilliseconds =
                averageSimulationTimeMilliseconds;

            MaximumSimulationTimeMilliseconds =
                maximumSimulationTimeMilliseconds;

            Passed =
                AverageSimulationTimeMilliseconds <=
                    maximumAllowedSimulationTimeMilliseconds &&
                MaximumSimulationTimeMilliseconds <=
                    maximumAllowedSimulationTimeMilliseconds;

            return true;
        }
    }

    public sealed class MassiveBattleTesting
    {
        private readonly Dictionary<
            string,
            MassiveBattleTest> tests =
            new Dictionary<
                string,
                MassiveBattleTest>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TestCount =>
            tests.Count;

        public int PassedTestCount
        {
            get
            {
                int count = 0;

                foreach (MassiveBattleTest test
                         in tests.Values)
                {
                    if (test.Passed)
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

            tests.Clear();

            Initialized = true;

            return true;
        }

        public bool BeginTest(
            string testId,
            int totalUnits)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId) ||
                totalUnits < 0)
            {
                return false;
            }

            string id =
                testId.Trim();

            if (tests.ContainsKey(id))
            {
                return false;
            }

            tests.Add(
                id,
                new MassiveBattleTest(
                    id,
                    totalUnits));

            return true;
        }

        public bool SetActiveUnits(
            string testId,
            int activeUnits)
        {
            MassiveBattleTest test =
                GetTest(testId);

            return test != null &&
                   test.SetActiveUnits(activeUnits);
        }

        public bool CompleteTest(
            string testId,
            float durationSeconds,
            float averageSimulationTimeMilliseconds,
            float maximumSimulationTimeMilliseconds,
            float maximumAllowedSimulationTimeMilliseconds)
        {
            MassiveBattleTest test =
                GetTest(testId);

            return test != null &&
                   test.Complete(
                       durationSeconds,
                       averageSimulationTimeMilliseconds,
                       maximumSimulationTimeMilliseconds,
                       maximumAllowedSimulationTimeMilliseconds);
        }

        public MassiveBattleTest GetTest(
            string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            tests.TryGetValue(
                testId.Trim(),
                out MassiveBattleTest test);

            return test;
        }

        public IReadOnlyCollection<
            MassiveBattleTest>
            GetTests()
        {
            return tests.Values;
        }

        public void Reset()
        {
            tests.Clear();

            Initialized = false;
        }
    }
}
