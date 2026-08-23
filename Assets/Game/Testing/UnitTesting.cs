using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class UnitTestResult
    {
        public string TestId { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public UnitTestResult(
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

    public sealed class UnitTesting
    {
        private readonly Dictionary<
            string,
            UnitTestResult> results =
            new Dictionary<
                string,
                UnitTestResult>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TestCount =>
            results.Count;

        public int PassedCount
        {
            get
            {
                int count = 0;

                foreach (UnitTestResult result
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
                new UnitTestResult(id));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            UnitTestResult result =
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
            UnitTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);

            return true;
        }

        public UnitTestResult GetResult(
            string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out UnitTestResult result);

            return result;
        }

        public IReadOnlyCollection<
            UnitTestResult>
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
