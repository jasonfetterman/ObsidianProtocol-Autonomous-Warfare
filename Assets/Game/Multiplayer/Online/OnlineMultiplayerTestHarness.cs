using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public enum OnlineTestResult
    {
        NotRun,
        Passed,
        Failed
    }

    public sealed class OnlineTestReport
    {
        private readonly List<string> failures =
            new List<string>();

        public OnlineTestResult Result { get; private set; }

        public int TestsRun { get; private set; }

        public int TestsPassed { get; private set; }

        public int TestsFailed =>
            failures.Count;

        public IReadOnlyCollection<string>
            Failures =>
            failures;

        public OnlineTestReport()
        {
            Result =
                OnlineTestResult.NotRun;
        }

        public void Record(
            string testName,
            bool passed)
        {
            TestsRun++;

            if (passed)
            {
                TestsPassed++;
                return;
            }

            failures.Add(
                string.IsNullOrWhiteSpace(testName)
                    ? "Unnamed test"
                    : testName);
        }

        public void Complete()
        {
            Result =
                failures.Count == 0
                    ? OnlineTestResult.Passed
                    : OnlineTestResult.Failed;
        }
    }

    public sealed class OnlineMultiplayerTestHarness
    {
        public bool RunCoreTests(
            OnlineServerAuthority authority,
            OnlinePlayerPermissions permissions,
            OnlineConnectionRecovery recovery,
            OnlineVoiceCommunication voice,
            OnlineAntiCheatFoundation antiCheat,
            OnlineProductionSynchronization production,
            OnlinePersistenceSynchronization persistence)
        {
            OnlineTestReport report =
                RunTests(
                    authority,
                    permissions,
                    recovery,
                    voice,
                    antiCheat,
                    production,
                    persistence);

            return report.Result ==
                   OnlineTestResult.Passed;
        }

        public OnlineTestReport RunTests(
            OnlineServerAuthority authority,
            OnlinePlayerPermissions permissions,
            OnlineConnectionRecovery recovery,
            OnlineVoiceCommunication voice,
            OnlineAntiCheatFoundation antiCheat,
            OnlineProductionSynchronization production,
            OnlinePersistenceSynchronization persistence)
        {
            OnlineTestReport report =
                new OnlineTestReport();

            report.Record(
                "Server authority initialized",
                authority != null &&
                authority.Initialized);

            report.Record(
                "Player permissions initialized",
                permissions != null &&
                permissions.Initialized);

            report.Record(
                "Connection recovery initialized",
                recovery != null &&
                recovery.Initialized);

            report.Record(
                "Voice communication initialized",
                voice != null &&
                voice.Initialized);

            report.Record(
                "Anti-cheat foundation initialized",
                antiCheat != null &&
                antiCheat.Initialized);

            report.Record(
                "Production synchronization initialized",
                production != null &&
                production.Initialized);

            report.Record(
                "Persistence synchronization initialized",
                persistence != null &&
                persistence.Initialized);

            if (authority != null &&
                authority.Initialized)
            {
                const string entityId =
                    "online-test-entity";

                bool registered =
                    authority.RegisterEntity(
                        entityId);

                bool assigned =
                    authority.AssignServerAuthority(
                        entityId,
                        1);

                bool valid =
                    authority.ValidateServerAuthority(
                        entityId,
                        1);

                report.Record(
                    "Server authority assignment",
                    registered &&
                    assigned &&
                    valid);
            }

            if (permissions != null &&
                permissions.Initialized)
            {
                const string playerId =
                    "online-test-player";

                bool registered =
                    permissions.RegisterPlayer(
                        playerId,
                        OnlinePermission.Command);

                bool valid =
                    permissions.HasPermission(
                        playerId,
                        OnlinePermission.Command);

                report.Record(
                    "Player permission validation",
                    registered &&
                    valid);
            }

            if (recovery != null &&
                recovery.Initialized)
            {
                const string playerId =
                    "online-test-player";

                bool registered =
                    recovery.RegisterPlayer(
                        playerId);

                bool disconnected =
                    recovery.MarkDisconnected(
                        playerId);

                bool reconnecting =
                    recovery.BeginReconnect(
                        playerId);

                bool recovered =
                    recovery.MarkRecovered(
                        playerId);

                report.Record(
                    "Connection recovery flow",
                    registered &&
                    disconnected &&
                    reconnecting &&
                    recovered);
            }

            if (voice != null &&
                voice.Initialized)
            {
                const string playerId =
                    "online-test-player";

                bool registered =
                    voice.RegisterParticipant(
                        playerId);

                bool connected =
                    voice.SetParticipantConnected(
                        playerId,
                        true);

                bool joined =
                    voice.JoinChannel(
                        playerId,
                        OnlineCommunicationChannel.Team);

                bool canTransmit =
                    voice.CanTransmit(
                        playerId,
                        OnlineCommunicationChannel.Team);

                report.Record(
                    "Voice communication flow",
                    registered &&
                    connected &&
                    joined &&
                    canTransmit);
            }

            if (antiCheat != null &&
                antiCheat.Initialized)
            {
                const string playerId =
                    "online-test-player";

                bool registered =
                    antiCheat.RegisterPlayer(
                        playerId);

                OnlineValidationResult result =
                    antiCheat.ValidateCommand(
                        playerId,
                        true,
                        true,
                        1);

                report.Record(
                    "Command validation flow",
                    registered &&
                    result ==
                    OnlineValidationResult.Valid);
            }

            if (production != null &&
                production.Initialized)
            {
                bool registered =
                    production.RegisterProduction(
                        "online-test-production",
                        "online-test-player",
                        "test-unit",
                        1);

                bool synchronized =
                    production.SynchronizeProduction(
                        "online-test-production",
                        "online-test-player",
                        "test-unit",
                        1,
                        0.5f,
                        OnlineProductionState.Producing,
                        1);

                report.Record(
                    "Production synchronization flow",
                    registered &&
                    synchronized);
            }

            if (persistence != null &&
                persistence.Initialized)
            {
                bool created =
                    persistence.CreateSnapshot(
                        "online-test-snapshot",
                        1);

                bool stored =
                    persistence.SynchronizeValue(
                        "TestState",
                        "Valid");

                bool retrieved =
                    persistence.TryGetValue(
                        "TestState",
                        out string value);

                report.Record(
                    "Persistence synchronization flow",
                    created &&
                    stored &&
                    retrieved &&
                    value == "Valid");
            }

            report.Complete();

            return report;
        }
    }
}
