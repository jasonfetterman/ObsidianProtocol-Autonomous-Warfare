# Outpost Zero

Outpost Zero (Remote Arctic / Extreme Environment, Limited Resources)

## Executive Summary


Outpost Zero is a harsh Arctic battlefield built for extremeâ€‘environment warfare, limitedâ€‘resource logistics, survivalâ€‘driven autonomy, and sensorâ€‘degraded combat. The terrain features frozen tundra, ice ridges, research domes, buried bunkers, geothermal vents, supply caches, and windâ€‘blasted plateaus. The map emphasizes Obsidian Protocolâ€™s autonomous coldâ€‘weather logic, resourceâ€‘rationing behaviors, sensorâ€‘fusion under blizzards, and logistics under severe environmental constraints. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of Arctic terrain. Key features include whiteout zones, limitedâ€‘resource supply nodes, frozen chokepoints, and survivalâ€‘driven tactical decisionâ€‘making.

## Design Goals & Gameplay


Extremeâ€‘Environment Warfare
Outpost Zero prioritizes coldâ€‘weather hazards: freezing temperatures, ice storms, low visibility, and equipment degradation. Units must adapt to environmental threats as much as enemy fire.
Limited Resources & Survival Logistics
Fuel, ammunition, and power are scarce. Controlling geothermal vents, supply caches, and research domes determines longâ€‘term survivability.
Autonomy & Survival Pathfinding

Autonomous units choose routes based on wind exposure, ice stability, thermal cover, and resource availability. Units avoid whiteout zones unless necessary.
Sensor & Information Warfare

Blizzards, ice fog, thermal distortion, and magnetic interference degrade sensors. Relay towers on ridges restore clarity. Destroying relays creates massive blind pockets across the tundra.
Logistics & Resources
Fuel caches, geothermal stations, and research domes serve as resupply nodes. Convoys must navigate frozen terrain and avoid ambushâ€‘prone ice corridors.
Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between survivalâ€‘optimized infantry, coldâ€‘weather drones, or heavy Arctic armor. Blue deploys from the southwest landing zone; Red deploys from the northeast geothermal command ridge.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² Arctic zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations

Blue HQ (Landing Zone Bravo) â€” (0,0), 150Ã—150â€¯m Snow berms, cargo crates, coldâ€‘weather staging area.
Red HQ (Geothermal Command Ridge) â€” (1000,1000), 150Ã—150â€¯m Heated command bunker with radar mast and thermal relays.
Central Ice Basin â€” (500,450), 300Ã—300â€¯m Frozen lake, unstable ice, low visibility, longâ€‘range fire lanes.
Research Dome Cluster â€” (250,800), 120Ã—120â€¯m Labs, generators, thermal shelters, interior combat.
Buried Bunker Network â€” (800,300), 200Ã—200â€¯m Underground tunnels, firing ports, hardened defensive positions.
Whiteout Corridor â€” (0,450 â†’ 400,900) Severe blizzard zone; extreme sensor degradation.
Frozen Trail Route â€” (300,0 â†’ 450,350) Safer but slow; ice cracks and wind exposure.
Thermal Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Thermal Relay B â€” (150,650), +260â€¯m Comms relay hub and geothermal control node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Whiteout Corridor
Severe visibility loss; ideal for stealth ambushes and survivalâ€‘driven tactics.

Buried Bunker Network

Interior flanking routes; extremely strong defensive positions.
Ice Basin
Unstable terrain; longâ€‘range engagements with environmental hazards.
Frozen Trail Route

Predictable but safe; vulnerable to overwatch from ridges.
## Sightlines

Thermal Relay A provides longâ€‘range sensor coverage (~300â€¯m). Blizzards and ice fog create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW landing zone + coldâ€‘weather staging.
Red Deployment: NE geothermal command ridge + thermal defenses.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Ice ridges, frozen lakes, snow berms, geothermal vents, buried bunkers.
## Vegetation


None â€” Arctic tundra only.
## Buildings & Props


Military: Radar dishes, thermal relays, bunkers. Industrial: Generators, pipelines, research domes. Logistics: Trucks, crates, fuel drums, drones.
## Effects


Snowstorms, ice fog, wind gusts, thermal distortion.
## Audio


Wind, cracking ice, distant machinery, radio chatter.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Ice Ridges/Terrain 3     80k       320k
50k       750k
Bunkers/Industrial4      30k       120k
10k       500k
Buildings/Researc1h5     â€”         ~2.29M

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

1. Prototype Layout â€” Month 1 Blockâ€‘out ice basin, bunker network, research domes, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, coldâ€‘weather autonomy behaviors, survival logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (snowstorms, ice fog, thermal distortion).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify coldâ€‘weather navigation, survival logic, hazard avoidance.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under blizzard conditions.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, survival logic, deployment budget enforcement, anchor drift, sensor accuracy.
