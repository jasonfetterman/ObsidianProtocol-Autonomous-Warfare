# Canyon Run

Canyon Run (Canyon / Highway / Mobility, Ambushes, Route Control)

## Executive Summary


Canyon Run is a highâ€‘speed canyonâ€‘highway battlefield built for mobilityâ€‘focused combat, ambush tactics, route control, and autonomous maneuvering through narrow vertical terrain. The environment features winding canyon highways, cliffside overhangs, collapsed tunnels, dry riverbeds, abandoned checkpoints, and elevated ridgelines. The map emphasizes Obsidian Protocolâ€™s autonomous route â€‘selection logic, sensorâ€‘fusion under dust and heat distortion, commsâ€‘relay warfare, and logistics under fastâ€‘moving highway conditions. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of canyon â€‘highway terrain. Key features include ambush corridors, highâ€‘speed traversal lanes, cliffside overwatch, and chokeâ€‘controlled mobility warfare.

## Design Goals & Gameplay


Mobilityâ€‘Driven Combat
Canyon Run prioritizes fast movement, convoy operations, and rapid flanking. Highway lanes allow highâ€‘speed traversal; canyon walls restrict lateral movement.

Ambush & Route Control

Narrow passes, blind corners, and cliff overhangs create ideal ambush zones. Controlling key segments of the highway determines reinforcement flow.

Vertical Overwatch & Canyon Tactics

Ridges and cliffs provide overwatch positions for snipers, artillery, and drones. Verticality shapes sightlines and engagement ranges.

Autonomy & Routeâ€‘Selection Logic

Autonomous units choose between highway lanes, cliffside trails, dry riverbeds, and tunnel bypasses. Units adapt to blocked roads, collapsed tunnels, and shifting ambush threats.

Sensor & Information Warfare

Dust storms, heat shimmer, canyon occlusion, and thermal plumes degrade sensors. Relay towers on ridges restore clarity. Destroying relays creates blind pockets across the canyon.

Logistics & Resources
Fuel depots, checkpoints, and roadside maintenance yards serve as resupply nodes. Convoys must navigate exposed roads and avoid ambush â€‘prone canyon passes.

Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between fast armor, ambush specialists, or longâ€‘range overwatch units. Blue deploys from the southwest canyon highway gate; Red deploys from the northeast ridgeâ€‘top command.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² canyonâ€‘highway zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations


Blue HQ (Highway Gate Command) â€” (0,0), 150Ã—150â€¯m Armored vehicles, drone pads, convoy staging.
Red HQ (Ridgeâ€‘Top Command Spire) â€” (1000,1000), 150Ã—150â€¯m Elevated bunker with radar mast, AA emplacements, and longâ€‘range sensors.
Central Canyon Highway â€” (500,450), 300Ã—300â€¯m Highâ€‘speed traversal zone; long sightlines, ambush corners, dust plumes.
Checkpoint Depot â€” (250,800), 120Ã—120â€¯m Barricades, fuel tanks, repair bays, interior flanking routes.
Ridge Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch, sniper nests, drone launch pads.
Ambush Corridor â€” (0,450 â†’ 400,900) Primary chokepoint; narrow, exposed, ideal for ambushes.
Dry Riverbed Route â€” (300,0 â†’ 450,350) Safer but slow; unstable terrain and limited cover.
Ridge Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Ridge Relay B â€” (150,650), +260â€¯m Comms relay hub and overwatch control node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Ambush Corridor

Major chokepoint; controlling it determines convoy movement.

Ridge Network
Extreme longâ€‘range sightlines; ideal for overwatch and artillery.

Canyon Highway
Highâ€‘speed engagements; dust plumes affect accuracy.

Dry Riverbed Route

Predictable but safe; vulnerable to ridge overwatch.

## Sightlines

Ridge Relay A provides longâ€‘range sensor coverage (~350â€¯m). Dust storms and heat shimmer create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW highway gate + convoy staging.
Red Deployment: NE ridgeâ€‘top command + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Canyons, ridges, highways, dry riverbeds, checkpoints.

## Vegetation


Sparse shrubs, desert grass, small trees.

## Buildings & Props


Military: AA guns, radar dishes, bunkers. Industrial: Checkpoints, generators, pipelines. Logistics: Trucks, crates, fuel drums, drones.

## Effects


Dust storms, heat shimmer, mirage distortion, wind gusts.

## Audio


Wind, engines, distant artillery, radio chatter.

## LOD & Budget


Asset CategCooruynt Tri/Item  Total Tris
600k
Canyon/Ridge Terr3ain200k     320k
750k
Industrial/Checkpo4ints80k    120k
500k
Buildings/Military15 50k      ~2.29M

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

1. Prototype Layout â€” Month 1 Blockâ€‘out canyon highway, ridges, checkpoints, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, mobility autonomy behaviors, ambush logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (dust storms, heat shimmer).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify mobility routing, ambushâ€‘aware navigation, hazard avoidance.

## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under dust and heat distortion.

## Performance


â‰¥60â€¯Hz AR render on target device.

## QA Checklist


Navigation, mobility logic, deployment budget enforcement, anchor drift, sensor accuracy.
