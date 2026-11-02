# Breaker Bay

Breaker Bay (Port / Industrial) Map Design

## Executive Summary


Breaker Bay is a highâ€‘intensity portâ€‘industrial battlefield designed for naval invasion, logistics warfare, and urban combat. The terrain features massive cargo terminals, fortified seawalls, container yards, ship berths, industrial cranes, coastal roads, and dense portâ€‘adjacent housing blocks. The map emphasizes Obsidian Protocolâ€™s autonomous multiâ€‘domain tactics, sensorâ€‘fusion warfare, commsâ€‘relay networks, and logistics under heavy industrial congestion. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of port territory. Key features include naval landing zones, containerâ€‘maze combat, elevated crane platforms, industrial choke corridors, and multiâ€‘route amphibious assault paths.

## Design Goals & Gameplay


Naval Invasion & Coastal Assault
Breaker Bay supports coordinated naval landings, amphibious pushes, and shipâ€‘toâ€‘shore combat. Players must secure beachheads, docks, and industrial platforms.
Autonomy & Multiâ€‘Route Logistics
Autonomous AI chooses between pier landings, seawall breaches, containerâ€‘yard infiltration, or elevated crane traversal. Units adapt to shifting cargo layouts and blocked roads.
## Sensor & Information Warfare

Fog, sea spray, industrial smoke, and metal clutter distort sensors. Relay towers on cranes and rooftops restore clarity. Destroying relays creates fogâ€‘ofâ€‘war pockets across port and sea.
## Logistics & Resources


Cargo terminals, fuel depots, and ship berths serve as resupply nodes. Long supply lines across exposed docks and narrow industrial corridors force careful convoy protection.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between naval landing craft, heavy ground armor, or urban infantry. Blue deploys from the southwest landing pier; Red deploys from the northeast industrial command tower.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² port zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Landing Pier Command) â€” (0,0), 140Ã—140â€¯m Amphibious staging zone with naval craft and armored vehicles.
Red HQ (Industrial Command Tower) â€” (1000,1000), 140Ã—140â€¯m Fortified portâ€‘authority tower with radar mast and AA emplacements.
Central Cargo Terminal â€” (500,450), 250Ã—250â€¯m Container stacks, forklifts, cranes, narrow industrial corridors.
Fuel & Logistics Depot â€” (250,800), 100Ã—100â€¯m Fuel tanks, pipelines, cargo trucks, supply crates.
Ship Berth Row â€” (800,300), 200Ã—150â€¯m Docked cargo ships, loading cranes, gangways.
Seawall & Breakwater â€” (0,450 â†’ 400,900) Fortified wall with artillery positions and breach points.
Container Maze â€” (300,0 â†’ 450,350) Dense stacked containers forming a closeâ€‘range combat labyrinth.
Crane Ridge â€” (650,150), +260â€¯m Highâ€‘ground crane platforms with longâ€‘range sensor coverage.
Harbor Rooftops â€” (150,650), +240â€¯m Comms relay tower and drone pads.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Seawall Breach Points

Critical for naval invasion; heavily fortified.
Container Maze
Closeâ€‘range combat with tight corridors and ambush angles.

Cargo Terminal Lanes

Long straight industrial lanes ideal for armor and snipers.
Ship Berth Row
Mixedâ€‘range combat; naval units can support ground forces.
## Sightlines

Crane Ridge provides longâ€‘range sensor coverage (~350â€¯m). Fog and industrial smoke create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW landing pier + naval staging area.
Red Deployment: NE industrial command tower + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Concrete docks, seawalls, breakwaters, industrial yards, coastal roads.
## Vegetation


Sparse palms, shrubs, industrial planters.
## Buildings & Props


Military: Bunkers, AA guns, radar dishes, watchtowers. Industrial: Cranes, pipelines, generators, cargo containers. Logistics: Boats, trucks, forklifts, crates, fuel drums.
## Effects


Fog, sea spray, smoke plumes, sparks, wind gusts.
## Audio


Waves, machinery, horns, radio chatter, seagulls.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Seawalls/Rock 3          80k       320k
50k       750k
Docks/Concrete 4         30k       120k
2k        300k
Buildings/Industri1a5l   10k       500k
â€”         ~2.59M
Roads/Paths 4

Vegetation 150

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

1. Prototype Layout â€” Month 1 Blockâ€‘out docks, cargo terminal, seawall, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, naval pathfinding, autonomy behaviors.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (fog, smoke, sea spray).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify naval landing logic, containerâ€‘maze navigation, aerial flanking.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist

Navigation, multiâ€‘domain coordination, deployment budget enforcement, anchor drift, sensor accuracy.

