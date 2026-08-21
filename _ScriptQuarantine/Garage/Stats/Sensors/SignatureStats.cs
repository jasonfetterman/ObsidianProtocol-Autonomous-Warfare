using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class SignatureStats
    {
        [Header("Detection Signatures")]
        [Range(0f, 1f)]
        public float visualSignature;

        [Range(0f, 1f)]
        public float thermalSignature;

        [Range(0f, 1f)]
        public float radarSignature;

        [Range(0f, 1f)]
        public float acousticSignature;

        [Range(0f, 1f)]
        public float electromagneticSignature;

        [Header("Environmental Signatures")]
        [Range(0f, 1f)]
        public float dustSignature;

        [Range(0f, 1f)]
        public float heatSignature;

        [Range(0f, 1f)]
        public float movementSignature;

        [Header("Stealth")]
        [Range(0f, 1f)]
        public float stealthRating;

        [Range(0f, 1f)]
        public float concealmentRating;

        public bool canReduceSignature;
        public bool canSuppressEmissions;
        public bool canOperateInStealthMode;
    }
}
