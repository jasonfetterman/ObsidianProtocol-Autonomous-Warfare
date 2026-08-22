using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Logistics
{
    public enum SupplyType
    {
        Fuel,
        Ammunition,
        Energy,
        SpareParts,
        FabricationMaterials,
        Resources
    }

    public enum SupplyPriority
    {
        Low,
        Normal,
        High,
        Critical
    }

    public enum SupplyRequestState
    {
        Pending,
        Fulfilled,
        Cancelled,
        Failed
    }

    public sealed class SupplyRequest
    {
        public string RequestId { get; }

        public string RecipientId { get; }

        public SupplyType SupplyType { get; }

        public float Amount { get; }

        public SupplyPriority Priority { get; }

        public SupplyRequestState State { get; private set; }

        public SupplyRequest(
            string requestId,
            string recipientId,
            SupplyType supplyType,
            float amount,
            SupplyPriority priority)
        {
            RequestId =
                requestId ?? string.Empty;

            RecipientId =
                recipientId ?? string.Empty;

            SupplyType =
                supplyType;

            Amount =
                Math.Max(
                    0f,
                    amount);

            Priority =
                priority;

            State =
                SupplyRequestState.Pending;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(RequestId) &&
            !string.IsNullOrWhiteSpace(RecipientId) &&
            Amount > 0f;

        public void Fulfill()
        {
            if (State ==
                SupplyRequestState.Pending)
            {
                State =
                    SupplyRequestState.Fulfilled;
            }
        }

        public void Cancel()
        {
            if (State ==
                SupplyRequestState.Pending)
            {
                State =
                    SupplyRequestState.Cancelled;
            }
        }

        public void Fail()
        {
            if (State ==
                SupplyRequestState.Pending)
            {
                State =
                    SupplyRequestState.Failed;
            }
        }
    }

    public sealed class SupplyFramework
    {
        private readonly Dictionary<string, SupplyRequest> requests =
            new Dictionary<string, SupplyRequest>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterRequest(
            SupplyRequest request)
        {
            if (request == null ||
                !request.Valid ||
                requests.ContainsKey(request.RequestId))
            {
                return false;
            }

            requests.Add(
                request.RequestId,
                request);

            return true;
        }

        public bool RemoveRequest(
            string requestId)
        {
            if (string.IsNullOrWhiteSpace(requestId))
            {
                return false;
            }

            return requests.Remove(
                requestId);
        }

        public bool TryGetRequest(
            string requestId,
            out SupplyRequest request)
        {
            return requests.TryGetValue(
                requestId,
                out request);
        }

        public void FulfillRequest(
            string requestId)
        {
            if (requests.TryGetValue(
                    requestId,
                    out SupplyRequest request))
            {
                request.Fulfill();
            }
        }

        public void CancelRequest(
            string requestId)
        {
            if (requests.TryGetValue(
                    requestId,
                    out SupplyRequest request))
            {
                request.Cancel();
            }
        }

        public void FailRequest(
            string requestId)
        {
            if (requests.TryGetValue(
                    requestId,
                    out SupplyRequest request))
            {
                request.Fail();
            }
        }

        public IReadOnlyCollection<SupplyRequest>
            GetRequests()
        {
            return requests.Values;
        }

        public IReadOnlyCollection<SupplyRequest>
            GetPendingRequests()
        {
            List<SupplyRequest> pending =
                new List<SupplyRequest>();

            foreach (
                SupplyRequest request
                in requests.Values)
            {
                if (request.State ==
                    SupplyRequestState.Pending)
                {
                    pending.Add(request);
                }
            }

            return pending;
        }
    }
}
