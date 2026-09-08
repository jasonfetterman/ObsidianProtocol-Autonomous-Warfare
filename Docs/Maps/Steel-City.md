# Steel City

Steel City (Major Urban / Largeâ€‘Scale Combined Arms)

## Executive Summary


Steel City is a massive urbanâ€‘industrial megabattlefield built for largeâ€‘scale combined arms warfare, integrating armor, infantry, drones, artillery, air support, and autonomous units across dense city blocks and heavy industrial zones. The terrain features high â€‘rise clusters, steelworks, rail corridors, elevated freeways, cargo terminals, fortified intersections, and multi â€‘level combat spaces. The map emphasizes Obsidian Protocolâ€™s autonomous coordination across multiple fronts, sensorâ€‘network routing, commsâ€‘relay warfare, and logistics under dense urban constraints. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of major urban terrain. Key features include multi â€‘tier street networks, industrial choke zones, rooftop combat, and largeâ€‘scale combined arms engagements.

## Design Goals & Gameplay


Largeâ€‘Scale Combined Arms Warfare

Steel City supports simultaneous armor pushes, infantry assaults, drone reconnaissance, artillery strikes, and air support. Multi â€‘level terrain enables layered combat.
Urbanâ€‘Industrial Hybrid Combat

Highâ€‘rise districts provide vertical combat; industrial zones provide longâ€‘range fire lanes; rail corridors enable flanking; elevated freeways create multiâ€‘tier movement.
Autonomy & Multiâ€‘Front Coordination

Autonomous units coordinate across multiple sectors, selecting optimal fronts to reinforce or breach based on sensor data and objective priority.

## Sensor & Information Warfare


Smoke, dust, steelworks heat, and dense building layouts distort sensors. Relay towers on rooftops restore clarity. Destroying relays creates blind pockets across the city grid.

## Logistics & Resources


Cargo terminals, steelworks, rail yards, and freeway checkpoints serve as resupply nodes. Convoys must navigate unpredictable street networks and avoid ambush â€‘prone industrial corridors.

## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between heavy armor, air support, artillery, or autonomous infantry squads. Blue deploys from the southwest industrial yard; Red deploys from the northeast high â€‘rise command tower.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² major urban zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations


Blue HQ (Industrial Yard Command) â€” (0,0), 150Ã—150â€¯m Steelworks, cargo cranes, armored staging area.

Red HQ (Highâ€‘Rise Command Tower) â€” (1000,1000), 150Ã—150â€¯m Skyscraper command node with radar mast and AA emplacements.

Central Steel District â€” (500,450), 300Ã—300â€¯m Factories, warehouses, smokestacks, longâ€‘range industrial fire lanes.

Cargo Terminal & Rail Hub â€” (250,800), 150Ã—150â€¯m Rail spurs, cargo cars, loading cranes, flanking routes.

Highâ€‘Rise Cluster â€” (800,300), 200Ã—200â€¯m Multiâ€‘level combat, rooftops, interior corridors.

Elevated Freeway Spine â€” (0,450 â†’ 400,900) Twoâ€‘tier freeway; critical movement corridor and chokepoint.

Lower Street Grid â€” (300,0 â†’ 450,350) Dense urban streets; ideal for infantry and autonomous squads.

Rooftop Relay A â€” (650,150), +260â€¯m Longâ€‘range sensor vantage point.

Rooftop Relay B â€” (150,650), +280â€¯m Comms relay tower and drone hub.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Elevated Freeway Spine

Major chokepoint; controlling it determines northâ€‘south movement.

Steel District Fire Lanes

Longâ€‘range industrial corridors ideal for armor and artillery.
Highâ€‘Rise Cluster

Vertical combat; rooftops provide overwatch and sensor dominance.

Lower Street Grid

Closeâ€‘range combat; ideal for autonomous infantry and ambushes.

## Sightlines


Rooftop Relay A provides longâ€‘range sensor coverage (~350â€¯m). Smoke and industrial heat create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW industrial yard + armored staging.

Red Deployment: NE highâ€‘rise command tower + AA emplacements.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Highâ€‘rises, industrial zones, rail yards, freeways, street grids.

## Vegetation


Sparse trees, planters, park strips.

## Buildings & Props


Military: Watchtowers, AA guns, radar dishes. Industrial: Factories, cranes, pipelines, generators. Urban: Apartments, offices, shops, parking structures. Logistics: Trucks, crates, fuel drums, drones.

## Effects


Smoke, sparks, dust, fog, heat shimmer.

## Audio


Machinery, traffic hum, distant artillery, radio chatter.

## LOD & Budget


Asset CategoCroyunt Tri/Item Total Tris

Highâ€‘Rise Buildings 3 200k            600k

Industrial Structures 4 80k           320k

Buildings/Urban 15 50k                750k

Roads/Freeways 4 30k                  120k

Vegetation     120 2k                 240k

Props          50 10k                 500k

Total       â€”      â€”                  ~2.53M

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


1. Prototype Layout â€” Month 1 Blockâ€‘out highâ€‘rises, steel district, freeway, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, combinedâ€‘arms autonomy behaviors.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (smoke, sparks, heat shimmer).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify multiâ€‘front coordination, combinedâ€‘arms logic, route selection.

## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.

## Performance


â‰¥60â€¯Hz AR render on target device.

## QA Checklist


Navigation, combinedâ€‘arms behavior, deployment budget enforcement, anchor drift, sensor accuracy.

