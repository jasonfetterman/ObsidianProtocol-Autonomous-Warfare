using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class FullSystemPlaytestResult
    {
        public string TestId { get; }

        public string BuildId { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public FullSystemPlaytestResult(
            string testId,
            string buildId)
        {
            TestId =
                testId ?? string.Empty;

            BuildId =
                buildId ?? string.Empty;

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

    public sealed class FullSystemPlaytesting
    {
        private readonly Dictionary<
            string,
            FullSystemPlaytestResult> results =
            new Dictionary<
                string,
                FullSystemPlaytestResult>(
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
                    FullSystemPlaytestResult result
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

        public bool ReadyForRelease =>
            Initialized &&
            TestCount > 0 &&
            FailedCount == 0;

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
            string buildId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId) ||
                string.IsNullOrWhiteSpace(buildId))
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
                new FullSystemPlaytestResult(
                    id,
                    buildId.Trim()));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            FullSystemPlaytestResult result =
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
            FullSystemPlaytestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);

            return true;
        }

        public FullSystemPlaytestResult GetResult(
            string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out FullSystemPlaytestResult result);

            return result;
        }

        public IReadOnlyCollection<
            FullSystemPlaytestResult>
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
