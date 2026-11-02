using UnityEditor;
using ObsidianProtocol.Game.Units;
using UnityEngine;

public static class PopulateDropCanonicalData
{
    private const string TargetId = "obs-air-drop";

    [MenuItem("Obsidian Protocol/Units/Populate Drop Canonical Data")]
    public static void Populate()
    {
        UnitDefinition asset = null;
        string[] ids = AssetDatabase.FindAssets("t:UnitDefinition");

        foreach (string guid in ids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var candidate = AssetDatabase.LoadAssetAtPath<UnitDefinition>(assetPath);
            if (candidate == null) continue;

            var candidateSO = new SerializedObject(candidate);
            var id = candidateSO.FindProperty("unitId");
            if (id != null && id.stringValue == TargetId)
            {
                if (asset != null)
                {
                    Debug.LogError("Multiple UnitDefinition assets use " + TargetId +
                                   ". No changes made.");
                    return;
                }
                asset = candidate;
            }
        }

        if (asset == null)
        {
            Debug.LogError("No existing UnitDefinition found with unitId " +
                           TargetId + ". No changes made.");
            return;
        }

        var so = new SerializedObject(asset);
        so.Update();

        // Identity
        S(so, "unitId", "obs-air-drop");
        S(so, "displayName", "Drop");
        S(so, "role", "Precision Payload Delivery");
        S(so, "tier", "Operational");
        S(so, "status", "Active");

        // Physical and technical
        F(so, "physical.lengthMeters", 4.5f);
        F(so, "physical.widthMeters", 3.2f);
        F(so, "physical.heightMeters", 1.5f);
        F(so, "physical.massKg", 30f);
        S(so, "physical.powerSource", "High-capacity battery");
        F(so, "physical.energyCapacityWh", 2600f);
        F(so, "physical.energyConsumptionWhPerMinute", 55f);
        S(so, "physical.endurance", "25–40 min");
        F(so, "physical.payloadCapacityKg", 2f);
        SS(so, "physical.modules", "Servo drop system");
        S(so, "physical.navigationSystem", "GPS + IMU");
        F(so, "physical.structuralIntegrityHp", 150f);
        F(so, "physical.armorRating", 0.2f);
        S(so, "physical.heatSignature", "Medium");
        S(so, "physical.noiseSignature", "Medium");
        F(so, "physical.topSpeedMetersPerSecond", 20f);
        S(so, "physical.topSpeedDescription", "20 m/s");
        S(so, "physical.acceleration", "Medium");
        S(so, "physical.turningRadius", "Moderate");
        S(so, "physical.climbRate", "Medium");
        S(so, "physical.stabilityRating", "High — optimized for drop accuracy");

        // Autonomy and delivery AI
        S(so, "autonomy.behaviorProfile", "Logistics / Delivery");
        F(so, "autonomy.aggression", 0f);
        F(so, "autonomy.caution", 0.7f);
        F(so, "autonomy.persistence", 0.8f);
        F(so, "autonomy.flankingLikelihood", 0f);
        F(so, "autonomy.retreatThreshold", 0.6f);
        F(so, "autonomy.riskTolerance", 0.2f);
        S(so, "autonomy.targetPriority", "Delivery coordinates");
        S(so, "autonomy.formationDiscipline", "Medium");
        S(so, "autonomy.retreatBehaviorDescription",
            "Withdraw under threat or low battery; abort if delivery safety thresholds are exceeded.");

        // Sensors
        F(so, "sensors.visualRangeMeters", 1200f);
        F(so, "sensors.thermalRangeMeters", 600f);
        S(so, "sensors.visualRangeDescription",
            "Delivery-zone identification and navigation; optical sensor");
        S(so, "sensors.thermalRangeDescription", "Thermal delivery-zone observation");
        S(so, "sensors.acousticRange", "Low");
        S(so, "sensors.radarRange", "None");
        S(so, "sensors.lidarRange", "None");
        S(so, "sensors.signalDetectionRange", "Medium");

        // Communications
        F(so, "communication.meshRangeKm", 6f);
        S(so, "communication.meshRangeDescription", "6 km mesh");
        F(so, "communication.relayCapacity", 0f);
        S(so, "communication.commandLinkType", "Standard tactical network");
        S(so, "communication.networkPriority", "Medium");

        // Combat and threats: Drop is noncombat
        F(so, "combat.firepowerRating", 0f);
        F(so, "combat.suppressionValue", 0f);
        S(so, "combat.firepowerDescription", "Noncombat aerial logistics platform");
        S(so, "combat.accuracy", "Payload delivery accuracy: ±0.5 m");
        S(so, "combat.stabilization", "High; optimized for precision payload release");
        S(so, "combat.weaponMounts", "None");
        S(so, "combat.weaponTypes", "None");
        S(so, "combat.trackingStrength", "High — delivery-zone tracking");
        S(so, "combat.disruptionStrength", "Low");
        S(so, "combat.breachCapability", "None");
        S(so, "combat.countermeasureRating", "Limited defensive countermeasures");
        I(so, "combat.threatLevel", 2);
        S(so, "combat.engagementDistance", "N/A — direct combat prohibited");
        S(so, "combat.preferredTerrain", "Open air / delivery corridors");
        S(so, "combat.preferredTargets", "Assigned delivery coordinates and drop zones");
        S(so, "combat.vulnerabilityProfile",
            "Ballistic damage, explosives, EMP, signal jamming, thermal stress, cyber intrusion, and severe weather");

        // Squad and command integration
        S(so, "squad.squadRole", "Payload Delivery");
        I(so, "squad.squadSizeContribution", 1);
        S(so, "squad.formationBehavior", "Delivery corridor spacing");
        S(so, "squad.intentInterpretationRules",
            "Prioritize delivery accuracy and payload preservation over speed.");
        S(so, "squad.retreatBehavior", "Medium; return to recovery point when unsafe");
        S(so, "squad.breachBehavior", "None");
        S(so, "squad.flankingBehavior", "None");
        S(so, "squad.reinforcementBehavior", "Supports logistics squads");
        S(so, "squad.parentCommandUnit", "Command Core / Overseer");
        S(so, "squad.networkDependencies", "Standard Warden mesh");
        SS(so, "squad.subordinateUnits");
        SS(so, "squad.relayNodes");

        // VR operator
        S(so, "vrOperator.operatorSeatType", "Drone View");
        S(so, "vrOperator.operatorHud", "Delivery Overlay");
        S(so, "vrOperator.cameraFeeds", "Optical");
        S(so, "vrOperator.movementControls", "Full drone flight");
        S(so, "vrOperator.sensorControls", "Drop-zone targeting");
        S(so, "vrOperator.abilityControls", "Payload Drop / Stabilize");
        S(so, "vrOperator.weaponControls", "None");
        S(so, "vrOperator.stabilizationControls", "Auto-hover / auto-level");
        B(so, "vrOperator.thermalView", true);
        B(so, "vrOperator.lidarView", false);
        B(so, "vrOperator.sonarView", false);
        B(so, "vrOperator.tacticalOverlay", true);
        SS(so, "vrOperator.operatorAbilities",
            "Precision Drop", "Drop-Zone Scan", "Stabilize",
            "Delivery Confirmation", "Abort Delivery", "Return to Base");

        // Economy and logistics
        I(so, "economy.creditCost", 0);
        S(so, "economy.buildCost", "160 Metal / 140 Electronics");
        F(so, "economy.buildTimeSeconds", 50f);
        S(so, "economy.factoryType", "Drone Bay");
        S(so, "economy.techRequirements", "Logistics I");
        S(so, "economy.maintenanceCost", "Medium");
        F(so, "economy.repairTimeSeconds", 14f);
        S(so, "economy.supplyConsumption", "Medium");
        S(so, "economy.operationalCost", "Moderate; 6 Metal / 5 Electronics per hour");
        S(so, "economy.deploymentRequirements", "Standard drone pad");
        S(so, "economy.transportCompatibility", "All aerial racks");
        S(so, "economy.launchMethod", "Vertical takeoff");
        S(so, "economy.collectionType", "Constructible Unit");
        S(so, "economy.acquisitionMethod",
            "Research / Construction / Authorized Acquisition");
        S(so, "economy.playerAcquisition",
            "Research / Construction / Authorized Acquisition");
        S(so, "economy.collectionCategory", "Aerial Logistics / Precision Delivery");
        S(so, "economy.buildable", "Yes");
        S(so, "economy.requiresResearch", "Logistics I");
        S(so, "economy.storage", "Aerial Rack / Drone Bay");
        S(so, "economy.repairLocation", "Drone Bay");
        S(so, "economy.upgradeLocation", "Drone Bay / Military Development");
        S(so, "economy.reusability", "Persistent Unit");
        S(so, "economy.lossState", "Unit lost if destroyed");
        S(so, "economy.veteranProgression", "Yes");
        S(so, "economy.persistentRecord", "Yes");

        // Environmental coefficients and operating conditions
        S(so, "environment.terrainAdaptationCoefficients",
            "Open Air=1; Urban=0.9; Forest=0.7; Mountain=0.75; Desert=0.9; Snow=0.8; Water=0");
        F(so, "environment.weatherSensitivityIndex", 0.74f);
        S(so, "environment.visibilityModifiers",
            "Day=1; Night=0.8; Fog=0.7; Rain=0.75; Heavy Storm=0.55");
        S(so, "environment.signalPropagationCurve",
            "Range decreases 0.25% per 10% humidity increase.");
        Environmental(so, "High Winds", "-15% flight stability; -12% delivery precision");
        Environmental(so, "Heavy Rain", "-15% signal range; -10% visual clarity");
        Environmental(so, "Fog", "-20% visual range; +5% thermal range");
        Environmental(so, "Night Operations", "-20% visual range; +10% thermal targeting precision");
        Environmental(so, "Electromagnetic Interference", "-35% signal integrity; -25% command reliability");
        Environmental(so, "Extreme Weather", "-20% delivery accuracy");

        // Network dynamics
        F(so, "network.nodeHandoffEfficiency", 0.94f);
        S(so, "network.packetLossRate", "1.5% under load");
        F(so, "network.encryptionLatencySeconds", 0.11f);
        S(so, "network.meshSelfHealingThreshold", "2 lost nodes");
        S(so, "network.relaySaturationCurve",
            "Throughput drops 6% per additional node beyond 5.");

        // Cognitive layer
        F(so, "cognitive.decisionConfidence", 0.89f);
        F(so, "cognitive.threatRecognitionAccuracy", 0.93f);
        F(so, "cognitive.adaptiveLearningRatePerMission", 0.06f);
        F(so, "cognitive.commandOverrideResistance", 0.1f);
        F(so, "cognitive.cooperativeBehaviorIndex", 0.85f);

        // AI behavior matrix
        Behavior(so, "Navigate Delivery Route", 0.92f, "Reach assigned delivery coordinates");
        Behavior(so, "Drop-Zone Scan", 0.90f, "Verify delivery area before release");
        Behavior(so, "Precision Drop", 0.95f, "Deliver payload within assigned accuracy");
        Behavior(so, "Stabilize", 0.94f, "Maintain stable flight during payload release");
        Behavior(so, "Delivery Confirmation", 0.93f, "Confirm successful payload delivery");
        Behavior(so, "Retreat", 0.70f, "Withdraw under threat or low battery");
        Behavior(so, "Re-Engage", 0.60f, "Resume delivery operations after recovery");
        Behavior(so, "Assist", 0.65f, "Support allied logistics operations");
        Behavior(so, "Override Response", 0.85f, "Accept authorized network commands");

        Mission(so, "Precision Delivery", "Route completion, drop-zone validation, and payload accuracy", 0.95f);
        Mission(so, "Resupply", "Deliver payload to assigned logistics destination", 0.91f);
        Mission(so, "Recovery", "Abort unsafe delivery and return to recovery point", 0.85f);

        // Inter-unit coordination
        Synergy(so, "Beacon", 0.93f, "Delivery-route communications");
        Synergy(so, "Warden", 0.88f, "Delivery-zone reconnaissance");
        Synergy(so, "Sentinel", 0.84f, "Tactical delivery authorization");
        Synergy(so, "Logistics Units", 0.91f, "Payload destination coordination");
        Synergy(so, "Overseer", 0.95f, "Mission-level delivery control");

        // Damage response
        Damage(so, "Ballistic", "Structural damage / flight degradation",
            "Abort delivery if integrity threshold is reached");
        Damage(so, "Explosive", "Flight-system and payload-bay damage",
            "Stabilize and return to recovery point");
        Damage(so, "EMP", "Navigation / communications disruption",
            "Switch to local IMU navigation and recovery logic");
        Damage(so, "Signal Jam", "Command-link degradation",
            "Continue current route if safe; seek alternate network");
        Damage(so, "Thermal", "Battery and electronics stress",
            "Reduce power load and retreat");
        Damage(so, "Weather", "Flight stability / precision degradation",
            "Reduce speed and stabilize before delivery");
        Damage(so, "Cyber Intrusion", "Command integrity risk",
            "Isolate compromised link and preserve delivery state");

        // Threat-response model
        Threat(so, "Ballistic", 0.35f, 0.42f);
        Threat(so, "Explosive", 0.70f, 0.38f);
        Threat(so, "EMP", 0.90f, 0.30f);
        Threat(so, "Thermal", 0.40f, 0.50f);
        Threat(so, "Signal Jam", 0.80f, 0.58f);
        Threat(so, "Severe Weather", 0.50f, 0.65f);

        // Tactical integration
        F(so, "tactical.formationAdaptability", 0.8f);
        F(so, "tactical.tacticalRoleWeight", 0.76f);
        F(so, "tactical.commandResponseDelaySeconds", 0.5f);
        S(so, "tactical.reinforcementTriggerConditions",
            "Battery below 25% OR signal loss above 35%");
        S(so, "tactical.retreatLogicThresholds",
            "Structural integrity below 30% OR payload delivered");
        S(so, "tactical.strategicPriority", "Medium");
        S(so, "tactical.preferredAssignments",
            "Precision delivery, logistics support, resupply");
        S(so, "tactical.directCombatAssignment", "Prohibited");
        S(so, "tactical.directUnitMicromanagement", "Optional");
        S(so, "tactical.autonomousAssignment", "Yes");
        S(so, "tactical.emergencyTrigger", "Critical flight-system damage");

        // Maintenance and energy analytics
        F(so, "maintenance.meanTimeBetweenFailuresHours", 110f);
        F(so, "maintenance.repairCycleSeconds", 14f);
        F(so, "maintenance.resourceDrainPerHourPercent", 0.60f);
        F(so, "maintenance.batteryEfficiencyPercent", 92f);
        F(so, "maintenance.rotorStabilityPercent", 94f);
        F(so, "maintenance.sensorCalibrationDriftPercentPerHour", 0.25f);
        F(so, "maintenance.coolingEfficiencyPercent", 88f);
        F(so, "maintenance.operationalUptimePercent", 96f);
        F(so, "maintenance.energyRecoveryRatePercent", 5f);
        F(so, "maintenance.energyRecoveryRatePercentPerIdleMinute", 5f);
        F(so, "maintenance.componentWearPercentPerHour", 0.4f);
        F(so, "maintenance.sparePartDependencyIndex", 0.58f);
        S(so, "maintenance.maintenancePriorityLevel", "Tier II");
        S(so, "maintenance.operationalCostPerHour", "6 Metal / 5 Electronics");

        // Performance metrics
        F(so, "performance.missionSuccessProbability", 0.90f);
        F(so, "performance.averageEngagementDurationMinutes", 3.8f);
        F(so, "performance.reconDataYieldMbPerMinute", 0f);
        F(so, "performance.communicationUptime", 0.97f);
        F(so, "performance.threatAvoidanceEfficiency", 0.81f);

        // Upgrades and their specified effects
        Upgrade(so, "Extended Battery Module", "+20% endurance; +10% operational uptime", 0.20f);
        Upgrade(so, "Silent Rotor Kit", "-25% noise signature; +5% threat avoidance", 0.05f);
        Upgrade(so, "Precision Drop Enhancer", "+20% payload accuracy", 0.20f);
        Upgrade(so, "Anti-Wind Stabilization Grid", "+15% delivery stability under high winds", 0.15f);
        Upgrade(so, "Reinforced Payload Bay", "+20% payload-bay structural integrity", 0.20f);
        Upgrade(so, "Advanced Navigation Module", "+10% route completion reliability", 0.10f);

        // Build requirements
        Requirement(so, "buildRequirements.commonComponents", "Medium Alloy Plates", 14);
        Requirement(so, "buildRequirements.commonComponents", "Composite Frame Rods", 10);
        Requirement(so, "buildRequirements.commonComponents", "Micro-Servo Actuators", 8);
        Requirement(so, "buildRequirements.commonComponents", "Stabilizer Fins", 4);
        Requirement(so, "buildRequirements.commonComponents", "Payload Drop Bay Housing", 1);

        Requirement(so, "buildRequirements.electronics", "Circuit Bundles", 12);
        Requirement(so, "buildRequirements.electronics", "Signal Processors", 8);
        Requirement(so, "buildRequirements.electronics", "Drone Control Chips", 4);
        Requirement(so, "buildRequirements.electronics", "IMU Stabilization Chips", 2);

        Requirement(so, "buildRequirements.power", "High-Capacity Battery Pack", 1);
        Requirement(so, "buildRequirements.power", "Power Regulation Nodes", 3);

        Requirement(so, "buildRequirements.rareTradeable", "Precision Servo Assembly", 1);
        Requirement(so, "buildRequirements.rareTradeable", "Encrypted Delivery Core", 1);

        SS(so, "buildRequirements.optionalUpgrades",
            "Extended Battery Module",
            "Silent Rotor Kit",
            "Precision Drop Enhancer",
            "Anti-Wind Stabilization Grid");

        // Command network analytics
        CommandNode(so, "Standard Warden Mesh", 32f, 0.5f, "Stable");
        CommandNode(so, "Logistics Command", 44f, 0.6f, "Active");
        CommandNode(so, "Overseer / Command Core", 48f, 0.5f, "Active");
        CommandNode(so, "Drone Bay Control", 41f, 0.7f, "Stable");
        CommandNode(so, "Delivery Network", 37f, 0.6f, "Operational");

        // Store extended specifications in existing descriptive fields.
        // These fields are not represented by dedicated typed properties in UnitDefinition.
        AppendToExistingText(so, "physical.endurance",
            " | Energy curve: idle 12 Wh/min; maximum delivery load 68 Wh/min; emergency power 10 Wh/min; idle recovery 5% per idle minute; battery efficiency 92%; standard high-capacity battery recharge.");
        AppendToExistingText(so, "environment.visibilityModifiers",
            " | Terrain data reliability 88%; severe weather reduces flight stability and delivery precision; environmental priority: maintain delivery accuracy over speed.");
        AppendToExistingText(so, "autonomy.retreatBehaviorDescription",
            " | Autonomous operation enabled; delivery-zone recognition 94%; drop prediction accuracy 91%; autonomous recovery enabled; AI sync 98%; command latency 0.5 s; tactical refresh 2 s; strategic refresh 5 s; decision depth Tier II; override priority Network > AI > Manual; command authority Tactical / Logistics; processing Medium; network capacity Standard; signal integrity 95%; relay load 32%.");
        AppendToExistingText(so, "combat.vulnerabilityProfile",
            " | Drop-zone recognition 94%; payload release reliability 96%; route completion reliability 93%; payload accuracy ±0.5 m; mission success 90%; average delivery duration 3.8 min.");
        AppendToExistingText(so, "squad.networkDependencies",
            " | Command hierarchy: Command Core > Overseer > Drop. Autonomous assignment enabled. Persistent unit record; veteran progression enabled; lost if destroyed.");
        AppendToExistingText(so, "maintenance.operationalCostPerHour",
            " | Idle energy recovery 5% per idle minute; recharge uses standard high-capacity battery recharge.");

        // Preserve category, storeImage, modelPrefab and all unrelated Unity references.
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();

        Debug.Log("Drop population saved: " + AssetDatabase.GetAssetPath(asset) +
                  ". Inspect warnings for schema fields not supported by this UnitDefinition.");
    }

