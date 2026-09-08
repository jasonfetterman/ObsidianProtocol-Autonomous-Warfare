# Mirage

Mirage (Desert / Military / Deception & Information Warfare)

## Executive Summary


Mirage is a military desert battlefield built for deception, misinformation, sensor manipulation, and autonomous counterâ€‘recon operations. The terrain features heatâ€‘warped flats, dune fields, military outposts, radar stations, decoy emplacements, false infrastructure, buried cables, and canyonâ€‘edge observation posts. The map emphasizes Obsidian Protocolâ€™s autonomous deception logic, sensorâ€‘fusion under mirage distortion, commsâ€‘relay spoofing, and logistics under extreme heat and visibility anomalies. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of desertâ€‘military terrain. Key features include decoy zones, false sensor signatures, longâ€‘range mirage corridors, and informationâ€‘warfareâ€‘driven engagements.

## Design Goals & Gameplay


Deceptionâ€‘Driven Warfare

Mirage prioritizes false signals, decoy units, spoofed radar returns, holographic projections, and misinformation traps. Players manipulate enemy perception more than terrain.
Information Warfare & Sensor Manipulation

Heat shimmer, mirage distortion, dust haze, and thermal plumes degrade sensors. Specialized units generate false signatures, jam comms, or create phantom targets.
Autonomy & Counterâ€‘Recon Logic

Autonomous units choose routes based on sensor clarity, deception zones, false positives, and enemy spoofing attempts. Units adapt to misleading data and prioritize verification.
Desert Military Operations
Longâ€‘range fire, drone reconnaissance, armored pushes, and radarâ€‘guided strikes dominate. Outposts and radar stations serve as both real and decoy objectives.
Logistics & Resources
Fuel depots, comms bunkers, and desert roads serve as resupply nodes. Convoys must navigate exposed flats and avoid ambushâ€‘prone deception corridors.
Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between deception units, longâ€‘range firepower, or sensorâ€‘resistant armor. Blue deploys from the southwest duneâ€‘base staging area; Red deploys from the northeast radarâ€‘spire command.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² desertâ€‘military zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations

Blue HQ (Duneâ€‘Base Command) â€” (0,0), 150Ã—150â€¯m Armored vehicles, drone pads, deceptionâ€‘unit staging.
Red HQ (Radarâ€‘Spire Command) â€” (1000,1000), 150Ã—150â€¯m Elevated bunker with radar mast, AA emplacements, and sensor arrays.
Central Mirage Flats â€” (500,450), 300Ã—300â€¯m Heat shimmer, false signatures, longâ€‘range fire lanes, deception traps.
Comms Bunker Cluster â€” (250,800), 120Ã—120â€¯m Servers, generators, decoy transmitters, interior flanking routes.
Canyon Observation Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch, sensor triangulation, counterâ€‘recon vantage points.
Deception Corridor â€” (0,450 â†’ 400,900) Primary misinformation zone; false radar returns and phantom units.
Desert Road Route â€” (300,0 â†’ 450,350) Safer but predictable; vulnerable to spoofed ambushes.
Radar Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Radar Relay B â€” (150,650), +260â€¯m Comms relay hub and deceptionâ€‘control node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Deception Corridor

Major chokepoint; filled with false signals and spoofed targets.

Canyon Observation Network
Extreme longâ€‘range sightlines; ideal for counterâ€‘recon and sensor triangulation.
Mirage Flats
Longâ€‘range engagements distorted by heat shimmer and false signatures.
Desert Road Route
Predictable but safe; vulnerable to deceptionâ€‘driven ambushes.
## Sightlines

Radar Relay A provides longâ€‘range sensor coverage (~400â€¯m). Heat distortion and dust haze create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW duneâ€‘base staging + deception units.
Red Deployment: NE radarâ€‘spire command + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Desert flats, dunes, canyons, roads, radar stations, comms bunkers.
## Vegetation


Sparse shrubs, desert grass.
## Buildings & Props


Military: AA guns, radar dishes, bunkers, decoy towers. Industrial: Generators, pipelines, comms arrays. Logistics: Trucks, crates, fuel drums, drones.
## Effects


Heat shimmer, dust storms, mirage distortion, false holographic signatures.
## Audio


Wind, distant machinery, radio chatter, drone hum.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Desert Terrain 3         80k       320k
50k       750k
Industrial/Military 4    30k       120k
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

1. Prototype Layout â€” Month 1 Blockâ€‘out mirage flats, canyon network, comms bunkers, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, deception autonomy behaviors, sensorâ€‘spoofing logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (heat shimmer, dust haze, holographic decoys).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify deceptionâ€‘aware navigation, counterâ€‘recon behavior, hazard avoidance.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under mirage distortion.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, deception logic, deployment budget enforcement, anchor drift, sensor accuracy.
