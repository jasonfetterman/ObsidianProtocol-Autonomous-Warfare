using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class ConstructionTestResult
    {
        public string TestId { get; }

        public string StructureId { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public ConstructionTestResult(
            string testId,
            string structureId)
        {
            TestId =
                testId ?? string.Empty;

            StructureId =
                structureId ?? string.Empty;

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

    public sealed class ConstructionTesting
    {
        private readonly Dictionary<
            string,
            ConstructionTestResult> results =
            new Dictionary<
                string,
                ConstructionTestResult>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TestCount =>
            results.Count;

        public int PassedCount
        {
            get
            {
                int count = 0;

                foreach (ConstructionTestResult result
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
            string structureId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId) ||
                string.IsNullOrWhiteSpace(structureId))
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
                new ConstructionTestResult(
                    id,
                    structureId.Trim()));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            ConstructionTestResult result =
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
            ConstructionTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);

            return true;
        }

        public ConstructionTestResult GetResult(
            string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out ConstructionTestResult result);

            return result;
        }

        public IReadOnlyCollection<
            ConstructionTestResult>
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
