# No Mans Grid

Blind Zone (Dense Terrain / Sensor Denial & Information Warfare)

## Executive Summary


Blind Zone is a denseâ€‘terrain electronicâ€‘warfare battlefield built for sensor denial, information manipulation, autonomous counterâ€‘recon behavior, and closeâ€‘range tactical deception. The environment features tangled vegetation, jagged rock formations, narrow ravines, abandoned bunkers, buried cabling, fog pockets, and natural EMâ€‘disruptive minerals. The map emphasizes Obsidian Protocolâ€™s autonomous sensorâ€‘degraded routing, EWâ€‘aware decisionâ€‘making, commsâ€‘relay warfare, and logistics across terrain specifically engineered to break line â€‘ofâ€‘sight and scramble sensors. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of dense, sensorâ€‘hostile terrain. Key features include blackout pockets, deception corridors, false â€‘signal zones, and informationâ€‘warfareâ€‘driven engagements.

## Design Goals & Gameplay


Sensor Denial Warfare

Blind Zone prioritizes jamming, occlusion, interference, false signatures, and sensor blackout pockets. Units operate under partial or total sensor loss.

Information Manipulation & Deception

Players deploy EW drones, falseâ€‘signal emitters, spoof beacons, and decoy units. Destroying or capturing relay nodes reshapes the entire tactical landscape.
Autonomous Counterâ€‘EW Routing

Autonomous units choose routes based on signal strength, interference pockets, terrain occlusion, and fallback relay availability. Units adapt to misleading data and prioritize verification.
Denseâ€‘Terrain Combat

Closeâ€‘range engagements dominate. Vegetation, rock formations, and fog create natural stealth zones and unpredictable movement corridors.

Logistics & Resources

Fuel caches, bunker stations, and buried cable hubs serve as resupply nodes. Convoys must navigate blackout corridors and avoid EWâ€‘driven ambush zones.

Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between EW dominance, stealth infiltration, or sensorâ€‘resistant armor. Blue deploys from the southwest ravine gate; Red deploys from the northeast bunkerâ€‘spire command.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² denseâ€‘terrain zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations


Blue HQ (Ravine Gate Command) â€” (0,0), 150Ã—150â€¯m Armored vehicles, EW trucks, drone pads.

Red HQ (Bunkerâ€‘Spire Command) â€” (1000,1000), 150Ã—150â€¯m Elevated bunker with radar mast, EW arrays, AA emplacements.

Central Blind Basin â€” (500,450), 300Ã—300â€¯m Dense vegetation, fog pockets, interference zones, closeâ€‘range EW duels.

Cable Hub Cluster â€” (250,800), 120Ã—120â€¯m Buried fiber lines, generators, interior flanking routes.

Ridgeâ€‘Shadow Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch, signal triangulation, counterâ€‘EW vantage points.

Blackout Corridor â€” (0,450 â†’ 400,900) Primary EW chokepoint; jamming fields, interference pockets, false signals.

Hidden Trail Route â€” (300,0 â†’ 450,350) Safer but slow; rock hazards and vegetation cover.

Relay Node A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.

Relay Node B â€” (150,650), +260â€¯m Comms relay hub and EWâ€‘control node.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Blackout Corridor

Major chokepoint; filled with jamming fields and false signals.
Ridgeâ€‘Shadow Network

Extreme longâ€‘range sightlines; ideal for counterâ€‘EW and recon triangulation.

Blind Basin

Longâ€‘range EW engagements distorted by interference and sensor loss.

Hidden Trail Route

Predictable but safe; vulnerable to ridge overwatch.

## Sightlines


Relay Node A provides longâ€‘range sensor coverage (~300â€¯m). Interference and fog create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW ravine gate + EW vehicles.

Red Deployment: NE bunkerâ€‘spire command + AA emplacements.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Ridges, ravines, dense vegetation, bunkers, cable hubs.

## Vegetation


Dense trees, shrubs, ferns, underbrush.

## Buildings & Props


Military: EW trucks, AA guns, radar dishes, bunkers. Industrial: Generators, transformers, pipelines, relay nodes. Logistics: Trucks, crates, fuel drums, drones.

## Effects


Signal distortion, interference waves, fog, dust, static bursts.

## Audio


Wind, generator hum, static bursts, radio chatter.

## LOD & Budget


Asset CategoCroyunt Tri/Item Total Tris

Dense Terrain   3 200k              600k

Industrial/EW   4 80k               320k

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


1. Prototype Layout â€” Month 1 Blockâ€‘out blind basin, ridges, cable hubs, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, EW autonomy behaviors, interference logic.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (interference waves, fog pockets).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify EWâ€‘aware navigation, interferenceâ€‘zone avoidance, fallback routing.

## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under jamming.

## Performance


â‰¥60â€¯Hz AR render on target device.

## QA Checklist


Navigation, EW logic, deployment budget enforcement, anchor drift, sensor accuracy.
