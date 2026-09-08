# Dustline

Dustline (Desert / Military) Map Design

## Executive Summary


Dustline is a wideâ€‘open desertâ€‘military battlefield built for highâ€‘mobility warfare, longâ€‘range engagements, and rapid reconnaissance. The terrain features rolling dunes, hardened military outposts, abandoned airstrips, dry canyons, and exposed open flats. The map emphasizes Obsidian Protocolâ€™s autonomous maneuvering, sensorâ€‘driven recon, commsâ€‘relay warfare, and logistics stretched across open terrain. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of desert military territory. Key features include mobile warfare corridors, elevated ridges for sensors, open flats for vehicle combat, and critical military infrastructure.

## Design Goals & Gameplay


Mobile Desert Warfare

Dustline prioritizes fast vehicle movement, longâ€‘range shooting lanes, and rapid repositioning. Open terrain favors armor, recon vehicles, and drone support.
Autonomy & Maneuver Logic

Autonomous AI selects optimal movement routes across dunes, avoids exposed flats during enemy artillery fire, and uses ridges for recon vantage.
## Sensor & Information Warfare


Dust storms, heat shimmer, and open terrain distort sensors. Relay towers on ridges restore clarity. Destroying relays creates massive fogâ€‘ofâ€‘war zones.
## Logistics & Resources


Supply convoys must cross exposed roads vulnerable to ambush. Controlling the Airstrip and Military Depot provides major resupply advantages.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between heavy armor or fast recon units. Blue deploys from the southwest dune base; Red deploys from the northeast military ridge.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² desert military zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Dune Base) â€” (0,0), 120Ã—120â€¯m Reinforced duneâ€‘side base with armored vehicles.

Red HQ (Military Ridge) â€” (1000,1000), 120Ã—120â€¯m Elevated ridge with radar mast and hardened bunkers.

Central Flats â€” (500,500), 300Ã—300â€¯m Open desert floor; ideal for mobile vehicle combat.

Military Depot â€” (250,800), 80Ã—80â€¯m Supply crates, fuel tanks, armored trucks.

Abandoned Airstrip â€” (800,300), 150Ã—150â€¯m Runway, hangars, radar dishes, drone pads.

Dry Canyon Route â€” (0,450 â†’ 400,900) Natural flanking corridor with partial cover.
Ridge A â€” (600,150), +260â€¯m Longâ€‘range sensor vantage point.

Ridge B â€” (150,650), +240â€¯m Comms relay tower and recon hub.
Mobile Warfare Corridor â€” (300,0 â†’ 450,350) Fastâ€‘movement path for vehicles.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Dry Canyon Route

Provides partial cover; key flanking path.
Airstrip Hangar Zone

Closeâ€‘range combat inside hangars; longâ€‘range outside.
Central Flats

Extreme longâ€‘range sightlines; armor and artillery dominate.
## Sightlines


Ridge A provides longâ€‘range sensor coverage (~350â€¯m). Dust storms create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW dune base + rear rally point.

Red Deployment: NE military ridge + rear rally point.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Sand dunes, rocky ridges, dry canyons, open flats.
## Vegetation


Sparse shrubs, dry grass patches.
## Buildings & Props


Military: Hangars, bunkers, radar dishes, watchtowers. Industrial: Fuel tanks, generators, pipelines. Logistics: Trucks, crates, fuel drums, recon drones.
## Effects


Dust storms, heat shimmer, smoke plumes, wind gusts.
## Audio


Wind, distant machinery, radio chatter, sand movement.

## LOD & Budget


Asset CategCooruynt Tri/Item Total Tris

Cliffs/Ridges   3 200k              600k

Dunes/Rocks     4 80k               320k

Buildings/Military15 50k            750k

Roads/Paths     4 30k               120k

Vegetation     150 2k               300k

Props          50 10k               500k

Total       â€”     â€”                 ~2.59M

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


1. Prototype Layout â€” Month 1 Blockâ€‘out dunes, ridges, airstrip, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, recon logic, autonomy behaviors.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (dust storms, heat shimmer).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify recon behavior, longâ€‘range targeting, pathfinding.
## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, cover usage, deployment budget enforcement, anchor drift, sensor accuracy.

