# Iron Valley

Iron Valley (Mountain / Mining) Map Design

## Executive Summary


Iron Valley is a harsh mountainousâ€‘mining battlefield designed for groundâ€‘focused warfare, logistics pressure, and brutal chokepoint engagements. The terrain features steep canyon walls, collapsed tunnels, active mining machinery, ore conveyors, and industrial refineries. The map supports Obsidian Protocolâ€™s autonomous decision â€‘making, sensorâ€‘driven tactics, commsâ€‘relay warfare, and logisticsâ€‘heavy gameplay loops. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of terrain. Key features include narrow canyon roads, ridge â€‘top sensor towers, mining pits, oreâ€‘processing facilities, and critical supply depots.

## Design Goals & Gameplay


Ground Warfare & Scale

Iron Valley emphasizes infantry and armor combat. The valley floor provides maneuver space, while canyon walls restrict movement and create natural ambush funnels. Mining roads and industrial yards allow heavy vehicles but limit air support due to dust, smoke, and machinery interference.

Autonomy & Flanking

Multiple routes â€” canyon passes, collapsed tunnels, conveyorâ€‘belt walkways, and ridgeâ€‘top paths â€” create nonlinear movement. Autonomous AI can choose ambush points, retreat paths, or flanking routes without micromanagement.

## Sensor & Information Warfare


Dust storms, machinery vibration, and oreâ€‘processing heat distort radar and thermal sensors. Ridgeâ€‘top relay towers provide clean sensor coverage. Destroying relays creates fogâ€‘ofâ€‘war pockets and comms dead zones.

## Logistics & Resources


Mining trucks, ore containers, and fuel depots serve as logistics nodes. Long supply lines through narrow roads force players to protect convoys. Controlling the refinery and fuel depot grants major resupply advantages.

## Deployment & Progress


A fixed deployment budget (~10,000 points) forces strategic tradeoffs: heavy armor through the canyon, or fast recon along ridge paths? Blue deploys from the west, Red from the east, with a neutral forward depot near the central mine.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Comfortable viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² battlefield (1:100 scale). Fits on a conference table.

## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; design assumes minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations


Location CoordinatesSize       Description

West Base ((B0l,u0e) HQ) 100Ã—100â€¯m Elevated staging yard with mining vehicles

East Base (R#e#d#H##Q#)## 100Ã—100â€¯m Cliffâ€‘side command post with comms tower

Central Mine-5C0o0m,4p0l0ex 180Ã—180â€¯m Excavation pit, conveyor belts, crushers

Ore Refinery-800,250 90Ã—90â€¯m Smelters, cooling towers, heavy pipes

Fuel Depot -250,800 60Ã—60â€¯m Fuel tanks, supply crates, truck bays

North Pass (400,1000 â†’â€”600,700) Narrow canyon, blocked LOS

South Tunn(e3l00,0 â†’ 40â€”0,400) Collapsed mine shaft, closeâ€‘quarters combat

River & Brid(g0e,5s00 â†’ 40â€”0,900) Two crossings at (150,600) and (350,500)

Peak A -600,100 +300â€¯m Sensor vantage point

Peak B -100,600 +250â€¯m Ridgeâ€‘top comms relay

Neutral Forw-a5r0d0,D9e5p0otâ€”  Drone resupply zone

## Chokepoints & Sightlines


North Pass

A tight canyon corridor with steep walls; LOS blocked except from ridgeâ€‘top units.

South Tunnel

Collapsed mining tunnel; extremely closeâ€‘range engagements.

Bridges

Two river crossings; controlling them prevents enemy flanking.

## Sightlines


Peak A provides longâ€‘range sensor coverage (~300â€¯m). Deep valleys create fogâ€‘ofâ€‘war pockets. Ridgeâ€‘top comms tower at East Base covers most of the map.

## Deployment Zones


Blue Deployment: West Base + rear rally point.

Red Deployment: East Base + rear rally point.

Neutral Forward Zone: Airfield at (500,950) for drone drops.

## Assets & Environment


## Terrain


Rocky cliffs, slag piles, mine shafts, conveyor belts, ore pits.

## Vegetation


Sparse pines, shrubs, grass patches (billboards).

## Buildings & Props


Industrial: Crushers, smelters, ore silos, pipelines.

Military: Watchtowers, antennas, radar dishes.

Logistics: Mining trucks, crates, barrels, forklifts.

Roads/Bridges: Canyon roads, metal truss bridges.

## Effects


Dust storms, smoke plumes, furnace glow, conveyor sparks.

## Audio


Wind gusts, machinery hum, ore crushers, radio chatter.

## LOD & Budget


Asset CategCooryunt Tri/Item Total Tris

Mountains/Cliffs 3 200k        600k

Slopes/Hills    4 80k          320k

Buildings      15 50k          750k

Bridges/Roads 4 30k            120k

Vegetation     150 2k          300k

Props          50 10k          500k

Total       â€”      â€”           2.59M

## Texture Budget


2048Â² for terrain/buildings, 1024Â² for props. DXT1/5 compression. Target â‰¤100â€¯MB.

## Unreal / Nreal Integration


## Engine & Plugins


Unreal ARTemplate + Nreal SDK (XREAL). ARKit/ARCore enabled.

## Rendering


Forward renderer, stationary lights, GPUâ€‘friendly lightmaps.

## Anchors


Single spatial anchor for entire map.

## Interaction


Phone pointer for pan/zoom; HUD as screenâ€‘space widgets.

## Multiplayer


Clientâ€‘server architecture; offload AI/physics to server.

## Implementation Plan & Testing


## Milestones


Milestone Timeframe Deliverables

1. PrototypeMLoanytohu1t Blockâ€‘out terrain, bases, mine pit, AR anchor test

2. Core SystMemosnth 2 NavMesh, pathfinding, autonomy behaviors

3. AR IntegrMatoionnth 3 Anchors, plane detection, basic interactivity

4. Content FMillonth 4â€“5 Final models, textures, lighting, audio

5. GameplayM&onPtohli6sâ€“h7 Objectives, balance, VFX (dust, sparks)

6. Testing &MQoAnth 8 Multiplayer stress test, sensor validation

## Testing Plan


## Autonomy


Verify AI intent logic (attack, hold, retreat).

## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.

## Performance


â‰¥60â€¯Hz AR render on target device.

## QA Checklist


Navigation, cover usage, deployment budget enforcement, anchor drift, sensor accuracy.

