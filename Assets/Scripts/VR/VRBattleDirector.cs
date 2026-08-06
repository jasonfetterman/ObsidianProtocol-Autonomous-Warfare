using UnityEngine;
using System.Collections.Generic;

namespace Obsidian.VR
{
    public class VRBattleDirector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRUnitFocusController _focus;
        [SerializeField] private VRCombatDirector _combat;

        [Header("Units")]
        [SerializeField] private List<BaseUnitVRController> squadUnits = new List<BaseUnitVRController>();

        [Header("Battle Style")]
        [SerializeField] private BattleStyle activeStyle;
        [SerializeField] private BattleStyle styleA;
        [SerializeField] private BattleStyle styleB;
        [SerializeField, Range(0f, 1f)] private float blendT;

        private float switchTimer;
        private float nextSwitchTime;

        private void Awake()
        {
            if (_session == null)
                _session = VRSessionManager.Instance;

            if (_focus == null)
                _focus = FindAnyObjectByType<VRUnitFocusController>();

            if (_combat == null)
                _combat = FindAnyObjectByType<VRCombatDirector>();

            styleA = BattleStylesLibrary.MilitarySim;
            styleB = BattleStylesLibrary.HeroicBlockbuster;
            activeStyle = styleA;

            ResetSwitchTimer();
        }

        private void Update()
        {
            if (!_session.SessionActive)
                return;

            activeStyle = BattleStyle.Lerp(styleA, styleB, blendT);

            UpdatePOVSwitching();
        }

        // ------------------------------------------------------------
        // STYLE SELECTION
        // ------------------------------------------------------------
        public void SetStyle(BattleStyle style)
        {
            activeStyle = style;
        }

        public void BlendStyles(BattleStyle a, BattleStyle b, float t)
        {
            styleA = a;
            styleB = b;
            blendT = Mathf.Clamp01(t);
        }

        public void UseMilitarySim() => SetStyle(BattleStylesLibrary.MilitarySim);
        public void UseHeroicBlockbuster() => SetStyle(BattleStylesLibrary.HeroicBlockbuster);
        public void UseDocumentaryRealism() => SetStyle(BattleStylesLibrary.DocumentaryRealism);
        public void UseAnimeWarOpera() => SetStyle(BattleStylesLibrary.AnimeWarOpera);

        // ------------------------------------------------------------
        // POV SWITCHING
        // ------------------------------------------------------------
        private void UpdatePOVSwitching()
        {
            switchTimer += Time.deltaTime;

            if (switchTimer < nextSwitchTime)
                return;

            switchTimer = 0f;
            ResetSwitchTimer();

            BaseUnitVRController best = SelectBestPOVUnit();
            if (best != null)
                _session.SetActiveUnit(best);
        }

        private void ResetSwitchTimer()
        {
            nextSwitchTime = Random.Range(activeStyle.minSwitchInterval, activeStyle.maxSwitchInterval);
        }

        private BaseUnitVRController SelectBestPOVUnit()
        {
            if (squadUnits.Count == 0)
                return null;

            BaseUnitVRController current = _session.ActiveUnit;
            BaseUnitVRController best = current;
            float bestScore = -999f;

            foreach (var u in squadUnits)
            {
                if (u == null || !u.IsAlive())
                    continue;

                float score = ComputeUnitScore(u);
                if (score > bestScore)
                {
                    bestScore = score;
                    best = u;
                }
            }

            return best;
        }

        // ------------------------------------------------------------
        // UNIT SCORING MODEL (style-driven)
        // ------------------------------------------------------------
        private float ComputeUnitScore(BaseUnitVRController u)
        {
            float score = 0f;

            score += u.StressLevel * activeStyle.heroFocusBias;
            score += (1f - Mathf.Clamp01(u.GetHealth() / 100f)) * activeStyle.heroFocusBias;
            score += Mathf.Clamp01(u.GetCurrentSpeed() / 6f) * activeStyle.heroFocusBias;

            score += ScanThreatsAround(u.transform.position) * activeStyle.threatBias;
            score += ComputeCohesion(u) * activeStyle.cohesionBias;

            return score;
        }

        private float ComputeCohesion(BaseUnitVRController u)
        {
            int count = 0;

            foreach (var other in squadUnits)
            {
                if (other == null || other == u)
                    continue;

                float dist = Vector3.Distance(u.transform.position, other.transform.position);
                if (dist <= 12f)
                    count++;
            }

            return Mathf.Clamp01(count / 4f);
        }

        private float ScanThreatsAround(Vector3 pos)
        {
            Collider[] hits = Physics.OverlapSphere(pos, 25f);
            float threat = 0f;

            foreach (var h in hits)
            {
                if (h.CompareTag("Enemy"))
                    threat += 0.2f;
            }

            return Mathf.Clamp01(threat);
        }

        // ------------------------------------------------------------
        // BATTLE EVENTS (style-driven)
        // ------------------------------------------------------------
        public void OnExplosion(Vector3 pos)
        {
            _combat.OnExplosion();
            _focus.TriggerExplosion(activeStyle.explosionBias);
        }

        public void OnKill(BaseUnitVRController killer, Vector3 direction)
        {
            _combat.OnKill(direction);
            _focus.TriggerImpact(direction, activeStyle.killBias);
        }

        public void OnSuppression(BaseUnitVRController unit, Vector3 direction)
        {
            _combat.OnSuppression(direction);
            _focus.TriggerImpact(direction, activeStyle.suppressionBias);
        }
    }
}
