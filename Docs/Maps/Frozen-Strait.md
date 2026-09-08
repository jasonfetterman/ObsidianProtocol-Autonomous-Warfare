# Frozen Strait

Frozen Strait (Arctic / Naval / Naval + Air + Ice Operations)

## Executive Summary


Frozen Strait is a hybrid Arctic naval battlefield built for naval warfare, air operations, iceâ€‘field maneuvering, and autonomous multiâ€‘domain coordination. The terrain features fractured sea ice, narrow straits, iceberg clusters, frozen channels, coastal cliffs, submarine vents, and forward Arctic stations. The map emphasizes Obsidian Protocolâ€™s autonomous naval routing, airâ€‘sea coordination, sensorâ€‘fusion under Arctic interference, and logistics under unstable ice conditions. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of Arctic maritime terrain. Key features include iceâ€‘breaker routes, naval chokepoints, airspace corridors, submarine vents, and multiâ€‘domain combat across sea, air, and ice.

## Design Goals & Gameplay


Naval Warfare in Iceâ€‘Choked Waters
Frozen Strait prioritizes shipâ€‘toâ€‘ship combat, torpedo lanes, iceâ€‘breaker routes, and hullâ€‘hazard navigation. Ice floes create dynamic cover and movement obstacles.
Air Operations & Reconnaissance
VTOL aircraft, drones, and longâ€‘range recon planes operate above the strait. Airspace corridors between cliffs and icebergs define movement and targeting.
Iceâ€‘Field Maneuvering

Ground units traverse frozen channels, ice ridges, and unstable floes. Heavy armor risks breaking ice; light units excel in mobility.
Autonomy & Multiâ€‘Domain Coordination
Autonomous units coordinate naval, air, and iceâ€‘field forces simultaneously. Units adapt to shifting ice patterns, fog pockets, and sonar interference.
Sensor & Information Warfare

Sea fog, ice mist, thermal distortion, sonar scatter, and magnetic interference degrade sensors. Relay towers on cliffs restore clarity. Destroying relays creates blind pockets across the strait.
Logistics & Resources
Fuel caches, coastal stations, and submarine vents serve as resupply nodes. Convoys must navigate iceâ€‘breaker routes and avoid ambushâ€‘prone channels.
Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between naval dominance, air superiority, or iceâ€‘field mobility. Blue deploys from the southwest coastal station; Red deploys from the northeast cliffside command.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² Arctic maritime zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Coastal Station Bravo) â€” (0,0), 150Ã—150â€¯m Docks, fuel tanks, drone pads, Arctic staging.
Red HQ (Cliffside Command Spire) â€” (1000,1000), 150Ã—150â€¯m Heated bunker with radar mast, AA emplacements, and sonar arrays.
Central Frozen Strait â€” (500,450), 350Ã—250â€¯m Fractured ice, open water lanes, iceberg clusters, naval combat zone.
Submarine Vent Field â€” (250,800), 120Ã—120â€¯m Thermal vents, sonar distortion, underwater hazards.
Ice Ridge Network â€” (800,300), 200Ã—200â€¯m Windâ€‘carved ridges, frozen tunnels, flanking routes.
Iceâ€‘Breaker Channel â€” (0,450 â†’ 400,900) Primary naval chokepoint; narrow, hazardous, constantly shifting.
Frozen Shore Route â€” (300,0 â†’ 450,350) Safer ground route with partial cover.
Cliff Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Cliff Relay B â€” (150,650), +260â€¯m Comms relay hub and sonar control node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Iceâ€‘Breaker Channel

Major naval chokepoint; controlling it determines sea movement.
Submarine Vent Field

Thermal distortion disrupts sonar; ideal for submarine ambushes.
Frozen Strait Center
Longâ€‘range naval and air engagements; dynamic ice cover.
Ice Ridge Network

Interior flanking routes; unpredictable cover due to drifting snow and ice.
## Sightlines

Cliff Relay A provides longâ€‘range sensor coverage (~350â€¯m). Sea fog and ice mist create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW coastal station + naval staging.
Red Deployment: NE cliffside command spire + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Ice floes, frozen channels, cliffs, submarine vents, coastal stations.
## Vegetation


None â€” Arctic maritime zone.
## Buildings & Props


Military: AA guns, radar dishes, sonar arrays, bunkers. Industrial: Generators, pipelines, docks. Logistics: Ships, cargo crates, fuel drums, drones.
## Effects


Sea fog, ice mist, snowstorms, thermal distortion, wave spray.
## Audio


Wind, cracking ice, ship engines, sonar pings, radio chatter.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Ice/Terrain          3   80k       320k
50k       750k
Naval/Industrial 4       30k       120k
10k       500k
Buildings/Military15     â€”         ~2.29M

Roads/Paths 4

Props                50

Total â€”

## Texture Budget


2048Â² for terrain/buildings, 1024Â² for props. DXT1/5 compression. Target â‰¤100â€¯MB.

## Unreal / Nreal Integration


## Engine & Plugins


Unreal ARTemplate + Nreal SDK (XREAL). ARKit/ARCore enabled.
## Rendering


Forward renderer, stationary lights, baked GI, aggressive culling.

## Anchors


Single spatial anchor for entire map.
## Interaction

Phone pointer for pan/zoom; HUD as screenâ€‘space widgets.
## Multiplayer

Clientâ€‘server architecture; offload AI/physics to server.

## Implementation Plan & Testing


## Milestones

1. Prototype Layout â€” Month 1 Blockâ€‘out strait, ice ridges, submarine vents, AR anchor test.
2. Core Systems â€” Month 2 NavMesh (ice + land), naval autonomy, airâ€‘sea coordination.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (sea fog, ice mist, thermal distortion).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify naval routing, airâ€‘sea coordination, iceâ€‘field navigation.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, sonar interference.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, naval logic, deployment budget enforcement, anchor drift, sensor accuracy.
