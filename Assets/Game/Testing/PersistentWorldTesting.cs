using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class PersistentWorldTestResult
    {
        public string TestId { get; }

        public string PersistenceArea { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public PersistentWorldTestResult(
            string testId,
            string persistenceArea)
        {
            TestId =
                testId ?? string.Empty;

            PersistenceArea =
                persistenceArea ?? string.Empty;

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

    public sealed class PersistentWorldTesting
    {
        private readonly Dictionary<
            string,
            PersistentWorldTestResult> results =
            new Dictionary<
                string,
                PersistentWorldTestResult>(
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
                    PersistentWorldTestResult result
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
            string persistenceArea)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId) ||
                string.IsNullOrWhiteSpace(persistenceArea))
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
                new PersistentWorldTestResult(
                    id,
                    persistenceArea.Trim()));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            PersistentWorldTestResult result =
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
            PersistentWorldTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);

            return true;
        }

        public PersistentWorldTestResult
            GetResult(string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out PersistentWorldTestResult result);

            return result;
        }

        public IReadOnlyCollection<
            PersistentWorldTestResult>
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