    private static SerializedProperty P(SerializedObject so, string path)
    {
        var p = so.FindProperty(path);
        if (p == null) Debug.LogWarning("Drop field unavailable; skipped: " + path);
        return p;
    }

    private static void S(SerializedObject so, string path, string value)
    {
        var p = P(so, path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.String) p.stringValue = value;
        else Debug.LogWarning("Drop field is not a string: " + path);
    }

    private static void F(SerializedObject so, string path, float value)
    {
        var p = P(so, path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
        else Debug.LogWarning("Drop field is not a float: " + path);
    }

    private static void I(SerializedObject so, string path, int value)
    {
        var p = P(so, path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Integer) p.intValue = value;
        else Debug.LogWarning("Drop field is not an integer: " + path);
    }

    private static void B(SerializedObject so, string path, bool value)
    {
        var p = P(so, path);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Boolean) p.boolValue = value;
        else Debug.LogWarning("Drop field is not a Boolean: " + path);
    }

    private static void SS(SerializedObject so, string path, params string[] values)
    {
        var p = P(so, path);
        if (p == null) return;
        if (!p.isArray || p.propertyType == SerializedPropertyType.String)
        {
            Debug.LogWarning("Drop field is not a string list: " + path);
            return;
        }

        p.arraySize = values.Length;
        for (int n = 0; n < values.Length; n++)
        {
            var e = p.GetArrayElementAtIndex(n);
            if (e.propertyType == SerializedPropertyType.String) e.stringValue = values[n];
            else Debug.LogWarning("Drop list element is not a string: " + path);
        }
    }

