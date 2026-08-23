using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class AutonomousBehaviorTestResult
    {
        public string TestId { get; }

        public string BehaviorId { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public AutonomousBehaviorTestResult(
            string testId,
            string behaviorId)
        {
            TestId =
                testId ?? string.Empty;

            BehaviorId =
                behaviorId ?? string.Empty;

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

    public sealed class AutonomousBehaviorTesting
    {
        private readonly Dictionary<
            string,
            AutonomousBehaviorTestResult> results =
            new Dictionary<
                string,
                AutonomousBehaviorTestResult>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TestCount =>
            results.Count;

        public int PassedCount
        {
            get
            {
                int count = 0;

                foreach (
                    AutonomousBehaviorTestResult result
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
            string behaviorId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId) ||
                string.IsNullOrWhiteSpace(behaviorId))
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
                new AutonomousBehaviorTestResult(
                    id,
                    behaviorId.Trim()));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            AutonomousBehaviorTestResult result =
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
            AutonomousBehaviorTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);

            return true;
        }

        public AutonomousBehaviorTestResult
            GetResult(
                string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out AutonomousBehaviorTestResult result);

            return result;
        }

        public IReadOnlyCollection<
            AutonomousBehaviorTestResult>
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
