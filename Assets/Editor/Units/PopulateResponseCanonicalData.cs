using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;
using System;

public static class PopulateResponseCanonicalData
{
    [MenuItem("Obsidian Protocol/Units/Populate Response Canonical Data")]
    public static void Populate()
    {
        const string responsePath = "Assets/Game/Units/Database/Air/Response/Response.asset";
        UnitDefinition unit = AssetDatabase.LoadAssetAtPath<UnitDefinition>(responsePath);

        if (unit == null)
        {
            Debug.LogError("Response population failed: no UnitDefinition found at " + responsePath +
                ". Confirm Response.asset exists and is a UnitDefinition asset.");
            return;
        }

        var so = new SerializedObject(unit);
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

        void A(string path, params string[] values)
        {
            var p = so.FindProperty(path);
            if (p == null || !p.isArray || p.propertyType == SerializedPropertyType.String)
                return;

            p.arraySize = 0;
            foreach (string value in values)
            {
                int index = p.arraySize;
                p.arraySize++;
                var element = p.GetArrayElementAtIndex(index);
                if (element.propertyType == SerializedPropertyType.String)
                    element.stringValue = value;
                else
                {
                    var child = element.FindPropertyRelative("abilityName")
                             ?? element.FindPropertyRelative("name")
                             ?? element.FindPropertyRelative("value");
                    if (child != null && child.propertyType == SerializedPropertyType.String)
                        child.stringValue = value;
                }
            }
        }

        void Resources(string path, (string name, int quantity)[] values)
        {
            var p = so.FindProperty(path);
            if (p == null || !p.isArray) return;
            p.arraySize = 0;

            foreach (var value in values)
            {
                int index = p.arraySize;
                p.arraySize++;
                var element = p.GetArrayElementAtIndex(index);
                var name = element.FindPropertyRelative("component");
                var quantity = element.FindPropertyRelative("quantity");
                if (name != null) name.stringValue = value.name;
                if (quantity != null) quantity.intValue = value.quantity;
            }
        }

        S("unitId", "obs-air-response");
        S("displayName", "Response");
        S("role", "Emergency Response / Search and Rescue");
        S("tier", "Operational Support");
        S("status", "Active");

        // PHYSICAL & TECHNICAL
        F("physical.lengthMeters", 2.4f);
        F("physical.widthMeters", 1.9f);
        F("physical.heightMeters", 1.0f);
        F("physical.massKg", 24f);
        S("physical.powerSource", "Dual high-density battery packs");
        F("physical.energyCapacityWh", 3200f);
        F("physical.energyConsumptionWhPerMinute", 48f);
        S("physical.endurance", "50–70 minutes; up to 85 minutes in low-power loiter");
        F("physical.payloadCapacityKg", 8f);
        F("physical.structuralIntegrityHp", 185f);
        F("physical.armorRating", 0.28f);
        F("physical.topSpeedMetersPerSecond", 22f);
        S("physical.topSpeedDescription", "22 m/s maximum; 8 m/s in precision rescue mode");
        S("physical.acceleration", "Medium-high");
        S("physical.turningRadius", "Tight");
        S("physical.climbRate", "High");
        S("physical.stabilityRating", "Very high");
        S("physical.navigationSystem", "GPS/INS, terrain mapping, visual-inertial navigation");
        S("physical.heatSignature", "Low-medium");
        S("physical.noiseSignature", "Medium");
        A("physical.modules",
            "Thermal search camera", "Medical supply pod", "Rescue winch",
            "Obstacle avoidance array", "Emergency beacon");

        // AUTONOMY & AI
        S("autonomy.behaviorProfile", "Search, Triage, Rescue and Evacuation Support");
        F("autonomy.aggression", 0.05f);
        F("autonomy.caution", 0.95f);
        F("autonomy.persistence", 0.92f);
        F("autonomy.flankingLikelihood", 0.0f);
        F("autonomy.retreatThreshold", 0.72f);
        F("autonomy.riskTolerance", 0.18f);
        S("autonomy.retreatBehaviorDescription", "Withdraws from unsafe flight conditions while maintaining victim tracking and beacon relay");
        S("autonomy.targetPriority", "Distress beacons, injured personnel, isolated squads, evacuation routes");
        S("autonomy.formationDiscipline", "High; maintains rescue corridor spacing");

        // SENSORS
        F("sensors.visualRangeMeters", 1800f);
        F("sensors.thermalRangeMeters", 1200f);
        S("sensors.visualRangeDescription", "High-resolution optical search and person detection to 1.8 km in clear conditions");
        S("sensors.thermalRangeDescription", "Human heat-source detection to 1.2 km, weather dependent");
        S("sensors.acousticRange", "Directional distress audio; effective to 350 m");
        S("sensors.radarRange", "Short-range obstacle radar to 500 m");
        S("sensors.lidarRange", "Mapping and obstacle avoidance to 220 m");
        S("sensors.signalDetectionRange", "Emergency beacon detection to 8 km line-of-sight");

        // COMMUNICATION
        F("communication.meshRangeKm", 16f);
        S("communication.meshRangeDescription", "16 km nominal line-of-sight; reduced by terrain and interference");
        F("communication.relayCapacity", 6f);
        S("communication.commandLinkType", "Emergency Tactical Grid");
        S("communication.networkPriority", "Critical rescue traffic");

        // COMBAT & THREAT MODEL
        F("combat.firepowerRating", 0f);
        F("combat.suppressionValue", 0f);
        I("combat.threatLevel", 1);
        S("combat.firepowerDescription", "Unarmed support aircraft; no offensive capability");
        S("combat.accuracy", "N/A");
        S("combat.stabilization", "High precision hover stabilization");
        S("combat.weaponMounts", "None");
        S("combat.weaponTypes", "None");
        S("combat.trackingStrength", "High for distress signals and human detection");
        S("combat.disruptionStrength", "Low; emergency signal repeater only");
        S("combat.breachCapability", "None");
        S("combat.countermeasureRating", "Medium; evasive flight and interference-aware routing");
        S("combat.engagementDistance", "N/A");
        S("combat.preferredTerrain", "Urban disaster zones, forest margins, rugged terrain, evacuation corridors");
        S("combat.preferredTargets", "N/A; prioritizes rescue subjects rather than hostile targets");
        S("combat.vulnerabilityProfile", "Small-arms fire, EMP, rotor damage, heavy weather, signal jamming");

        // SQUAD & COMMAND
        S("squad.squadRole", "Search and Rescue Support");
        I("squad.squadSizeContribution", 4);
        S("squad.formationBehavior", "Maintains a safe search grid and reserves clear approach lanes");
        S("squad.intentInterpretationRules", "Prioritize confirmed distress, verify identity, avoid interfering with active combat formations");
        S("squad.retreatBehavior", "Return to safe holding point when damage, weather, or battery thresholds are exceeded");
        S("squad.breachBehavior", "None");
        S("squad.flankingBehavior", "None");
        S("squad.reinforcementBehavior", "Marks casualties, relays locations, delivers emergency supplies, guides recovery units");
        S("squad.parentCommandUnit", "NexusGrid / Overseer");
        S("squad.networkDependencies", "Emergency Tactical Grid");
        A("squad.subordinateUnits");
        A("squad.relayNodes", "Rescue Beacon", "Medical Relay", "Recovery Relay");

        // VR OPERATOR
        S("vrOperator.operatorSeatType", "Remote Drone View");
        S("vrOperator.operatorHud", "Rescue Coordination HUD");
        S("vrOperator.cameraFeeds", "Optical, thermal, downward payload camera");
        S("vrOperator.movementControls", "Full flight control with assisted precision hover");
        S("vrOperator.sensorControls", "Optical zoom, thermal scan, beacon triangulation, terrain map");
        S("vrOperator.abilityControls", "Deploy Medical Pack, Activate Rescue Beacon, Mark Casualty, Guide Evacuation");
        S("vrOperator.weaponControls", "None");
        S("vrOperator.stabilizationControls", "Auto-hover, wind compensation, rescue winch stabilization");
        B("vrOperator.thermalView", true);
        B("vrOperator.lidarView", true);
        B("vrOperator.sonarView", false);
        B("vrOperator.tacticalOverlay", true);
        A("vrOperator.operatorAbilities",
            "Deploy Medical Pack", "Activate Rescue Beacon", "Mark Casualty",
            "Guide Evacuation", "Emergency Supply Drop", "Rescue Winch");

        // ECONOMY & LOGISTICS
        I("economy.creditCost", 420);
        I("economy.deploymentPoints", 3);
        B("economy.deploymentPointsDefined", true);
        S("economy.buildCost", "210 Metal / 260 Electronics / 90 Polymer");
        F("economy.buildTimeSeconds", 75f);
        S("economy.factoryType", "Rescue Drone Bay");
        S("economy.techRequirements", "Emergency Logistics I");
        S("economy.maintenanceCost", "Moderate");
        F("economy.repairTimeSeconds", 24f);
        S("economy.supplyConsumption", "Low");
        S("economy.operationalCost", "Low-medium");
        S("economy.deploymentRequirements", "Clear drone pad or stable launch surface");
        S("economy.transportCompatibility", "Standard aerial racks and emergency carrier bays");
        S("economy.launchMethod", "Vertical takeoff");

        // PLAYER COLLECTION BUILD REQUIREMENTS
        Resources("buildRequirements.commonComponents", new (string, int)[]
        {
            ("Light Alloy Plates", 12),
            ("Composite Frame Rods", 8),
            ("Micro-Servo Actuators", 6),
            ("Stabilizer Assemblies", 4),
            ("Rescue Winch Assembly", 1),
            ("Medical Payload Cradle", 1)
        });
        Resources("buildRequirements.electronics", new (string, int)[]
        {
            ("Circuit Bundles", 16),
            ("Thermal Sensor Modules", 4),
            ("Navigation Processors", 3),
            ("Emergency Beacon Chips", 3),
            ("Autonomous Flight Controller", 2)
        });
        Resources("buildRequirements.power", new (string, int)[]
        {
            ("High-Density Battery Pack", 2),
            ("Power Regulation Nodes", 3),
            ("Emergency Reserve Cell", 1)
        });
        Resources("buildRequirements.rareTradeable", new (string, int)[]
        {
            ("Rescue Guidance Core", 1),
            ("Medical Logistics Processor", 1)
        });
        A("buildRequirements.optionalUpgrades",
            "Extended Flight Battery", "All-Weather Sensor Housing",
            "Advanced Thermal Imaging", "Rapid Medical Supply Pod",
            "Encrypted Emergency Relay");

        // ENVIRONMENT
        S("environment.terrainAdaptationCoefficients", "Urban 1.00; forest 0.82; mountain 0.76; open terrain 1.00; confined interiors 0.45");
        S("environment.visibilityModifiers", "Fog -35%; heavy rain -25%; smoke -45%; night vision enabled by thermal sensor");
        S("environment.signalPropagationCurve", "Line-of-sight optimal; dense structures and terrain reduce link range");
        F("environment.weatherSensitivityIndex", 0.42f);

        // NETWORK
        F("network.nodeHandoffEfficiency", 0.94f);
        F("network.encryptionLatencySeconds", 0.035f);
        S("network.packetLossRate", "Under 1.5% nominal; increases under interference");
        S("network.meshSelfHealingThreshold", "Reconnect or reroute after 2 seconds of link loss");
        S("network.relaySaturationCurve", "Up to 6 simultaneous priority relay links");

        // COGNITIVE
        F("cognitive.decisionConfidence", 0.93f);
        F("cognitive.threatRecognitionAccuracy", 0.88f);
        F("cognitive.adaptiveLearningRatePerMission", 0.06f);
        F("cognitive.commandOverrideResistance", 0.03f);
        F("cognitive.cooperativeBehaviorIndex", 0.97f);

        // TACTICAL
        F("tactical.formationAdaptability", 0.91f);
        F("tactical.tacticalRoleWeight", 0.98f);
        F("tactical.commandResponseDelaySeconds", 0.12f);
        S("tactical.reinforcementTriggerConditions", "Confirmed casualty, distress beacon, isolated friendly squad, blocked evacuation route");
        S("tactical.retreatLogicThresholds", "Battery below 18%; structural integrity below 35%; unsafe wind or loss of reliable navigation");

        // MAINTENANCE
        F("maintenance.meanTimeBetweenFailuresHours", 180f);
        F("maintenance.repairCycleSeconds", 24f);
        F("maintenance.resourceDrainPerHourPercent", 3.2f);
        F("maintenance.batteryEfficiencyPercent", 91f);
        F("maintenance.rotorStabilityPercent", 94f);
        F("maintenance.sensorCalibrationDriftPercentPerHour", 0.35f);
        F("maintenance.coolingEfficiencyPercent", 89f);
        F("maintenance.operationalUptimePercent", 96f);
        F("maintenance.energyRecoveryRatePercent", 0f);
        F("maintenance.energyRecoveryRatePercentPerIdleMinute", 0f);
        F("maintenance.componentWearPercentPerHour", 0.42f);
        F("maintenance.sparePartDependencyIndex", 0.31f);
        S("maintenance.maintenancePriorityLevel", "High");
        S("maintenance.operationalCostPerHour", "Low-medium");

        // PERFORMANCE
        F("performance.missionSuccessProbability", 0.91f);
        F("performance.averageEngagementDurationMinutes", 0f);
        F("performance.reconDataYieldMbPerMinute", 18f);
        F("performance.communicationUptime", 0.965f);
        F("performance.threatAvoidanceEfficiency", 0.89f);

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(unit);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Response canonical data saved: {AssetDatabase.GetAssetPath(unit)}. Existing model prefab and store image were preserved.");
    }
}


