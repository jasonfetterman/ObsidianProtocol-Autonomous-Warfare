# Tidal Reach

Tidal Reach (Island / Ocean / Naval Warfare & Amphibious Operations)

## Executive Summary


Tidal Reach is a tropical islandâ€“ocean hybrid battlefield built for naval warfare, amphibious landings, littoral combat, and autonomous multiâ€‘domain coordination. The terrain features coral shallows, deepâ€‘water channels, sandy beaches, mangrove swamps, cliffside fortifications, island villages, and offshore naval platforms. The map emphasizes Obsidian Protocolâ€™s autonomous amphibious routing, navalâ€‘air coordination, sensorâ€‘fusion across water and land, and logistics under dynamic tide conditions. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of islandâ€‘ocean terrain. Key features include landing beaches, naval chokepoints, airspace corridors, reef hazards, and multiâ€‘domain combat across sea, air, and land.

## Design Goals & Gameplay


Naval Warfare in Littoral Waters
Tidal Reach prioritizes shipâ€‘toâ€‘ship combat, torpedo lanes, reef navigation, and hullâ€‘hazard maneuvering. Coral shelves create dynamic cover and movement obstacles.
Amphibious Operations & Beach Landings

Players conduct coordinated landings using amphibious armor, drones, and infantry. Beaches, mangroves, and cliffs create layered defensive zones.
Air Operations & Reconnaissance
VTOL aircraft, drones, and longâ€‘range recon planes operate above the island chain. Airspace corridors between cliffs and offshore platforms define movement and targeting.
Autonomy & Multiâ€‘Domain Coordination

Autonomous units coordinate naval, air, and land forces simultaneously. Units adapt to shifting tides, fog pockets, and sonar interference.
Sensor & Information Warfare

Sea spray, humidity, thermal distortion, sonar scatter, and magnetic interference degrade sensors. Relay towers on cliffs restore clarity. Destroying relays creates blind pockets across the island chain.
Logistics & Resources
Fuel caches, island depots, and offshore platforms serve as resupply nodes. Convoys must navigate beach approaches and avoid ambushâ€‘prone mangrove corridors.
Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between naval dominance, air superiority, or amphibious mobility. Blue deploys from the southwest offshore staging platform; Red deploys from the northeast cliffside fortress.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² islandâ€‘ocean zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Offshore Staging Platform) â€” (0,0), 150Ã—150â€¯m Naval docks, drone pads, amphibious armor staging.
Red HQ (Cliffside Fortress Command) â€” (1000,1000), 150Ã—150â€¯m Hardened cliff bunker with radar mast, AA emplacements, and sonar arrays.
Central Tidal Channel â€” (500,450), 350Ã—250â€¯m Deepâ€‘water lane, coral shelves, naval combat zone.
Island Depot & Village â€” (250,800), 120Ã—120â€¯m Supply huts, docks, interior flanking routes.
Mangrove Swamp Network â€” (800,300), 200Ã—200â€¯m Dense vegetation, shallow water, stealth movement.
Landing Beach Corridor â€” (0,450 â†’ 400,900) Primary amphibious chokepoint; layered defenses and tideâ€‘driven hazards.
Cliffside Trail Route â€” (300,0 â†’ 450,350) Safer land route with partial cover.
Cliff Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Cliff Relay B â€” (150,650), +260â€¯m Comms relay hub and sonar control node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Landing Beach Corridor

Major amphibious chokepoint; controlling it determines land access.
Tidal Channel
Longâ€‘range naval and air engagements; dynamic coral cover.
Mangrove Swamp Network

Interior flanking routes; stealthy but slow.
Cliffside Trail

Predictable but safe; vulnerable to overwatch from cliffs.
## Sightlines

Cliff Relay A provides longâ€‘range sensor coverage (~350â€¯m). Sea spray and humidity create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW offshore platform + naval staging.
Red Deployment: NE cliffside fortress + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Beaches, cliffs, coral shelves, mangroves, offshore platforms.
## Vegetation


Palm trees, mangroves, shrubs, beach grass.
## Buildings & Props


Military: AA guns, radar dishes, sonar arrays, bunkers. Industrial: Docks, generators, pipelines. Logistics: Ships, cargo crates, fuel drums, drones.
## Effects


Sea spray, fog, humidity haze, wave impacts, thermal distortion.
## Audio


Waves, wind, ship engines, sonar pings, radio chatter.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Coral/Ice/Terrain 3      80k       320k
50k       750k
Naval/Industrial 4       30k       120k
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

1. Prototype Layout â€” Month 1 Blockâ€‘out strait, beaches, mangroves, AR anchor test.
2. Core Systems â€” Month 2 NavMesh (sea + land), amphibious autonomy, airâ€‘sea coordination.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (sea spray, fog, thermal distortion).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify naval routing, amphibious landings, airâ€‘sea coordination.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, sonar interference.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, amphibious logic, deployment budget enforcement, anchor drift, sensor accuracy.
