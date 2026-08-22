using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public sealed class UnitAudioRecord
    {
        public string UnitId { get; }

        public string EngineAudioPath { get; private set; }
        public string WeaponAudioPath { get; private set; }
        public string AlertAudioPath { get; private set; }
        public string VoiceAudioPath { get; private set; }

        public UnitAudioRecord(string unitId)
        {
            UnitId = unitId ?? string.Empty;

            EngineAudioPath = string.Empty;
            WeaponAudioPath = string.Empty;
            AlertAudioPath = string.Empty;
            VoiceAudioPath = string.Empty;
        }

        public void Configure(
            string engineAudioPath,
            string weaponAudioPath,
            string alertAudioPath,
            string voiceAudioPath)
        {
            EngineAudioPath =
                engineAudioPath ?? string.Empty;

            WeaponAudioPath =
                weaponAudioPath ?? string.Empty;

            AlertAudioPath =
                alertAudioPath ?? string.Empty;

            VoiceAudioPath =
                voiceAudioPath ?? string.Empty;
        }

        public bool HasEngineAudio =>
            !string.IsNullOrWhiteSpace(EngineAudioPath);

        public bool HasWeaponAudio =>
            !string.IsNullOrWhiteSpace(WeaponAudioPath);

        public bool HasAlertAudio =>
            !string.IsNullOrWhiteSpace(AlertAudioPath);

        public bool HasVoiceAudio =>
            !string.IsNullOrWhiteSpace(VoiceAudioPath);
    }

    public sealed class UnitAudioIntegrationSystem
    {
        private readonly Dictionary<string, UnitAudioRecord> audio =
            new Dictionary<string, UnitAudioRecord>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!audio.ContainsKey(unitId))
            {
                audio.Add(
                    unitId,
                    new UnitAudioRecord(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            string engineAudioPath,
            string weaponAudioPath,
            string alertAudioPath,
            string voiceAudioPath)
        {
            RegisterUnit(unitId);

            audio[unitId].Configure(
                engineAudioPath,
                weaponAudioPath,
                alertAudioPath,
                voiceAudioPath);
        }

        public bool TryGetAudio(
            string unitId,
            out UnitAudioRecord record)
        {
            return audio.TryGetValue(
                unitId,
                out record);
        }

        public void RemoveUnit(string unitId)
        {
            audio.Remove(unitId);
        }

        public void Clear()
        {
            audio.Clear();
        }
    }
}
