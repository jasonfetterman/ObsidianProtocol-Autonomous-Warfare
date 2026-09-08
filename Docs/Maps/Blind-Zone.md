# Blind Zone

Signal Lost (Remote Facility / Communications Degradation & Electronic Warfare)

## Executive Summary


Signal Lost is a remote communicationsâ€‘facility battlefield built for electronic warfare, comms degradation, autonomous counterâ€‘EW behavior, and precision operations across isolated terrain. The environment features satellite dishes, microwave towers, buried fiber lines, hardened relay bunkers, diesel generators, snow â€‘scoured ridges or dry scrub flats (depending on season), and longâ€‘range sensor arrays. The map emphasizes Obsidian Protocolâ€™s autonomous EWâ€‘aware routing, sensorâ€‘fusion under jamming and interference, commsâ€‘relay warfare, and logistics across a remote, infrastructureâ€‘dependent battlespace. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of remote â€‘facility terrain. Key features include EW zones, blackout corridors, relayâ€‘tower control, and commsâ€‘dependent maneuver warfare.

## Design Goals & Gameplay


Communications Degradation Warfare

Signal Lost prioritizes jamming, spoofing, interference, blackout zones, and commsâ€‘dependent tactics. Units operate under partial or total signal loss.

Electronic Warfare (EW) Dominance

Players deploy EW vehicles, drone jammers, signal scramblers, and counterâ€‘EW units. Destroying or capturing relay towers shifts the entire tactical landscape.
Autonomous EWâ€‘Aware Routing

Autonomous units choose routes based on signal strength, interference pockets, terrain occlusion, and fallback relay availability.
Remoteâ€‘Facility Combat

Longâ€‘range fire, drone reconnaissance, armored pushes, and stealth infiltration revolve around controlling the facilityâ€™s communication backbone.

Logistics & Resources

Fuel caches, generator stations, and buried cable hubs serve as resupply nodes. Convoys must navigate blackout corridors and avoid EWâ€‘driven ambush zones.

Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between EW dominance, stealth infiltration, or signal â€‘resistant armor. Blue deploys from the southwest serviceâ€‘road checkpoint; Red deploys from the northeast relayâ€‘spire command.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² remoteâ€‘facility zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations


Blue HQ (Serviceâ€‘Road Checkpoint) â€” (0,0), 150Ã—150â€¯m Armored vehicles, EW trucks, drone pads.

Red HQ (Relayâ€‘Spire Command) â€” (1000,1000), 150Ã—150â€¯m Elevated bunker with radar mast, EW arrays, AA emplacements.

Central Relay Basin â€” (500,450), 300Ã—300â€¯m Relay towers, interference pockets, blackout zones, longâ€‘range EW duels.

Generator Station Cluster â€” (250,800), 120Ã—120â€¯m Diesel generators, transformers, interior flanking routes.

Sensor Array Ridge â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch, signal triangulation, counterâ€‘EW vantage points.

Blackout Corridor â€” (0,450 â†’ 400,900) Primary EW chokepoint; jamming fields, interference pockets, false signals.

Service Road Route â€” (300,0 â†’ 450,350) Safer but predictable; vulnerable to EWâ€‘driven ambushes.

Relay Tower A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.

Relay Tower B â€” (150,650), +260â€¯m Comms relay hub and EWâ€‘control node.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Blackout Corridor

Major chokepoint; filled with jamming fields and false signals.

Sensor Array Ridge

Extreme longâ€‘range sightlines; ideal for counterâ€‘EW and recon triangulation.

Relay Basin

Longâ€‘range EW engagements distorted by interference and signal loss.

Service Road Route

Predictable but safe; vulnerable to EWâ€‘driven ambushes.

## Sightlines


Relay Tower A provides longâ€‘range sensor coverage (~350â€¯m). Interference and jamming create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW serviceâ€‘road checkpoint + EW vehicles.

Red Deployment: NE relayâ€‘spire command + AA emplacements.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Ridges, flats, relay towers, generator stations, service roads.

## Vegetation


Sparse shrubs, dry grass, or snow patches depending on climate.

## Buildings & Props


Military: EW trucks, AA guns, radar dishes, bunkers. Industrial: Generators, transformers, pipelines, relay towers. Logistics: Trucks, crates, fuel drums, drones.

## Effects


Signal distortion, interference waves, smoke, dust, fog.

## Audio


Wind, generator hum, static bursts, radio chatter.

## LOD & Budget


Asset CategoCroyunt Tri/Item Total Tris

Facility/Ridge Terrain3 200k        600k

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


1. Prototype Layout â€” Month 1 Blockâ€‘out relay basin, ridges, generator stations, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, EW autonomy behaviors, interference logic.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (interference waves, static bursts).

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
