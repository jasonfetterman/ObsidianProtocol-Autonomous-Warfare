# The Corridor

The Corridor (Valley / Highway / Chokepoints & Convoy Warfare)

## Executive Summary


The Corridor is a valleyâ€‘highway battlefield built for convoy warfare, chokepoint control, longâ€‘range ambushes, and logistics disruption. The terrain features a narrow valley floor, elevated highway segments, tunnels, overpasses, fuel depots, roadside industrial zones, and steep rock walls that funnel movement into predictable lanes. The map emphasizes Obsidian Protocolâ€™s autonomous convoy logic, sensorâ€‘driven ambush detection, commsâ€‘relay networks, and logistics under extreme linear constraints. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of valley terrain. Key features include multiâ€‘tier highway combat, tunnel chokepoints, ambushâ€‘ready rock shelves, and strategic control of convoy routes.

## Design Goals & Gameplay


Convoy Warfare & Route Control

The Corridor prioritizes convoy escort, interception, and disruption. Highway segments create predictable movement paths ideal for ambushes and defensive setups.
Chokepointâ€‘Driven Combat

Tunnels, overpasses, and narrow valley bends create natural chokepoints. Controlling these determines the flow of the entire battle.
Autonomy & Routeâ€‘Selection Logic
Autonomous AI chooses between upper highway lanes, lower valley roads, tunnel bypasses, or offâ€‘road rock paths. Units adapt to blocked roads and ambush threats.
## Sensor & Information Warfare


Dust, exhaust, tunnel darkness, and rock occlusion distort sensors. Relay towers on cliffs restore clarity. Destroying relays creates blind zones along the highway.
## Logistics & Resources

Fuel depots, roadside industrial yards, and highway checkpoints serve as resupply nodes. Convoys must navigate exposed stretches and avoid ambushâ€‘prone bends.
## Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between convoy escorts, heavy armor, or ambushâ€‘specialized units. Blue deploys from the southwest valley checkpoint; Red deploys from the northeast elevated highway command.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² valley zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Valley Checkpoint) â€” (0,0), 140Ã—140â€¯m Roadblock, watchtower, convoy staging area.
Red HQ (Highway Command Overlook) â€” (1000,1000), 140Ã—140â€¯m Elevated command post with radar mast and AA emplacements.
Central Highway Spine â€” (500,450), 300Ã—200â€¯m Main combat zone; multiâ€‘lane elevated highway + lower valley road.
Fuel & Logistics Depot â€” (250,800), 100Ã—100â€¯m Fuel tanks, trucks, pipelines, cargo crates.
Tunnel Complex â€” (800,300), 150Ã—150â€¯m Two tunnels, ventilation shafts, interior ambush routes.
Valley Bend â€” (0,450 â†’ 400,900) Narrow rock corridor; extreme chokepoint.
Lower Road Route â€” (300,0 â†’ 450,350) Safer but slower convoy path with partial cover.
Cliff Shelf A â€” (650,150), +240â€¯m Ambush vantage point overlooking highway.
Cliff Shelf B â€” (150,650), +260â€¯m Comms relay tower and recon hub.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Valley Bend

The most critical chokepoint; perfect for ambushes and artillery traps.

Tunnel Complex
Interior routes allow stealth flanking and sudden closeâ€‘range engagements.
Highway Spine
Longâ€‘range sightlines; ideal for convoy escort or disruption.
Lower Road Route

Safer but predictable; vulnerable to cliffside overwatch.
## Sightlines

Cliff Shelf A provides longâ€‘range sensor coverage (~350â€¯m). Dust and tunnel darkness create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW valley checkpoint + convoy staging.
Red Deployment: NE highway command overlook + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Valley floor, cliffs, rock shelves, elevated highway, tunnels.
## Vegetation


Sparse shrubs, dry grass patches.
## Buildings & Props


Military: Watchtowers, bunkers, radar dishes, AA guns. Industrial: Fuel tanks, pipelines, generators, cargo yards. Logistics: Trucks, crates, fuel drums, convoy vehicles.
## Effects


Dust clouds, exhaust haze, tunnel fog, wind gusts.
## Audio


Engine noise, wind, distant artillery, radio chatter.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Cliffs/Rock Walls 3      80k       320k
50k       750k
Highway/Tunnels 4        30k       120k
2k        200k
Buildings/Industri1a5l   10k       500k
â€”         ~2.49M
Roads/Paths 4

Vegetation 100

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

1. Prototype Layout â€” Month 1 Blockâ€‘out highway, tunnels, valley bend, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, convoy logic, ambush detection, autonomy behaviors.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (dust, exhaust haze).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify convoy logic, ambush detection, tunnel navigation.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, chokepoint behavior, convoy pathing, deployment budget enforcement, anchor drift, sensor accuracy.

