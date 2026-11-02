using UnityEditor;
using UnityEngine;

public static class PopulateLocatorCanonicalData
{
    private const string UnitId = "obs-air-locator";

    [MenuItem("Obsidian Protocol/Units/Populate Locator Canonical Data")]
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
            var display = test.FindProperty("displayName");

            if ((id != null && id.stringValue == UnitId) ||
                (display != null && display.stringValue == "Locator"))
            {
                asset = candidate;
                break;
            }
        }

        if (asset == null)
        {
            Debug.LogError("Locator population failed: existing UnitDefinition not found for " + UnitId);
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
            foreach (string value in values) {
                int n = a.arraySize++;
                a.GetArrayElementAtIndex(n).stringValue = value;
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
        void Resource(string path, string component, int quantity) {
            var a = A(path, false);
            if (a == null) return;
            int n = a.arraySize++;
            var e = a.GetArrayElementAtIndex(n);
            var p = e.FindPropertyRelative("component"); if (p != null) p.stringValue = component;
            p = e.FindPropertyRelative("quantity"); if (p != null) p.intValue = quantity;
        }

        S("unitId", UnitId);
        S("displayName", "Locator");
        S("role", "Target Location / Signal Tracking");
        S("tier", "Advanced Recon");
        S("status", "Active");

        // Physical and technical.
        F("physical.lengthMeters", 2.3f);
        F("physical.widthMeters", 1.7f);
        F("physical.heightMeters", 0.8f);
        F("physical.massKg", 18f);
        S("physical.powerSource", "High-efficiency battery");
        F("physical.energyCapacityWh", 2000f);
        F("physical.energyConsumptionWhPerMinute", 40f);
        S("physical.endurance", "35–50 min");
        F("physical.payloadCapacityKg", 1.5f);
        Strings("physical.modules", "Tracking sensor array");
        S("physical.navigationSystem", "GPS + Tracking AI");
        F("physical.structuralIntegrityHp", 120f);
        F("physical.armorRating", 0.15f);
        S("physical.heatSignature", "Low");
        S("physical.noiseSignature", "Low");
        F("physical.topSpeedMetersPerSecond", 22f);
        S("physical.topSpeedDescription", "22 m/s");
        S("physical.acceleration", "Medium");
        S("physical.turningRadius", "Tight");
        S("physical.climbRate", "Medium");
        S("physical.stabilityRating", "High");

        // Autonomy.
        S("autonomy.behaviorProfile", "Tracking / Recon");
        F("autonomy.aggression", 0.1f);
        F("autonomy.caution", 0.8f);
        F("autonomy.persistence", 0.9f);
        F("autonomy.flankingLikelihood", 0.1f);
        F("autonomy.retreatThreshold", 0.7f);
        F("autonomy.riskTolerance", 0.15f);
        S("autonomy.retreatBehaviorDescription", "Medium; maintain lock while avoiding conflict and retreat from unsafe conditions.");
        S("autonomy.targetPriority", "Moving targets / signal sources");
        S("autonomy.formationDiscipline", "High");

        // Sensors.
        F("sensors.visualRangeMeters", 1600f);
        F("sensors.thermalRangeMeters", 900f);
        S("sensors.visualRangeDescription", "1600 m");
        S("sensors.thermalRangeDescription", "900 m");
        S("sensors.acousticRange", "Medium");
        S("sensors.radarRange", "None");
        S("sensors.lidarRange", "Short-range");
        S("sensors.signalDetectionRange", "Very High");

        // Communication.
        F("communication.meshRangeKm", 6f);
        S("communication.meshRangeDescription", "6 km");
        F("communication.relayCapacity", 0f);
        S("communication.commandLinkType", "Tracking Grid");
        S("communication.networkPriority", "High");
        F("network.nodeHandoffEfficiency", 0.96f);
        S("network.packetLossRate", "Not specified");
        F("network.encryptionLatencySeconds", 0f);
        S("network.meshSelfHealingThreshold", "Tracking Grid dependency");
        S("network.relaySaturationCurve", "Not specified");

        // Noncombatant threat model.
        F("combat.firepowerRating", 0f);
        F("combat.suppressionValue", 0f);
        S("combat.firepowerDescription", "Noncombatant target-location and tracking drone");
        S("combat.accuracy", "N/A");
        S("combat.stabilization", "High");
        S("combat.weaponMounts", "None");
        S("combat.weaponTypes", "None");
        S("combat.trackingStrength", "High");
        S("combat.disruptionStrength", "Medium");
        S("combat.breachCapability", "None");
        S("combat.countermeasureRating", "Low");
        S("combat.engagementDistance", "N/A");
        S("combat.preferredTerrain", "Recon corridors");
        S("combat.preferredTargets", "Moving targets / signal sources");
        S("combat.vulnerabilityProfile", "Ballistic / weather / jamming");
        I("combat.threatLevel", 3);

        // Squad integration.
        S("squad.squadRole", "Target Location");
        I("squad.squadSizeContribution", 1);
        S("squad.formationBehavior", "Tracking-optimized spacing");
        S("squad.intentInterpretationRules", "Maintain lock, avoid conflict");
        S("squad.retreatBehavior", "Medium");
        S("squad.breachBehavior", "None");
        S("squad.flankingBehavior", "Minimal");
        S("squad.reinforcementBehavior", "Supports recon and targeting squads");
        S("squad.parentCommandUnit", "Overseer / Insight");
        S("squad.networkDependencies", "Tracking Grid");
        Strings("squad.subordinateUnits");
        Strings("squad.relayNodes");

        // VR operator.
        S("vrOperator.operatorSeatType", "Drone View");
        S("vrOperator.operatorHud", "Tracking Overlay");
        S("vrOperator.cameraFeeds", "Optical");
        S("vrOperator.movementControls", "Full drone flight");
        S("vrOperator.sensorControls", "Thermal / tracking / zoom");
        S("vrOperator.abilityControls", "Lock Target, Scan, Stabilize");
        S("vrOperator.weaponControls", "None");
        S("vrOperator.stabilizationControls", "Auto-hover");
        B("vrOperator.thermalView", true);
        B("vrOperator.lidarView", true);
        B("vrOperator.sonarView", false);
        B("vrOperator.tacticalOverlay", true);
        Strings("vrOperator.operatorAbilities",
            "Target Lock", "Signal Scan", "Thermal Search",
            "Stabilize", "Relay Tracking Data");

        // Economy and logistics.
        // Preserve the unit asset's existing credit price.
        I("economy.deploymentPoints", 0);
        B("economy.deploymentPointsDefined", false);
        S("economy.buildCost", "170 Metal / 150 Electronics");
        F("economy.buildTimeSeconds", 55f);
        S("economy.factoryType", "Drone Bay");
        S("economy.techRequirements", "Tracking Systems I");
        S("economy.maintenanceCost", "Medium");
        S("economy.supplyConsumption", "Medium");
        S("economy.operationalCost", "Moderate");
        F("economy.repairTimeSeconds", 14f);
        S("economy.deploymentRequirements", "Standard drone pad");
        S("economy.transportCompatibility", "All aerial racks");
        S("economy.launchMethod", "Vertical takeoff");

        // Build requirements.
        A("buildRequirements.commonComponents");
        Resource("buildRequirements.commonComponents", "Light Alloy Plates", 14);
        Resource("buildRequirements.commonComponents", "Composite Frame Rods", 10);
        Resource("buildRequirements.commonComponents", "Micro-Servo Actuators", 6);
        Resource("buildRequirements.commonComponents", "Stabilizer Fins", 3);
        Resource("buildRequirements.commonComponents", "Tracking Sensor Housing", 1);

        A("buildRequirements.electronics");
        Resource("buildRequirements.electronics", "Circuit Bundles", 12);
        Resource("buildRequirements.electronics", "Signal Processors", 8);
        Resource("buildRequirements.electronics", "Drone Control Chips", 4);
        Resource("buildRequirements.electronics", "Tracking AI Chips", 3);

        A("buildRequirements.power");
        Resource("buildRequirements.power", "High-Efficiency Battery Pack", 1);
        Resource("buildRequirements.power", "Power Regulation Nodes", 2);

        A("buildRequirements.rareTradeable");
        Resource("buildRequirements.rareTradeable", "Advanced Tracking Core", 1);
        Resource("buildRequirements.rareTradeable", "Signal Amplification Processor", 1);

        Strings("buildRequirements.optionalUpgrades",
            "Extended Tracking Range Module", "Thermal Boost Lens",
            "Anti-Jamming Shielding", "Silent Rotor Kit");

        // Preserve source-provided fields; values not specified are left untouched where possible.
        F("environment.weatherSensitivityIndex", 0.35f);
        S("environment.terrainAdaptationCoefficients", "Not specified");
        S("environment.visibilityModifiers", "Not specified");
        S("environment.signalPropagationCurve", "Not specified");

        // Behavior profile parameters.
        F("cognitive.decisionConfidence", 0.94f);
        F("cognitive.threatRecognitionAccuracy", 0.94f);
        F("cognitive.adaptiveLearningRatePerMission", 0.04f);
        F("cognitive.commandOverrideResistance", 0.05f);
        F("cognitive.cooperativeBehaviorIndex", 0.90f);

        F("tactical.formationAdaptability", 0.88f);
        F("tactical.tacticalRoleWeight", 0.85f);
        F("tactical.commandResponseDelaySeconds", 0.30f);
        S("tactical.reinforcementTriggerConditions", "Moving target detected or signal source identified");
        S("tactical.retreatLogicThresholds", "Use cautious retreat when tracking integrity is threatened");

        // No unsupported invented maintenance or performance values are forced.
        A("behaviorNodes");
        Behavior("Target Tracking", 0.98f, "Maintain reliable lock on moving targets");
        Behavior("Signal Source Detection", 0.97f, "Locate and track signal sources");
        Behavior("Tracking Persistence", 0.95f, "Continue tracking through movement");
        Behavior("Thermal Search", 0.92f, "Locate targets using thermal sensors");
        Behavior("Recon Scanning", 0.94f, "Scan recon corridors");
        Behavior("Threat Avoidance", 0.88f, "Avoid hostile or dangerous zones");
        Behavior("Lock Handoff", 0.93f, "Share tracking data with allied units");
        Behavior("Operator Override", 0.88f, "Accept authorized targeting commands");

        A("missionBehaviors");
        Mission("Target Location", "Find and locate moving targets", 0.98f);
        Mission("Signal Tracking", "Identify signal sources", 0.97f);
        Mission("Thermal Search", "Locate targets thermally", 0.92f);
        Mission("Recon Scan", "Scan designated recon corridors", 0.94f);
        Mission("Tracking Data Relay", "Share tracking data", 0.93f);

        A("unitSynergies");
        Synergy("Beacon", 0.94f, "Signal-source triangulation");
        Synergy("Sentinel", 0.90f, "Shared aerial surveillance");
        Synergy("Warden", 0.88f, "Provide target-location data to protected units");

        A("damageResponses");
        Damage("Ballistic", "Tracking sensor performance reduced", "Disengage from exposed corridor");
        Damage("Weather", "Tracking stability reduced", "Reduce speed and maintain safe spacing");
        Damage("Jamming", "Signal tracking degraded", "Use alternate tracking signals and report uncertainty");
        Damage("Sensor damage", "Lock confidence reduced", "Reacquire target before reporting position");

        A("upgrades");
        Upgrade("Extended Tracking Range Module", "Increase tracking range", 0.25f);
        Upgrade("Thermal Boost Lens", "Improve thermal detection", 0.20f);
        Upgrade("Anti-Jamming Shielding", "Improve resistance to signal interference", 0.30f);
        Upgrade("Silent Rotor Kit", "Reduce noise signature by 25%; improve survivability", 0.06f);

        A("threatResponses");
        Threat("Ballistic", 0.4f, 0.45f);
        Threat("Thermal", 0.4f, 0.55f);
        Threat("Signal jam", 0.2f, 0.65f);
        Threat("Weather", 0.5f, 0.50f);

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Locator canonical data saved: " + AssetDatabase.GetAssetPath(asset) +
                  ". Existing model prefab and store image were preserved.");
    }
}
