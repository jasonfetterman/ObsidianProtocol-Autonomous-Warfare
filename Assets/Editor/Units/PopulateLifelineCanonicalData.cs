using UnityEditor;
using UnityEngine;

public static class PopulateLifelineCanonicalData
{
    private const string UnitId = "obs-air-lifeline";

    [MenuItem("Obsidian Protocol/Units/Populate Lifeline Canonical Data")]
    public static void Populate()
    {
        Object asset = null;

        foreach (string guid in AssetDatabase.FindAssets("t:UnitDefinition"))
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Object candidate = AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (candidate == null) continue;

            var test = new SerializedObject(candidate);
            var id = test.FindProperty("unitId");
            var name = test.FindProperty("displayName");

            if ((id != null && id.stringValue == UnitId) ||
                (name != null && name.stringValue == "Lifeline"))
            {
                asset = candidate;
                break;
            }
        }

        if (asset == null)
        {
            Debug.LogError("Lifeline population failed: existing UnitDefinition asset not found for " + UnitId);
            return;
        }

        var so = new SerializedObject(asset);
        so.Update();

        void S(string p, string v) {
            var x = so.FindProperty(p);
            if (x != null && x.propertyType == SerializedPropertyType.String) x.stringValue = v;
        }
        void F(string p, float v) {
            var x = so.FindProperty(p);
            if (x != null && x.propertyType == SerializedPropertyType.Float) x.floatValue = v;
        }
        void I(string p, int v) {
            var x = so.FindProperty(p);
            if (x != null && x.propertyType == SerializedPropertyType.Integer) x.intValue = v;
        }
        void B(string p, bool v) {
            var x = so.FindProperty(p);
            if (x != null && x.propertyType == SerializedPropertyType.Boolean) x.boolValue = v;
        }
        SerializedProperty A(string p, bool clear = true) {
            var x = so.FindProperty(p);
            if (x != null && x.isArray && clear) x.ClearArray();
            return x;
        }
        void Strings(string p, params string[] values) {
            var a = A(p);
            if (a == null) return;
            foreach (var value in values) {
                int n = a.arraySize++;
                a.GetArrayElementAtIndex(n).stringValue = value;
            }
        }
        void Entry(string path, params (string field, string value)[] fields) {
            var a = A(path, false);
            if (a == null) return;
            int n = a.arraySize++;
            var e = a.GetArrayElementAtIndex(n);
            foreach (var pair in fields) {
                var x = e.FindPropertyRelative(pair.field);
                if (x != null && x.propertyType == SerializedPropertyType.String)
                    x.stringValue = pair.value;
            }
        }
        void Behavior(string name, float weight, string description) {
            var a = A("behaviorNodes", false);
            if (a == null) return;
            int n = a.arraySize++;
            var e = a.GetArrayElementAtIndex(n);
            var p = e.FindPropertyRelative("behaviorNode"); if (p != null) p.stringValue = name;
            p = e.FindPropertyRelative("weight"); if (p != null) p.floatValue = weight;
            p = e.FindPropertyRelative("description"); if (p != null) p.stringValue = description;
        }
        void Mission(string name, string focus, float efficiency) {
            var a = A("missionBehaviors", false);
            if (a == null) return;
            int n = a.arraySize++;
            var e = a.GetArrayElementAtIndex(n);
            var p = e.FindPropertyRelative("missionType"); if (p != null) p.stringValue = name;
            p = e.FindPropertyRelative("behaviorFocus"); if (p != null) p.stringValue = focus;
            p = e.FindPropertyRelative("efficiency"); if (p != null) p.floatValue = efficiency;
        }
        void Synergy(string partner, float rating, string function) {
            var a = A("unitSynergies", false);
            if (a == null) return;
            int n = a.arraySize++;
            var e = a.GetArrayElementAtIndex(n);
            var p = e.FindPropertyRelative("partnerUnit"); if (p != null) p.stringValue = partner;
            p = e.FindPropertyRelative("synergyRating"); if (p != null) p.floatValue = rating;
            p = e.FindPropertyRelative("function"); if (p != null) p.stringValue = function;
        }
        void Damage(string type, string effect, string recovery) {
            var a = A("damageResponses", false);
            if (a == null) return;
            int n = a.arraySize++;
            var e = a.GetArrayElementAtIndex(n);
            var p = e.FindPropertyRelative("damageType"); if (p != null) p.stringValue = type;
            p = e.FindPropertyRelative("effect"); if (p != null) p.stringValue = effect;
            p = e.FindPropertyRelative("recovery"); if (p != null) p.stringValue = recovery;
        }
        void Upgrade(string name, string effect, float gain) {
            var a = A("upgrades", false);
            if (a == null) return;
            int n = a.arraySize++;
            var e = a.GetArrayElementAtIndex(n);
            var p = e.FindPropertyRelative("upgrade"); if (p != null) p.stringValue = name;
            p = e.FindPropertyRelative("effect"); if (p != null) p.stringValue = effect;
            p = e.FindPropertyRelative("efficiencyGain"); if (p != null) p.floatValue = gain;
        }
        void Threat(string type, float reaction, float efficiency) {
            var a = A("threatResponses", false);
            if (a == null) return;
            int n = a.arraySize++;
            var e = a.GetArrayElementAtIndex(n);
            var p = e.FindPropertyRelative("threatType"); if (p != null) p.stringValue = type;
            p = e.FindPropertyRelative("reactionTimeSeconds"); if (p != null) p.floatValue = reaction;
            p = e.FindPropertyRelative("countermeasureEfficiency"); if (p != null) p.floatValue = efficiency;
        }
        void Network(string node, float load, float latency, string status) {
            var a = A("commandNetwork", false);
            if (a == null) return;
            int n = a.arraySize++;
            var e = a.GetArrayElementAtIndex(n);
            var p = e.FindPropertyRelative("node"); if (p != null) p.stringValue = node;
            p = e.FindPropertyRelative("loadPercent"); if (p != null) p.floatValue = load;
            p = e.FindPropertyRelative("latencySeconds"); if (p != null) p.floatValue = latency;
            p = e.FindPropertyRelative("status"); if (p != null) p.stringValue = status;
        }
        void Modifier(string condition, string effect) {
            var a = A("environment.modifiers", false);
            if (a == null) return;
            int n = a.arraySize++;
            var e = a.GetArrayElementAtIndex(n);
            var p = e.FindPropertyRelative("condition"); if (p != null) p.stringValue = condition;
            p = e.FindPropertyRelative("effect"); if (p != null) p.stringValue = effect;
        }
        void Resources(string path, params (string component, int quantity)[] items) {
            var a = A(path);
            if (a == null) return;
            foreach (var item in items) {
                int n = a.arraySize++;
                var e = a.GetArrayElementAtIndex(n);
                var p = e.FindPropertyRelative("component"); if (p != null) p.stringValue = item.component;
                p = e.FindPropertyRelative("quantity"); if (p != null) p.intValue = item.quantity;
            }
        }

