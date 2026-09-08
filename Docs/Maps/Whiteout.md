# Whiteout

Whiteout (Arctic / Snow / Visibility, Thermal Sensors, Navigation)

## Executive Summary


Whiteout is a snowâ€‘choked Arctic battlefield built for visibilityâ€‘degraded combat, thermalâ€‘sensor warfare, and hazardous navigation. The terrain features blizzard corridors, ice dunes, frozen ravines, research shelters, buried comms towers, and windâ€‘carved ridges. The map emphasizes Obsidian Protocolâ€™s autonomous navigation logic under nearâ€‘zero visibility, thermalâ€‘sensor fusion, commsâ€‘relay warfare, and logistics under severe Arctic conditions. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of Arctic terrain. Key features include whiteout zones, thermalâ€‘blind pockets, unstable ice routes, and navigationâ€‘driven tactical decisionâ€‘making.

## Design Goals & Gameplay


Visibilityâ€‘Degraded Warfare

Whiteout prioritizes combat in nearâ€‘zero visibility. Blizzards, drifting snow, and ice fog reduce sightlines to a few meters. Units rely heavily on thermal sensors and autonomous pathfinding.
Thermalâ€‘Sensor Dominance

Heat signatures become the primary detection method. Terrain features like ice ridges, snow berms, and geothermal vents distort thermal readings, creating deceptive pockets.
Autonomy & Hazard Navigation

Autonomous units choose routes based on wind exposure, ice stability, thermal cover, and sensor clarity. Units avoid whiteout corridors unless strategically necessary.
Sensor & Information Warfare

Blizzards, ice fog, thermal distortion, and magnetic interference degrade sensors. Relay towers on ridges restore clarity. Destroying relays creates massive blind pockets across the snowfields.
Logistics & Resources

Fuel caches, heated shelters, and research stations serve as resupply nodes. Convoys must navigate unstable ice and avoid ambushâ€‘prone snow corridors.
Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between thermalâ€‘optimized infantry, Arctic drones, or heavy snowâ€‘capable armor. Blue deploys from the southwest snowfield camp; Red deploys from the northeast ridge shelter.

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


Blue HQ (Snowfield Camp) â€” (0,0), 150Ã—150â€¯m Snow berms, heated tents, drone pads, Arctic staging.

Red HQ (Ridge Shelter Command) â€” (1000,1000), 150Ã—150â€¯m Heated bunker with radar mast and thermal relays.
Central Whiteout Basin â€” (500,450), 300Ã—300â€¯m Blizzard epicenter; nearâ€‘zero visibility, unstable ice, thermal distortion.

Research Shelter Cluster â€” (250,800), 120Ã—120â€¯m Labs, generators, thermal shelters, interior combat.
Ice Ridge Network â€” (800,300), 200Ã—200â€¯m Windâ€‘carved ridges, snow tunnels, flanking routes.

Blizzard Corridor â€” (0,450 â†’ 400,900) Severe whiteout zone; extreme sensor degradation.

Frozen Trail Route â€” (300,0 â†’ 450,350) Safer but slow; ice cracks and wind exposure.
Thermal Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.

Thermal Relay B â€” (150,650), +260â€¯m Comms relay hub and geothermal control node.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Blizzard Corridor

Severe visibility loss; ideal for stealth ambushes and thermal deception.
Ice Ridge Network

Interior flanking routes; unpredictable cover due to drifting snow.
Whiteout Basin

Unstable terrain; longâ€‘range thermal engagements with environmental hazards.
Frozen Trail Route

Predictable but safe; vulnerable to overwatch from ridges.
## Sightlines


Thermal Relay A provides longâ€‘range sensor coverage (~300â€¯m). Blizzards and ice fog create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW snowfield camp + Arctic staging.

Red Deployment: NE ridge shelter + thermal defenses.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Ice ridges, snow dunes, frozen lakes, blizzard corridors, research shelters.
## Vegetation


None â€” Arctic tundra only.
## Buildings & Props


Military: Radar dishes, thermal relays, bunkers. Industrial: Generators, pipelines, research shelters. Logistics: Trucks, crates, fuel drums, drones.
## Effects


Snowstorms, ice fog, wind gusts, thermal distortion.
## Audio


Wind, cracking ice, distant machinery, radio chatter.

## LOD & Budget


Asset CategCooruynt Tri/Item Total Tris

Ice Ridges/Terrain 3 200k           600k

Shelters/Industrial 4 80k           320k

Buildings/Research15 50k            750k

Roads/Paths     4 30k               120k

Props        50 10k                 500k

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


1. Prototype Layout â€” Month 1 Blockâ€‘out whiteout basin, ridge network, research shelters, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, coldâ€‘weather autonomy behaviors, thermal logic.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (snowstorms, ice fog, thermal distortion).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify coldâ€‘weather navigation, thermalâ€‘sensor logic, hazard avoidance.
## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under blizzard conditions.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, thermal logic, deployment budget enforcement, anchor drift, sensor accuracy.
