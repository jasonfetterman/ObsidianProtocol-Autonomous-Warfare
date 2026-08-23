using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class SystemTestResult
    {
        public string TestId { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public SystemTestResult(
            string testId)
        {
            TestId =
                testId ?? string.Empty;

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

    public sealed class SystemTesting
    {
        private readonly Dictionary<
            string,
            SystemTestResult> results =
            new Dictionary<
                string,
                SystemTestResult>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TestCount =>
            results.Count;

        public int PassedCount
        {
            get
            {
                int count = 0;

                foreach (SystemTestResult result
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
            string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
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
                new SystemTestResult(id));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            SystemTestResult result =
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
            SystemTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);

            return true;
        }

        public SystemTestResult GetResult(
            string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out SystemTestResult result);

            return result;
        }

        public IReadOnlyCollection<
            SystemTestResult>
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