    private static void Environmental(SerializedObject so, string condition, string effect)
    {
        var p = P(so, "environment.modifiers");
        if (p == null || !p.isArray) return;
        int i = p.arraySize++;
        var e = p.GetArrayElementAtIndex(i);
        SetRelative(e, "condition", condition);
        SetRelative(e, "effect", effect);
    }

    private static void Behavior(SerializedObject so, string name, float weight, string description)
    {
        var p = P(so, "behaviorNodes");
        if (p == null || !p.isArray) return;
        int i = p.arraySize++;
        var e = p.GetArrayElementAtIndex(i);
        SetRelative(e, "behaviorNode", name);
        SetRelative(e, "weight", weight);
        SetRelative(e, "description", description);
    }

    private static void Mission(SerializedObject so, string type, string focus, float efficiency)
    {
        var p = P(so, "missionBehaviors");
        if (p == null || !p.isArray) return;
        int i = p.arraySize++;
        var e = p.GetArrayElementAtIndex(i);
        SetRelative(e, "missionType", type);
        SetRelative(e, "behaviorFocus", focus);
        SetRelative(e, "efficiency", efficiency);
    }

    private static void Synergy(SerializedObject so, string partner, float rating, string function)
    {
        var p = P(so, "unitSynergies");
        if (p == null || !p.isArray) return;
        int i = p.arraySize++;
        var e = p.GetArrayElementAtIndex(i);
        SetRelative(e, "partnerUnit", partner);
        SetRelative(e, "synergyRating", rating);
        SetRelative(e, "function", function);
    }

