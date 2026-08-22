using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public enum OnlineCommunicationChannel
    {
        None,
        Team,
        Squad,
        Command
    }

    public sealed class OnlineVoiceParticipant
    {
        public string PlayerId { get; }

        public bool Connected { get; private set; }

        public bool Muted { get; private set; }

        public OnlineVoiceParticipant(
            string playerId)
        {
            PlayerId =
                playerId ?? string.Empty;
        }

        public bool SetConnected(
            bool connected)
        {
            if (string.IsNullOrWhiteSpace(PlayerId))
            {
                return false;
            }

            Connected = connected;

            return true;
        }

        public bool SetMuted(
            bool muted)
        {
            if (string.IsNullOrWhiteSpace(PlayerId))
            {
                return false;
            }

            Muted = muted;

            return true;
        }
    }

    public sealed class OnlineVoiceCommunication
    {
        private readonly Dictionary<
            string,
            OnlineVoiceParticipant> participants =
            new Dictionary<
                string,
                OnlineVoiceParticipant>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<
            OnlineCommunicationChannel,
            HashSet<string>> channelMembers =
            new Dictionary<
                OnlineCommunicationChannel,
                HashSet<string>>();

        public bool Initialized { get; private set; }

        public int ParticipantCount =>
            participants.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            participants.Clear();
            channelMembers.Clear();

            channelMembers.Add(
                OnlineCommunicationChannel.Team,
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase));

            channelMembers.Add(
                OnlineCommunicationChannel.Squad,
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase));

            channelMembers.Add(
                OnlineCommunicationChannel.Command,
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase));

            Initialized = true;

            return true;
        }

        public bool RegisterParticipant(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            string id =
                playerId.Trim();

            if (participants.ContainsKey(id))
            {
                return false;
            }

            participants.Add(
                id,
                new OnlineVoiceParticipant(id));

            return true;
        }

        public bool SetParticipantConnected(
            string playerId,
            bool connected)
        {
            OnlineVoiceParticipant participant =
                GetParticipant(playerId);

            return participant != null &&
                   participant.SetConnected(
                       connected);
        }

        public bool SetParticipantMuted(
            string playerId,
            bool muted)
        {
            OnlineVoiceParticipant participant =
                GetParticipant(playerId);

            return participant != null &&
                   participant.SetMuted(muted);
        }

        public bool JoinChannel(
            string playerId,
            OnlineCommunicationChannel channel)
        {
            if (!IsValidChannel(channel) ||
                GetParticipant(playerId) == null)
            {
                return false;
            }

            return channelMembers[channel].Add(
                playerId.Trim());
        }

        public bool LeaveChannel(
            string playerId,
            OnlineCommunicationChannel channel)
        {
            if (!IsValidChannel(channel) ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            return channelMembers[channel].Remove(
                playerId.Trim());
        }

        public bool CanTransmit(
            string playerId,
            OnlineCommunicationChannel channel)
        {
            if (!Initialized ||
                !IsValidChannel(channel))
            {
                return false;
            }

            OnlineVoiceParticipant participant =
                GetParticipant(playerId);

            if (participant == null ||
                !participant.Connected ||
                participant.Muted)
            {
                return false;
            }

            return channelMembers[channel].Contains(
                participant.PlayerId);
        }

        public OnlineVoiceParticipant
            GetParticipant(
                string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return null;
            }

            participants.TryGetValue(
                playerId.Trim(),
                out OnlineVoiceParticipant participant);

            return participant;
        }

        public IReadOnlyCollection<
            OnlineVoiceParticipant>
            GetParticipants()
        {
            return participants.Values;
        }

        private bool IsValidChannel(
            OnlineCommunicationChannel channel)
        {
            return channel !=
                       OnlineCommunicationChannel.None &&
                   channelMembers.ContainsKey(channel);
        }

        public void Reset()
        {
            participants.Clear();
            channelMembers.Clear();

            Initialized = false;
        }
    }
}
