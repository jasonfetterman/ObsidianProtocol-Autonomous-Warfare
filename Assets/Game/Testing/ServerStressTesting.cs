using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Testing
{
    public sealed class ServerStressTestResult
    {
        public string TestId { get; }

        public int SimulatedPlayers { get; }

        public int SimulatedUnits { get; }

        public bool Passed { get; private set; }

        public string Message { get; private set; }

        public ServerStressTestResult(
            string testId,
            int simulatedPlayers,
            int simulatedUnits)
        {
            TestId =
                testId ?? string.Empty;

            SimulatedPlayers =
                Math.Max(0, simulatedPlayers);

            SimulatedUnits =
                Math.Max(0, simulatedUnits);

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

    public sealed class ServerStressTesting
    {
        private readonly Dictionary<
            string,
            ServerStressTestResult> results =
            new Dictionary<
                string,
                ServerStressTestResult>(
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
                    ServerStressTestResult result
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
            int simulatedPlayers,
            int simulatedUnits)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId) ||
                simulatedPlayers < 0 ||
                simulatedUnits < 0)
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
                new ServerStressTestResult(
                    id,
                    simulatedPlayers,
                    simulatedUnits));

            return true;
        }

        public bool PassTest(
            string testId,
            string message)
        {
            ServerStressTestResult result =
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
            ServerStressTestResult result =
                GetResult(testId);

            if (result == null)
            {
                return false;
            }

            result.Fail(message);

            return true;
        }

        public ServerStressTestResult GetResult(
            string testId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(testId))
            {
                return null;
            }

            results.TryGetValue(
                testId.Trim(),
                out ServerStressTestResult result);

            return result;
        }

        public IReadOnlyCollection<
            ServerStressTestResult>
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
