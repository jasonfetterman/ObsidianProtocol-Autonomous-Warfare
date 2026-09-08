# Harbor Black

Harbor Black (Mega Port / Naval + Ground + Air)

## Executive Summary


Harbor Black is a megaâ€‘port combinedâ€‘arms battlefield built for naval warfare, ground combat, air superiority, logistics disruption, and autonomous multiâ€‘domain coordination. The terrain features massive cargo terminals, multiâ€‘level container stacks, ship berths, drydocks, cranes, refineries, elevated highways, rail spurs, and fortified control towers. The map emphasizes Obsidian Protocolâ€™s autonomous port â€‘infrastructure routing, sensorâ€‘fusion under industrial clutter, commsâ€‘relay warfare, and logisticsâ€‘driven strategic play across sea, land, and air. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of mega â€‘port terrain. Key features include naval berths, elevated overwatch, containerâ€‘maze ground combat, air corridors, and multiâ€‘domain chokepoints.

## Design Goals & Gameplay


Naval + Ground + Air Integration
Harbor Black prioritizes simultaneous multiâ€‘domain combat: shipâ€‘toâ€‘shore fire, drone reconnaissance, armored pushes through container yards, and air strikes from above.

Logistics Warfare

Cargo terminals, refineries, and rail yards serve as logistics hubs. Destroying or capturing them affects reinforcement flow and resource availability.

Containerâ€‘Maze Ground Combat

Stacked containers create dense cover, ambush lanes, interior corridors, and unpredictable sightlines.

Air Superiority & Vertical Control

Aircraft, VTOLs, and drones dominate the vertical dimension. Air corridors between cranes and control towers define recon, interception, and strike patterns.

Autonomy & Infrastructureâ€‘Aware Routing

Autonomous units choose routes based on container density, crane hazards, water depth, elevated road stability, and sensor clarity.

Sensor & Information Warfare

Smoke, steam, metallic clutter, water reflection, and industrial noise degrade sensors. Relay towers on control towers restore clarity. Destroying relays creates blind pockets across the port.

Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between naval dominance, air superiority, or groundâ€‘based logistics disruption. Blue deploys from the southwest cargo terminal; Red deploys from the northeast refinery command.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² megaâ€‘port zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations


Blue HQ (Cargo Terminal Command) â€” (0,0), 150Ã—150â€¯m Cranes, containers, amphibious staging, drone pads.
Red HQ (Refinery Command Spire) â€” (1000,1000), 150Ã—150â€¯m Refinery tower with radar mast, AA emplacements, and sensor arrays.
Central Berth & Drydock Zone â€” (500,450), 300Ã—300â€¯m Ship berths, drydocks, naval combat, amphibious landings.
Container Yard Cluster â€” (250,800), 120Ã—120â€¯m Stacked containers, interior corridors, ambush lanes.
Elevated Highway Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch, sniper nests, airâ€‘corridor control.
Harbor Crossing Corridor â€” (0,450 â†’ 400,900) Primary chokepoint; bridges, water hazards, industrial clutter.
Service Road Route â€” (300,0 â†’ 450,350) Safer but predictable; vulnerable to overwatch from cranes and towers.
Control Tower Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Control Tower Relay B â€” (150,650), +260â€¯m Comms relay hub and logisticsâ€‘monitoring node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Harbor Crossing Corridor

Major chokepoint; controlling bridges determines movement between port sectors.

Container Yard Cluster

Dense interior flanking routes; unpredictable cover and ambush potential.

Berth & Drydock Zone

Naval + ground + air engagements; water reflection affects accuracy.

Elevated Highway Network
Longâ€‘range overwatch; ideal for airâ€‘ground coordination.

## Sightlines

Control Tower Relay A provides longâ€‘range sensor coverage (~350â€¯m). Smoke and water reflection create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW cargo terminal + amphibious staging.
Red Deployment: NE refinery command + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Berths, docks, drydocks, container yards, refineries, elevated highways.

## Vegetation


Sparse industrial weeds, riverbank grass.

## Buildings & Props


Military: AA guns, radar dishes, bunkers. Industrial: Warehouses, cranes, pipelines, refineries. Logistics: Trucks, crates, fuel drums, boats, drones.

## Effects


Smoke, steam, water spray, sparks, industrial noise.

## Audio


Machinery, water flow, distant alarms, radio chatter.

## LOD & Budget


Asset CategCooruynt Tri/Item  Total Tris
600k
Port/Industrial Terr3ain200k  320k
750k
Industrial/Logistics4 80k     120k
500k
Buildings/Industri1a5l 50k    ~2.29M

Roads/Paths 4 30k

Props          50 10k

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

1. Prototype Layout â€” Month 1 Blockâ€‘out berths, container yards, elevated highways, AR anchor test.
2. Core Systems â€” Month 2 NavMesh (land + water), multiâ€‘domain autonomy, industrial hazard logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (smoke, steam, water spray).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify amphibious routing, containerâ€‘maze navigation, airâ€‘ground coordination.

## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under industrial smoke.

## Performance


â‰¥60â€¯Hz AR render on target device.

## QA Checklist

Navigation, multiâ€‘domain logic, deployment budget enforcement, anchor drift, sensor accuracy.
