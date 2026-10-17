# Dead River

Dead River (Industrial River / Bridges, Water, Industry, Logistics)

## Executive Summary


Dead River is an industrial riverâ€‘zone battlefield built for bridge control, waterway dominance, logistics warfare, and autonomous maneuvering through dense industrial infrastructure. The terrain features cargo docks, steel bridges, refineries, pumping stations, rail spurs, container yards, floodgates, and polluted river channels. The map emphasizes Obsidian Protocolâ€™s autonomous logistics â€‘aware routing, sensorâ€‘fusion under industrial smoke and water reflection, commsâ€‘relay warfare, and multiâ€‘domain operations across land, water, and elevated bridge networks. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of industrial river terrain. Key features include bridge chokepoints, waterborne routes, industrial hazards, and logistics â€‘driven strategic play.

## Design Goals & Gameplay


Bridge Control & Chokepoint Warfare
Dead River prioritizes control of steel bridges, overpasses, and elevated walkways. Holding bridges determines movement between industrial sectors.

Waterway Operations
Boats, amphibious armor, and drones navigate polluted river channels, floodgates, and narrow industrial waterways.

Industrial Logistics Warfare
Container yards, rail spurs, and refineries create logistics hubs. Destroying or capturing them affects reinforcement flow and resource availability.
Autonomy & Infrastructureâ€‘Aware Routing
Autonomous units choose routes based on bridge stability, water depth, industrial hazards, and sensor clarity. Units adapt to blocked roads, collapsed walkways, and shifting river conditions.

Sensor & Information Warfare
Smoke, steam, water reflection, metallic clutter, and industrial noise degrade sensors. Relay towers on refinery rooftops restore clarity. Destroying relays creates blind pockets across the river zone.

Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between amphibious units, logistics â€‘focused armor, or industrialâ€‘sector infantry. Blue deploys from the southwest cargo dock; Red deploys from the northeast refinery command.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² industrial river zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System

1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations

Blue HQ (Cargo Dock Command) â€” (0,0), 150Ã—150â€¯m Boats, cranes, containers, amphibious staging.
Red HQ (Refinery Command Spire) â€” (1000,1000), 150Ã—150â€¯m Refinery tower with radar mast, AA emplacements, and sensor arrays.
Central Bridge Network â€” (500,450), 300Ã—300â€¯m Steel bridges, elevated walkways, chokepoints, longâ€‘range overwatch.
Industrial Depot Cluster â€” (250,800), 120Ã—120â€¯m Warehouses, rail spurs, generators, interior flanking routes.
Floodgate Sector â€” (800,300), 200Ã—200â€¯m Waterâ€‘control structures, narrow channels, ambush zones.
River Crossing Corridor â€” (0,450 â†’ 400,900) Primary chokepoint; bridges, water hazards, industrial clutter.
Service Road Route â€” (300,0 â†’ 450,350) Safer but predictable; vulnerable to overwatch from bridges.
Refinery Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Refinery Relay B â€” (150,650), +260â€¯m Comms relay hub and logisticsâ€‘monitoring node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


River Crossing Corridor
Major chokepoint; controlling bridges determines movement between sectors.

Floodgate Sector
Narrow water routes; ideal for ambushes and amphibious traps.

Bridge Network
Longâ€‘range engagements; elevation provides overwatch.

Service Road Route
Predictable but safe; vulnerable to elevated fire.

## Sightlines

Refinery Relay A provides longâ€‘range sensor coverage (~350â€¯m). Smoke and water reflection create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW cargo dock + amphibious staging.
Red Deployment: NE refinery command + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain

River channels, bridges, docks, refineries, rail yards, floodgates.

## Vegetation

Sparse industrial weeds, riverbank grass.

## Buildings & Props

Military: AA guns, radar dishes, bunkers. Industrial: Warehouses, cranes, pipelines, refineries. Logistics: Trucks, crates, fuel drums, boats, drones.

## Effects

Smoke, steam, water spray, sparks, industrial noise.

## Audio

Machinery, water flow, distant alarms, radio chatter.

## LOD & Budget


Asset CategCooruynt Tri/Item   Total Tris
600k
River/Industrial Te3rrai2n00k  320k
750k
Industrial/Logistics4 80k      120k
500k
Buildings/Industri1a5l 50k     ~2.29M

Roads/Paths 4 30k

Props    50 10k

Total â€”  â€”

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

1. Prototype Layout â€” Month 1 Blockâ€‘out river channels, bridges, depots, AR anchor test.
2. Core Systems â€” Month 2 NavMesh (land + water), logistics autonomy, industrial hazard logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (smoke, steam, water spray).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify amphibious routing, bridgeâ€‘aware navigation, hazard avoidance.

## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under industrial smoke.

## Performance

â‰¥60â€¯Hz AR render on target device.

## QA Checklist

Navigation, logistics logic, deployment budget enforcement, anchor drift, sensor accuracy.
