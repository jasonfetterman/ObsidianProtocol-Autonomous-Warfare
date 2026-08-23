using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class SecurityAntiCheatTestResult
    {
        public string TestId { get; }

        public string ThreatScenario { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public SecurityAntiCheatTestResult(
            string testId,
            string threatScenario)
        {
            TestId =
                testId ?? string.Empty;

            ThreatScenario =
                threatScenario ?? string.Empty;

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

    public sealed class SecurityAntiCheatTesting
    {
        private readonly Dictionary<
            string,
            SecurityAntiCheatTestResult> results =
            new Dictionary<
                string,
                SecurityAntiCheatTestResult>(
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
                    SecurityAntiCheatTestResult result
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
            string threatScenario)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId) ||
                string.IsNullOrWhiteSpace(threatScenario))
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
                new SecurityAntiCheatTestResult(
                    id,
                    threatScenario.Trim()));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            SecurityAntiCheatTestResult result =
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
            SecurityAntiCheatTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);

            return true;
        }

        public SecurityAntiCheatTestResult
            GetResult(string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out SecurityAntiCheatTestResult result);

            return result;
        }

        public IReadOnlyCollection<
            SecurityAntiCheatTestResult>
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