        // Identity and general classification. Preserve category, prefab, and store image.
        S("unitId", UnitId);
        S("displayName", "Lifeline");
        S("role", "Emergency Support / Medical Supply / Rapid Response");
        S("tier", "Advanced Support");
        S("status", "Active");

        // Physical and technical.
        F("physical.lengthMeters", 3.2f);
        F("physical.widthMeters", 2.4f);
        F("physical.heightMeters", 1.2f);
        F("physical.massKg", 30f);
        S("physical.powerSource", "High-capacity battery");
        F("physical.energyCapacityWh", 2600f);
        F("physical.energyConsumptionWhPerMinute", 60f);
        S("physical.endurance", "30–45 min");
        F("physical.payloadCapacityKg", 3f);
        Strings("physical.modules", "Medical supply bay", "Stabilization rotors");
        S("physical.navigationSystem", "GPS + Emergency Routing AI");
        F("physical.structuralIntegrityHp", 150f);
        F("physical.armorRating", 0.22f);
        S("physical.heatSignature", "Medium");
        S("physical.noiseSignature", "Medium");
        F("physical.topSpeedMetersPerSecond", 21f);
        S("physical.topSpeedDescription", "21 m/s");
        S("physical.acceleration", "Medium");
        S("physical.turningRadius", "Moderate");
        S("physical.climbRate", "Medium");
        S("physical.stabilityRating", "Very High (safe medical delivery)");

        // Autonomy and rescue priorities.
        S("autonomy.behaviorProfile", "Rescue Support / Medical Delivery");
        F("autonomy.aggression", 0f);
        F("autonomy.caution", 0.9f);
        F("autonomy.persistence", 0.8f);
        F("autonomy.flankingLikelihood", 0f);
        F("autonomy.retreatThreshold", 0.7f);
        F("autonomy.riskTolerance", 0.15f);
        S("autonomy.retreatBehaviorDescription",
          "Return to support if integrity <30%, battery <20%, payload compromised, or rescue corridor impassable.");
        S("autonomy.targetPriority", "Injured units / emergency zones");
        S("autonomy.formationDiscipline", "Medium");

