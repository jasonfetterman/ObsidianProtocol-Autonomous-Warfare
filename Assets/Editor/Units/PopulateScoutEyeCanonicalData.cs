using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public static class PopulateScoutEyeCanonicalData
{
    [MenuItem("Obsidian Protocol/Units/Populate Scout Eye Canonical Data")]
    public static void Populate()
    {
        string[] guids = AssetDatabase.FindAssets("t:UnitDefinition");
        UnitDefinition target = null;

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var candidate = AssetDatabase.LoadAssetAtPath<UnitDefinition>(assetPath);
            if (candidate == null) continue;

            var check = new SerializedObject(candidate);
            string id = check.FindProperty("unitId")?.stringValue ?? "";
            string name = check.FindProperty("displayName")?.stringValue ?? "";

            if (name == "Scout Eye" ||
                id == "obs-air-scout-eye" ||
                id == "obs-air-scouteye")
            {
                target = candidate;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError("Scout Eye population failed: existing UnitDefinition not found.");
            return;
        }

        var so = new SerializedObject(target);
        so.Update();

        void S(string p, string v)
        {
            var x = so.FindProperty(p);
            if (x != null && x.propertyType == SerializedPropertyType.String)
                x.stringValue = v;
        }
        void F(string p, float v)
        {
            var x = so.FindProperty(p);
            if (x != null && x.propertyType == SerializedPropertyType.Float)
                x.floatValue = v;
        }
        void I(string p, int v)
        {
            var x = so.FindProperty(p);
            if (x != null && x.propertyType == SerializedPropertyType.Integer)
                x.intValue = v;
        }
        void B(string p, bool v)
        {
            var x = so.FindProperty(p);
            if (x != null && x.propertyType == SerializedPropertyType.Boolean)
                x.boolValue = v;
        }
        void Strings(string p, params string[] values)
        {
            var x = so.FindProperty(p);
            if (x == null || !x.isArray) return;
            x.ClearArray();
            foreach (string value in values)
            {
                int n = x.arraySize;
                x.arraySize = n + 1;
                var e = x.GetArrayElementAtIndex(n);
                if (e.propertyType == SerializedPropertyType.String)
                    e.stringValue = value;
            }
        }

        // Identity — preserve modelPrefab and storeImage.
        S("unitId", "obs-air-scouteye");
        S("displayName", "Scout Eye");
        S("role", "Lightweight Recon / Rapid Visual Scan");
        S("tier", "Basic");
        S("status", "Active");

        // Physical and technical.
        F("physical.lengthMeters", 1.2f);
        F("physical.widthMeters", 0.9f);
        F("physical.heightMeters", 0.5f);
        F("physical.massKg", 10f);
        S("physical.powerSource", "Compact battery");
        F("physical.energyCapacityWh", 1200f);
        F("physical.energyConsumptionWhPerMinute", 25f);
        S("physical.endurance", "25–40 min");
        F("physical.payloadCapacityKg", 0.8f);
        I("physical.structuralIntegrityHp", 80);
        F("physical.armorRating", 0.10f);
        S("physical.navigationSystem", "GPS + Basic Stabilization");
        S("physical.heatSignature", "Very Low");
        S("physical.noiseSignature", "Very Low");
        F("physical.topSpeedMetersPerSecond", 20f);
        S("physical.topSpeedDescription", "20 m/s");
        S("physical.acceleration", "High");
        S("physical.turningRadius", "Very Tight");
        S("physical.climbRate", "Medium");
        S("physical.stabilityRating", "Medium");
        Strings("physical.modules", "Micro-camera array");

        // Autonomy.
        S("autonomy.behaviorProfile", "Light Recon");
        F("autonomy.aggression", 0f);
        F("autonomy.caution", 0.9f);
        F("autonomy.persistence", 0.5f);
        F("autonomy.flankingLikelihood", 0f);
        F("autonomy.retreatThreshold", 0.8f);
        S("autonomy.targetPriority", "Recon targets");
        S("autonomy.formationDiscipline", "Medium");
        S("autonomy.retreatBehaviorDescription", "Early withdrawal; prioritize survival over information gain");

        // Sensors and communications.
        F("sensors.visualRangeMeters", 1200f);
        F("sensors.thermalRangeMeters", 400f);
        S("sensors.visualRangeDescription", "1200 m");
        S("sensors.thermalRangeDescription", "400 m, low-resolution thermal");
        S("sensors.acousticRange", "Low");
        S("sensors.radarRange", "None");
        S("sensors.lidarRange", "None");
        S("sensors.signalDetectionRange", "Low");
        F("communication.meshRangeKm", 4f);
        S("communication.meshRangeDescription", "4 km");
        F("communication.relayCapacity", 0f);
        S("communication.commandLinkType", "Standard Warden mesh");
        S("communication.networkPriority", "Low");

        // Environment.
        S("environment.terrainAdaptationCoefficients",
          "Open Air 1.0 / Urban 0.85 / Forest 0.70 / Mountain 0.75 / Desert 0.90 / Snow 0.65 / Water 0.0");
        F("environment.weatherSensitivityIndex", 0.72f);
        S("environment.visibilityModifiers",
          "Day 1.0 / Night 0.65 / Fog 0.50 / Rain 0.70 / Heavy Storm 0.45");
        S("environment.signalPropagationCurve",
          "Effective range decreases 0.25% per 10% atmospheric interference");
        Strings("environment.modifiers",
          "High Winds: -18% stability / -10% scan accuracy",
          "Heavy Rain: -25% visual range / -10% optical clarity",
          "Fog: -35% visual range / -5% thermal clarity",
          "Night Operations: -35% visual range / +8% thermal effectiveness",
          "Extreme Weather: -40% recon efficiency",
          "Electromagnetic Interference: -40% signal integrity");

        // Network and cognitive performance.
        F("network.nodeHandoffEfficiency", 0.92f);
        S("network.packetLossRate", "2.0% under load");
        F("network.encryptionLatencySeconds", 0.12f);
        S("network.meshSelfHealingThreshold", "1 lost node");
        S("network.relaySaturationCurve", "Throughput decreases 7% per active node beyond 4");

        F("cognitive.decisionConfidence", 0.82f);
        F("cognitive.threatRecognitionAccuracy", 0.84f);
        F("cognitive.adaptiveLearningRatePerMission", 0.03f);
        F("cognitive.commandOverrideResistance", 0.10f);
        F("cognitive.cooperativeBehaviorIndex", 0.88f);

        // Combat profile.
        F("combat.firepowerRating", 0f);
        F("combat.suppressionValue", 0f);
        I("combat.threatLevel", 1);
        S("combat.accuracy", "N/A");
        S("combat.stabilization", "Medium");
        S("combat.weaponMounts", "None");
        S("combat.weaponTypes", "None");
        S("combat.trackingStrength", "Low");
        S("combat.disruptionStrength", "Low");
        S("combat.breachCapability", "None");
        S("combat.countermeasureRating", "Low");
        S("combat.engagementDistance", "N/A");
        S("combat.preferredTerrain", "Low-risk airspace");
        S("combat.preferredTargets", "None");
        S("combat.vulnerabilityProfile", "Ballistic / weather");

        // Tactical integration.
        F("tactical.formationAdaptability", 0.72f);
        F("tactical.tacticalRoleWeight", 0.68f);
        F("tactical.commandResponseDelaySeconds", 0.5f);
        S("tactical.reinforcementTriggerConditions", "Battery <25% OR Signal Integrity <55%");
        S("tactical.retreatLogicThresholds",
          "Structural Integrity <35% OR Threat Level >3 OR Battery <20%");

        // Maintenance and energy.
        F("maintenance.energyRecoveryRatePercentPerIdleMinute", 8f);
        F("maintenance.componentWearPercentPerHour", 0.25f);
        S("maintenance.maintenancePriorityLevel", "Tier III");
        F("maintenance.sparePartDependencyIndex", 0.38f);
        S("maintenance.operationalCostPerHour", "2 Metal / 2 Electronics");
        F("maintenance.batteryEfficiencyPercent", 91f);
        F("maintenance.sensorCalibrationDriftPercentPerHour", 8f);
        F("maintenance.coolingEfficiencyPercent", 87f);
        F("maintenance.operationalUptimePercent", 94f);

        // Mission performance.
        F("performance.missionSuccessProbability", 0.86f);
        F("performance.averageEngagementDurationMinutes", 6f);
        F("performance.reconDataYieldMbPerMinute", 10f);
        F("performance.communicationUptime", 0.95f);
        F("performance.threatAvoidanceEfficiency", 0.91f);

        // VR operator.
        S("vrOperator.operatorSeatType", "Drone View");
        S("vrOperator.operatorHud", "Minimal Recon Overlay");
        S("vrOperator.cameraFeeds", "Optical");
        S("vrOperator.movementControls", "Full drone flight");
        S("vrOperator.sensorControls", "Zoom / thermal");
        S("vrOperator.abilityControls", "Scan, Mark Target");
        S("vrOperator.weaponControls", "None");
        S("vrOperator.stabilizationControls", "Auto-hover");
        B("vrOperator.thermalView", true);
        B("vrOperator.lidarView", false);
        B("vrOperator.sonarView", false);
        B("vrOperator.tacticalOverlay", true);
        Strings("vrOperator.operatorAbilities",
          "Quick Scan", "Mark Target", "Relay Recon Data");

        // Economy and build.
        // Preserve the unit asset's existing credit price.
        I("economy.deploymentPoints", 0);
        S("economy.buildCost", "60 Metal / 40 Electronics");
        S("economy.factoryType", "Drone Bay");
        S("economy.techRequirements", "Basic Recon");
        S("economy.maintenanceCost", "Very Low");
        I("economy.repairTimeSeconds", 8);
        S("economy.supplyConsumption", "Very Low");
        S("economy.operationalCost", "Minimal");
        S("economy.deploymentRequirements", "Standard drone pad");
        S("economy.transportCompatibility", "All aerial racks");
        S("economy.launchMethod", "Vertical takeoff");
        F("economy.buildTimeSeconds", 25f);

        // Canonical upgrade list.
        Strings("upgrades",
          "Silent Rotor Kit: -25% noise signature; +5% stealth efficiency",
          "Thermal Boost Module: +20% thermal range; +8% thermal detection",
          "Extended Battery Pack: +25% endurance; +8% operational uptime",
          "Micro-Optical Enhancement: +15% visual range; +8% scan accuracy",
          "Basic Recon Processor: +10% target recognition accuracy; +5% processing efficiency");

        // Canonical command and squad information.
        S("squad.squadRole", "Recon");
        I("squad.squadSizeContribution", 1);
        S("squad.formationBehavior", "Loose recon spacing");
        S("squad.intentInterpretationRules", "Avoid threats, prioritize visibility");
        S("squad.retreatBehavior", "Early");
        S("squad.breachBehavior", "None");
        S("squad.flankingBehavior", "None");
        S("squad.reinforcementBehavior", "Supports recon squads");
        S("squad.parentCommandUnit", "Sentinel / Overseer");
        S("squad.networkDependencies", "Standard Warden mesh");
        Strings("squad.subordinateUnits");
        Strings("squad.relayNodes");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();

        Debug.Log("Scout Eye canonical population saved: " +
                  AssetDatabase.GetAssetPath(target) +
                  ". Existing modelPrefab and storeImage were preserved.");
    }
}
