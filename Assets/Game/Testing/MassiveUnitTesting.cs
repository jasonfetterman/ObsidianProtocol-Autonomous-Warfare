using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class MassiveUnitTestResult
    {
        public string TestId { get; }

        public int UnitCount { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public MassiveUnitTestResult(
            string testId,
            int unitCount)
        {
            TestId =
                testId ?? string.Empty;

            UnitCount =
                Math.Max(0, unitCount);

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

    public sealed class MassiveUnitTesting
    {
        private readonly Dictionary<
            string,
            MassiveUnitTestResult> results =
            new Dictionary<
                string,
                MassiveUnitTestResult>(
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
                    MassiveUnitTestResult result
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

        public int MaximumTestedUnitCount
        {
            get
            {
                int maximum = 0;

                foreach (
                    MassiveUnitTestResult result
                    in results.Values)
                {
                    if (result.UnitCount > maximum)
                    {
                        maximum = result.UnitCount;
                    }
                }

                return maximum;
            }
        }

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
            int unitCount)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId) ||
                unitCount < 0)
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
                new MassiveUnitTestResult(
                    id,
                    unitCount));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            MassiveUnitTestResult result =
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
            MassiveUnitTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);

            return true;
        }

        public MassiveUnitTestResult GetResult(
            string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out MassiveUnitTestResult result);

            return result;
        }

        public IReadOnlyCollection<
            MassiveUnitTestResult>
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
