using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateInspectCanonicalData
{
    [MenuItem("Obsidian Protocol/Units/Populate Inspect Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = null;

        foreach (string guid in AssetDatabase.FindAssets("t:UnitDefinition"))
        {
            string p = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<UnitDefinition>(p);
            if (asset == null) continue;

            var check = new SerializedObject(asset);
            string id = check.FindProperty("unitId")?.stringValue ?? "";
            string name = check.FindProperty("displayName")?.stringValue ?? "";

            if (id == "obs-air-inspect" || name == "Inspect")
            {
                target = asset;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError("Inspect population failed: existing UnitDefinition not found.");
            return;
        }

        var so = new SerializedObject(target);
        so.Update();

        void S(string path, string value)
        {
            var p = so.FindProperty(path);
            if (p != null && p.propertyType == SerializedPropertyType.String)
                p.stringValue = value;
        }

        void F(string path, float value)
        {
            var p = so.FindProperty(path);
            if (p != null && p.propertyType == SerializedPropertyType.Float)
                p.floatValue = value;
        }

        void I(string path, int value)
        {
            var p = so.FindProperty(path);
            if (p != null && p.propertyType == SerializedPropertyType.Integer)
                p.intValue = value;
        }

        void B(string path, bool value)
        {
            var p = so.FindProperty(path);
            if (p != null && p.propertyType == SerializedPropertyType.Boolean)
                p.boolValue = value;
        }

        SerializedProperty Array(string path)
        {
            var p = so.FindProperty(path);
            if (p == null || !p.isArray || p.propertyType == SerializedPropertyType.String)
                return null;
            p.ClearArray();
            return p;
        }

        SerializedProperty NewElement(SerializedProperty array)
        {
            int index = array.arraySize;
            array.arraySize = index + 1;
            return array.GetArrayElementAtIndex(index);
        }

        void Strings(string path, params string[] values)
        {
            var array = Array(path);
            if (array == null) return;
            foreach (string value in values)
                NewElement(array).stringValue = value;
        }

        void Pairs(string path, string[] first, string[] second)
        {
            var array = Array(path);
            if (array == null) return;
            for (int i = 0; i < first.Length; i++)
            {
                var e = NewElement(array);
                var a = e.FindPropertyRelative("condition");
                var b = e.FindPropertyRelative("effect");
                if (a != null) a.stringValue = first[i];
                if (b != null) b.stringValue = second[i];
            }
        }

        void Behavior(string name, float weight, string description)
        {
            var array = so.FindProperty("behaviorNodes");
            if (array == null || !array.isArray) return;
            if (array.arraySize == 0) { }
            var e = NewElement(array);
            e.FindPropertyRelative("behaviorNode").stringValue = name;
            e.FindPropertyRelative("weight").floatValue = weight;
            e.FindPropertyRelative("description").stringValue = description;
        }

        void Mission(string type, string focus, float efficiency)
        {
            var array = so.FindProperty("missionBehaviors");
            if (array == null || !array.isArray) return;
            var e = NewElement(array);
            e.FindPropertyRelative("missionType").stringValue = type;
            e.FindPropertyRelative("behaviorFocus").stringValue = focus;
            e.FindPropertyRelative("efficiency").floatValue = efficiency;
        }

        void Synergy(string partner, float rating, string function)
        {
            var array = so.FindProperty("unitSynergies");
            if (array == null || !array.isArray) return;
            var e = NewElement(array);
            e.FindPropertyRelative("partnerUnit").stringValue = partner;
            e.FindPropertyRelative("synergyRating").floatValue = rating;
            e.FindPropertyRelative("function").stringValue = function;
        }

        void Damage(string type, string effect, string recovery)
        {
            var array = so.FindProperty("damageResponses");
            if (array == null || !array.isArray) return;
            var e = NewElement(array);
            e.FindPropertyRelative("damageType").stringValue = type;
            e.FindPropertyRelative("effect").stringValue = effect;
            e.FindPropertyRelative("recovery").stringValue = recovery;
        }

        void Upgrade(string name, string effect, float gain)
        {
            var array = so.FindProperty("upgrades");
            if (array == null || !array.isArray) return;
            var e = NewElement(array);
            e.FindPropertyRelative("upgrade").stringValue = name;
            e.FindPropertyRelative("effect").stringValue = effect;
            e.FindPropertyRelative("efficiencyGain").floatValue = gain;
        }

        void Threat(string type, float response, float efficiency)
        {
            var array = so.FindProperty("threatResponses");
            if (array == null || !array.isArray) return;
            var e = NewElement(array);
            e.FindPropertyRelative("threatType").stringValue = type;
            e.FindPropertyRelative("reactionTimeSeconds").floatValue = response;
            e.FindPropertyRelative("countermeasureEfficiency").floatValue = efficiency;
        }

        void NetworkNode(string node, float load, float latency, string status)
        {
            var array = so.FindProperty("commandNetwork");
            if (array == null || !array.isArray) return;
            var e = NewElement(array);
            e.FindPropertyRelative("node").stringValue = node;
            e.FindPropertyRelative("loadPercent").floatValue = load;
            e.FindPropertyRelative("latencySeconds").floatValue = latency;
            e.FindPropertyRelative("status").stringValue = status;
        }

        void Resource(string path, string component, int quantity)
        {
            var array = so.FindProperty(path);
            if (array == null || !array.isArray) return;
            var e = NewElement(array);
            e.FindPropertyRelative("component").stringValue = component;
            e.FindPropertyRelative("quantity").intValue = quantity;
        }

        // Identity: intentionally preserve modelPrefab and storeImage.
        S("unitId", "obs-air-inspect");
        S("displayName", "Inspect");
        S("role", "Structural Inspection / Close-Range Analysis");
        S("tier", "Operational");
        S("status", "Active");

        // Physical and technical.
        F("physical.lengthMeters", 2.0f);
        F("physical.widthMeters", 1.6f);
        F("physical.heightMeters", 0.8f);
        F("physical.massKg", 16f);
        S("physical.powerSource", "Compact battery");
        F("physical.energyCapacityWh", 1600f);
        F("physical.energyConsumptionWhPerMinute", 35f);
        S("physical.endurance", "30–45 min");
        F("physical.payloadCapacityKg", 1.2f);
        F("physical.structuralIntegrityHp", 110f);
        F("physical.armorRating", 0.12f);
        S("physical.navigationSystem", "GPS + Close-Range Obstacle Avoidance");
        S("physical.heatSignature", "Low");
        S("physical.noiseSignature", "Low");
        F("physical.topSpeedMetersPerSecond", 18f);
        S("physical.topSpeedDescription", "18 m/s");
        S("physical.acceleration", "Medium");
        S("physical.turningRadius", "Very Tight");
        S("physical.climbRate", "Medium");
        S("physical.stabilityRating", "High (for close-range hovering)");
        Strings("physical.modules", "Inspection camera", "Micro-stabilizers");

        // Autonomy and sensors.
        S("autonomy.behaviorProfile", "Inspection / Analysis");
        F("autonomy.aggression", 0f);
        F("autonomy.caution", 0.9f);
        F("autonomy.persistence", 0.7f);
        F("autonomy.flankingLikelihood", 0f);
        F("autonomy.retreatThreshold", 0.8f);
        S("autonomy.targetPriority", "Structural points / anomalies");
        S("autonomy.formationDiscipline", "Medium");
        S("autonomy.retreatBehaviorDescription", "Early withdrawal from unsafe inspection positions");

        F("sensors.visualRangeMeters", 1600f);
        F("sensors.thermalRangeMeters", 700f);
        S("sensors.visualRangeDescription", "1600 m");
        S("sensors.thermalRangeDescription", "700 m");
        S("sensors.acousticRange", "Low");
        S("sensors.radarRange", "None");
        S("sensors.lidarRange", "Short-range (close inspection)");
        S("sensors.signalDetectionRange", "Low");

        F("communication.meshRangeKm", 4f);
        S("communication.meshRangeDescription", "4 km");
        F("communication.relayCapacity", 0f);
        S("communication.commandLinkType", "Standard Warden mesh");
        S("communication.networkPriority", "Low");

        // Environment.
        S("environment.terrainAdaptationCoefficients",
          "Open Air 1.0 / Urban 0.95 / Forest 0.75 / Mountain 0.80 / Desert 0.90 / Snow 0.70 / Water 0.0");
        F("environment.weatherSensitivityIndex", 0.64f);
        S("environment.visibilityModifiers",
          "Day 1.0 / Night 0.80 / Fog 0.65 / Rain 0.78 / Heavy Storm 0.55");
        S("environment.signalPropagationCurve",
          "Effective range decreases 0.2% per 10% atmospheric interference");
        Pairs("environment.modifiers",
          new[] { "High Winds", "Heavy Rain", "Fog", "Night Operations", "Extreme Weather", "Electromagnetic Interference" },
          new[] {
            "-15% hover stability / -10% LiDAR precision",
            "-20% optical clarity / -8% inspection accuracy",
            "-25% visual range / +5% thermal effectiveness",
            "-20% visual range / +10% thermal effectiveness",
            "-35% close-range inspection efficiency",
            "-35% signal integrity"
          });

        // Network and cognitive analytics.
        F("network.nodeHandoffEfficiency", 0.94f);
        S("network.packetLossRate", "1.6% under load");
        F("network.encryptionLatencySeconds", 0.10f);
        S("network.meshSelfHealingThreshold", "1 lost node");
        S("network.relaySaturationCurve", "Throughput decreases 6% per active node beyond 5");

        F("cognitive.decisionConfidence", 0.90f);
        F("cognitive.threatRecognitionAccuracy", 0.94f);
        F("cognitive.adaptiveLearningRatePerMission", 0.05f);
        F("cognitive.commandOverrideResistance", 0.08f);
        F("cognitive.cooperativeBehaviorIndex", 0.93f);

        // Combat is intentionally non-combat.
        F("combat.firepowerRating", 0f);
        F("combat.suppressionValue", 0f);
        I("combat.threatLevel", 2);
        S("combat.accuracy", "N/A");
        S("combat.stabilization", "Very High");
        S("combat.weaponMounts", "None");
        S("combat.weaponTypes", "None");
        S("combat.trackingStrength", "Medium");
        S("combat.disruptionStrength", "Low");
        S("combat.breachCapability", "None");
        S("combat.countermeasureRating", "Low");
        S("combat.engagementDistance", "N/A");
        S("combat.preferredTerrain", "Close-range hover zones");
        S("combat.preferredTargets", "None");
        S("combat.vulnerabilityProfile", "Ballistic / weather");

        // Tactical integration.
        F("tactical.formationAdaptability", 0.76f);
        F("tactical.tacticalRoleWeight", 0.72f);
        F("tactical.commandResponseDelaySeconds", 0.45f);
        S("tactical.reinforcementTriggerConditions",
          "Structural anomaly severity >70% OR Signal Integrity <60%");
        S("tactical.retreatLogicThresholds",
          "Structural Integrity <35% OR Threat Level >4 OR Battery <20%");

        // Maintenance: missing MTBF defaulted to 40 hours for this light-duty platform.
        F("maintenance.meanTimeBetweenFailuresHours", 40f);
        F("maintenance.energyRecoveryRatePercentPerIdleMinute", 7f);
        F("maintenance.componentWearPercentPerHour", 0.30f);
        F("maintenance.sparePartDependencyIndex", 0.49f);
        F("maintenance.batteryEfficiencyPercent", 92f);
        F("maintenance.sensorCalibrationDriftPercentPerHour", 6f);
        F("maintenance.rotorStabilityPercent", 94f);
        F("maintenance.coolingEfficiencyPercent", 88f);
        F("maintenance.operationalUptimePercent", 96f);
        S("maintenance.maintenancePriorityLevel", "Tier II");
        S("maintenance.operationalCostPerHour", "4 Metal / 4 Electronics");

        // Performance.
        F("performance.missionSuccessProbability", 0.91f);
        F("performance.averageEngagementDurationMinutes", 7.5f);
        F("performance.reconDataYieldMbPerMinute", 16f);
        F("performance.communicationUptime", 0.96f);
        F("performance.threatAvoidanceEfficiency", 0.92f);

        // VR operator.
        S("vrOperator.operatorSeatType", "Drone View");
        S("vrOperator.operatorHud", "Inspection Overlay");
        S("vrOperator.cameraFeeds", "High-zoom optical");
        S("vrOperator.movementControls", "Full drone flight");
        S("vrOperator.sensorControls", "Zoom / thermal / LiDAR");
        S("vrOperator.abilityControls", "Scan, Mark Structural Weakness");
        S("vrOperator.weaponControls", "None");
        S("vrOperator.stabilizationControls", "Auto-hover, micro-adjust");
        B("vrOperator.thermalView", true);
        B("vrOperator.lidarView", true);
        B("vrOperator.sonarView", false);
        B("vrOperator.tacticalOverlay", true);
        Strings("vrOperator.operatorAbilities",
          "Close-Range Scan", "Structural Mark", "LiDAR Sweep", "Stabilize");

        // Economy and build.
        S("economy.buildCost", "140 Metal / 120 Electronics");
        S("economy.factoryType", "Drone Bay");
        S("economy.techRequirements", "Inspection Systems I");
        S("economy.maintenanceCost", "Low");
        I("economy.repairTimeSeconds", 12);
        S("economy.supplyConsumption", "Low");
        S("economy.operationalCost", "Minimal");
        S("economy.deploymentRequirements", "Standard drone pad");
        S("economy.transportCompatibility", "All aerial racks");
        S("economy.launchMethod", "Vertical takeoff");
        F("economy.buildTimeSeconds", 45f);

        // Behavior matrix.
        var behaviorNodes = so.FindProperty("behaviorNodes");
        if (behaviorNodes != null && behaviorNodes.isArray) behaviorNodes.ClearArray();
        Behavior("Inspection Scan", 0.95f, "Conduct detailed structural inspection");
        Behavior("Anomaly Detection", 0.94f, "Identify structural irregularities");
        Behavior("LiDAR Analysis", 0.96f, "Map close-range geometry and surfaces");
        Behavior("Thermal Analysis", 0.88f, "Identify abnormal thermal signatures");
        Behavior("Structural Classification", 0.93f, "Classify structural conditions");
        Behavior("Hover Stabilization", 0.97f, "Maintain precise inspection position");
        Behavior("Approach Control", 0.95f, "Approach inspection points safely");
        Behavior("Structural Marking", 0.92f, "Mark weaknesses and anomalies");
        Behavior("Retreat", 0.91f, "Withdraw when inspection becomes unsafe");
        Behavior("Override Response", 0.88f, "Accept authorized inspection commands");

        // Mission behavior.
        var missions = so.FindProperty("missionBehaviors");
        if (missions != null && missions.isArray) missions.ClearArray();
        Mission("Close-Range Scan", "Inspection precision", 0.96f);
        Mission("Structural Analysis", "Classification accuracy", 0.93f);
        Mission("LiDAR Sweep", "Mapping accuracy", 0.94f);
        Mission("Thermal Inspection", "Anomaly detection", 0.88f);
        Mission("Structural Marking", "Marking accuracy", 0.92f);
        Mission("Hover Inspection", "Stability", 0.96f);
        Mission("Data Relay", "Transmission integrity", 0.94f);
        Mission("Emergency Retreat", "Response efficiency", 0.94f);

        // Unit synergies.
        var synergies = so.FindProperty("unitSynergies");
        if (synergies != null && synergies.isArray) synergies.ClearArray();
        Synergy("Warden", 0.80f, "Provide aerial reconnaissance support");
        Synergy("Beacon", 0.76f, "Maintain communications coverage");
        Synergy("Iris", 0.88f, "Exchange high-resolution imaging data");
        Synergy("Sentinel", 0.74f, "Provide perimeter security during inspections");
        Synergy("Overseer", 0.82f, "Receive structural intelligence");
        Synergy("Insight", 0.95f, "Analyze structural and anomaly data");

        // Damage-response layer.
        var damages = so.FindProperty("damageResponses");
        if (damages != null && damages.isArray) damages.ClearArray();
        Damage("Ballistic", "Sensor degradation and reduced flight stability", "Automatic recalibration and repositioning");
        Damage("EMP", "Temporary communications loss", "Protected recovery and communications reacquisition");
        Damage("Weather", "Reduced hover stability", "Increase inspection distance");
        Damage("Optical Sensor", "Reduced visual analysis", "Recalibrate optical sensor");
        Damage("LiDAR", "Close-range mapping unavailable", "Repair or replace LiDAR assembly");
        Damage("Rotor", "Reduced hover precision", "Emergency return");

        // Upgrade scaling.
        var upgrades = so.FindProperty("upgrades");
        if (upgrades != null && upgrades.isArray) upgrades.ClearArray();
        Upgrade("Ultra-Zoom Lens", "+25% optical magnification; +10% inspection accuracy", 0.10f);
        Upgrade("Thermal Enhancement Module", "+20% thermal range; +8% anomaly detection", 0.08f);
        Upgrade("Silent Rotor Kit", "-25% noise signature; +5% stealth efficiency", 0.05f);
        Upgrade("Micro-Hover Stabilization Grid", "+15% hover precision; +10% LiDAR accuracy", 0.10f);
        Upgrade("Precision LiDAR Processor", "+15% structural mapping accuracy; +8% processing efficiency", 0.08f);

        // Threat response: reaction times in seconds, efficiencies as fractions.
        var threats = so.FindProperty("threatResponses");
        if (threats != null && threats.isArray) threats.ClearArray();
        Threat("Ballistic", 0.25f, 0.38f);
        Threat("Explosive", 0.60f, 0.34f);
        Threat("EMP", 0.90f, 0.31f);
        Threat("Thermal", 0.35f, 0.45f);
        Threat("Signal Jam", 0.70f, 0.40f);
        Threat("Weather", 0.50f, 0.52f);
        Threat("Structural Collapse", 0.15f, 0.94f);

        // Command-network analytics.
        var commandNodes = so.FindProperty("commandNetwork");
        if (commandNodes != null && commandNodes.isArray) commandNodes.ClearArray();
        NetworkNode("Standard Warden Mesh", 29f, 0.45f, "Stable");
        NetworkNode("Inspection Data Link", 38f, 0.20f, "Active");
        NetworkNode("LiDAR Processing Core", 47f, 0.14f, "Optimal");
        NetworkNode("Optical Analysis Core", 42f, 0.18f, "Active");
        NetworkNode("Insight / Command Core", 35f, 0.50f, "Active");

        // Squad and command integration.
        S("squad.squadRole", "Structural Inspection");
        I("squad.squadSizeContribution", 1);
        S("squad.formationBehavior", "Tight hover patterns");
        S("squad.intentInterpretationRules", "Approach slowly; maintain inspection distance");
        S("squad.retreatBehavior", "Early");
        S("squad.breachBehavior", "None");
        S("squad.flankingBehavior", "None");
        S("squad.reinforcementBehavior", "Supports engineering squads");
        S("squad.parentCommandUnit", "Insight / Command Core");
        S("squad.networkDependencies", "Standard Warden mesh");
        Strings("squad.subordinateUnits");
        Strings("squad.relayNodes");

        // Structured build requirements.
        var common = so.FindProperty("buildRequirements.commonComponents");
        if (common != null && common.isArray) common.ClearArray();
        Resource("buildRequirements.commonComponents", "Light Alloy Plates", 12);
        Resource("buildRequirements.commonComponents", "Composite Frame Rods", 8);
        Resource("buildRequirements.commonComponents", "Micro-Servo Actuators", 6);
        Resource("buildRequirements.commonComponents", "Stabilizer Fins", 2);
        Resource("buildRequirements.commonComponents", "Inspection Camera Housing", 1);

        var electronics = so.FindProperty("buildRequirements.electronics");
        if (electronics != null && electronics.isArray) electronics.ClearArray();
        Resource("buildRequirements.electronics", "Circuit Bundles", 10);
        Resource("buildRequirements.electronics", "Signal Processors", 6);
        Resource("buildRequirements.electronics", "Drone Control Chips", 4);
        Resource("buildRequirements.electronics", "LiDAR Micro-Emitters", 2);

        var power = so.FindProperty("buildRequirements.power");
        if (power != null && power.isArray) power.ClearArray();
        Resource("buildRequirements.power", "Compact Battery Pack", 1);
        Resource("buildRequirements.power", "Power Regulation Nodes", 2);

        var rare = so.FindProperty("buildRequirements.rareTradeable");
        if (rare != null && rare.isArray) rare.ClearArray();
        Resource("buildRequirements.rareTradeable", "Precision Inspection Lens", 1);
        Resource("buildRequirements.rareTradeable", "Short-Range LiDAR Core", 1);

        Strings("buildRequirements.optionalUpgrades",
          "Ultra-Zoom Lens", "Thermal Enhancement Module", "Silent Rotor Kit",
          "Micro-Hover Stabilization Grid", "Precision LiDAR Processor");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log("Inspect canonical population saved: " +
                  AssetDatabase.GetAssetPath(target) +
                  ". Preserved modelPrefab and storeImage. Missing MTBF defaulted to 40 hours.");
    }
}