    private static void Damage(SerializedObject so, string type, string effect, string recovery)
    {
        var p = P(so, "damageResponses");
        if (p == null || !p.isArray) return;
        int i = p.arraySize++;
        var e = p.GetArrayElementAtIndex(i);
        SetRelative(e, "damageType", type);
        SetRelative(e, "effect", effect);
        SetRelative(e, "recovery", recovery);
    }

    private static void Threat(SerializedObject so, string type, float reaction, float efficiency)
    {
        var p = P(so, "threatResponses");
        if (p == null || !p.isArray) return;
        int i = p.arraySize++;
        var e = p.GetArrayElementAtIndex(i);
        SetRelative(e, "threatType", type);
        SetRelative(e, "reactionTimeSeconds", reaction);
        SetRelative(e, "countermeasureEfficiency", efficiency);
    }

    private static void Upgrade(SerializedObject so, string name, string effect, float gain)
    {
        var p = P(so, "upgrades");
        if (p == null || !p.isArray) return;
        int i = p.arraySize++;
        var e = p.GetArrayElementAtIndex(i);
        SetRelative(e, "upgrade", name);
        SetRelative(e, "effect", effect);
        SetRelative(e, "efficiencyGain", gain);
    }

    private static void Requirement(SerializedObject so, string path, string component, int quantity)
    {
        var p = P(so, path);
        if (p == null || !p.isArray) return;
        int i = p.arraySize++;
        var e = p.GetArrayElementAtIndex(i);
        SetRelative(e, "component", component);
        SetRelative(e, "quantity", quantity);
    }

    private static void CommandNode(SerializedObject so, string node, float load, float latency, string status)
    {
        var p = P(so, "commandNetwork");
        if (p == null || !p.isArray) return;
        int i = p.arraySize++;
        var e = p.GetArrayElementAtIndex(i);
        SetRelative(e, "node", node);
        SetRelative(e, "loadPercent", load);
        SetRelative(e, "latencySeconds", latency);
        SetRelative(e, "status", status);
    }

    private static void SetRelative(SerializedProperty parent, string name, string value)
    {
        var p = parent.FindPropertyRelative(name);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.String) p.stringValue = value;
    }

    private static void SetRelative(SerializedProperty parent, string name, float value)
    {
        var p = parent.FindPropertyRelative(name);
        if (p == null) return;
        if (p.propertyType == SerializedPropertyType.Float) p.floatValue = value;
    }

    private static void AppendToExistingText(SerializedObject so, string path, string text)
    {
        var p = P(so, path);
        if (p == null || p.propertyType != SerializedPropertyType.String) return;
        if (!string.IsNullOrEmpty(p.stringValue) && !p.stringValue.Contains(text))
            p.stringValue += text;
    }
}

