using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum TravelState
    {
        Planned,
        Departing,
        Traveling,
        Arrived,
        Interrupted,
        Cancelled
    }

    public sealed class LongDistanceTravelRecord
    {
        public string TravelId { get; }

        public string OwnerId { get; }

        public string OriginRegionId { get; }

        public string DestinationRegionId { get; }

        public TravelState State { get; private set; }

        public float Progress { get; private set; }

        public long StartTick { get; private set; }

        public long ArrivalTick { get; private set; }

        public LongDistanceTravelRecord(
            string travelId,
            string ownerId,
            string originRegionId,
            string destinationRegionId)
        {
            TravelId =
                travelId ?? string.Empty;

            OwnerId =
                ownerId ?? string.Empty;

            OriginRegionId =
                originRegionId ?? string.Empty;

            DestinationRegionId =
                destinationRegionId ?? string.Empty;

            State =
                TravelState.Planned;

            Progress = 0f;
            StartTick = -1;
            ArrivalTick = -1;
        }

        public bool Start(
            long startTick)
        {
            if (State !=
                TravelState.Planned ||
                startTick < 0)
            {
                return false;
            }

            StartTick =
                startTick;

            State =
                TravelState.Traveling;

            return true;
        }

        public bool SetProgress(
            float progress)
        {
            if (State !=
                    TravelState.Traveling &&
                State !=
                    TravelState.Departing)
            {
                return false;
            }

            if (progress < 0f ||
                progress > 100f)
            {
                return false;
            }

            Progress =
                progress;

            if (Progress >= 100f)
            {
                State =
                    TravelState.Arrived;
            }

            return true;
        }

        public bool SetArrival(
            long arrivalTick)
        {
            if (State !=
                TravelState.Arrived ||
                arrivalTick < StartTick)
            {
                return false;
            }

            ArrivalTick =
                arrivalTick;

            return true;
        }

        public bool Interrupt()
        {
            if (State !=
                TravelState.Traveling)
            {
                return false;
            }

            State =
                TravelState.Interrupted;

            return true;
        }

        public bool Cancel()
        {
            if (State ==
                    TravelState.Arrived ||
                State ==
                    TravelState.Cancelled)
            {
                return false;
            }

            State =
                TravelState.Cancelled;

            return true;
        }
    }

    public sealed class LongDistanceTravel
    {
        private readonly Dictionary<
            string,
            LongDistanceTravelRecord> travels =
            new Dictionary<
                string,
                LongDistanceTravelRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TravelCount =>
            travels.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            travels.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterTravel(
            string travelId,
            string ownerId,
            string originRegionId,
            string destinationRegionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(travelId) ||
                string.IsNullOrWhiteSpace(ownerId) ||
                string.IsNullOrWhiteSpace(originRegionId) ||
                string.IsNullOrWhiteSpace(destinationRegionId))
            {
                return false;
            }

            string id =
                travelId.Trim();

            if (travels.ContainsKey(id))
            {
                return false;
            }

            travels.Add(
                id,
                new LongDistanceTravelRecord(
                    id,
                    ownerId.Trim(),
                    originRegionId.Trim(),
                    destinationRegionId.Trim()));

            return true;
        }

        public bool StartTravel(
            string travelId,
            long startTick)
        {
            LongDistanceTravelRecord travel =
                GetTravel(travelId);

            return travel != null &&
                   travel.Start(startTick);
        }

        public bool SetProgress(
            string travelId,
            float progress)
        {
            LongDistanceTravelRecord travel =
                GetTravel(travelId);

            return travel != null &&
                   travel.SetProgress(progress);
        }

        public bool SetArrival(
            string travelId,
            long arrivalTick)
        {
            LongDistanceTravelRecord travel =
                GetTravel(travelId);

            return travel != null &&
                   travel.SetArrival(arrivalTick);
        }

        public bool Interrupt(
            string travelId)
        {
            LongDistanceTravelRecord travel =
                GetTravel(travelId);

            return travel != null &&
                   travel.Interrupt();
        }

        public bool Cancel(
            string travelId)
        {
            LongDistanceTravelRecord travel =
                GetTravel(travelId);

            return travel != null &&
                   travel.Cancel();
        }

        public LongDistanceTravelRecord GetTravel(
            string travelId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(travelId))
            {
                return null;
            }

            travels.TryGetValue(
                travelId.Trim(),
                out LongDistanceTravelRecord travel);

            return travel;
        }

        public IReadOnlyCollection<
            LongDistanceTravelRecord>
            GetTravels()
        {
            return travels.Values;
        }

        public void Reset()
        {
            travels.Clear();
            Initialized = false;
        }
    }
}
