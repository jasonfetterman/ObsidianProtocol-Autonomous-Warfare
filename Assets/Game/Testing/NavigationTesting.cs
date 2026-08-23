using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class NavigationTestResult
    {
        public string TestId { get; }

        public string RouteId { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public NavigationTestResult(
            string testId,
            string routeId)
        {
            TestId =
                testId ?? string.Empty;

            RouteId =
                routeId ?? string.Empty;

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

    public sealed class NavigationTesting
    {
        private readonly Dictionary<
            string,
            NavigationTestResult> results =
            new Dictionary<
                string,
                NavigationTestResult>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TestCount =>
            results.Count;

        public int PassedCount
        {
            get
            {
                int count = 0;

                foreach (NavigationTestResult result
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
            string routeId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId) ||
                string.IsNullOrWhiteSpace(routeId))
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
                new NavigationTestResult(
                    id,
                    routeId.Trim()));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            NavigationTestResult result =
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
            NavigationTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);

            return true;
        }

        public NavigationTestResult GetResult(
            string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out NavigationTestResult result);

            return result;
        }

        public IReadOnlyCollection<
            NavigationTestResult>
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