        // Sensors.
        F("sensors.visualRangeMeters", 1400f);
        F("sensors.thermalRangeMeters", 800f);
        S("sensors.visualRangeDescription", "1400 m");
        S("sensors.thermalRangeDescription", "800 m");
        S("sensors.acousticRange", "Low");
        S("sensors.radarRange", "None");
        S("sensors.lidarRange", "Short-range");
        S("sensors.signalDetectionRange", "Medium");

        // Communication and mesh network.
        F("communication.meshRangeKm", 6f);
        S("communication.meshRangeDescription", "6 km");
        F("communication.relayCapacity", 0f);
        S("communication.commandLinkType", "Rescue Grid / Command Mesh");
        S("communication.networkPriority", "High");
        F("network.nodeHandoffEfficiency", 0.97f);
        S("network.packetLossRate", "1.0% nominal");
        F("network.encryptionLatencySeconds", 0.07f);
        S("network.meshSelfHealingThreshold", "2 lost nodes");
        S("network.relaySaturationCurve", "Throughput decreases 4% per active relay beyond 8 connected nodes");

        // Combat model: noncombatant support drone.
        F("combat.firepowerRating", 0f);
        F("combat.suppressionValue", 0f);
        S("combat.firepowerDescription", "Noncombatant emergency support platform");
        S("combat.accuracy", "N/A");
        S("combat.stabilization", "Very High");
        S("combat.weaponMounts", "None");
        S("combat.weaponTypes", "None");
        S("combat.trackingStrength", "Medium");
        S("combat.disruptionStrength", "Low");
        S("combat.breachCapability", "None");
        S("combat.countermeasureRating", "Low");
        S("combat.engagementDistance", "N/A");
        S("combat.preferredTerrain", "Rescue corridors");
        S("combat.preferredTargets", "None");
        S("combat.vulnerabilityProfile", "Ballistic / weather");
        I("combat.threatLevel", 3);

        // Squad and command integration.
        S("squad.squadRole", "Emergency Support");
        I("squad.squadSizeContribution", 1);
        S("squad.formationBehavior", "Rescue corridor spacing");
        S("squad.intentInterpretationRules", "Prioritize medical delivery over speed");
        S("squad.retreatBehavior", "Medium");
        S("squad.breachBehavior", "None");
        S("squad.flankingBehavior", "None");
        S("squad.reinforcementBehavior", "Supports rescue and logistics squads");
        S("squad.parentCommandUnit", "Rescue / Command Core");
        S("squad.networkDependencies", "Rescue Grid");
        Strings("squad.subordinateUnits");
        Strings("squad.relayNodes");

        // VR/operator controls.
        S("vrOperator.operatorSeatType", "Drone View");
        S("vrOperator.operatorHud", "Medical Overlay");
        S("vrOperator.cameraFeeds", "Optical");
        S("vrOperator.movementControls", "Full drone flight");
        S("vrOperator.sensorControls", "Thermal / LiDAR / zoom");
        S("vrOperator.abilityControls", "Medical Drop, Scan, Stabilize");
        S("vrOperator.weaponControls", "None");
        S("vrOperator.stabilizationControls", "Auto-hover");
        B("vrOperator.thermalView", true);
        B("vrOperator.lidarView", true);
        B("vrOperator.sonarView", false);
        B("vrOperator.tacticalOverlay", true);
        Strings("vrOperator.operatorAbilities",
            "Medical Payload Drop", "Rescue Scan", "Thermal Search",
            "Stabilize", "Relay Rescue Data");

        // Economy and deployment.
        // Preserve the unit asset's existing credit price.
        I("economy.deploymentPoints", 0);
        B("economy.deploymentPointsDefined", false);
        S("economy.buildCost", "200 Metal / 220 Electronics");
        F("economy.buildTimeSeconds", 65f);
        S("economy.factoryType", "Drone Bay");
        S("economy.techRequirements", "Rescue Systems I");
        S("economy.maintenanceCost", "Medium");
        S("economy.supplyConsumption", "Medium");
        S("economy.operationalCost", "7 Metal / 7 Electronics per hour");
        F("economy.repairTimeSeconds", 16f);
        S("economy.deploymentRequirements", "Standard drone pad");
        S("economy.transportCompatibility", "All aerial racks");
        S("economy.launchMethod", "Vertical takeoff");

