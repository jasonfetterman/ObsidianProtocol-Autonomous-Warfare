using UnityEditor;
using UnityEngine;

public static class PopulateLinkCanonicalData
{
    private const string UnitId = "obs-air-link";

    [MenuItem("Obsidian Protocol/Units/Populate Link Canonical Data")]
    public static void Populate()
    {
        Object asset = null;
        foreach (string guid in AssetDatabase.FindAssets("t:UnitDefinition"))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Object candidate = AssetDatabase.LoadMainAssetAtPath(path);
            if (candidate == null) continue;

            var test = new SerializedObject(candidate);
            var id = test.FindProperty("unitId");
            var name = test.FindProperty("displayName");
            if ((id != null && id.stringValue == UnitId) ||
                (name != null && name.stringValue == "Link"))
            {
                asset = candidate;
                break;
            }
        }

        if (asset == null)
        {
            Debug.LogError("Link population failed: existing UnitDefinition not found for " + UnitId);
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
        void Resource(string path, string component, int quantity) {
            var a = A(path, false);
            if (a == null) return;
            int n = a.arraySize++;
            var e = a.GetArrayElementAtIndex(n);
            var p = e.FindPropertyRelative("component"); if (p != null) p.stringValue = component;
            p = e.FindPropertyRelative("quantity"); if (p != null) p.intValue = quantity;
        }

        S("unitId", UnitId);
        S("displayName", "Link");
        S("role", "Network Bridge / Signal Routing Drone");
        S("tier", "Operational");
        S("status", "Active");

        // Physical / technical.
        F("physical.lengthMeters", 1.9f);
        F("physical.widthMeters", 1.4f);
        F("physical.heightMeters", 0.7f);
        F("physical.massKg", 14f);
        S("physical.powerSource", "High-efficiency battery");
        F("physical.energyCapacityWh", 1700f);
        F("physical.energyConsumptionWhPerMinute", 30f);
        S("physical.endurance", "40–55 min");
        F("physical.payloadCapacityKg", 1f);
        Strings("physical.modules", "Dual antenna array");
        S("physical.navigationSystem", "GPS + Mesh Routing AI");
        F("physical.structuralIntegrityHp", 100f);
        F("physical.armorRating", 0.12f);
        S("physical.heatSignature", "Low");
        S("physical.noiseSignature", "Low");
        F("physical.topSpeedMetersPerSecond", 18f);
        S("physical.topSpeedDescription", "18 m/s");
        S("physical.acceleration", "Medium");
        S("physical.turningRadius", "Tight");
        S("physical.climbRate", "Medium");
        S("physical.stabilityRating", "High");

        // Autonomy.
        S("autonomy.behaviorProfile", "Network Bridge / Signal Routing");
        F("autonomy.aggression", 0f);
        F("autonomy.caution", 0.8f);
        F("autonomy.persistence", 0.7f);
        F("autonomy.flankingLikelihood", 0f);
        F("autonomy.retreatThreshold", 0.8f);
        F("autonomy.riskTolerance", 0.15f);
        S("autonomy.retreatBehaviorDescription",
          "Relocate or retreat if integrity <25%, battery <15%, or signal integrity <40%; preserve network coverage.");
        S("autonomy.targetPriority", "Network gaps / weak mesh zones");
        S("autonomy.formationDiscipline", "High");

        // Sensors.
        F("sensors.visualRangeMeters", 900f);
        F("sensors.thermalRangeMeters", 400f);
        S("sensors.visualRangeDescription", "900 m");
        S("sensors.thermalRangeDescription", "400 m");
        S("sensors.acousticRange", "Low");
        S("sensors.radarRange", "None");
        S("sensors.lidarRange", "None");
        S("sensors.signalDetectionRange", "High");

        // Communication and network.
        F("communication.meshRangeKm", 15f);
        S("communication.meshRangeDescription", "15 km");
        F("communication.relayCapacity", 6f);
        S("communication.commandLinkType", "Mesh-Optimized");
        S("communication.networkPriority", "High");
        F("network.nodeHandoffEfficiency", 0.98f);
        S("network.packetLossRate", "0.8% nominal");
        F("network.encryptionLatencySeconds", 0.05f);
        S("network.meshSelfHealingThreshold", "2 lost nodes");
        S("network.relaySaturationCurve", "Throughput decreases 3% per active relay beyond 6 connected nodes");

        // Noncombatant threat model.
        F("combat.firepowerRating", 0f);
        F("combat.suppressionValue", 0f);
        S("combat.firepowerDescription", "Noncombatant network support drone");
        S("combat.accuracy", "N/A");
        S("combat.stabilization", "High");
        S("combat.weaponMounts", "None");
        S("combat.weaponTypes", "None");
        S("combat.trackingStrength", "Low");
        S("combat.disruptionStrength", "Medium");
        S("combat.breachCapability", "None");
        S("combat.countermeasureRating", "Low");
        S("combat.engagementDistance", "N/A");
        S("combat.preferredTerrain", "High-altitude mesh corridors");
        S("combat.preferredTargets", "None");
        S("combat.vulnerabilityProfile", "Ballistic / EMP / jamming");
        I("combat.threatLevel", 2);

        // Squad integration.
        S("squad.squadRole", "Network Bridge");
        I("squad.squadSizeContribution", 1);
        S("squad.formationBehavior", "Mesh-optimized spacing");
        S("squad.intentInterpretationRules", "Maintain coverage, avoid conflict; prioritize command traffic and restore critical links.");
        S("squad.retreatBehavior", "Early");
        S("squad.breachBehavior", "None");
        S("squad.flankingBehavior", "None");
        S("squad.reinforcementBehavior", "Supports communication squads");
        S("squad.parentCommandUnit", "NexusGrid / Overseer");
        S("squad.networkDependencies", "Warden mesh grid / Command Mesh");
        Strings("squad.subordinateUnits");
        Strings("squad.relayNodes", "Relay Node 1", "Relay Node 2", "Relay Node 3",
            "Relay Node 4", "Relay Node 5", "Relay Node 6");

        // Operator.
        S("vrOperator.operatorSeatType", "Drone View");
        S("vrOperator.operatorHud", "Mesh Bridge Overlay");
        S("vrOperator.cameraFeeds", "Optical");
        S("vrOperator.movementControls", "Full drone flight");
        S("vrOperator.sensorControls", "Signal scan / relay diagnostics");
        S("vrOperator.abilityControls", "Deploy Bridge, Boost Signal");
        S("vrOperator.weaponControls", "None");
        S("vrOperator.stabilizationControls", "Auto-hover");
        B("vrOperator.thermalView", true);
        B("vrOperator.lidarView", false);
        B("vrOperator.sonarView", false);
        B("vrOperator.tacticalOverlay", true);
        Strings("vrOperator.operatorAbilities",
            "Deploy Bridge Node", "Boost Signal", "Network Scan", "Mesh Diagnostics");

        // Economy.
        // Preserve the unit asset's existing credit price.
        I("economy.deploymentPoints", 0);
        B("economy.deploymentPointsDefined", false);
        S("economy.buildCost", "150 Metal / 140 Electronics");
        F("economy.buildTimeSeconds", 50f);
        S("economy.factoryType", "Drone Bay");
        S("economy.techRequirements", "Mesh Networking I");
        S("economy.maintenanceCost", "Low");
        S("economy.supplyConsumption", "Low");
        S("economy.operationalCost", "3 Metal / 5 Electronics per hour; Minimal");
        F("economy.repairTimeSeconds", 12f);
        S("economy.deploymentRequirements", "Standard drone pad");
        S("economy.transportCompatibility", "All aerial racks");
        S("economy.launchMethod", "Vertical takeoff");

        // Build requirements.
        Resource("buildRequirements.commonComponents", "Light Alloy Plates", 12);
        Resource("buildRequirements.commonComponents", "Composite Frame Rods", 8);
        Resource("buildRequirements.commonComponents", "Micro-Servo Actuators", 6);
        Resource("buildRequirements.commonComponents", "Stabilizer Fins", 2);
        Resource("buildRequirements.commonComponents", "Dual Antenna Housing", 1);
        Resource("buildRequirements.electronics", "Circuit Bundles", 12);
        Resource("buildRequirements.electronics", "Signal Processors", 8);
        Resource("buildRequirements.electronics", "Drone Control Chips", 4);
        Resource("buildRequirements.electronics", "Mesh Routing Chips", 2);
        Resource("buildRequirements.power", "High-Efficiency Battery Pack", 1);
        Resource("buildRequirements.power", "Power Regulation Nodes", 2);
        Resource("buildRequirements.rareTradeable", "Mesh-Grade Antenna Array", 1);
        Resource("buildRequirements.rareTradeable", "Encrypted Bridge Core", 1);
        Strings("buildRequirements.optionalUpgrades",
            "Extended Mesh Range Module", "Signal Boost Amplifier",
            "Anti-Jamming Shielding", "Silent Rotor Kit",
            "Multi-Node Relay Processor", "Hardened Bridge Core");

        // Environment.
        S("environment.terrainAdaptationCoefficients",
          "Open Air 1.00 / Urban 0.95 / Forest 0.85 / Mountain 0.90 / Desert 0.95 / Snow 0.85 / Water 1.00");
        F("environment.weatherSensitivityIndex", 0.30f);
        S("environment.visibilityModifiers", "Day 1.00 / Night 0.90 / Fog 0.80 / Rain 0.85 / Heavy Storm 0.65");
        S("environment.signalPropagationCurve",
          "Atmospheric interference reduces effective relay efficiency by 0.20% per 10% interference");
        A("environment.modifiers");
        Modifier("Severe weather", "Relay range reduced by 20%");
        Modifier("Electromagnetic interference", "External network efficiency reduced by 35%");
        Modifier("Mesh signal", "98% nominal signal reliability");

        // Cognitive and tactical.
        F("cognitive.decisionConfidence", 0.95f);
        F("cognitive.threatRecognitionAccuracy", 0.94f);
        F("cognitive.adaptiveLearningRatePerMission", 0.04f);
        F("cognitive.commandOverrideResistance", 0.04f);
        F("cognitive.cooperativeBehaviorIndex", 0.99f);
        F("tactical.formationAdaptability", 0.94f);
        F("tactical.tacticalRoleWeight", 0.88f);
        F("tactical.commandResponseDelaySeconds", 0.30f);
        S("tactical.reinforcementTriggerConditions", "Network coverage <75% or mesh loss >20%");
        S("tactical.retreatLogicThresholds", "Integrity <25%; battery <15%; signal integrity <40%");

        // Maintenance.
        F("maintenance.meanTimeBetweenFailuresHours", 1600f);
        F("maintenance.repairCycleSeconds", 12f);
        F("maintenance.resourceDrainPerHourPercent", 0.24f);
        F("maintenance.batteryEfficiencyPercent", 96f);
        F("maintenance.componentWearPercentPerHour", 0.002f);
        F("maintenance.sparePartDependencyIndex", 0.48f);
        F("maintenance.operationalUptimePercent", 99f);
        F("maintenance.energyRecoveryRatePercentPerIdleMinute", 7f);
        S("maintenance.maintenancePriorityLevel", "Tier I");
        S("maintenance.operationalCostPerHour", "3 Metal / 5 Electronics");
        S("maintenance.antennaEfficiency", "97%");
        S("maintenance.flightHardwareWear", "0.20% per 100 h");

        // Performance values stored as ratios where the schema expects normalized values.
        F("performance.missionSuccessProbability", 0.98f);
        F("performance.averageEngagementDurationMinutes", 0.30f);
        F("performance.reconDataYieldMbPerMinute", 24f);
        F("performance.communicationUptime", 0.99f);
        F("performance.threatAvoidanceEfficiency", 0.89f);

        // AI behavior matrix.
        A("behaviorNodes");
        Behavior("Network Gap Detection", 0.98f, "Identify weak or broken mesh");
        Behavior("Relay Placement", 0.96f, "Position for optimal network coverage");
        Behavior("Signal Routing", 0.97f, "Select efficient communication paths");
        Behavior("Bridge Maintenance", 0.95f, "Maintain active relay connections");
        Behavior("Signal Boosting", 0.91f, "Increase weak-link signal strength");
        Behavior("Network Reconfiguration", 0.94f, "Adapt mesh topology");
        Behavior("Threat Avoidance", 0.88f, "Avoid hostile or dangerous zones");
        Behavior("Relay Handoff", 0.96f, "Transfer connections between nodes");
        Behavior("Operator Override", 0.88f, "Accept authorized network commands");

        A("missionBehaviors");
        Mission("Network Gap Detection", "Identify broken mesh links", 0.98f);
        Mission("Relay Placement", "Place relay for coverage", 0.96f);
        Mission("Signal Recovery", "Restore signal", 0.95f);
        Mission("Mesh Bridge Maintenance", "Maintain relay links", 0.97f);
        Mission("Weak-Link Identification", "Find weak links", 0.94f);
        Mission("Communication Reroute", "Route around disruption", 0.96f);
        Mission("Emergency Network Recovery", "Recover critical network", 0.92f);

        A("unitSynergies");
        Synergy("Beacon", 0.96f, "Signal coordination and mesh extension");
        Synergy("Warden", 0.90f, "Integrate with Warden mesh grid");
        Synergy("Sentinel", 0.82f, "Share surveillance and command connectivity");

        A("damageResponses");
        Damage("Ballistic", "Antenna efficiency reduced by 20%", "Automatically relocate relay");
        Damage("Explosive", "Relay stability reduced by 25%", "Activate emergency handoff");
        Damage("EMP", "Network signal reduced by 50%", "Initiate autonomous mesh recovery");
        Damage("Jamming", "Effective mesh range reduced by 45%", "Use alternate routing");
        Damage("Antenna damage", "Relay capacity reduced", "Prioritize remaining nodes");
        Damage("Network failure", "Current connection unavailable", "Establish nearest viable relay connection");
        Damage("Emergency recovery", "Current mesh corridor unsafe", "Return to safe mesh corridor");

        A("upgrades");
        Upgrade("Extended Mesh Range Module", "+25% relay range; +8% coverage", 0.25f);
        Upgrade("Signal Boost Amplifier", "+20% signal strength; +10% weak-link recovery", 0.20f);
        Upgrade("Anti-Jamming Shielding", "+30% resistance; +8% network uptime", 0.30f);
        Upgrade("Silent Rotor Kit", "-25% noise signature; +6% survivability", 0.06f);
        Upgrade("Multi-Node Relay Processor", "+2 relay capacity; +8% routing efficiency", 0.08f);
        Upgrade("Hardened Bridge Core", "+20% EMP resistance; +10% network recovery", 0.20f);

        A("threatResponses");
        Threat("Ballistic", 0.30f, 0.50f);
        Threat("Explosive", 0.70f, 0.45f);
        Threat("EMP", 0.15f, 0.48f);
        Threat("Thermal", 0.30f, 0.55f);
        Threat("Signal jam", 0.10f, 0.72f);
        Threat("Network disruption", 0.20f, 0.86f);

        A("commandNetwork");
        Network("Mesh-Optimized Core", 48f, 0.30f, "Optimal");
        Network("Relay Node 1", 44f, 0.42f, "Stable");
        Network("Relay Node 2", 47f, 0.46f, "Stable");
        Network("Relay Node 3", 39f, 0.50f, "Active");
        Network("NexusGrid / Overseer", 52f, 0.35f, "Active");
        Network("Warden Mesh Grid", 56f, 0.40f, "Active");

        // Preserve the full command vocabulary and remaining operational notes.
        S("squad.intentInterpretationRules",
          "Maintain coverage and avoid conflict. Commands: establish network bridge; restore signal; route command traffic; cover network gap; boost weak signal; reconfigure mesh; restore communication.");
        S("squad.reinforcementBehavior",
          "Supports communication squads; global mesh range 15 km; redundant links Warden Mesh Grid / Command Mesh.");
        S("physical.endurance",
          "40–55 min; active relay 30 Wh/min; maximum relay load 45; signal boosting 40; emergency routing 48; idle 8; emergency power 12");
        S("vrOperator.operatorHud",
          "Mesh Bridge Overlay; mesh coverage visualization");
        S("economy.operationalCost", "Minimal; 3 Metal / 5 Electronics per hour");
        S("combat.preferredTargets", "None; direct combat assignment prohibited");

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Link canonical data saved: " + AssetDatabase.GetAssetPath(asset) +
                  ". Existing model prefab and store image were preserved. Check Console warnings for unsupported fields.");
    }
}
