# Rainfall

Rainfall (Jungle / Tropical / Weather, Concealment, Sensor Degradation)

## Executive Summary


Rainfall is a dense tropical jungle battlefield built for weatherâ€‘driven combat, concealment, sensor degradation, and autonomous navigation through vegetationâ€‘heavy terrain. The environment features monsoon downpours, thick canopy cover, muddy trails, river gorges, waterfalls, cliffs, abandoned research huts, and overgrown ruins. The map emphasizes Obsidian Protocolâ€™s autonomous jungle â€‘navigation logic, sensorâ€‘fusion under heavy rainfall, commsâ€‘relay warfare, and logistics under extreme humidity and visibility loss. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of tropical jungle terrain. Key features include monsoon â€‘driven fogâ€‘ofâ€‘war, concealment pockets, unstable mud routes, and sensorâ€‘degraded engagements.

## Design Goals & Gameplay


Weatherâ€‘Driven Combat
Rainfall prioritizes combat during heavy monsoon rain. Visibility drops, mud slows movement, and waterlogged terrain reshapes cover and traversal.

Concealment & Ambush Warfare
Dense foliage, canopy shadows, and rainâ€‘driven fog create natural stealth zones. Ambushes, flanking, and sudden closeâ€‘range engagements dominate.

Autonomy & Jungle Navigation
Autonomous units choose routes based on vegetation density, mud stability, canopy cover, and sensor clarity. Units avoid flooded ravines unless strategically necessary.

Sensor & Information Warfare
Rain, fog, humidity, thermal distortion, and vegetation clutter degrade sensors. Relay towers on cliffs restore clarity. Destroying relays creates blind pockets across the jungle.

Logistics & Resources
Fuel caches, research huts, and river docks serve as resupply nodes. Convoys must navigate muddy trails and avoid ambushâ€‘prone canopy corridors.

Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between stealth infantry, jungle drones, or heavy armor adapted for wet terrain. Blue deploys from the southwest river dock; Red deploys from the northeast cliffside command hut.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² jungle zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System

1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations

Blue HQ (River Dock Camp) â€” (0,0), 150Ã—150â€¯m Boats, crates, jungle staging area.
Red HQ (Cliffside Command Hut) â€” (1000,1000), 150Ã—150â€¯m Elevated bunker with radar mast and thermal relays.
Central Jungle Basin â€” (500,450), 300Ã—300â€¯m Dense canopy, heavy rainfall, fog pockets, longâ€‘range sightlines broken by vegetation.
Research Hut Cluster â€” (250,800), 120Ã—120â€¯m Abandoned labs, generators, interior flanking routes.
Canopy Ridge Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch, treeâ€‘top concealment, stealth movement.
Monsoon Corridor â€” (0,450 â†’ 400,900) Severe rainfall zone; extreme sensor degradation.
Muddy Trail Route â€” (300,0 â†’ 450,350) Safer but slow; mud hazards and vegetation cover.
Cliff Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Cliff Relay B â€” (150,650), +260â€¯m Comms relay hub and weatherâ€‘monitoring node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Monsoon Corridor
Severe visibility loss; ideal for stealth ambushes and sensor deception.

Canopy Ridge Network
Interior flanking routes; unpredictable cover due to foliage.

Jungle Basin
Longâ€‘range engagements broken by vegetation; rain reduces accuracy.

Muddy Trail Route
Predictable but safe; vulnerable to overwatch from ridges.

## Sightlines

Cliff Relay A provides longâ€‘range sensor coverage (~300â€¯m). Rain and fog create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW river dock + jungle staging.
Red Deployment: NE cliffside command hut + thermal defenses.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain

Jungle canopy, cliffs, rivers, mud trails, research huts.

## Vegetation

Dense trees, vines, ferns, underbrush.

## Buildings & Props

Military: Radar dishes, thermal relays, bunkers. Rural: Huts, bridges, wooden structures. Logistics: Trucks, crates, fuel drums, boats, drones.

## Effects

Rainstorms, fog, humidity haze, mud splashes, canopy shadows.

## Audio

Rainfall, insects, distant machinery, river flow, radio chatter.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Jungle Terrain 3         80k       320k
50k       750k
Rural/Industrial 4       30k       120k
10k       500k
Buildings/Rural 15       â€”         ~2.29M

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

1. Prototype Layout â€” Month 1 Blockâ€‘out jungle basin, ridges, huts, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, jungle autonomy behaviors, weather logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (rain, fog, mud).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify jungle navigation, weatherâ€‘aware pathfinding, hazard avoidance.

## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under monsoon conditions.

## Performance

â‰¥60â€¯Hz AR render on target device.

## QA Checklist

Navigation, weather logic, deployment budget enforcement, anchor drift, sensor accuracy.
