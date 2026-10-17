using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class FirstTenPlanetsCompleteBuilder
{
    private const string SCENES = "Assets/Scenes/";

    [Serializable]
    private class Planet
    {
        public string id;
        public string name;
        public string folder;
        public string scene;

        public string environment;
        public string warfare;
        public string faction;

        public string[] continents;
        public string[] regions;
        public string[] biomes;
        public string[] islands;

        public Color surface;
        public Color terrain;
        public Color atmosphere;
        public Color accent;

        public int style;

        public Planet(
            string id,
            string name,
            string folder,
            string scene,
            string environment,
            string warfare,
            string faction,
            string[] continents,
            string[] regions,
            string[] biomes,
            string[] islands,
            Color surface,
            Color terrain,
            Color atmosphere,
            Color accent,
            int style)
        {
            this.id = id;
            this.name = name;
            this.folder = folder;
            this.scene = scene;
            this.environment = environment;
            this.warfare = warfare;
            this.faction = faction;
            this.continents = continents;
            this.regions = regions;
            this.biomes = biomes;
            this.islands = islands;
            this.surface = surface;
            this.terrain = terrain;
            this.atmosphere = atmosphere;
            this.accent = accent;
            this.style = style;
        }
    }

    private static Planet[] P()
    {
        return new[]
        {
            new Planet(
                "PLN-01","Acheron",
                "SCN30_ACHERON","Acheron",
                "Mountain / Industrial / Volcanic",
                "Combined arms, elevation, industrial combat",
                "Dominion of Acheron",
                new[]{"Blackspire Continent","Riftforge Continent","Ashfall Continent","Ashen Frontier Continent"},
                new[]{"Blackspire Range","Obsidian Lowlands","Rift Plateau Zone","Ashen Frontier"},
                new[]{"Volcanic Highlands","Basalt Coast","Charred Desert","Industrial Badlands"},
                new[]{"Blackspire Expanse","Ashen Wastes","Obsidian Coast","Rift Plateau"},
                new Color(.10f,.055f,.04f),
                new Color(.28f,.09f,.04f),
                new Color(.65f,.12f,.035f),
                new Color(1f,.25f,.025f),0),

            new Planet(
                "PLN-02","Virellia",
                "SCN31_VIRELLIA","Virellia",
                "Desert / Coastal / Ancient Forest",
                "Long-range warfare, reconnaissance",
                "Virellian Rangers",
                new[]{"Verdant Continent","Bloom Continent","Sylvan Continent","Sylvan Divide Continent"},
                new[]{"Emerald Belt","Shoal Reaches","Blooming Marches","Sylvan Divide"},
                new[]{"Dense Rainforest","Tropical Coast","Floral Plains","Ancient Forest"},
                new[]{"Verdant Crown","Emerald Shoals","Blooming Reaches","Sylvan Divide"},
                new Color(.035f,.18f,.055f),
                new Color(.08f,.32f,.09f),
                new Color(.08f,.55f,.25f),
                new Color(.25f,1f,.35f),1),

            new Planet(
                "PLN-03","Karthos",
                "SCN32_KARTHOS","Karthos",
                "River / Wetlands / Storm / Forest / Mountain",
                "Water crossings, naval support, ambushes, sensors",
                "Karthos Iron Legion",
                new[]{"Ironmarch Continent","Ember Continent","Titan Continent","Ironmarch Forge Continent"},
                new[]{"Ironmarch Basin","Forge Peninsula","Emberfall Fields","Titan's Spine Range"},
                new[]{"Metallic Badlands","Industrial Coast","Magma Fields","Jagged Mountain Range"},
                new[]{"Ironmarch","Forge Peninsula","Emberfall","Titan's Spine"},
                new Color(.10f,.13f,.15f),
                new Color(.25f,.27f,.27f),
                new Color(.08f,.35f,.45f),
                new Color(.25f,.75f,1f),2),

            new Planet(
                "PLN-04","Solara Prime",
                "SCN33_SOLARA_PRIME","Solara_Prime",
                "Mountain / Valley / Rural",
                "Elevation, airspace, defensive warfare",
                "Solara Skyward",
                new[]{"Sunforge Continent","Radiant Continent","Dawn Continent","Helios Continent"},
                new[]{"Sunforge Basin","Radiant Isles","Dawncrest Plains","Helios Strand"},
                new[]{"Sun-blasted Desert","Bright Coral Islands","Golden Savannah","Solar-heated Beaches"},
                new[]{"Sunforge Basin","Radiant Archipelago","Dawncrest","Helios Strand"},
                new Color(.36f,.20f,.035f),
                new Color(.70f,.40f,.07f),
                new Color(1f,.40f,.025f),
                new Color(1f,.80f,.20f),3),

            new Planet(
                "PLN-05","Nox Meridian",
                "SCN34_NOX_MERIDIAN","Nox_Meridian",
                "Suburban / Urban / Airport / High-rise",
                "Distributed objectives and autonomous forces",
                "Nox Urban Net",
                new[]{"Midnight Continent","Shadow Continent","Lunar Continent","Eclipse Continent"},
                new[]{"Midnight Shoals","Shadowfront Zone","Lunar Mire","Eclipse Reach"},
                new[]{"Dark Ocean Waters","Twilight Marsh","Moonlit Swamp","Permanent Night Coast"},
                new[]{"Midnight Shoal","Shadowfront","Lunar Mire","Eclipse Reach"},
                new Color(.015f,.02f,.045f),
                new Color(.045f,.055f,.10f),
                new Color(.16f,.045f,.38f),
                new Color(.65f,.08f,1f),4),

            new Planet(
                "PLN-06","Thalassa IX",
                "SCN35_THALASSA_IX","Thalassa_IX",
                "Rail / Heavy Industry / Military Base",
                "Logistics, infrastructure, armored warfare",
                "Thalassa Railguard",
                new[]{"Deepwater Continent","Coral Continent","Leviathan Continent","Tideglass Continent"},
                new[]{"Deepwater Verge","Coralspire Isles","Leviathan Shelf","Tideglass Coast"},
                new[]{"Deep Ocean Shelf","Coral Megastructures","Abyssal Drop-off","Crystal Tidepools"},
                new[]{"Deepwater Verge","Coralspire","Leviathan Shelf","Tideglass Coast"},
                new Color(.02f,.12f,.19f),
                new Color(.04f,.27f,.32f),
                new Color(.02f,.45f,.65f),
                new Color(.15f,.90f,.90f),5),

            new Planet(
                "PLN-07","Dravon's Reach",
                "SCN36_DRAVONS_REACH","Dravons_Reach",
                "Remote Arctic / Arctic / Island",
                "Extreme environment, thermal sensors, naval operations",
                "Dravon Arctic Corps",
                new[]{"Dragon Continent","Ember Continent","Scorch Continent","Pyre Continent"},
                new[]{"Dragon's Maw Range","Embercliff Ridge","Scorchlands Basin","Pyre Coast"},
                new[]{"Volcanic Cliffs","Burning Ridge","Fire-scarred Plains","Smoldering Shoreline"},
                new[]{"Dragon's Maw","Embercliff","Scorchlands","Pyre Coast"},
                new Color(.12f,.10f,.12f),
                new Color(.30f,.27f,.28f),
                new Color(.35f,.50f,.65f),
                new Color(1f,.22f,.03f),6),

            new Planet(
                "PLN-08","Erebos Station",
                "SCN37_EREBOS_STATION","Erebos_Station",
                "Island Chain / Open Ocean / Coastal / River",
                "Distributed warfare and naval control",
                "Void Rim Syndicate",
                new[]{"Void Continent","Blacklight Continent","Bastion Continent","Null Continent"},
                new[]{"Void Rim Sector","Blacklight Sector","Outer Bastion","Nullfield Zone"},
                new[]{"Vacuum-scarred Expanse","Neon Industrial Zone","Reinforced Outpost","Electromagnetic Dead Zone"},
                new[]{"Void Rim","Blacklight Sector","Outer Bastion","Nullfield"},
                new Color(.012f,.015f,.025f),
                new Color(.035f,.045f,.07f),
                new Color(.04f,.25f,.60f),
                new Color(.05f,1f,.90f),7),

            new Planet(
                "PLN-09","Halcyon Verge",
                "SCN38_HALCYON_VERGE","Halcyon_Verge",
                "Jungle / Tropical / Desert / Canyon",
                "Weather, concealment, sensor degradation, reconnaissance",
                "Halcyon Jungle Corps",
                new[]{"Tranquil Continent","Crystal Continent","Serene Continent","Whisper Continent"},
                new[]{"Tranquil Shoals","Crystal Plains","Serene Crest","Whispering Coast"},
                new[]{"Calm Tropical Waters","Crystal Grasslands","Peaceful Highlands","Quiet Windy Beaches"},
                new[]{"Tranquil Shoals","Crystal Plains","Serene Crest","Whispering Coast"},
                new Color(.04f,.20f,.11f),
                new Color(.20f,.40f,.16f),
                new Color(.10f,.55f,.38f),
                new Color(.25f,1f,.55f),8),

            new Planet(
                "PLN-10","Myridian Core",
                "SCN39_MYRIDIAN_CORE","Myridian_Core",
                "Desert / Rural / Destroyed Urban / War-torn City",
                "Deception and information warfare",
                "Coreheart Signal Corps",
                new[]{"Coreheart Continent","Myrid Continent","Prism Continent","Lattice Continent"},
                new[]{"Coreheart Plateau","Myrid Spires","Prism Valley","Lattice Coast"},
                new[]{"Geometric Plateau","Crystal Spires","Refractive Valley","Patterned Shoreline"},
                new[]{"Coreheart Plateau","Myrid Spires","Prism Valley","Lattice Coast"},
                new Color(.09f,.08f,.13f),
                new Color(.23f,.22f,.32f),
                new Color(.30f,.08f,.58f),
                new Color(.75f,.25f,1f),9)
        };
    }


    private static Planet[] PNEXT10()
    {
        return new[]
        {
            new Planet(
                "PLN-11","Vantor Helix",
                "SCN40_VANTOR_HELIX","Vantor_Helix",
                "Industrial River / Mega Port / Mountain Pass / Open Plains",
                "Bridges, water, industry, logistics; Naval + ground + air; Strategic chokepoint warfare; Maximum-distance combat",
                "Spiral River Authority",
                new[]{"Spiral Continent","Helix Continent","Coil Continent","Axis Continent"},
                new[]{"Spiral Shoal","Helix Ridge","Vantor Coil","Axis Strand"},
                new[]{"Spiral Sandbar","Twisted Ridge","Coiled Highlands","Linear Coastline"},
                new[]{"Spiral Shoal","Helix Ridge","Vantor Coil","Axis Strand"},
                new Color(.10f,.12f,.16f),
                new Color(.22f,.30f,.38f),
                new Color(.20f,.55f,.65f),
                new Color(.55f,.90f,1f),10),

            new Planet(
                "PLN-12","Orpheon Delta",
                "SCN41_ORPHEON_DELTA","Orpheon_Delta",
                "Open Military Range / Remote Facility / Dense Terrain / Remote Airbase",
                "Pure tactical experimentation; Communications degradation / EW; Sensor denial and information warfare; Air combat / anti-air",
                "Delta Test Command",
                new[]{"Delta Continent","Riverglass Continent","Flood Continent","Marshlight Continent"},
                new[]{"Delta Crown","Riverglass Reach","Floodspire","Marshlight Coast"},
                new[]{"River Delta Highlands","Glassy Wetlands","Flooded Pillars","Luminous Marsh Coast"},
                new[]{"Delta Crown","Riverglass Reach","Floodspire","Marshlight Coast"},
                new Color(.08f,.12f,.14f),
                new Color(.16f,.32f,.30f),
                new Color(.18f,.62f,.52f),
                new Color(.45f,1f,.75f),11),

            new Planet(
                "PLN-13","Cryostria",
                "SCN42_CRYOSTRIA","Cryostria",
                "Ocean / Mixed Terrain / Mountain / Mountain",
                "Naval / Fleet warfare; Large combined-arms battle; Combined arms, elevation, industrial combat; Ground warfare, logistics, chokepoints",
                "Frostveil Armada",
                new[]{"Frost Continent","Glacial Continent","Icebound Continent","Snowglass Continent"},
                new[]{"Frostveil Plains","Glacial Shoals","Icebound Crest","Snowglass Coast"},
                new[]{"Frozen Plains","Ice Shoals","Snowy Ridge","Crystal Ice Coast"},
                new[]{"Frostveil","Glacial Shoals","Icebound Crest","Snowglass Coast"},
                new Color(.08f,.12f,.18f),
                new Color(.20f,.32f,.48f),
                new Color(.35f,.58f,.78f),
                new Color(.72f,.92f,1f),12),

            new Planet(
                "PLN-14","Zephyria",
                "SCN43_ZEPHYRIA","Zephyria",
                "Urban / Volcanic / Desert / Desert",
                "Close-range urban warfare; Harsh terrain, visibility, environmental effects; Long-range warfare, reconnaissance; Mobile warfare, open terrain",
                "Windspire Street Guard",
                new[]{"Wind Continent","Gale Continent","Skyreach Continent","Whisperwind Continent"},
                new[]{"Windspire Range","Gale Shoals","Skyreach Plateau","Whisperwind Coast"},
                new[]{"Wind-carved Spires","Stormy Shoals","High Altitude Plateau","Breezy Coastline"},
                new[]{"Windspire","Gale Shoals","Skyreach","Whisperwind Coast"},
                new Color(.10f,.14f,.20f),
                new Color(.25f,.38f,.48f),
                new Color(.38f,.68f,.72f),
                new Color(.70f,.95f,1f),13),

            new Planet(
                "PLN-15","Umbra-7",
                "SCN44_UMBRA_7","Umbra_7",
                "Coastal / Port / River / Coastal",
                "Ground + naval + air operations; Naval invasion, logistics, urban combat; Water crossings, naval support, ambushes; Weather, visibility, sensors",
                "Darkwater Trident",
                new[]{"Darkwater Continent","Rift Continent","Nightfall Continent","Shadowstrand Continent"},
                new[]{"Darkwater Shoal","Umbra Rift","Nightfall Crest","Shadowstrand Coast"},
                new[]{"Blackwater Shoals","Shadow Riftlands","Night-covered Highlands","Darkened Beaches"},
                new[]{"Darkwater Shoal","Umbra Rift","Nightfall Crest","Shadowstrand"},
                new Color(.025f,.025f,.045f),
                new Color(.09f,.10f,.16f),
                new Color(.18f,.20f,.35f),
                new Color(.48f,.42f,.82f),14),

            new Planet(
                "PLN-16","Caelus Rift",
                "SCN45_CAELUS_RIFT","Caelus_Rift",
                "Forest / Dense Forest / Mountain / Mountain",
                "Recon, concealment, ambush warfare; Thermal, sensors, information warfare; Elevation, airspace, defensive warfare; Command networks and strategic positioning",
                "Riftfall Recon",
                new[]{"Riftfall Continent","Caelus Continent","Skyforge Continent","Horizon Continent"},
                new[]{"Riftfall Shoals","Caelus Break","Skyforge Rise","Horizon Strand"},
                new[]{"Rift-torn Shoals","Broken Highlands","Sky-heated Plateau","Endless Horizon Coast"},
                new[]{"Riftfall Shoals","Caelus Break","Skyforge Rise","Horizon Strand"},
                new Color(.06f,.11f,.13f),
                new Color(.16f,.28f,.25f),
                new Color(.30f,.52f,.44f),
                new Color(.65f,.92f,.72f),15),

            new Planet(
                "PLN-17","Pyrrhos Gate",
                "SCN46_PYRRHOS_GATE","Pyrrhos_Gate",
                "Valley / Rural / Suburban / Major Urban",
                "Chokepoints and convoy warfare; Multiple routes and strategic control; Distributed objectives and autonomous forces; Large-scale combined arms",
                "Gatefront Convoy Guard",
                new[]{"Gatefront Continent","Ember Continent","Flame Continent","Cinder Continent"},
                new[]{"Pyrrhos Gatefront","Ember Shoals","Flamecrest","Cinder Coast"},
                new[]{"Fiery Gatefront","Ember-lit Shoals","Flame-touched Crest","Ash-covered Coast"},
                new[]{"Pyrrhos Gatefront","Ember Shoals","Flamecrest","Cinder Coast"},
                new Color(.16f,.055f,.025f),
                new Color(.38f,.16f,.06f),
                new Color(.75f,.28f,.05f),
                new Color(1f,.58f,.08f),16),

            new Planet(
                "PLN-18","Luminara",
                "SCN47_LUMINARA","Luminara",
                "Airport / Urban / Rail / Heavy Industry",
                "Air operations and ground warfare; Air reconnaissance and vertical battlefield; Logistics, infrastructure, armored warfare; Close-range ground combat and destruction",
                "Luminara Airgrid",
                new[]{"Luminara Continent","Glow Continent","Radiant Continent","Lightglass Continent"},
                new[]{"Luminara Shoals","Glowspire","Radiant Crest","Lightglass Coast"},
                new[]{"Glowing Shoals","Bioluminescent Spires","Radiant Highlands","Light-reflective Coast"},
                new[]{"Luminara Shoals","Glowspire","Radiant Crest","Lightglass Coast"},
                new Color(.10f,.08f,.16f),
                new Color(.25f,.18f,.40f),
                new Color(.55f,.35f,.72f),
                new Color(1f,.80f,.35f),17),

            new Planet(
                "PLN-19","Vexilon",
                "SCN48_VEXILON","Vexilon",
                "Secret Military Facility / Military Base / Remote Arctic / Arctic",
                "Experimental warfare, sensors, EW; Defensive warfare and siege; Extreme environment, limited resources; Visibility, thermal sensors, navigation",
                "Vexilon Black Command",
                new[]{"Reach Continent","Storm Continent","Tempest Continent","Thunderstrand Continent"},
                new[]{"Vexilon Reach","Stormspire","Tempest Crest","Thunderstrand Coast"},
                new[]{"Storm-charged Reach","Thunder Spires","Tempest Highlands","Lightning-scarred Coast"},
                new[]{"Vexilon Reach","Stormspire","Tempest Crest","Thunderstrand"},
                new Color(.025f,.045f,.07f),
                new Color(.08f,.18f,.25f),
                new Color(.12f,.42f,.58f),
                new Color(.35f,.85f,1f),18),

            new Planet(
                "PLN-20","Astryx",
                "SCN49_ASTRYX","Astryx",
                "Arctic / Island / Island Chain / Open Ocean",
                "Naval + air + ice operations; Naval warfare and amphibious operations; Distributed warfare and naval control; Primarily naval warfare",
                "Astryx Ice Fleet",
                new[]{"Astryx Continent","Starfall Continent","Nebula Continent","Cosmos Continent"},
                new[]{"Astryx Shoal","Starfall Reaches","Nebula Crest","Cosmos Strand"},
                new[]{"Star-lit Shoal","Meteor-scarred Reaches","Nebula-colored Crest","Cosmic Shoreline"},
                new[]{"Astryx Shoal","Starfall Reaches","Nebula Crest","Cosmos Strand"},
                new Color(.035f,.055f,.10f),
                new Color(.10f,.20f,.35f),
                new Color(.20f,.42f,.68f),
                new Color(.55f,.82f,1f),19)
        };
    }

    [MenuItem("Obsidian Protocol/Worlds/BUILD NEXT 10 PLANETS + ALL HUD")]
    public static void BuildNext10()
    {
        foreach (Planet p in PNEXT10())
            BuildPlanet(p);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[OPAW] NEXT 10 PLANETS + ALL HUD STRUCTURE COMPLETE.");
    }

    private static Planet[] PNEXT10_B()
    {
        return new[]
        {
            new Planet(
                "PLN-21","Kairoth",
                "SCN50_KAIROTH","Kairoth",
                "Mountain / Industrial / Mountain / Industrial",
                "Combined arms, elevation, industrial combat; Ground warfare, logistics, chokepoints; Close-range urban warfare; Harsh terrain, visibility, environmental effects",
                "Kairoth Forge Guard",
                new[]{"Kairoth Continent","Rift Continent","Crest Continent","Riftstrand Continent"},
                new[]{"Kairoth Shoals","Riftspire","Crest of Kairoth","Riftstrand Coast"},
                new[]{"Rift-touched Shoals","Rift Spires","Rift Crest","Rift-scarred Coast"},
                new[]{"Kairoth Shoals","Riftspire","Crest of Kairoth","Riftstrand"},
                new Color(.08f,.09f,.12f),
                new Color(.20f,.24f,.30f),
                new Color(.34f,.40f,.48f),
                new Color(.70f,.78f,.88f),20),

            new Planet(
                "PLN-22","Nemeris",
                "SCN51_NEMERIS","Nemeris",
                "Desert / Desert / Coastal / Port / Industrial",
                "Long-range warfare, reconnaissance; Mobile warfare, open terrain; Ground + naval + air operations; Naval invasion, logistics, urban combat",
                "Nemeris Longshot",
                new[]{"Nemeris Continent","Deepcrest Continent","Abyssal Continent","Darkwater Continent"},
                new[]{"Nemeris Shoal","Deepcrest Highlands","Abyssal Rise","Darkwater Strand"},
                new[]{"Abyssal Shoal","Deepwater Highlands","Abyssal Plateau","Deep Dark Coast"},
                new[]{"Nemeris Shoal","Deepcrest","Abyssal Rise","Darkwater Strand"},
                new Color(.12f,.08f,.05f),
                new Color(.30f,.22f,.12f),
                new Color(.52f,.38f,.20f),
                new Color(.82f,.62f,.32f),21),

            new Planet(
                "PLN-23","Odyssar",
                "SCN52_ODYSSAR","Odyssar",
                "River / Wetlands / Coastal / Storm / Forest / Dense Forest / Night",
                "Water crossings, naval support, ambushes; Weather, visibility, sensors; Recon, concealment, ambush warfare; Thermal, sensors, information warfare",
                "Odyssar River Corps",
                new[]{"Odyssar Continent","Crest Continent","Odyssey Continent","Voyager Continent"},
                new[]{"Odyssar Shoals","Crest of Odyssar","Odyssey Rise","Voyager Strand"},
                new[]{"Explorer's Shoals","Explorer's Crest","Journey Plateau","Voyager Coast"},
                new[]{"Odyssar Shoals","Crest of Odyssar","Odyssey Rise","Voyager Strand"},
                new Color(.04f,.10f,.08f),
                new Color(.10f,.28f,.20f),
                new Color(.18f,.52f,.34f),
                new Color(.42f,.86f,.58f),22),

            new Planet(
                "PLN-24","Valtor Prime",
                "SCN53_VALTOR_PRIME","Valtor_Prime",
                "Mountain / Mountain / Valley / Highway / Rural / Infrastructure",
                "Elevation, airspace, defensive warfare; Command networks and strategic positioning; Chokepoints and convoy warfare; Multiple routes and strategic control",
                "Valtor Imperial Guard",
                new[]{"Valtor Continent","Prime Continent","Dominion Continent","Sovereign Continent"},
                new[]{"Valtor Shoals","Primecrest","Dominion Rise","Sovereign Strand"},
                new[]{"Imperial Shoals","Prime Highlands","Dominion Plateau","Sovereign Coast"},
                new[]{"Valtor Shoals","Primecrest","Dominion Rise","Sovereign Strand"},
                new Color(.09f,.09f,.14f),
                new Color(.22f,.22f,.34f),
                new Color(.38f,.34f,.58f),
                new Color(.72f,.64f,1f),23),

            new Planet(
                "PLN-25","Seraphis",
                "SCN54_SERAPHIS","Seraphis",
                "Suburban / Urban / Major Urban / Airport / Industrial / Urban / High-rise",
                "Distributed objectives and autonomous forces; Large-scale combined arms; Air operations and ground warfare; Air reconnaissance and vertical battlefield",
                "Seraphis Grid",
                new[]{"Seraphis Continent","Halo Continent","Angelic Continent","Celestial Continent"},
                new[]{"Seraphis Shoals","Halo Crest","Angelic Rise","Celestial Strand"},
                new[]{"Celestial Shoals","Halo Highlands","Angelic Plateau","Celestial Coast"},
                new[]{"Seraphis Shoals","Halo Crest","Angelic Rise","Celestial Strand"},
                new Color(.12f,.10f,.16f),
                new Color(.28f,.22f,.38f),
                new Color(.52f,.38f,.62f),
                new Color(.92f,.76f,1f),24),

            new Planet(
                "PLN-26","Kronis-4",
                "SCN55_KRONIS_4","Kronis_4",
                "Rail / Industrial / Heavy Industry / Secret Military Facility / Military Base",
                "Logistics, infrastructure, armored warfare; Close-range ground combat and destruction; Experimental warfare, sensors, EW; Defensive warfare and siege",
                "Kronis Railguard",
                new[]{"Kronis Continent","Ridge Continent","Crest Continent","Strand Continent"},
                new[]{"Kronis Shoals","Kronis Ridge","Kronis Crest","Kronis Strand"},
                new[]{"Rocky Shoals","Harsh Ridge","Rugged Crest","Stone Coast"},
                new[]{"Kronis Shoals","Kronis Ridge","Kronis Crest","Kronis Strand"},
                new Color(.10f,.09f,.08f),
                new Color(.26f,.22f,.18f),
                new Color(.48f,.38f,.28f),
                new Color(.78f,.62f,.40f),25),

            new Planet(
                "PLN-27","Eon's Cradle",
                "SCN56_EONS_CRADLE","Eons_Cradle",
                "Remote Arctic / Arctic / Snow / Arctic / Naval / Island / Ocean",
                "Extreme environment, limited resources; Visibility, thermal sensors, navigation; Naval + air + ice operations; Naval warfare and amphibious operations",
                "Eon Arctic Corps",
                new[]{"Eon Continent","Cradle Continent","Timeless Continent","Eternal Continent"},
                new[]{"Eon Shoals","Cradle Crest","Timeless Rise","Eternal Strand"},
                new[]{"Timeless Shoals","Cradle Highlands","Eternal Plateau","Eternal Coast"},
                new[]{"Eon Shoals","Cradle Crest","Timeless Rise","Eternal Strand"},
                new Color(.07f,.10f,.15f),
                new Color(.16f,.25f,.36f),
                new Color(.30f,.48f,.64f),
                new Color(.68f,.88f,1f),26),

            new Planet(
                "PLN-28","Virex-Theta",
                "SCN57_VIREX_THETA","Virex_Theta",
                "Island Chain / Open Ocean / Coastal / Defensive / River / Rural",
                "Distributed warfare and naval control; Primarily naval warfare; Fortifications and defensive operations; Dynamic water and terrain conditions",
                "Virex Chain Command",
                new[]{"Virex Continent","Theta Continent","Rise Continent","Strand Continent"},
                new[]{"Virex Shoals","Theta Crest","Virex Rise","Theta Strand"},
                new[]{"Theta Shoals","Theta Highlands","Virex Plateau","Theta Coast"},
                new[]{"Virex Shoals","Theta Crest","Virex Rise","Theta Strand"},
                new Color(.035f,.08f,.12f),
                new Color(.08f,.22f,.30f),
                new Color(.14f,.42f,.52f),
                new Color(.36f,.82f,.90f),27),

            new Planet(
                "PLN-29","Solaris Echo",
                "SCN58_SOLARIS_ECHO","Solaris_Echo",
                "Jungle / Tropical / Dense Jungle / Desert / Canyon / Highway",
                "Weather, concealment, sensor degradation; Recon and autonomous search; Long-range fire and air superiority; Mobility, ambushes, route control",
                "Solaris Jungle Corps",
                new[]{"Solaris Continent","Echo Continent","Sunfall Continent","Strand Continent"},
                new[]{"Solaris Shoals","Echo Crest","Sunfall Rise","Echo Strand"},
                new[]{"Echoing Shoals","Echo Highlands","Sunset Plateau","Echo Coast"},
                new[]{"Solaris Shoals","Echo Crest","Sunfall Rise","Echo Strand"},
                new Color(.05f,.14f,.07f),
                new Color(.15f,.34f,.12f),
                new Color(.32f,.62f,.18f),
                new Color(.70f,1f,.38f),28),

            new Planet(
                "PLN-30","Obsidian World",
                "SCN59_OBSIDIAN_WORLD","Obsidian_World",
                "Desert / Military / Rural / Mountain / Destroyed Urban / War-torn City",
                "Deception and information warfare; Recon and stealth; Battlefield aftermath and dynamic terrain; Destruction-focused combined arms",
                "Obsidian Signal Corps",
                new[]{"Obsidian Continent","Blackglass Continent","Stoneforge Continent","Strand Continent"},
                new[]{"Obsidian Shoals","Blackglass Crest","Stoneforge Rise","Obsidian Strand"},
                new[]{"Obsidian Shoals","Blackglass Highlands","Stoneforge Plateau","Obsidian Coast"},
                new[]{"Obsidian Shoals","Blackglass Crest","Stoneforge Rise","Obsidian Strand"},
                new Color(.025f,.025f,.035f),
                new Color(.08f,.08f,.11f),
                new Color(.18f,.16f,.22f),
                new Color(.42f,.38f,.50f),29)
        };
    }

    [MenuItem("Obsidian Protocol/Worlds/BUILD SCN50-SCN59 + ALL HUD")]
    public static void BuildNext10B()
    {
        foreach (Planet p in PNEXT10_B())
            BuildPlanet(p);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[OPAW] SCN50-SCN59 + ALL HUD STRUCTURE COMPLETE.");
    }
    [MenuItem("Obsidian Protocol/Worlds/BUILD FIRST 10 PLANETS + ALL HUD")]
    public static void Build()
    {
        foreach (Planet p in P())
            BuildPlanet(p);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[OPAW] FIRST 10 PLANETS + ALL HUD STRUCTURE COMPLETE.");
    }

    private static void BuildPlanet(Planet p)
    {
        string folder = SCENES + p.folder;
        string scenePath = folder + "/" + p.scene + ".unity";

        EnsureFolder(folder);

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        GameObject root = new GameObject(p.folder);

        BuildWorld(root,p);
        BuildHUD(root,p);

        EditorSceneManager.SaveScene(scene,scenePath);
    }

    private static void ValidatePlanetData(Planet p)
    {
        if (p == null)
            throw new System.Exception("Planet data is null.");

        if (p.continents == null || p.continents.Length != 4)
            throw new System.Exception(
                "PLANET DATA ERROR: " + p.name +
                " continents array has " +
                (p.continents == null ? 0 : p.continents.Length) +
                " entries. Expected 4.");

        if (p.regions == null || p.regions.Length != 4)
            throw new System.Exception(
                "PLANET DATA ERROR: " + p.name +
                " regions array has " +
                (p.regions == null ? 0 : p.regions.Length) +
                " entries. Expected 4.");

        if (p.biomes == null || p.biomes.Length != 4)
            throw new System.Exception(
                "PLANET DATA ERROR: " + p.name +
                " biomes array has " +
                (p.biomes == null ? 0 : p.biomes.Length) +
                " entries. Expected 4.");

        if (p.islands == null || p.islands.Length != 4)
            throw new System.Exception(
                "PLANET DATA ERROR: " + p.name +
                " islands array has " +
                (p.islands == null ? 0 : p.islands.Length) +
                " entries. Expected 4.");
    }
    // ========================================================
    // WORLD
    // ========================================================

    private static void BuildWorld(GameObject root,Planet p)
    {
        ValidatePlanetData(p);
        GameObject world = GO("WORLD",root.transform);

        GameObject planet = Primitive(
            PrimitiveType.Sphere,
            "PLANET_SURFACE",
            world.transform);

        planet.transform.localScale =
            Vector3.one * 36f;

        Material(planet,p.surface,.2f,.5f);

        GameObject terrain = GO(
            "CONTINENTS_AND_TERRAIN",
            world.transform);

        for(int i=0;i<4;i++)
        {
            GameObject continent = GO(
                "CONT-"+(i+1).ToString("00"),
                terrain.transform);

            Vector3 pos = PlanetPoint(
                17.5f,
                -35f+i*25f,
                -140f+i*92f);

            continent.transform.position=pos;
            continent.transform.rotation=
                Quaternion.LookRotation(pos.normalized);

            BuildContinent(continent,p,i);
        }

        GameObject atmosphere =
            Primitive(
                PrimitiveType.Sphere,
                "ATMOSPHERE",
                world.transform);

        atmosphere.transform.localScale =
            Vector3.one*37.4f;

        Material(
            atmosphere,
            p.atmosphere,
            0f,
            .85f);

        RemoveCollider(atmosphere);

        GameObject metadata =
            GO("PLANET_METADATA",root.transform);

        PlanetMetadata md =
            metadata.AddComponent<PlanetMetadata>();

        md.PlanetID=p.id;
        md.PlanetName=p.name;
        md.Environment=p.environment;
        md.WarfareFocus=p.warfare;
        md.FactionControl=p.faction;

        for(int i=0;i<4;i++)
        {
            md.Continents.Add(p.continents[i]);
            md.Regions.Add(p.regions[i]);
            md.Biomes.Add(p.biomes[i]);
            md.Islands.Add(p.islands[i]);
        }

        GameObject sun=GO("SUN",root.transform);
        Light sl=sun.AddComponent<Light>();
        sl.type=LightType.Directional;
        sl.intensity=1.35f;
        sl.shadows=LightShadows.Soft;
        sun.transform.rotation=
            Quaternion.Euler(30f,-35f,0f);

        RenderSettings.ambientIntensity=.35f;

        GameObject cam=GO("WORLD_CAMERA",root.transform);
        Camera c=cam.AddComponent<Camera>();
        c.fieldOfView=40f;
        c.nearClipPlane=.1f;
        c.farClipPlane=1000f;
        cam.transform.position=
            new Vector3(0,10,90);
        cam.transform.LookAt(Vector3.zero);

        GameObject stars=GO(
            "STARFIELD",
            root.transform);

        UnityEngine.Random.InitState(
            p.id.GetHashCode());

        for(int i=0;i<150;i++)
        {
            GameObject star=Primitive(
                PrimitiveType.Sphere,
                "STAR_"+i.ToString("000"),
                stars.transform);

            star.transform.position=
                UnityEngine.Random.onUnitSphere*
                UnityEngine.Random.Range(150f,250f);

            star.transform.localScale=
                Vector3.one*
                UnityEngine.Random.Range(.06f,.18f);

            Material(
                star,
                Color.white,
                0f,
                0f);

            RemoveCollider(star);
        }
    }

    private static void BuildContinent(
        GameObject parent,
        Planet p,
        int index)
    {
        if(p.style==0)
        {
            for(int i=0;i<7;i++)
            {
                GameObject m=Primitive(
                    PrimitiveType.Cylinder,
                    "VOLCANIC_MOUNTAIN_"+i,
                    parent.transform);

                m.transform.localPosition=
                    new Vector3(
                        (i-3)*1.7f,
                        .8f+(i%3)*.7f,
                        Mathf.Sin(i)*2f);

                m.transform.localScale=
                    new Vector3(
                        1.2f,
                        2.5f+(i%3),
                        1.2f);

                Material(m,p.terrain,.05f,.3f);
                RemoveCollider(m);
            }
        }
        else if(p.style==1)
        {
            for(int i=0;i<28;i++)
            {
                GameObject t=Primitive(
                    PrimitiveType.Cylinder,
                    "FOREST_TREE_"+i,
                    parent.transform);

                float a=i*.9f;
                float r=1f+(i%5)*.45f;

                t.transform.localPosition=
                    new Vector3(
                        Mathf.Cos(a)*r,
                        .8f,
                        Mathf.Sin(a)*r);

                t.transform.localScale=
                    new Vector3(
                        .25f,
                        1.2f+(i%4)*.4f,
                        .25f);

                Material(t,p.terrain,0,.35f);
                RemoveCollider(t);
            }
        }
        else if(p.style==2)
        {
            for(int i=0;i<10;i++)
            {
                GameObject b=Primitive(
                    PrimitiveType.Cube,
                    "FORGE_STRUCTURE_"+i,
                    parent.transform);

                b.transform.localPosition=
                    new Vector3(
                        (i%5-2)*1.4f,
                        .7f+(i%3),
                        (i/5)*2.2f-1f);

                b.transform.localScale=
                    new Vector3(
                        .9f,
                        1.4f+(i%3),
                        .9f);

                Material(b,p.terrain,.65f,.25f);
                RemoveCollider(b);
            }
        }
        else if(p.style==3)
        {
            for(int i=0;i<6;i++)
            {
                GameObject dune=Primitive(
                    PrimitiveType.Sphere,
                    "DESERT_DUNE_"+i,
                    parent.transform);

                dune.transform.localPosition=
                    new Vector3(
                        (i-3)*1.8f,
                        .4f,
                        Mathf.Sin(i)*1.8f);

                dune.transform.localScale=
                    new Vector3(
                        2.6f,
                        .65f,
                        1.7f);

                Material(dune,p.terrain,0,.8f);
                RemoveCollider(dune);
            }
        }
        else if(p.style==4)
        {
            for(int i=0;i<12;i++)
            {
                GameObject tower=Primitive(
                    PrimitiveType.Cube,
                    "NIGHT_CITY_TOWER_"+i,
                    parent.transform);

                tower.transform.localPosition=
                    new Vector3(
                        (i%6-2.5f)*1.1f,
                        1.5f+(i%5)*.75f,
                        (i/6)*2f-1f);

                tower.transform.localScale=
                    new Vector3(
                        .55f,
                        1.5f+(i%5),
                        .55f);

                Material(tower,p.accent,.1f,.7f);
                RemoveCollider(tower);
            }
        }
        else if(p.style==5)
        {
            for(int i=0;i<8;i++)
            {
                GameObject platform=Primitive(
                    PrimitiveType.Cylinder,
                    "OCEAN_PLATFORM_"+i,
                    parent.transform);

                platform.transform.localPosition=
                    new Vector3(
                        (i%4-1.5f)*1.7f,
                        .5f,
                        (i/4)*2.4f-1.2f);

                platform.transform.localScale=
                    new Vector3(
                        1.2f,
                        .3f,
                        1.2f);

                Material(platform,p.accent,.7f,.5f);
                RemoveCollider(platform);
            }
        }
        else if(p.style==6)
        {
            for(int i=0;i<6;i++)
            {
                GameObject peak=Primitive(
                    PrimitiveType.Cylinder,
                    "FROSTFIRE_PEAK_"+i,
                    parent.transform);

                peak.transform.localPosition=
                    new Vector3(
                        (i-3)*1.8f,
                        1f,
                        Mathf.Cos(i)*1.5f);

                peak.transform.localScale=
                    new Vector3(
                        1.1f,
                        3f+(i%2),
                        1.1f);

                Material(peak,p.terrain,.05f,.3f);
                RemoveCollider(peak);
            }
        }
        else if(p.style==7)
        {
            for(int i=0;i<10;i++)
            {
                GameObject module=Primitive(
                    PrimitiveType.Cube,
                    "STATION_MODULE_"+i,
                    parent.transform);

                module.transform.localPosition=
                    new Vector3(
                        (i%5-2)*1.5f,
                        .6f+(i%2)*1.5f,
                        (i/5)*2.4f-1.2f);

                module.transform.localScale=
                    new Vector3(
                        1.0f,
                        .8f+(i%3),
                        1.0f);

                Material(module,p.accent,.75f,.35f);
                RemoveCollider(module);
            }
        }
        else if(p.style==8)
        {
            for(int i=0;i<18;i++)
            {
                GameObject crystal=Primitive(
                    PrimitiveType.Capsule,
                    "CRYSTAL_FOREST_"+i,
                    parent.transform);

                float a=i*.7f;
                float r=1f+(i%4)*.5f;

                crystal.transform.localPosition=
                    new Vector3(
                        Mathf.Cos(a)*r,
                        .9f,
                        Mathf.Sin(a)*r);

                crystal.transform.localScale=
                    new Vector3(
                        .35f,
                        1.5f+(i%4)*.4f,
                        .35f);

                Material(crystal,p.accent,.05f,.9f);
                RemoveCollider(crystal);
            }
        }
        else
        {
            for(int i=0;i<12;i++)
            {
                GameObject block=Primitive(
                    PrimitiveType.Cube,
                    "GEOMETRIC_RUIN_"+i,
                    parent.transform);

                block.transform.localPosition=
                    new Vector3(
                        (i%4-1.5f)*1.5f,
                        .5f+(i%4)*.6f,
                        (i/4)*2f-2f);

                block.transform.localScale=
                    new Vector3(
                        1.1f,
                        .7f+(i%4),
                        1.1f);

                Material(block,p.terrain,.15f,.55f);
                RemoveCollider(block);
            }
        }

        GameObject location =
            GO("LOCATION_DATA",parent.transform);

        LocationMetadata lm =
            location.AddComponent<LocationMetadata>();

        lm.Continent=p.continents[index];
        lm.Region=p.regions[index];
        lm.Biome=p.biomes[index];
        lm.Island=p.islands[index];
    }

    // ========================================================
    // HUD SYSTEM
    // ========================================================

    private static void BuildHUD(
        GameObject root,
        Planet p)
    {
        GameObject hud=GO(
            "HUD",
            root.transform);

        Canvas canvas=
            hud.AddComponent<Canvas>();

        canvas.renderMode=
            RenderMode.ScreenSpaceOverlay;

        hud.AddComponent<
            CanvasScaler>();

        hud.AddComponent<
            GraphicRaycaster>();

        GameObject eventSystem=
            new GameObject(
                "EventSystem",
                typeof(UnityEngine.EventSystems.EventSystem),
                typeof(UnityEngine.EventSystems.StandaloneInputModule));

        eventSystem.transform.SetParent(
            root.transform);

        // ----------------------------------------------------
        // HUD-201 GALAXY
        // ----------------------------------------------------

        GameObject galaxy=Panel(
            "HUD-201 GalaxyMapHUD",
            hud.transform,
            new Vector2(0,0),
            new Vector2(1,1));

        Panel(
            "HUD-201-01 TopBar",
            galaxy.transform,
            new Vector2(0,0.88f),
            new Vector2(1,1));

        Text(
            "CurrentGalaxyNameDisplay",
            galaxy.transform,
            "GALAXY",
            new Vector2(.02f,.93f),
            new Vector2(.25f,.98f));

        Text(
            "CurrentSectorNameDisplay",
            galaxy.transform,
            p.name.ToUpper(),
            new Vector2(.25f,.93f),
            new Vector2(.48f,.98f));

        Panel(
            "HUD-201-01D PlayerResourcesPanel",
            galaxy.transform,
            new Vector2(.72f,.90f),
            new Vector2(.98f,.98f));

        Panel(
            "HUD-201-01E TimeControlPanel",
            galaxy.transform,
            new Vector2(.50f,.90f),
            new Vector2(.70f,.98f));

        Panel(
            "HUD-201-02 NavigationPanel",
            galaxy.transform,
            new Vector2(.02f,.56f),
            new Vector2(.22f,.86f));

        Button(
            "GalaxyZoomInButton",
            galaxy.transform,
            "ZOOM +",
            new Vector2(.04f,.79f),
            new Vector2(.20f,.85f));

        Button(
            "GalaxyZoomOutButton",
            galaxy.transform,
            "ZOOM -",
            new Vector2(.04f,.72f),
            new Vector2(.20f,.78f));

        Button(
            "SectorListDropdown",
            galaxy.transform,
            "SECTORS",
            new Vector2(.04f,.65f),
            new Vector2(.20f,.71f));

        Button(
            "PlanetSearchField",
            galaxy.transform,
            "SEARCH PLANETS",
            new Vector2(.04f,.58f),
            new Vector2(.20f,.64f));

        Button(
            "FilterPanelButton",
            galaxy.transform,
            "FILTERS",
            new Vector2(.04f,.51f),
            new Vector2(.20f,.57f));

        Panel(
            "HUD-201-03 MapLayerPanel",
            galaxy.transform,
            new Vector2(.02f,.22f),
            new Vector2(.22f,.48f));

        string[] galaxyLayers=
        {
            "Galaxy",
            "Sectors",
            "Planets",
            "Factions",
            "WarfareZones",
            "Resources",
            "TradeRoutes"
        };

        for(int i=0;i<galaxyLayers.Length;i++)
        {
            Button(
                "LayerToggle_"+galaxyLayers[i],
                galaxy.transform,
                galaxyLayers[i].ToUpper(),
                new Vector2(.04f,.43f-i*.03f),
                new Vector2(.20f,.455f-i*.03f));
        }

        Panel(
            "HUD-201-04 GalaxyObjectInfoPanel",
            galaxy.transform,
            new Vector2(.76f,.35f),
            new Vector2(.98f,.86f));

        Text(
            "ObjectNameDisplay",
            galaxy.transform,
            p.name,
            new Vector2(.78f,.78f),
            new Vector2(.96f,.83f));

        Text(
            "ObjectTypeDisplay",
            galaxy.transform,
            "PLANET",
            new Vector2(.78f,.73f),
            new Vector2(.96f,.78f));

        Text(
            "ObjectOwnerFactionDisplay",
            galaxy.transform,
            p.faction,
            new Vector2(.78f,.67f),
            new Vector2(.96f,.72f));

        Text(
            "ObjectStatusDisplay",
            galaxy.transform,
            "ACTIVE",
            new Vector2(.78f,.61f),
            new Vector2(.96f,.66f));

        Text(
            "ObjectThreatLevelDisplay",
            galaxy.transform,
            "THREAT: ACTIVE",
            new Vector2(.78f,.55f),
            new Vector2(.96f,.60f));

        Button(
            "ObjectEnterButton",
            galaxy.transform,
            "ENTER SYSTEM",
            new Vector2(.78f,.46f),
            new Vector2(.96f,.52f));

        Panel(
            "HUD-201-05 GalaxyLegendPanel",
            galaxy.transform,
            new Vector2(.76f,.18f),
            new Vector2(.98f,.33f));

        Panel(
            "HUD-201-06 QuickActionsPanel",
            galaxy.transform,
            new Vector2(.30f,.03f),
            new Vector2(.68f,.15f));

        Button(
            "JumpToHomeworldButton",
            galaxy.transform,
            "HOMEWORLD",
            new Vector2(.31f,.06f),
            new Vector2(.38f,.12f));

        Button(
            "JumpToLastViewedPlanetButton",
            galaxy.transform,
            "LAST PLANET",
            new Vector2(.39f,.06f),
            new Vector2(.47f,.12f));

        Button(
            "OpenIntelNetworkButton",
            galaxy.transform,
            "INTEL",
            new Vector2(.48f,.06f),
            new Vector2(.55f,.12f));

        Button(
            "OpenDoctrineManagerButton",
            galaxy.transform,
            "DOCTRINE",
            new Vector2(.56f,.06f),
            new Vector2(.63f,.12f));

        Button(
            "OpenLogisticsOverviewButton",
            galaxy.transform,
            "LOGISTICS",
            new Vector2(.64f,.06f),
            new Vector2(.71f,.12f));

        Panel(
            "HUD-201-07 AlertsPanel",
            galaxy.transform,
            new Vector2(.76f,.03f),
            new Vector2(.98f,.15f));

        Panel(
            "HUD-201-08 BottomBar",
            galaxy.transform,
            new Vector2(.02f,.02f),
            new Vector2(.27f,.15f));

        Button(
            "BackButton",
            galaxy.transform,
            "BACK",
            new Vector2(.03f,.05f),
            new Vector2(.08f,.12f));

        Button(
            "ExitToCommandCenterButton",
            galaxy.transform,
            "COMMAND",
            new Vector2(.09f,.05f),
            new Vector2(.17f,.12f));

        Button(
            "SettingsButton",
            galaxy.transform,
            "SETTINGS",
            new Vector2(.18f,.05f),
            new Vector2(.22f,.12f));

        Button(
            "HelpButton",
            galaxy.transform,
            "HELP",
            new Vector2(.23f,.05f),
            new Vector2(.27f,.12f));

        // ----------------------------------------------------
        // HUD-202 STAR SYSTEM
        // ----------------------------------------------------

        GameObject system=Panel(
            "HUD-202 StarSystemHUD",
            hud.transform,
            new Vector2(0,0),
            new Vector2(1,1));

        system.SetActive(false);

        Panel(
            "HUD-202-01 TopBar",
            system.transform,
            new Vector2(0,.88f),
            new Vector2(1,1));

        Text(
            "CurrentSystemNameDisplay",
            system.transform,
            p.name.ToUpper()+" SYSTEM",
            new Vector2(.02f,.93f),
            new Vector2(.35f,.98f));

        Text(
            "CurrentStarNameDisplay",
            system.transform,
            "PRIMARY STAR",
            new Vector2(.36f,.93f),
            new Vector2(.55f,.98f));

        Panel(
            "HUD-202-01D PlayerResourcesPanel",
            system.transform,
            new Vector2(.72f,.90f),
            new Vector2(.98f,.98f));

        Panel(
            "HUD-202-01E TimeControlPanel",
            system.transform,
            new Vector2(.56f,.90f),
            new Vector2(.70f,.98f));

        Panel(
            "HUD-202-02 NavigationPanel",
            system.transform,
            new Vector2(.02f,.56f),
            new Vector2(.22f,.86f));

        Button(
            "SystemZoomInButton",
            system.transform,
            "ZOOM +",
            new Vector2(.04f,.79f),
            new Vector2(.20f,.85f));

        Button(
            "SystemZoomOutButton",
            system.transform,
            "ZOOM -",
            new Vector2(.04f,.72f),
            new Vector2(.20f,.78f));

        Button(
            "PlanetListDropdown",
            system.transform,
            "PLANETS",
            new Vector2(.04f,.65f),
            new Vector2(.20f,.71f));

        Button(
            "SystemSearchField",
            system.transform,
            "SEARCH",
            new Vector2(.04f,.58f),
            new Vector2(.20f,.64f));

        Button(
            "SystemFilterPanelButton",
            system.transform,
            "FILTERS",
            new Vector2(.04f,.51f),
            new Vector2(.20f,.57f));

        Panel(
            "HUD-202-03 MapLayerPanel",
            system.transform,
            new Vector2(.02f,.22f),
            new Vector2(.22f,.48f));

        string[] systemLayers=
        {
            "Star",
            "Planets",
            "Orbits",
            "Factions",
            "WarfareZones",
            "Resources",
            "LogisticsRoutes"
        };

        for(int i=0;i<systemLayers.Length;i++)
        {
            Button(
                "LayerToggle_"+systemLayers[i],
                system.transform,
                systemLayers[i].ToUpper(),
                new Vector2(.04f,.43f-i*.03f),
                new Vector2(.20f,.455f-i*.03f));
        }

        Panel(
            "HUD-202-04 SystemObjectInfoPanel",
            system.transform,
            new Vector2(.76f,.35f),
            new Vector2(.98f,.86f));

        Text(
            "SystemObjectNameDisplay",
            system.transform,
            p.name,
            new Vector2(.78f,.78f),
            new Vector2(.96f,.83f));

        Text(
            "SystemObjectTypeDisplay",
            system.transform,
            "PLANET",
            new Vector2(.78f,.73f),
            new Vector2(.96f,.78f));

        Text(
            "SystemObjectOwnerFactionDisplay",
            system.transform,
            p.faction,
            new Vector2(.78f,.67f),
            new Vector2(.96f,.72f));

        Text(
            "SystemObjectStatusDisplay",
            system.transform,
            "ACTIVE",
            new Vector2(.78f,.61f),
            new Vector2(.96f,.66f));

        Text(
            "SystemObjectThreatLevelDisplay",
            system.transform,
            "THREAT: ACTIVE",
            new Vector2(.78f,.55f),
            new Vector2(.96f,.60f));

        Button(
            "SystemObjectEnterButton",
            system.transform,
            "ENTER PLANET",
            new Vector2(.78f,.46f),
            new Vector2(.96f,.52f));

        Panel(
            "HUD-202-05 OrbitPanel",
            system.transform,
            new Vector2(.76f,.20f),
            new Vector2(.98f,.43f));

        Button(
            "OrbitPathToggle",
            system.transform,
            "ORBIT PATH",
            new Vector2(.78f,.36f),
            new Vector2(.96f,.41f));

        Text(
            "OrbitalVelocityIndicator",
            system.transform,
            "VELOCITY: NOMINAL",
            new Vector2(.78f,.31f),
            new Vector2(.96f,.35f));

        Text(
            "OrbitalDistanceDisplay",
            system.transform,
            "DISTANCE: STABLE",
            new Vector2(.78f,.26f),
            new Vector2(.96f,.30f));

        Text(
            "OrbitalStabilityIndicator",
            system.transform,
            "STABILITY: 100%",
            new Vector2(.78f,.21f),
            new Vector2(.96f,.25f));

        Panel(
            "HUD-202-06 QuickActionsPanel",
            system.transform,
            new Vector2(.30f,.03f),
            new Vector2(.71f,.15f));

        Button(
            "JumpToGalaxyMapButton",
            system.transform,
            "GALAXY",
            new Vector2(.31f,.06f),
            new Vector2(.38f,.12f));

        Button(
            "JumpToPlanetOverviewButton",
            system.transform,
            "PLANET",
            new Vector2(.39f,.06f),
            new Vector2(.46f,.12f));

        Button(
            "SystemIntelButton",
            system.transform,
            "INTEL",
            new Vector2(.47f,.06f),
            new Vector2(.54f,.12f));

        Button(
            "SystemDoctrineButton",
            system.transform,
            "DOCTRINE",
            new Vector2(.55f,.06f),
            new Vector2(.63f,.12f));

        Button(
            "SystemLogisticsButton",
            system.transform,
            "LOGISTICS",
            new Vector2(.64f,.06f),
            new Vector2(.71f,.12f));

        Panel(
            "HUD-202-07 AlertsPanel",
            system.transform,
            new Vector2(.76f,.03f),
            new Vector2(.98f,.15f));

        Panel(
            "HUD-202-08 BottomBar",
            system.transform,
            new Vector2(.02f,.02f),
            new Vector2(.27f,.15f));

        Button(
            "SystemBackButton",
            system.transform,
            "BACK",
            new Vector2(.03f,.05f),
            new Vector2(.08f,.12f));

        Button(
            "SystemCommandCenterButton",
            system.transform,
            "COMMAND",
            new Vector2(.09f,.05f),
            new Vector2(.17f,.12f));

        Button(
            "SystemSettingsButton",
            system.transform,
            "SETTINGS",
            new Vector2(.18f,.05f),
            new Vector2(.22f,.12f));

        Button(
            "SystemHelpButton",
            system.transform,
            "HELP",
            new Vector2(.23f,.05f),
            new Vector2(.27f,.12f));

        // ----------------------------------------------------
        // PLANET OVERVIEW HUD
        // ----------------------------------------------------

        GameObject planetHUD=Panel(
            "PLANET OVERVIEW HUD",
            hud.transform,
            new Vector2(0,0),
            new Vector2(1,1));

        planetHUD.SetActive(false);

        Panel(
            "PLANET TOP BAR",
            planetHUD.transform,
            new Vector2(0,.88f),
            new Vector2(1,1));

        Text(
            "PlanetName",
            planetHUD.transform,
            p.name.ToUpper(),
            new Vector2(.02f,.93f),
            new Vector2(.40f,.98f));

        Text(
            "PlanetID",
            planetHUD.transform,
            p.id,
            new Vector2(.40f,.93f),
            new Vector2(.52f,.98f));

        Text(
            "Faction",
            planetHUD.transform,
            p.faction,
            new Vector2(.53f,.93f),
            new Vector2(.78f,.98f));

        Panel(
            "PLANET LOCATION PANEL",
            planetHUD.transform,
            new Vector2(.02f,.20f),
            new Vector2(.30f,.86f));

        for(int i=0;i<4;i++)
        {
            Button(
                "Continent_"+(i+1),
                planetHUD.transform,
                p.continents[i].ToUpper(),
                new Vector2(.04f,.76f-i*.13f),
                new Vector2(.28f,.82f-i*.13f));
        }

        Panel(
            "PLANET INFORMATION PANEL",
            planetHUD.transform,
            new Vector2(.70f,.35f),
            new Vector2(.98f,.86f));

        Text(
            "Environment",
            planetHUD.transform,
            p.environment,
            new Vector2(.72f,.76f),
            new Vector2(.96f,.82f));

        Text(
            "WarfareFocus",
            planetHUD.transform,
            p.warfare,
            new Vector2(.72f,.64f),
            new Vector2(.96f,.74f));

        Text(
            "FactionControl",
            planetHUD.transform,
            p.faction,
            new Vector2(.72f,.53f),
            new Vector2(.96f,.61f));

        Panel(
            "PLANET ACTION PANEL",
            planetHUD.transform,
            new Vector2(.30f,.04f),
            new Vector2(.68f,.16f));

        Button(
            "OpenGalaxyMap",
            planetHUD.transform,
            "GALAXY",
            new Vector2(.31f,.07f),
            new Vector2(.38f,.13f));

        Button(
            "OpenStarSystem",
            planetHUD.transform,
            "SYSTEM",
            new Vector2(.39f,.07f),
            new Vector2(.46f,.13f));

        Button(
            "OpenIntel",
            planetHUD.transform,
            "INTEL",
            new Vector2(.47f,.07f),
            new Vector2(.54f,.13f));

        Button(
            "OpenOperations",
            planetHUD.transform,
            "OPERATIONS",
            new Vector2(.55f,.07f),
            new Vector2(.63f,.13f));

        Button(
            "OpenDeployment",
            planetHUD.transform,
            "DEPLOY",
            new Vector2(.64f,.07f),
            new Vector2(.71f,.13f));

        Button(
            "BackToSystem",
            planetHUD.transform,
            "BACK",
            new Vector2(.03f,.04f),
            new Vector2(.12f,.12f));

        Button(
            "ClosePlanetOverview",
            planetHUD.transform,
            "CLOSE",
            new Vector2(.88f,.04f),
            new Vector2(.97f,.12f));
    }

    // ========================================================
    // UI HELPERS
    // ========================================================

    private static GameObject Panel(
        string name,
        Transform parent,
        Vector2 min,
        Vector2 max)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        go.transform.SetParent(
            parent,
            false);

        RectTransform rt=
            go.GetComponent<RectTransform>();

        rt.anchorMin=min;
        rt.anchorMax=max;
        rt.offsetMin=Vector2.zero;
        rt.offsetMax=Vector2.zero;

        go.GetComponent<Image>().raycastTarget=false;

        return go;
    }

    private static GameObject Button(
        string name,
        Transform parent,
        string label,
        Vector2 min,
        Vector2 max)
    {
        GameObject go=
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        go.transform.SetParent(
            parent,
            false);

        RectTransform rt=
            go.GetComponent<RectTransform>();

        rt.anchorMin=min;
        rt.anchorMax=max;
        rt.offsetMin=Vector2.zero;
        rt.offsetMax=Vector2.zero;

        Image image =
            go.GetComponent<Image>();

        if(image==null)
            image=go.AddComponent<Image>();

        image.raycastTarget=true;

        GameObject textObject =
            new GameObject(
                "LABEL",
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        textObject.transform.SetParent(
            go.transform,
            false);

        TextMeshProUGUI text =
            textObject.GetComponent<TextMeshProUGUI>();

        if(text==null)
            throw new System.Exception(
                "Failed to create TextMeshProUGUI for button: " + name);

        text.text = label;
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 14;
        text.color = Color.white;
        text.raycastTarget = false;

        RectTransform tr =
            textObject.GetComponent<RectTransform>();

        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = Vector2.zero;
        tr.offsetMax = Vector2.zero;

        return go;
    }

    private static GameObject Text(
        string name,
        Transform parent,
        string value,
        Vector2 min,
        Vector2 max)
    {
        GameObject go=
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Text));

        go.transform.SetParent(
            parent,
            false);

        RectTransform rt=
            go.GetComponent<RectTransform>();

        rt.anchorMin=min;
        rt.anchorMax=max;
        rt.offsetMin=Vector2.zero;
        rt.offsetMax=Vector2.zero;

        Text text=
            go.GetComponent<Text>();

        text.text=value;
        text.alignment=
            TextAnchor.MiddleCenter;
        text.fontSize=13;
        text.font=
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");
        text.color=Color.white;

        return go;
    }

    private static GameObject GO(
        string name,
        Transform parent)
    {
        GameObject go=
            new GameObject(name);

        go.transform.SetParent(
            parent,
            false);

        return go;
    }

    private static GameObject Primitive(
        PrimitiveType type,
        string name,
        Transform parent)
    {
        GameObject go=
            GameObject.CreatePrimitive(type);

        go.name=name;
        go.transform.SetParent(
            parent,
            false);

        return go;
    }

    private static void Material(
        GameObject go,
        Color color,
        float metallic,
        float smoothness)
    {
        Renderer r=
            go.GetComponent<Renderer>();

        if(r==null)
            return;

        Shader shader=
            Shader.Find(
                "Universal Render Pipeline/Lit");

        if(shader==null)
            shader=Shader.Find("Standard");

        Material m=
            new Material(shader);

        m.color=color;
        m.SetFloat("_Metallic",metallic);
        m.SetFloat("_Smoothness",smoothness);

        r.sharedMaterial=m;
    }

    private static void RemoveCollider(GameObject go)
    {
        Collider c=
            go.GetComponent<Collider>();

        if(c!=null)
            UnityEngine.Object.DestroyImmediate(c);
    }

    private static Vector3 PlanetPoint(
        float radius,
        float latitude,
        float longitude)
    {
        float lat=
            latitude*Mathf.Deg2Rad;

        float lon=
            longitude*Mathf.Deg2Rad;

        return new Vector3(
            radius*Mathf.Cos(lat)*Mathf.Cos(lon),
            radius*Mathf.Sin(lat),
            radius*Mathf.Cos(lat)*Mathf.Sin(lon));
    }

    private static void EnsureFolder(
        string folder)
    {
        string[] parts=
            folder.Split('/');

        string current=
            parts[0];

        for(int i=1;i<parts.Length;i++)
        {
            string next=
                current+"/"+parts[i];

            if(!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(
                    current,
                    parts[i]);
            }

            current=next;
        }
    }

    public class PlanetMetadata : MonoBehaviour
    {
        public string PlanetID;
        public string PlanetName;
        public string Environment;
        public string WarfareFocus;
        public string FactionControl;

        public List<string> Continents=
            new List<string>();

        public List<string> Regions=
            new List<string>();

        public List<string> Biomes=
            new List<string>();

        public List<string> Islands=
            new List<string>();
    }

    public class LocationMetadata : MonoBehaviour
    {
        public string Continent;
        public string Region;
        public string Biome;
        public string Island;
    }
}









