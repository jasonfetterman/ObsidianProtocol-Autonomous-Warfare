# Terminal

Terminal (Airport / Industrial / Air Operations & Ground Warfare)

## Executive Summary


Terminal is a massive airportâ€‘industrial battlefield built for air operations, runway control, ground warfare, logistics disruption, and multiâ€‘domain coordination. The terrain features runways, taxiways, hangars, cargo terminals, fuel farms, radar towers, maintenance bays, and elevated service roads. The map emphasizes Obsidian Protocolâ€™s autonomous airâ€‘ground coordination, sensorâ€‘network routing, commsâ€‘relay warfare, and logistics under highâ€‘traffic industrial constraints. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of airport territory. Key features include runway chokepoints, hangar interior combat, airspace corridors, cargo â€‘yard flanking routes, and strategic control of radar and comms infrastructure.

## Design Goals & Gameplay


Air Operations & Runway Control
Terminal prioritizes aerial combat, runway denial, VTOL deployment, drone reconnaissance, and airâ€‘ground coordination. Controlling runways determines air superiority.
Industrial Ground Warfare
Cargo yards, hangars, and maintenance bays create mixedâ€‘range combat zones. Long fire lanes contrast with tight interior routes.
Autonomy & Multiâ€‘Domain Coordination

Autonomous units coordinate air and ground forces, selecting optimal routes based on runway status, radar coverage, and enemy movement.
## Sensor & Information Warfare


Heat shimmer, exhaust, smoke, and radar interference distort sensors. Relay towers on control towers restore clarity. Destroying relays creates blind pockets across the airport grid.
## Logistics & Resources

Fuel farms, cargo terminals, and maintenance depots serve as resupply nodes. Convoys must navigate exposed runways and avoid ambushâ€‘prone industrial corridors.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between air support, heavy armor, or autonomous infantry squads. Blue deploys from the southwest cargo apron; Red deploys from the northeast control tower stronghold.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² airport zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Cargo Apron Command) â€” (0,0), 150Ã—150â€¯m Cargo loaders, forklifts, drone pads, armored staging.
Red HQ (Control Tower Stronghold) â€” (1000,1000), 150Ã—150â€¯m Radar mast, AA emplacements, hardened command center.
Central Runway Complex â€” (500,450), 350Ã—200â€¯m Two runways, taxiways, airspace corridors, longâ€‘range fire lanes.
Cargo Terminal & Rail Spur â€” (250,800), 150Ã—150â€¯m Cargo containers, cranes, rail access, flanking routes.
Hangar Row â€” (800,300), 200Ã—200â€¯m Interior combat, maintenance bays, aircraft shelters.
Fuel Farm â€” (0,450 â†’ 400,900) Fuel tanks, pipelines, hazardous chokepoint.
Service Road Route â€” (300,0 â†’ 450,350) Safer ground route with partial cover.
Radar Peak A â€” (650,150), +260â€¯m Longâ€‘range sensor vantage point.
Relay Tower B â€” (150,650), +280â€¯m Comms relay tower and drone hub.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Runway Complex
Extreme longâ€‘range sightlines; controlling runways determines air dominance.

Hangar Row
Interior flanking routes; closeâ€‘range engagements.
Fuel Farm

Hazardous chokepoint; explosions and fire risk.
Cargo Terminal

Industrial cover; ideal for ambushes and flanking.
## Sightlines

Radar Peak A provides longâ€‘range sensor coverage (~350â€¯m). Heat shimmer and smoke create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW cargo apron + armored staging.
Red Deployment: NE control tower stronghold + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Runways, taxiways, hangars, cargo yards, fuel farms, service roads.
## Vegetation


Sparse shrubs, grass strips, airport landscaping.
## Buildings & Props


Military: AA guns, radar dishes, watchtowers. Industrial: Hangars, cranes, pipelines, generators. Logistics: Trucks, cargo containers, fuel drums, drones.
## Effects


Smoke, sparks, dust, heat shimmer, exhaust plumes.
## Audio


Aircraft noise, machinery, radio chatter, wind.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Runways/Taxiways3        80k       320k
50k       750k
Hangars/Industrial4      30k       120k
2k        240k
Buildings/Urban 15       10k       500k
â€”         ~2.53M
Roads/Paths 4

Vegetation 120

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

1. Prototype Layout â€” Month 1 Blockâ€‘out runways, hangars, cargo terminal, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, airâ€‘ground autonomy behaviors.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (heat shimmer, smoke, sparks).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify airâ€‘ground coordination, runway logic, hangar navigation.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist

Navigation, combinedâ€‘arms behavior, deployment budget enforcement, anchor drift, sensor accuracy.

