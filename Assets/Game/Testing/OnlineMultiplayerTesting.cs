using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class OnlineMultiplayerTestResult
    {
        public string TestId { get; }

        public string ScenarioId { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public OnlineMultiplayerTestResult(
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

        public void Pass(string message)
        {
            Passed = true;
            Message = message ?? string.Empty;
        }

        public void Fail(string message)
        {
            Passed = false;
            Message = message ?? string.Empty;
        }
    }

    public sealed class OnlineMultiplayerTesting
    {
        private readonly Dictionary<
            string,
            OnlineMultiplayerTestResult> results =
            new Dictionary<
                string,
                OnlineMultiplayerTestResult>(
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
                    OnlineMultiplayerTestResult result
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
                new OnlineMultiplayerTestResult(
                    id,
                    scenarioId.Trim()));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            OnlineMultiplayerTestResult result =
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
            OnlineMultiplayerTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);

            return true;
        }

        public OnlineMultiplayerTestResult
            GetResult(string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out OnlineMultiplayerTestResult result);

            return result;
        }

        public IReadOnlyCollection<
            OnlineMultiplayerTestResult>
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
