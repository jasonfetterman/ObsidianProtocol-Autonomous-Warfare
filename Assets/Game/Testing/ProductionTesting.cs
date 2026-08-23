using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class ProductionTestResult
    {
        public string TestId { get; }

        public string ProductionId { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public ProductionTestResult(
            string testId,
            string productionId)
        {
            TestId =
                testId ?? string.Empty;

            ProductionId =
                productionId ?? string.Empty;

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

    public sealed class ProductionTesting
    {
        private readonly Dictionary<
            string,
            ProductionTestResult> results =
            new Dictionary<
                string,
                ProductionTestResult>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TestCount => results.Count;

        public int PassedCount
        {
            get
            {
                int count = 0;

                foreach (ProductionTestResult result in results.Values)
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
            string productionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId) ||
                string.IsNullOrWhiteSpace(productionId))
            {
                return false;
            }

            string id = testId.Trim();

            if (results.ContainsKey(id))
            {
                return false;
            }

            results.Add(
                id,
                new ProductionTestResult(
                    id,
                    productionId.Trim()));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            ProductionTestResult result =
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
            ProductionTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);
            return true;
        }

        public ProductionTestResult GetResult(
            string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out ProductionTestResult result);

            return result;
        }

        public IReadOnlyCollection<
            ProductionTestResult>
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