        // Build requirements.
        Resources("buildRequirements.commonComponents",
            ("Medium Alloy Plates", 16), ("Composite Frame Rods", 12),
            ("Micro-Servo Actuators", 8), ("Stabilizer Fins", 4),
            ("Medical Payload Bay Housing", 1));
        Resources("buildRequirements.electronics",
            ("Circuit Bundles", 14), ("Signal Processors", 10),
            ("Drone Control Chips", 6), ("Rescue AI Chips", 2));
        Resources("buildRequirements.power",
            ("High-Capacity Battery Pack", 1), ("Power Regulation Nodes", 3));
        Resources("buildRequirements.rareTradeable",
            ("Medical Delivery Core", 1), ("Thermal Search Processor", 1));
        Strings("buildRequirements.optionalUpgrades",
            "Extended Battery Module", "Silent Rotor Kit",
            "High-Capacity Medical Bay", "Anti-Jamming Shielding",
            "Thermal Search Enhancement", "Emergency Navigation Core");

        // Environment.
        S("environment.terrainAdaptationCoefficients",
          "Open Air 1.00 / Urban 0.95 / Forest 0.85 / Mountain 0.88 / Desert 0.92 / Snow 0.82 / Water 0.65");
        F("environment.weatherSensitivityIndex", 0.35f);
        S("environment.visibilityModifiers", "Day 1.00 / Night 0.85 / Fog 0.75 / Rain 0.80 / Heavy Storm 0.60");
        S("environment.signalPropagationCurve",
          "Atmospheric interference reduces effective relay efficiency by 0.15% per 10% interference");
        A("environment.modifiers");
        Modifier("Severe weather", "Medical delivery efficiency reduced by 25%");
        Modifier("Electromagnetic interference", "External network efficiency reduced by 30%");
        Modifier("Rescue route", "96% nominal route reliability");
        Modifier("Thermal search", "94% nominal search reliability");

        // Cognitive and tactical.
        F("cognitive.decisionConfidence", 0.94f);
        F("cognitive.threatRecognitionAccuracy", 0.96f);
        F("cognitive.adaptiveLearningRatePerMission", 0.05f);
        F("cognitive.commandOverrideResistance", 0.04f);
        F("cognitive.cooperativeBehaviorIndex", 0.98f);
        F("tactical.formationAdaptability", 0.84f);
        F("tactical.tacticalRoleWeight", 0.86f);
        F("tactical.commandResponseDelaySeconds", 0.35f);
        S("tactical.reinforcementTriggerConditions",
          "Active casualty detected or medical support request received");
        S("tactical.retreatLogicThresholds",
          "Integrity <30%; battery <20%; payload compromised; rescue corridor impassable");

        // Maintenance and energy.
        F("maintenance.meanTimeBetweenFailuresHours", 1300f);
        F("maintenance.repairCycleSeconds", 16f);
        F("maintenance.resourceDrainPerHourPercent", 0.40f);
        F("maintenance.batteryEfficiencyPercent", 92f);
        F("maintenance.coolingEfficiencyPercent", 96f);
        F("maintenance.componentWearPercentPerHour", 0.0032f);
        F("maintenance.sparePartDependencyIndex", 0.60f);
        F("maintenance.operationalUptimePercent", 98.5f);
        F("maintenance.energyRecoveryRatePercentPerIdleMinute", 6f);
        S("maintenance.maintenancePriorityLevel", "Tier II");
        S("maintenance.operationalCostPerHour", "7 Metal / 7 Electronics");
        S("maintenance.flightHardwareWear", "0.32% per 100 h");
        S("maintenance.medicalBayEfficiency", "96%");

        // Performance.
        F("performance.missionSuccessProbability", 0.96f);
        F("performance.averageEngagementDurationMinutes", 3.2f);
        F("performance.reconDataYieldMbPerMinute", 14f);
        F("performance.communicationUptime", 0.985f);
        F("performance.threatAvoidanceEfficiency", 0.91f);

        // AI behavior matrix.
        A("behaviorNodes");
        Behavior("Rescue Prioritization", 0.98f, "Prioritize active medical emergencies");
        Behavior("Medical Routing", 0.96f, "Generate safe delivery routes");
        Behavior("Casualty Detection", 0.93f, "Identify injured units and emergencies");
        Behavior("Thermal Search", 0.92f, "Locate personnel in obscured areas");
        Behavior("Delivery Precision", 0.95f, "Deliver medical payload accurately");
        Behavior("Emergency Avoidance", 0.94f, "Avoid active hazards during rescue");
        Behavior("Route Recalculation", 0.90f, "Recalculate when corridors are blocked");
        Behavior("Survival Priority", 0.97f, "Preserve aircraft and medical payload");
        Behavior("Operator Override", 0.88f, "Accept authorized rescue commands");

