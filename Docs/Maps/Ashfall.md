# Ashfall

Ashfall (Volcanic / Industrial) Map Design

## Executive Summary


Ashfall is a hostile volcanicâ€‘industrial battlefield defined by unstable terrain, low visibility, and constant environmental hazards. Lava flows, ash storms, fissures, and geothermal machinery create a dynamic, highâ€‘risk combat zone. The map emphasizes Obsidian Protocolâ€™s autonomous decisionâ€‘making, sensor degradation, commsâ€‘relay warfare, and logistics under extreme environmental pressure. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of volcanic terrain. Key features include active lava channels, industrial extraction rigs, geothermal plants, hardened bunkers, and critical chokepoints formed by collapsed rock and molten rivers.

## Design Goals & Gameplay


Volcanic Hazard Combat

Ashfall focuses on infantry and lightâ€‘vehicle combat in unstable terrain. Lava flows block paths, ash reduces visibility, and eruptions create temporary hazards. Units must adapt quickly.
Autonomy & Hazard Avoidance

Autonomous AI chooses safe routes around fissures, avoids lava surges, and repositions when ash storms reduce sensor range. Units adapt without micromanagement.
## Sensor & Information Warfare


Volcanic heat, smoke, and ash distort radar, thermal, and acoustic sensors. Relay towers on hardened ridges restore clarity. Destroying relays plunges entire sectors into fogâ€‘ofâ€‘war.
## Logistics & Resources


Geothermal plants and extraction rigs serve as resource nodes. Supply convoys must navigate unstable roads and avoid lava flows. Controlling the Refinery and Geothermal Plant grants major resupply advantages.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between heavy hazardâ€‘resistant units or fast recon squads. Blue deploys from the southwest hardened bunker; Red deploys from the northeast industrial ridge.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² volcanic district (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Hardened Bunker) â€” (0,0), 120Ã—120â€¯m Reinforced bunker with hazardâ€‘resistant staging yard.
Red HQ (Industrial Ridge) â€” (1000,1000), 120Ã—120â€¯m Highâ€‘ground industrial platform with comms tower.

Central Geothermal Plant â€” (500,450), 200Ã—200â€¯m Turbines, heat exchangers, steam vents, molten channels.
Extraction Rig Complex â€” (300,250), 150Ã—150â€¯m Drills, conveyors, oreâ€‘processing machinery.

Volcanic Refinery â€” (800,300), 100Ã—100â€¯m Smelters, cooling towers, hardened pipes.

Lava Channels â€” (0,600 â†’ 400,900) Active molten river with two hardened crossings at (150,700) and (350,650).

Fissure Network â€” (400,0 â†’ 600,350) Unstable cracked terrain; hazard zone.

Ridge A â€” (650,150), +280â€¯m Sensor vantage point.

Ridge B â€” (150,650), +240â€¯m Comms relay tower.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Lava Crossings

Two hardened bridges; controlling them prevents flanking.
Fissure Network

Unstable terrain; closeâ€‘range combat and hazard avoidance.
Steam Vent Fields

Visibility drops to near zero during vent surges.
## Sightlines


Ridge A provides longâ€‘range sensor coverage (~250â€¯m). Ash storms create heavy fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW hardened bunker + rear rally point.

Red Deployment: NE industrial ridge + rear rally point.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Volcanic rock, lava flows, fissures, ash fields, hardened industrial platforms.
## Vegetation


None; occasional dead trees and scorched shrubs.
## Buildings & Props


Industrial: Turbines, smelters, pipes, extraction rigs. Military: Hardened bunkers, watchtowers, comms masts. Logistics: Trucks, crates, fuel drums, hazardâ€‘resistant containers.
## Effects


Lava glow, ash storms, smoke plumes, steam vents, sparks.
## Audio


Eruptions, rumbling earth, machinery hum, radio chatter.

## LOD & Budget


Asset CategCooruynt Tri/Item Total Tris

Mountains/Cliffs 3 200k             600k

Lava/Rock Meshes 4 80k              320k

Buildings/Industria1l5 50k          750k

Bridges/Platforms 4 30k             120k

Props           50 10k              500k

Vegetation (scorch5e0d) 2k          100k

Total    â€”        â€”                 â‰ˆ2.39M

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


1. Prototype Layout â€” Month 1 Blockâ€‘out lava channels, fissures, industrial zones, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, hazard avoidance logic, autonomy behaviors.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (lava glow, ash storms).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation, hazard simulation.

## Testing Plan


## Autonomy


Verify hazardâ€‘avoidance logic, pathfinding around lava/fissures.
## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, hazard response, deployment budget enforcement, anchor drift, sensor accuracy.

