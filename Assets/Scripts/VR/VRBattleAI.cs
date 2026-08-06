using UnityEngine;

namespace Obsidian.VR
{
    public class VRBattleAI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private VRBattleDirector battleDirector;
        [SerializeField] private VRSessionManager session;

        public enum Phase
        {
            Stealth,
            Breach,
            Assault,
            Suppressed,
            Retreat,
            LastStand
        }

        [Header("Mission Phase")]
        [SerializeField] private Phase currentPhase;

        [Header("Adaptive Parameters")]
        [SerializeField] private float threatLevel;
        [SerializeField] private float cohesionLevel;
        [SerializeField] private float explosionIntensity;
        [SerializeField] private float killMomentum;
        [SerializeField] private float suppressionLevel;

        private float phaseBlendT;

        private void Awake()
        {
            if (battleDirector == null)
                battleDirector = FindAnyObjectByType<VRBattleDirector>();

            if (session == null)
                session = VRSessionManager.Instance;
        }

        private void Update()
        {
            if (!session.SessionActive)
                return;

            UpdateBattleMetrics();
            UpdatePhase();
            ApplyPhaseStyle();
        }

        private void UpdateBattleMetrics()
        {
            threatLevel = Mathf.Lerp(threatLevel, 0f, Time.deltaTime * 0.5f);
            explosionIntensity = Mathf.Lerp(explosionIntensity, 0f, Time.deltaTime * 1.5f);
            killMomentum = Mathf.Lerp(killMomentum, 0f, Time.deltaTime * 0.3f);
            suppressionLevel = Mathf.Lerp(suppressionLevel, 0f, Time.deltaTime * 0.8f);
            cohesionLevel = ComputeCohesion();
        }

        private float ComputeCohesion()
        {
            var units = battleDirector != null ? battleDirector.GetComponentsInChildren<BaseUnitVRController>() : null;

            if (units == null || units.Length == 0)
                return 0f;

            int closePairs = 0;

            foreach (var u in units)
            {
                foreach (var v in units)
                {
                    if (u == v) continue;
                    float dist = Vector3.Distance(u.transform.position, v.transform.position);
                    if (dist < 10f)
                        closePairs++;
                }
            }

            return Mathf.Clamp01(closePairs / 10f);
        }

        private void UpdatePhase()
        {
            if (threatLevel < 0.2f && suppressionLevel < 0.2f && explosionIntensity < 0.1f)
                currentPhase = Phase.Stealth;

            if (explosionIntensity > 0.3f || threatLevel > 0.4f)
                currentPhase = Phase.Breach;

            if (suppressionLevel > 0.5f)
                currentPhase = Phase.Suppressed;

            if (suppressionLevel > 0.7f && cohesionLevel < 0.3f)
                currentPhase = Phase.Retreat;

            if (cohesionLevel < 0.2f && threatLevel > 0.7f && suppressionLevel > 0.7f)
                currentPhase = Phase.LastStand;
        }

        private void ApplyPhaseStyle()
        {
            switch (currentPhase)
            {
                case Phase.Stealth:
                    battleDirector.SetStyle(BattleStylesLibrary.DocumentaryRealism);
                    break;

                case Phase.Breach:
                    phaseBlendT = Mathf.Clamp01(explosionIntensity + threatLevel);
                    battleDirector.BlendStyles(
                        BattleStylesLibrary.MilitarySim,
                        BattleStylesLibrary.HeroicBlockbuster,
                        phaseBlendT
                    );
                    break;

                case Phase.Assault:
                    phaseBlendT = Mathf.Clamp01(threatLevel + killMomentum);
                    battleDirector.BlendStyles(
                        BattleStylesLibrary.HeroicBlockbuster,
                        BattleStylesLibrary.AnimeWarOpera,
                        phaseBlendT
                    );
                    break;

                case Phase.Suppressed:
                    phaseBlendT = Mathf.Clamp01(suppressionLevel);
                    battleDirector.BlendStyles(
                        BattleStylesLibrary.MilitarySim,
                        BattleStylesLibrary.DocumentaryRealism,
                        phaseBlendT
                    );
                    break;

                case Phase.Retreat:
                    battleDirector.SetStyle(BattleStylesLibrary.MilitarySim);
                    break;

                case Phase.LastStand:
                    battleDirector.SetStyle(BattleStylesLibrary.AnimeWarOpera);
                    break;
            }
        }

        public void OnExplosion()
        {
            explosionIntensity += 0.5f;
        }

        public void OnKill()
        {
            killMomentum += 0.4f;
        }

        public void OnSuppression()
        {
            suppressionLevel += 0.3f;
        }

        public void OnThreatSpike()
        {
            threatLevel += 0.4f;
        }
    }
}
