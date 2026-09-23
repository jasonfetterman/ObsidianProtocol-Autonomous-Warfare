using UnityEngine;
using ObsidianProtocol.Game.CommandUnits;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class IntelligenceRuntime : MonoBehaviour
    {
        public static IntelligenceRuntime Instance { get; private set; }

        public BattlefieldIntelligenceModel Intelligence { get; private set; }
        public DetectionSystem Detection { get; private set; }
        public SensorFramework Sensors { get; private set; }
        public SensorFusionSystem SensorFusion { get; private set; }
        public TrackingSystem Tracking { get; private set; }
        public TargetIdentificationSystem Identification { get; private set; }
        public TargetClassificationSystem Classification { get; private set; }
        public BattlefieldMappingSystem Mapping { get; private set; }
        public FogOfWarSystem FogOfWar { get; private set; }
        public NetworkRelaySystem Network { get; private set; }
        public IntelligenceSharingSystem Sharing { get; private set; }
        public IntelligencePersistenceSystem Persistence { get; private set; }
        public LostContactBehaviorSystem LostContact { get; private set; }
        public ReconnaissanceSystem Reconnaissance { get; private set; }
        public EnemyDetectionSystem EnemyDetection { get; private set; }
        public AudioDetectionSystem AudioDetection { get; private set; }
        public LidarDetectionSystem LidarDetection { get; private set; }
        public RadarDetectionSystem RadarDetection { get; private set; }
        public ThermalDetectionSystem ThermalDetection { get; private set; }
        public VisualDetectionSystem VisualDetection { get; private set; }
        public IntelligenceProcessingSystem Processing { get; private set; }
        public IntelligenceSurveillanceFeed SurveillanceFeeds { get; private set; }

        public int ActiveSensorCount
        {
            get
            {
                int count = 0;

                foreach (SensorDefinition sensor in Sensors.GetSensors())
                {
                    if (sensor.Status == SensorStatus.Active)
                        count++;
                }

                return count;
            }
        }

        public int SensorCount =>
            Sensors == null ? 0 : Sensors.GetSensors().Count;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            Intelligence = new BattlefieldIntelligenceModel();
            Detection = new DetectionSystem();
            Sensors = new SensorFramework();
            SensorFusion = new SensorFusionSystem();
            Tracking = new TrackingSystem();
            Identification = new TargetIdentificationSystem();
            Classification = new TargetClassificationSystem();
            Mapping = new BattlefieldMappingSystem();
            FogOfWar = new FogOfWarSystem();
            Network = new NetworkRelaySystem();
            Sharing = new IntelligenceSharingSystem();
            Persistence = new IntelligencePersistenceSystem();
            LostContact = new LostContactBehaviorSystem();
            Reconnaissance = new ReconnaissanceSystem();
            EnemyDetection = new EnemyDetectionSystem();
            AudioDetection = new AudioDetectionSystem();
            LidarDetection = new LidarDetectionSystem();
            RadarDetection = new RadarDetectionSystem();
            ThermalDetection = new ThermalDetectionSystem();
            VisualDetection = new VisualDetectionSystem();
            Processing = new IntelligenceProcessingSystem();
            SurveillanceFeeds = new IntelligenceSurveillanceFeed();
            SurveillanceFeeds.RegisterFeed("FEED-A", "SENSOR-ARRAY-A");
            SurveillanceFeeds.RegisterFeed("FEED-B", "SENSOR-ARRAY-B");
            SurveillanceFeeds.RegisterFeed("FEED-C", "SENSOR-ARRAY-C");
            SurveillanceFeeds.ActivateFeed("FEED-A");
            SurveillanceFeeds.ActivateFeed("FEED-B");
            SurveillanceFeeds.ActivateFeed("FEED-C");
            Network.RegisterRelay(1, 50f);
            Network.RegisterRelay(2, 50f);
            Network.ActivateRelay(1);
            Network.ActivateRelay(2);
            Network.Connect(1, 2);

            InitializeSensors();
            InitializeTestContacts();

            Tracking.UpdateContact(1001, 1, 0.91f, 12.4f, 42f);

            Processing.SubmitReport(
                "IR-2047",
                IntelligenceType.Threat,
                IntelligencePriority.High,
                "SENSOR-ARRAY-A",
                "1001",
                "Unknown contact detected at 12.4 KM.",
                0.91f);

            Debug.Log($"[INTELLIGENCE] Reports verified: {Processing.Count()}");
            Debug.Log($"[INTELLIGENCE] Network verified: Relay 1={Network.IsActive(1)}, Relay 2={Network.IsActive(2)}, Linked={Network.IsConnected(1, 2)}");

            Debug.Log(
                $"[INTELLIGENCE] Runtime initialized. Sensors: {SensorCount}, Active: {ActiveSensorCount}");
        }

        private void InitializeTestContacts()
        {
            Intelligence.UpdateDetection(
                1001,
                1,
                0.91f,
                12.4f,
                42f);

            Intelligence.UpdateIdentification(
                1001,
                "UNKNOWN",
                "UNKNOWN",
                IdentificationStatus.Unknown);

            Intelligence.UpdateClassification(
                1001,
                TargetClassification.Unknown);

            Detection.ReportDetection("SENSOR-ARRAY-A", 1001,
                12.4f,
                DetectionConfidence.High);

            Debug.Log(
                "[INTELLIGENCE] Test contact registered: 1001 UNKNOWN.");
        }

        private void InitializeSensors()
        {
            Sensors.RegisterSensor(
                "SENSOR-ARRAY-A",
                SensorType.MultiSensor,
                18.6f,
                0.94f);

            Sensors.RegisterSensor(
                "SENSOR-ARRAY-B",
                SensorType.MultiSensor,
                18.6f,
                0.94f);

            Sensors.RegisterSensor(
                "SENSOR-ARRAY-C",
                SensorType.MultiSensor,
                18.6f,
                0.94f);

            Sensors.ActivateSensor("SENSOR-ARRAY-A");
            Sensors.ActivateSensor("SENSOR-ARRAY-B");
            Sensors.ActivateSensor("SENSOR-ARRAY-C");

            Debug.Log(
                "[INTELLIGENCE] Sensor arrays registered and active.");
        }
    }
}












