# The Yard

The Yard (Rail / Industrial / Logistics, Infrastructure, Armored Warfare)

## Executive Summary


The Yard is a railâ€‘industrial battlefield built for logistics warfare, armored maneuvering, infrastructure control, and autonomous force coordination. The terrain features rail spurs, cargo yards, switching stations, warehouses, fuel depots, loading cranes, elevated service roads, and industrial choke corridors. The map emphasizes Obsidian Protocolâ€™s autonomous logistics logic, sensorâ€‘network routing, commsâ€‘relay warfare, and armored operations under dense industrial constraints. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of railâ€‘industrial terrain. Key features include multiâ€‘track maneuver zones, warehouse interior combat, railâ€‘line chokepoints, and strategic control of cargo infrastructure.

## Design Goals & Gameplay


Logistics & Infrastructure Warfare

The Yard prioritizes control of rail lines, cargo hubs, switching stations, and fuel depots. Logistics dominance directly affects reinforcement and resupply speed.
Armored Industrial Combat

Long industrial fire lanes favor armor; tight warehouse interiors favor infantry and autonomous squads. Rail cars create dynamic cover and movement obstacles.
Autonomy & Routeâ€‘Selection Logic
Autonomous units choose between railâ€‘side paths, cargo lanes, warehouse interiors, elevated service roads, or offâ€‘road industrial bypasses. Units adapt to blocked tracks and shifting logistics priorities.
## Sensor & Information Warfare


Smoke, dust, heat plumes, and industrial clutter distort sensors. Relay towers on cranes and rooftops restore clarity. Destroying relays creates blind pockets across the yard.
## Logistics & Resources

Fuel depots, switching stations, and cargo terminals serve as resupply nodes. Convoys must navigate exposed rail corridors and avoid ambushâ€‘prone warehouse zones.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between armored pushes, logistics disruption units, or autonomous infantry. Blue deploys from the southwest switching station; Red deploys from the northeast cargo command tower.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² industrial zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Switching Station Command) â€” (0,0), 150Ã—150â€¯m Rail control building, armored staging, drone pads.
Red HQ (Cargo Command Tower) â€” (1000,1000), 150Ã—150â€¯m Elevated industrial command node with radar mast and AA emplacements.
Central Rail Yard â€” (500,450), 300Ã—300â€¯m Multiâ€‘track zone, cargo cars, cranes, long fire lanes.
Fuel & Logistics Depot â€” (250,800), 120Ã—120â€¯m Fuel tanks, pipelines, trucks, hazardous chokepoint.
Warehouse Row â€” (800,300), 200Ã—200â€¯m Interior combat, loading docks, flanking corridors.
Rail Line Corridor â€” (0,450 â†’ 400,900) Critical chokepoint; long straight track with minimal cover.
Service Road Route â€” (300,0 â†’ 450,350) Safer ground route with partial cover.
Crane Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Rooftop Relay B â€” (150,650), +260â€¯m Comms relay tower and recon hub.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Rail Line Corridor
Extreme longâ€‘range sightlines; ideal for armor and artillery.

Warehouse Row
Interior flanking routes; closeâ€‘range engagements.
Fuel Depot

Hazardous chokepoint; explosions and fire risk.
Central Rail Yard

Dynamic cover from cargo cars; ideal for armored maneuvering.
## Sightlines

Crane Relay A provides longâ€‘range sensor coverage (~350â€¯m). Smoke and industrial heat create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW switching station + armored staging.
Red Deployment: NE cargo command tower + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Rail lines, cargo yards, warehouses, fuel depots, service roads.
## Vegetation


Sparse shrubs, industrial planters, grass patches.
## Buildings & Props


Military: AA guns, radar dishes, watchtowers. Industrial: Cranes, pipelines, generators, warehouses. Logistics: Trucks, cargo containers, fuel drums, drones.
## Effects


Smoke, sparks, dust, heat shimmer, exhaust plumes.
## Audio


Rail movement, machinery, radio chatter, wind.

## LOD & Budget


Asset CategCooruynt Tri/Item  Total Tris
600k
Rail Lines/Yard 3 200k        320k
750k
Warehouses/Indus4tria8l0k     120k
240k
Buildings/Urban 15 50k        500k
~2.53M
Roads/Paths 4 30k

Vegetation 120 2k

Props     50 10k

Total â€”            â€”

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

1. Prototype Layout â€” Month 1 Blockâ€‘out rail yard, warehouses, fuel depot, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, armored autonomy behaviors, logistics logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (smoke, sparks, heat shimmer).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify logistics logic, armored pathing, warehouse navigation.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, armored behavior, deployment budget enforcement, anchor drift, sensor accuracy.