        A("missionBehaviors");
        Mission("Emergency Detection", "Detect medical emergencies", 0.96f);
        Mission("Casualty Location", "Locate injured units", 0.95f);
        Mission("Medical Delivery", "Complete medical payload delivery", 0.96f);
        Mission("Rescue Route", "Complete rescue routes", 0.93f);
        Mission("Thermal Search", "Locate casualties using thermal sensors", 0.94f);
        Mission("Rescue Data Relay", "Transmit rescue data", 0.97f);
        Mission("Emergency Reroute", "Generate alternate routes", 0.91f);

        A("unitSynergies");
        Synergy("Inspect", 0.92f, "Casualty and damage assessment");
        Synergy("Warden", 0.86f, "Protected rescue corridor support");
        Synergy("Drop", 0.90f, "Aerial logistics and medical supply delivery");

        A("damageResponses");
        Damage("Ballistic", "Medical bay protection reduced by 15%", "Return if payload integrity is threatened");
        Damage("Explosive", "Stabilization reduced by 20%", "Activate emergency hover compensation");
        Damage("EMP", "Rescue Grid signal reduced by 45%", "Enable autonomous recovery");
        Damage("Weather", "Delivery stability reduced by 20%", "Reduce route speed");
        Damage("Jamming", "Rescue Grid range reduced by 40%", "Switch to alternate command mesh");
        Damage("Sensor damage", "Thermal/LiDAR confidence reduced", "Reduce casualty-search confidence");
        Damage("Medical payload damage", "Payload falls below safe threshold", "Abort delivery and return to support");

        A("upgrades");
        Upgrade("Extended Battery Module", "+20% endurance; +8% emergency uptime", 0.20f);
        Upgrade("Silent Rotor Kit", "-25% noise signature; +6% survivability", 0.06f);
        Upgrade("High-Capacity Medical Bay", "+40% medical payload capacity; +10% delivery flexibility", 0.40f);
        Upgrade("Anti-Jamming Shielding", "+30% Rescue Grid resilience; +8% uptime", 0.30f);
        Upgrade("Thermal Search Enhancement", "+20% thermal range; +10% casualty detection accuracy", 0.20f);
        Upgrade("Emergency Navigation Core", "+15% route prediction; +8% route safety", 0.15f);

        A("threatResponses");
        Threat("Ballistic", 0.4f, 0.45f);
        Threat("Explosive", 0.8f, 0.42f);
        Threat("EMP", 0.2f, 0.40f);
        Threat("Thermal", 0.4f, 0.55f);
        Threat("Signal jam", 0.2f, 0.60f);
        Threat("Rescue corridor blockage", 0.3f, 0.82f);

        A("commandNetwork");
        Network("Rescue Grid", 42f, 0.35f, "Optimal");
        Network("Medical Routing Core", 51f, 0.10f, "Optimal");
        Network("Emergency Coordination Link", 47f, 0.25f, "Stable");
        Network("Rescue Command Core", 44f, 0.30f, "Active");
        Network("Command Core", 39f, 0.40f, "Active");

        // Metadata represented as descriptive fields where the schema has no dedicated property.
        S("physical.endurance", "30–45 min; active drain 60 Wh/min; medical delivery 68; thermal search 72; emergency response 80; idle 12; emergency power 15");
        S("squad.intentInterpretationRules",
          "Prioritize medical delivery over speed. Commands: respond to medical emergency; deliver supplies; locate injured unit; search rescue zone; establish rescue corridor; reroute around threat; report casualty status.");
        S("squad.reinforcementBehavior",
          "Supports rescue and logistics squads. Rescue Grid range 6 km; redundant links Rescue Grid / Command Mesh.");
        S("vrOperator.operatorHud",
          "Medical Overlay; rescue markers / medical zones; direct combat prohibited; autonomous assignment enabled.");
        S("economy.operationalCost", "7 Metal / 7 Electronics per hour; standard drone bay infrastructure");
        S("combat.preferredTargets", "None; direct combat assignment prohibited");
        S("maintenance.maintenancePriorityLevel", "Tier II");
        S("maintenance.operationalCostPerHour", "7 Metal / 7 Electronics");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Lifeline canonical data saved: " +
                  AssetDatabase.GetAssetPath(asset) +
                  ". Existing model prefab and store image were preserved. Check warnings for unsupported schema fields.");
    }
}
