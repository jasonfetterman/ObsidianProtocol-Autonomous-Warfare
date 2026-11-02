using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateLidarCanonicalData
{
    [MenuItem("Obsidian Protocol/Units/Populate LiDAR Canonical Data")]
    public static void Populate()
    {
        UnitDefinition target = null;

        foreach (string guid in AssetDatabase.FindAssets("t:UnitDefinition"))
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<UnitDefinition>(assetPath);
            if (asset == null) continue;

            var check = new SerializedObject(asset);
            string id = check.FindProperty("unitId")?.stringValue ?? "";
            string display = check.FindProperty("displayName")?.stringValue ?? "";

            if (id == "obs-air-lidar" || display == "LiDAR")
            {
                target = asset;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError("LiDAR population failed: existing UnitDefinition not found.");
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

        SerializedProperty GetArray(string path, bool clear = true)
        {
            var p = so.FindProperty(path);
            if (p == null || !p.isArray || p.propertyType == SerializedPropertyType.String)
                return null;
            if (clear) p.ClearArray();
            return p;
        }

        SerializedProperty Add(SerializedProperty array)
        {
            int index = array.arraySize;
            array.arraySize = index + 1;
            return array.GetArrayElementAtIndex(index);
        }

        void Strings(string path, params string[] values)
        {
            var array = GetArray(path);
            if (array == null) return;
            foreach (string value in values)
                Add(array).stringValue = value;
        }

        void Modifier(string condition, string effect)
        {
            var array = GetArray("environment.modifiers", false);
            if (array == null) return;
            var e = Add(array);
            var c = e.FindPropertyRelative("condition");
            var f = e.FindPropertyRelative("effect");
            if (c != null) c.stringValue = condition;
            if (f != null) f.stringValue = effect;
        }

        void Behavior(string node, float weight, string description)
        {
            var e = Add(GetArray("behaviorNodes", false));
            if (e == null) return;
            e.FindPropertyRelative("behaviorNode").stringValue = node;
            e.FindPropertyRelative("weight").floatValue = weight;
            e.FindPropertyRelative("description").stringValue = description;
        }

        void Mission(string type, string focus, float efficiency)
        {
            var e = Add(GetArray("missionBehaviors", false));
            if (e == null) return;
            e.FindPropertyRelative("missionType").stringValue = type;
            e.FindPropertyRelative("behaviorFocus").stringValue = focus;
            e.FindPropertyRelative("efficiency").floatValue = efficiency;
        }

        void Synergy(string partner, float rating, string function)
        {
            var e = Add(GetArray("unitSynergies", false));
            if (e == null) return;
            e.FindPropertyRelative("partnerUnit").stringValue = partner;
            e.FindPropertyRelative("synergyRating").floatValue = rating;
            e.FindPropertyRelative("function").stringValue = function;
        }

        void Damage(string type, string effect, string recovery)
        {
            var e = Add(GetArray("damageResponses", false));
            if (e == null) return;
            e.FindPropertyRelative("damageType").stringValue = type;
            e.FindPropertyRelative("effect").stringValue = effect;
            e.FindPropertyRelative("recovery").stringValue = recovery;
        }

        void Upgrade(string name, string effect, float gain)
        {
            var e = Add(GetArray("upgrades", false));
            if (e == null) return;
            e.FindPropertyRelative("upgrade").stringValue = name;
            e.FindPropertyRelative("effect").stringValue = effect;
            e.FindPropertyRelative("efficiencyGain").floatValue = gain;
        }

        void Threat(string type, float response, float efficiency)
        {
            var e = Add(GetArray("threatResponses", false));
            if (e == null) return;
            e.FindPropertyRelative("threatType").stringValue = type;
            e.FindPropertyRelative("reactionTimeSeconds").floatValue = response;
            e.FindPropertyRelative("countermeasureEfficiency").floatValue = efficiency;
        }

        void NetworkNode(string node, float load, float latency, string status)
        {
            var e = Add(GetArray("commandNetwork", false));
            if (e == null) return;
            e.FindPropertyRelative("node").stringValue = node;
            e.FindPropertyRelative("loadPercent").floatValue = load;
            e.FindPropertyRelative("latencySeconds").floatValue = latency;
            e.FindPropertyRelative("status").stringValue = status;
        }

        void Resource(string path, string component, int quantity)
        {
            var e = Add(GetArray(path, false));
            if (e == null) return;
            e.FindPropertyRelative("component").stringValue = component;
            e.FindPropertyRelative("quantity").intValue = quantity;
        }

        // Identity. Keep modelPrefab and storeImage untouched.
        S("unitId", "obs-air-lidar");
        S("displayName", "LiDAR");
        S("role", "3D Terrain Mapping / Structural Scanning");
        S("tier", "Advanced");
        S("status", "Active");

        // Physical and technical.
        F("physical.lengthMeters", 2.8f);
        F("physical.widthMeters", 2.1f);
        F("physical.heightMeters", 1.0f);
        F("physical.massKg", 28f);
        S("physical.powerSource", "High-capacity battery");
        F("physical.energyCapacityWh", 2600f);
        F("physical.energyConsumptionWhPerMinute", 60f);
        S("physical.endurance", "30–45 min");
        F("physical.payloadCapacityKg", 2.0f);
        F("physical.structuralIntegrityHp", 140f);
        F("physical.armorRating", 0.20f);
        S("physical.navigationSystem", "GPS + LiDAR-assisted stabilization");
        S("physical.heatSignature", "Medium");
        S("physical.noiseSignature", "Medium");
        F("physical.topSpeedMetersPerSecond", 20f);
        S("physical.topSpeedDescription", "20 m/s");
        S("physical.acceleration", "Medium");
        S("physical.turningRadius", "Tight");
        S("physical.climbRate", "Medium");
        S("physical.stabilityRating", "Very High (mapping precision)");
        Strings("physical.modules", "LiDAR emitter array");

        // Autonomy.
        S("autonomy.behaviorProfile", "Mapping / Survey");
        F("autonomy.aggression", 0f);
        F("autonomy.caution", 0.8f);
        F("autonomy.persistence", 0.9f);
        F("autonomy.flankingLikelihood", 0f);
        F("autonomy.retreatThreshold", 0.7f);
        S("autonomy.targetPriority", "Terrain features / structures");
        S("autonomy.formationDiscipline", "High");
        S("autonomy.retreatBehaviorDescription", "Withdraw when mapping risk exceeds thresholds");

        // Sensors and communications.
        F("sensors.visualRangeMeters", 1400f);
        F("sensors.thermalRangeMeters", 600f);
        S("sensors.visualRangeDescription", "1400 m");
        S("sensors.thermalRangeDescription", "600 m");
        S("sensors.acousticRange", "Low");
        S("sensors.radarRange", "None");
        S("sensors.lidarRange", "2000 m (high-resolution)");
        S("sensors.signalDetectionRange", "Medium");

        F("communication.meshRangeKm", 6f);
        S("communication.meshRangeDescription", "6 km");
        F("communication.relayCapacity", 0f);
        S("communication.commandLinkType", "Mapping Grid / Command Mesh");
        S("communication.networkPriority", "High");

        // Environment.
        S("environment.terrainAdaptationCoefficients",
          "Open Air 1.00 / Urban 0.95 / Forest 0.90 / Mountain 0.95 / Desert 0.92 / Snow 0.85 / Water 0.60");
        F("environment.weatherSensitivityIndex", 0.35f);
        S("environment.visibilityModifiers",
          "Day 1.00 / Night 0.85 / Fog 0.70 / Rain 0.78 / Heavy Storm 0.60");
        S("environment.signalPropagationCurve",
          "Effective relay efficiency decreases 0.15% per 10% atmospheric interference");
        Modifier("Severe Weather", "-25% LiDAR mapping efficiency");
        Modifier("Electromagnetic Interference", "-30% external network efficiency");
        Modifier("Ballistic Damage", "LiDAR housing scan accuracy -12%; recalibrate in 35 seconds");
        Modifier("Explosive Damage", "Stabilization -20%; flight compensation in 25 seconds");
        Modifier("EMP", "Mapping Grid signal -45%; reboot in 75 seconds");
        Modifier("Weather Damage", "Scan efficiency -20%; automatically adjust scan density");
        Modifier("Jamming", "Mapping Grid range -40%; enable alternate command mesh");
        Modifier("Sensor Damage", "Reduce LiDAR accuracy and terrain confidence");

        // Network and cognitive.
        F("network.nodeHandoffEfficiency", 0.97f);
        S("network.packetLossRate", "1.0% nominal");
        F("network.encryptionLatencySeconds", 0.07f);
        S("network.meshSelfHealingThreshold", "2 lost nodes");
        S("network.relaySaturationCurve", "Throughput decreases 4% per active relay beyond 8 connected nodes");

        F("cognitive.decisionConfidence", 0.94f);
        F("cognitive.threatRecognitionAccuracy", 0.98f);
        F("cognitive.adaptiveLearningRatePerMission", 0.05f);
        F("cognitive.commandOverrideResistance", 0.05f);
        F("cognitive.cooperativeBehaviorIndex", 0.97f);

        // Non-combat threat profile.
        F("combat.firepowerRating", 0f);
        F("combat.suppressionValue", 0f);
        I("combat.threatLevel", 3);
        S("combat.accuracy", "N/A");
        S("combat.stabilization", "Very High");
        S("combat.weaponMounts", "None");
        S("combat.weaponTypes", "None");
        S("combat.trackingStrength", "Medium");
        S("combat.disruptionStrength", "Low");
        S("combat.breachCapability", "None");
        S("combat.countermeasureRating", "Low");
        S("combat.engagementDistance", "N/A");
        S("combat.preferredTerrain", "Mapping corridors / open air");
        S("combat.preferredTargets", "None");
        S("combat.vulnerabilityProfile", "Ballistic / weather");

        // Tactical.
        F("tactical.formationAdaptability", 0.91f);
        F("tactical.tacticalRoleWeight", 0.82f);
        F("tactical.commandResponseDelaySeconds", 0.35f);
        S("tactical.reinforcementTriggerConditions", "Mapping coverage loss >25% OR scan quality <70%");
        S("tactical.retreatLogicThresholds", "Terrain integrity risk >30% OR battery <20%");

        // Maintenance: convert the supplied rates to schema units where applicable.
        F("maintenance.meanTimeBetweenFailuresHours", 1200f);
        F("maintenance.repairCycleSeconds", 18f);
        F("maintenance.resourceDrainPerHourPercent", 0.42f);
        F("maintenance.batteryEfficiencyPercent", 92f);
        F("maintenance.sensorCalibrationDriftPercentPerHour", 0.35f);
        F("maintenance.rotorStabilityPercent", 96f);
        F("maintenance.coolingEfficiencyPercent", 90f);
        F("maintenance.operationalUptimePercent", 98.5f);
        F("maintenance.sparePartDependencyIndex", 0.61f);
        S("maintenance.maintenancePriorityLevel", "Tier II");
        S("maintenance.operationalCostPerHour", "7 Metal / 6 Electronics");

        // Performance.
        F("performance.missionSuccessProbability", 0.98f);
        F("performance.averageEngagementDurationMinutes", 4.5f);
        F("performance.reconDataYieldMbPerMinute", 32f);
        F("performance.communicationUptime", 0.985f);
        F("performance.threatAvoidanceEfficiency", 0.86f);

        // VR operator.
        S("vrOperator.operatorSeatType", "Drone View");
        S("vrOperator.operatorHud", "LiDAR Grid Overlay");
        S("vrOperator.cameraFeeds", "Optical");
        S("vrOperator.movementControls", "Full drone flight");
        S("vrOperator.sensorControls", "LiDAR sweep / density adjustment");
        S("vrOperator.abilityControls", "Scan, Map Terrain, Structural Sweep");
        S("vrOperator.weaponControls", "None");
        S("vrOperator.stabilizationControls", "Auto-hover, mapping-lock");
        B("vrOperator.thermalView", true);
        B("vrOperator.lidarView", true);
        B("vrOperator.sonarView", false);
        B("vrOperator.tacticalOverlay", true);
        Strings("vrOperator.operatorAbilities",
          "LiDAR Sweep", "Terrain Mesh Generation", "Structural Scan", "Mark Terrain Features");

        // Economy.
        S("economy.buildCost", "220 Metal / 200 Electronics");
        S("economy.factoryType", "Drone Bay");
        S("economy.techRequirements", "Mapping Systems I");
        S("economy.maintenanceCost", "Medium");
        F("economy.buildTimeSeconds", 70f);
        F("economy.repairTimeSeconds", 18f);
        S("economy.supplyConsumption", "Medium");
        S("economy.operationalCost", "Moderate");
        S("economy.deploymentRequirements", "Standard drone pad");
        S("economy.transportCompatibility", "All aerial racks");
        S("economy.launchMethod", "Vertical takeoff");

        // Behavior matrix.
        GetArray("behaviorNodes");
        Behavior("Terrain Mapping", 0.98f, "Generate continuous 3D terrain data");
        Behavior("LiDAR Scanning", 0.99f, "Perform high-resolution terrain sweeps");
        Behavior("Structural Analysis", 0.94f, "Identify structural geometry and anomalies");
        Behavior("Terrain Classification", 0.93f, "Classify terrain and obstacles");
        Behavior("Coverage Optimization", 0.95f, "Maintain complete mapping coverage");
        Behavior("Scan Density Control", 0.91f, "Adjust scan density to mission needs");
        Behavior("Feature Marking", 0.92f, "Mark important terrain features");
        Behavior("Route Adaptation", 0.88f, "Adjust flight path around terrain");
        Behavior("Retreat Logic", 0.70f, "Withdraw when mapping risk exceeds threshold");
        Behavior("Operator Override", 0.85f, "Accept authorized mapping commands");

        // Mission behavior.
        GetArray("missionBehaviors");
        Mission("Terrain Sweep Completion", "Completion", 0.95f);
        Mission("Structural Scan Completion", "Completion", 0.92f);
        Mission("Terrain Feature Marking", "Marking accuracy", 0.94f);
        Mission("Route Obstacle Identification", "Obstacle identification", 0.93f);
        Mission("Mapping Data Relay", "Transmission integrity", 0.97f);
        Mission("Emergency Safe-Route Generation", "Safe-route generation", 0.89f);

        // Inter-unit coordination.
        GetArray("unitSynergies");
        Synergy("Warden", 0.90f, "Supply aerial reconnaissance context");
        Synergy("Inspect", 0.94f, "Combine terrain mesh and structural inspection");
        Synergy("Iris", 0.88f, "Correlate imaging with 3D mapping");

        // Damage responses.
        GetArray("damageResponses");
        Damage("Ballistic", "LiDAR housing scan accuracy reduced by 12%", "Recalibrate in 35 seconds");
        Damage("Explosive", "Stabilization reduced by 20%", "Flight compensation in 25 seconds");
        Damage("EMP", "Mapping Grid signal reduced by 45%", "Reboot in 75 seconds");
        Damage("Weather", "Scan efficiency reduced by 20%", "Automatically adjust scan density");
        Damage("Jamming", "Mapping Grid range reduced by 40%", "Switch to alternate command mesh");
        Damage("Sensor Damage", "LiDAR accuracy and terrain confidence reduced", "Repair or recalibrate sensor");
        Damage("Structural Hazard", "Unsafe geometry detected", "Mark hazard and update terrain mesh");

        // Upgrade scaling.
        GetArray("upgrades");
        Upgrade("Extended LiDAR Range Module", "+25% scan range; +8% terrain coverage", 0.08f);
        Upgrade("High-Density Scan Grid", "+20% point density; +10% structural scan", 0.10f);
        Upgrade("Anti-Jamming Shielding", "+30% mapping-link resilience; +8% uptime", 0.08f);
        Upgrade("Silent Rotor Kit", "-25% noise signature; +7% covert survey", 0.07f);
        Upgrade("Terrain Processing Core", "+15% terrain classification; +8% processing", 0.08f);
        Upgrade("Adaptive Scan Stabilizer", "+20% mapping stability; +6% scan accuracy", 0.06f);

        // Threat-response efficiencies are stored as fractions.
        GetArray("threatResponses");
        Threat("Ballistic", 0.40f, 0.45f);
        Threat("Explosive", 0.80f, 0.40f);
        Threat("EMP", 0.20f, 0.38f);
        Threat("Thermal", 0.40f, 0.52f);
        Threat("Signal Jam", 0.20f, 0.58f);
        Threat("Structural Hazard", 0.30f, 0.78f);

        // Command network.
        GetArray("commandNetwork");
        NetworkNode("Mapping Grid", 44f, 0.35f, "Optimal");
        NetworkNode("LiDAR Processing Core", 57f, 0.09f, "Optimal");
        NetworkNode("Terrain Mesh Processor", 49f, 0.12f, "Stable");
        NetworkNode("Cartographer Link", 46f, 0.30f, "Active");
        NetworkNode("Insight Link", 42f, 0.35f, "Active");
        NetworkNode("Command Core", 39f, 0.40f, "Active");

        // Squad and command.
        S("squad.squadRole", "3D Mapping");
        I("squad.squadSizeContribution", 1);
        S("squad.formationBehavior", "Grid-pattern scanning");
        S("squad.intentInterpretationRules", "Maintain mapping altitude and coverage");
        S("squad.retreatBehavior", "Medium");
        S("squad.breachBehavior", "None");
        S("squad.flankingBehavior", "None");
        S("squad.reinforcementBehavior", "Supports recon and engineering squads");
        S("squad.parentCommandUnit", "Cartographer / Insight");
        S("squad.networkDependencies", "Mapping Grid");
        Strings("squad.subordinateUnits");
        Strings("squad.relayNodes");

        // Build requirements.
        GetArray("buildRequirements.commonComponents");
        Resource("buildRequirements.commonComponents", "Medium Alloy Plates", 16);
        Resource("buildRequirements.commonComponents", "Composite Frame Rods", 12);
        Resource("buildRequirements.commonComponents", "Micro-Servo Actuators", 8);
        Resource("buildRequirements.commonComponents", "Stabilizer Fins", 4);
        Resource("buildRequirements.commonComponents", "LiDAR Emitter Housing", 1);

        GetArray("buildRequirements.electronics");
        Resource("buildRequirements.electronics", "Circuit Bundles", 16);
        Resource("buildRequirements.electronics", "Signal Processors", 12);
        Resource("buildRequirements.electronics", "Drone Control Chips", 6);
        Resource("buildRequirements.electronics", "LiDAR Processing Chips", 4);

        GetArray("buildRequirements.power");
        Resource("buildRequirements.power", "High-Capacity Battery Pack", 1);
        Resource("buildRequirements.power", "Power Regulation Nodes", 3);

        GetArray("buildRequirements.rareTradeable");
        Resource("buildRequirements.rareTradeable", "High-Resolution LiDAR Core", 1);
        Resource("buildRequirements.rareTradeable", "Mapping Grid Microprocessor", 1);

        Strings("buildRequirements.optionalUpgrades",
          "Extended LiDAR Range Module", "High-Density Scan Grid",
          "Anti-Jamming Shielding", "Silent Rotor Kit",
          "Terrain Processing Core", "Adaptive Scan Stabilizer");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log("LiDAR canonical population saved: " +
                  AssetDatabase.GetAssetPath(target) +
                  ". Preserved modelPrefab and storeImage.");
    }
}
