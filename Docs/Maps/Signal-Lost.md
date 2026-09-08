# Signal Lost

No Manâ€™s Grid (Open Military Range / Pure Tactical Experimentation)

## Executive Summary


No Manâ€™s Grid is a wideâ€‘open military testing range built for pure tactical experimentation, sandbox maneuvering, autonomous behavior testing, and unrestricted combinedâ€‘arms engagements. The terrain features flat test fields, modular hardpoints, firing lanes, sensor towers, mock urban blocks, drone pads, artillery pits, and configurable obstacle zones. The map emphasizes Obsidian Protocolâ€™s autonomous decision â€‘making, routeâ€‘selection logic, sensorâ€‘fusion under controlled conditions, and logistics across a perfectly readable, intentionally unthematic battlefield. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of open test terrain. Key features include maximum â€‘clarity sightlines, configurable objectives, unrestricted movement, and pure tactical sandbox behavior.

## Design Goals & Gameplay


Pure Tactical Sandbox

No Manâ€™s Grid is designed for experimentation, not narrative. Every lane, block, and hardpoint exists to test tactics, autonomy, and combined â€‘arms coordination.

Maximum Clarity & Predictability

Flat terrain, clean geometry, and modular obstacles ensure predictable movement and sightlines. Ideal for testing AI logic, weapon ranges, and recon patterns.

Configurable Engagement Zones

Players can treat the map as a blank canvas: longâ€‘range duels, closeâ€‘quarters tests, drone recon trials, armor pushes, or mixedâ€‘domain experiments.

Autonomy & Behavior Testing

Autonomous units choose routes based on pure logic: cover density, elevation microâ€‘changes, sensor clarity, and threat vectors. No environmental noise.

Sensor & Information Warfare

Minimal clutter means sensors operate at peak efficiency. Relay towers allow controlled degradation testsâ€”destroying them simulates fogâ€‘ofâ€‘war conditions.

Logistics & Resources

Fuel depots, ammo pits, and modular supply crates serve as resupply nodes. Convoys navigate clean, predictable roads ideal for testing escort logic.

Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between artillery tests, recon trials, armor pushes, or mixed â€‘domain experiments. Blue deploys from the southwest staging grid; Red deploys from the northeast command block.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² open test range (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations


Blue HQ (Staging Grid Alpha) â€” (0,0), 150Ã—150â€¯m Armor pads, drone launch zones, modular supply crates.

Red HQ (Command Block Omega) â€” (1000,1000), 150Ã—150â€¯m Control tower, radar mast, AA emplacements.

Central Test Basin â€” (500,450), 300Ã—300â€¯m Flat open terrain; maximum sightlines; ideal for longâ€‘range tests.

Modular Hardpoint Cluster â€” (250,800), 120Ã—120â€¯m Reconfigurable cover blocks, firing positions, interior test routes.

Sensor Tower Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground vantage points; perfect for recon and LOS testing.

Experimentation Corridor â€” (0,450 â†’ 400,900) Primary sandbox lane; configurable obstacles and firing lanes.

Service Road Route â€” (300,0 â†’ 450,350) Predictable movement path; ideal for convoy and escort tests.

Relay Tower A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.

Relay Tower B â€” (150,650), +260â€¯m Comms relay hub and controlled sensorâ€‘degradation node.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Experimentation Corridor

Major test lane; ideal for controlled ambush, range, and movement experiments.

Sensor Tower Network

Extreme longâ€‘range sightlines; perfect for recon and targeting logic.

Test Basin

Maximumâ€‘range engagements; heat shimmer and dust optional via toggled VFX.

Service Road Route

Predictable but safe; ideal for escort and convoy logic.

## Sightlines


Relay Tower A provides longâ€‘range sensor coverage (~400â€¯m). Minimal clutter ensures nearâ€‘perfect LOS unless manually degraded.

## Deployment Zones


Blue Deployment: SW staging grid + armor/drone pads.

Red Deployment: NE command block + AA emplacements.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Flat plains, modular obstacles, test blocks, sensor towers.

## Vegetation


Minimalâ€”short grass and sparse shrubs for clarity.

## Buildings & Props


Military: AA guns, radar dishes, bunkers. Industrial/Test: Hardpoints, towers, generators. Logistics: Trucks, crates, fuel drums, drones.

## Effects


Optional dust haze, heat shimmer, controlled smoke plumes.

## Audio


Wind, distant machinery, radio chatter, drone hum.

## LOD & Budget


Asset CategoCroyunt Tri/Item Total Tris

Plains/Test Terrain 3 200k          600k

Industrial/Test Blocks4 80k         320k

Buildings/Military 15 50k           750k

Roads/Paths     4 30k               120k

Props          50 10k               500k

Total    â€”         â€”                ~2.29M

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


1. Prototype Layout â€” Month 1 Blockâ€‘out test basin, hardpoints, sensor towers, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, sandbox autonomy behaviors, range logic.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (optional dust, heat shimmer).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify sandbox navigation, longâ€‘range targeting, hazardâ€‘free pathfinding.

## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war simulation, LOS checks under controlled conditions.

## Performance


â‰¥60â€¯Hz AR render on target device.

## QA Checklist


Navigation, sandbox logic, deployment budget enforcement, anchor drift, sensor accuracy.
