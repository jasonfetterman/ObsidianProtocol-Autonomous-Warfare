# Ghost Coast

Ghost Coast (Coastal / Ground + Naval + Air Operations) Map Design

## Executive Summary


Ghost Coast is a hybrid coastal battlefield designed for synchronized ground, naval, and air operations. The terrain features rugged cliffs, sandy beaches, offshore platforms, naval docks, submerged ruins, and elevated coastal roads. The map emphasizes Obsidian Protocolâ€™s autonomous multiâ€‘domain tactics, sensorâ€‘fusion warfare, commsâ€‘relay networks, and logistics across land, sea, and air. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of coastal territory. Key features include naval landing zones, cliffside artillery positions, offshore industrial rigs, underwater hazards, and multiâ€‘route amphibious assault paths.

## Design Goals & Gameplay


Multiâ€‘Domain Combat (Land + Sea + Air)

Ghost Coast supports simultaneous ground vehicles, infantry, naval craft, drones, and aircraft. Players coordinate amphibious landings, airstrikes, and coastal pushes.
Autonomy & Multiâ€‘Route Decisionâ€‘Making

Autonomous AI chooses between beach landings, cliff ascents, underwater approaches, or aerial flanking routes. Units adapt to tides, waves, and shifting visibility.
## Sensor & Information Warfare


Sea mist, wave spray, and fog distort sensors. Radar stations on cliffs restore clarity. Destroying relays creates fogâ€‘ofâ€‘war pockets across sea and land.
## Logistics & Resources


Naval docks, fuel depots, and offshore platforms serve as resupply nodes. Long supply lines across open water and exposed beaches force careful convoy protection.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between naval landing craft, air support, or heavy ground armor. Blue deploys from the southwest beachhead; Red deploys from the northeast cliff fortress.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² coastal zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Beachhead Command) â€” (0,0), 140Ã—140â€¯m Amphibious landing zone with naval craft and armored vehicles.

Red HQ (Cliff Fortress) â€” (1000,1000), 140Ã—140â€¯m Fortified cliffside base with radar mast and AA emplacements.

Central Coastal Town â€” (500,450), 200Ã—200â€¯m Shops, docks, piers, narrow streets, partial ruins.

Naval Dockyard â€” (250,800), 120Ã—120â€¯m Fuel tanks, cranes, cargo containers, patrol boats.

Offshore Industrial Rig â€” (800,300), 150Ã—150â€¯m Pipelines, generators, helipad, underwater supports.

Underwater Ruins â€” (400,0 â†’ 600,350) Submerged structures; stealth approach route.

Cliff Road Network â€” (0,450 â†’ 400,900) Elevated road with artillery positions.

Airstrip Plateau â€” (650,150), +260â€¯m Runway, hangars, drone pads.

Radar Ridge â€” (150,650), +240â€¯m Comms relay tower and sensor hub.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Beach Landing Zone

Open but exposed; naval craft vulnerable during landing.
Cliff Road Network

Narrow elevated path; ideal for artillery ambushes.
Coastal Town Streets

Closeâ€‘range combat; interior routes and alley flanks.
Offshore Rig

Highâ€‘ground for air units; naval units must approach carefully.
## Sightlines


Radar Ridge provides longâ€‘range sensor coverage (~350â€¯m). Sea mist and fog create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW beachhead + naval staging area.

Red Deployment: NE cliff fortress + AA emplacements.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Cliffs, beaches, dunes, submerged ruins, coastal roads.
## Vegetation


Sparse palms, shrubs, dune grass.
## Buildings & Props


Military: Bunkers, AA guns, radar dishes, watchtowers. Industrial: Cranes, pipelines, generators, offshore rig structures. Logistics: Boats, trucks, crates, fuel drums, drones.
## Effects


Sea mist, fog, wave spray, smoke plumes, wind gusts.
## Audio


Waves, wind, distant machinery, radio chatter, seagulls.

## LOD & Budget


Asset CategCooruynt Tri/Item Total Tris

Cliffs/Coastal Rock 3 200k          600k

Beaches/Dunes 4 80k                 320k

Buildings/Industria1l5 50k          750k

Roads/Docks     4 30k               120k

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


1. Prototype Layout â€” Month 1 Blockâ€‘out cliffs, beaches, offshore rig, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, naval pathfinding, autonomy behaviors.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (sea mist, fog, wave spray).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify naval landing logic, aerial flanking, underwater pathfinding.
## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, multiâ€‘domain coordination, deployment budget enforcement, anchor drift, sensor accuracy.

