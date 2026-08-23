using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class CombatTestResult
    {
        public string TestId { get; }

        public string ScenarioId { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public CombatTestResult(
            string testId,
            string scenarioId)
        {
            TestId =
                testId ?? string.Empty;

            ScenarioId =
                scenarioId ?? string.Empty;

            Passed = false;
            Message = string.Empty;
        }

        public void Pass(
            string message)
        {
            Passed = true;
            Message =
                message ?? string.Empty;
        }

        public void Fail(
            string message)
        {
            Passed = false;
            Message =
                message ?? string.Empty;
        }
    }

    public sealed class CombatTesting
    {
        private readonly Dictionary<
            string,
            CombatTestResult> results =
            new Dictionary<
                string,
                CombatTestResult>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TestCount =>
            results.Count;

        public int PassedCount
        {
            get
            {
                int count = 0;

                foreach (CombatTestResult result
                         in results.Values)
                {
                    if (result.Passed)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public int FailedCount =>
            TestCount - PassedCount;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            results.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterTest(
            string testId,
            string scenarioId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId) ||
                string.IsNullOrWhiteSpace(scenarioId))
            {
                return false;
            }

            string id =
                testId.Trim();

            if (results.ContainsKey(id))
            {
                return false;
            }

            results.Add(
                id,
                new CombatTestResult(
                    id,
                    scenarioId.Trim()));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            CombatTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Pass(message);

            return true;
        }

        public bool FailTest(
            string testId,
            string message)
        {
            CombatTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);

            return true;
        }

        public CombatTestResult GetResult(
            string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out CombatTestResult result);

            return result;
        }

        public IReadOnlyCollection<
            CombatTestResult>
            GetResults()
        {
            return results.Values;
        }

        public void Reset()
        {
            results.Clear();
            Initialized = false;
        }
    }
}
