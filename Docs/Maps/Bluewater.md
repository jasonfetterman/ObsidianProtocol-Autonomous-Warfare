# Bluewater

Bluewater (Open Ocean / Primarily Naval Warfare)

## Executive Summary


Bluewater is a vast openâ€‘ocean battlefield built for pure naval warfare, longâ€‘range engagements, carrier operations, submarine combat, and autonomous fleet coordination. The terrain is dominated by deep water, rolling swells, scattered reefs, thermal vents, and a few remote support platforms. The map emphasizes Obsidian Protocolâ€™s autonomous naval routing, sensorâ€‘fusion across sonar and radar, commsâ€‘relay warfare, and logistics under wideâ€‘open maritime conditions. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of open ocean. Key features include longâ€‘range naval duels, carrier air operations, submarine stealth zones, and strategic control of deepâ€‘water lanes.

## Design Goals & Gameplay


Pure Naval Combat
Bluewater prioritizes shipâ€‘toâ€‘ship warfare: missile duels, torpedo lanes, longâ€‘range gunnery, and carrierâ€‘based air support. No land combat except on small support platforms.
Carrier & Air Operations
Aircraft carriers launch drones, VTOLs, and strike craft. Airspace corridors above the ocean define recon, interception, and strike patterns.
Submarine Warfare & Stealth
Thermal vents, deep trenches, and sonarâ€‘scatter zones create stealth pockets. Submarines excel in ambushes and longâ€‘range torpedo strikes.
Autonomy & Fleet Coordination
Autonomous units coordinate across surface, air, and subsurface domains. Fleets adapt to shifting sonar conditions, radar interference, and missile trajectories.
Sensor & Information Warfare
Sea fog, humidity, thermal distortion, sonar scatter, and magnetic interference degrade sensors. Relay buoys restore clarity. Destroying relays creates blind pockets across the ocean.
Logistics & Resources
Fuel barges, offshore platforms, and carrier decks serve as resupply nodes. Convoys must navigate open water and avoid ambushâ€‘prone deepâ€‘water lanes.
Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between carrier dominance, submarine superiority, or missileâ€‘heavy surface fleets. Blue deploys from the southwest carrier group; Red deploys from the northeast strike flotilla.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² openâ€‘ocean zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System

1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations

Blue HQ (Carrier Group Bravo) â€” (0,0), 150Ã—150â€¯m Carrier deck, drone pads, missile destroyers.
Red HQ (Strike Flotilla Command) â€” (1000,1000), 150Ã—150â€¯m Cruisers, radar ships, submarine tenders.
Central Deepâ€‘Water Lane â€” (500,450), 350Ã—300â€¯m Longâ€‘range naval combat zone; ideal for missile duels.
Offshore Support Platform â€” (250,800), 120Ã—120â€¯m Fuel barges, repair cranes, resupply docks.
Thermal Vent Field â€” (800,300), 200Ã—200â€¯m Sonar distortion, submarine stealth routes.
Openâ€‘Ocean Corridor â€” (0,450 â†’ 400,900) Primary naval chokepoint; long sightlines and minimal cover.
Surface Patrol Route â€” (300,0 â†’ 450,350) Safer but predictable; vulnerable to submarine ambush.
Relay Buoy A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Relay Buoy B â€” (150,650), +260â€¯m Comms relay hub and sonar control node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Openâ€‘Ocean Corridor
Major naval chokepoint; controlling it determines fleet movement.
Thermal Vent Field
Sonar distortion; ideal for submarine ambushes.
Deepâ€‘Water Lane
Longâ€‘range missile and gunnery engagements.
Surface Patrol Route
Predictable but safe; vulnerable to subsurface attacks.
## Sightlines

Relay Buoy A provides longâ€‘range sensor coverage (~400â€¯m). Sea fog and humidity create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW carrier group + air staging.
Red Deployment: NE strike flotilla + radar ships.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain

Open ocean, deepâ€‘water lanes, thermal vents, offshore platforms.
## Vegetation

None â€” maritime zone only.
## Buildings & Props

Military: Radar ships, AA guns, sonar arrays, carriers. Industrial: Platforms, cranes, fuel barges. Logistics: Ships, cargo crates, drones.
## Effects

Sea spray, fog, humidity haze, wave impacts, thermal distortion.
## Audio

Waves, wind, ship engines, sonar pings, radio chatter.

## LOD & Budget


Asset CategCooryunt Tri/Item Total Tris

Ocean/Terrain 3 200k    600k

Naval/Industrial 4 80k  320k

Ships/Military 15 50k   750k

Platforms/Paths 4 30k   120k

Props     50 10k        500k

Total  â€”        â€”       ~2.29M

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

1. Prototype Layout â€” Month 1 Blockâ€‘out ocean lanes, platforms, vent field, AR anchor test.
2. Core Systems â€” Month 2 NavMesh (sea), naval autonomy, airâ€‘sea coordination.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (sea spray, fog, thermal distortion).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify naval routing, airâ€‘sea coordination, submarine stealth logic.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, sonar interference.
## Performance

â‰¥60â€¯Hz AR render on target device.
## QA Checklist

Navigation, naval logic, deployment budget enforcement, anchor drift, sensor accuracy.
