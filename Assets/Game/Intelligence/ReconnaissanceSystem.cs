using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public enum ReconStatus
    {
        Idle,
        Scanning,
        Investigating,
        Complete
    }

    public sealed class ReconMission
    {
        public int ReconUnitId;
        public int AreaId;
        public ReconStatus Status;
        public float Progress;

        public ReconMission(
            int reconUnitId,
            int areaId)
        {
            ReconUnitId = reconUnitId;
            AreaId = areaId;
            Status = ReconStatus.Idle;
            Progress = 0f;
        }
    }

    public sealed class ReconnaissanceSystem
    {
        private readonly Dictionary<int, ReconMission> missions =
            new Dictionary<int, ReconMission>();

        public void RegisterMission(
            int reconUnitId,
            int areaId)
        {
            if (reconUnitId < 0 ||
                areaId < 0)
            {
                return;
            }

            missions[reconUnitId] =
                new ReconMission(
                    reconUnitId,
                    areaId);
        }

        public void StartRecon(int reconUnitId)
        {
            if (missions.TryGetValue(
                    reconUnitId,
                    out ReconMission mission))
            {
                mission.Status =
                    ReconStatus.Scanning;
            }
        }

        public void SetInvestigating(int reconUnitId)
        {
            if (missions.TryGetValue(
                    reconUnitId,
                    out ReconMission mission))
            {
                mission.Status =
                    ReconStatus.Investigating;
            }
        }

        public void UpdateProgress(
            int reconUnitId,
            float progress)
        {
            if (!missions.TryGetValue(
                    reconUnitId,
                    out ReconMission mission))
            {
                return;
            }

            mission.Progress =
                System.Math.Clamp(
                    progress,
                    0f,
                    1f);

            if (mission.Progress >= 1f)
            {
                mission.Status =
                    ReconStatus.Complete;
            }
        }

        public bool TryGetMission(
            int reconUnitId,
            out ReconMission mission)
        {
            return missions.TryGetValue(
                reconUnitId,
                out mission);
        }

        public void RemoveMission(int reconUnitId)
        {
            missions.Remove(reconUnitId);
        }

        public void Clear()
        {
            missions.Clear();
        }
    }
}
