# Crossroads

Crossroads (Rural / Infrastructure / Multiple Routes & Strategic Control)

## Executive Summary


Crossroads is a ruralâ€‘infrastructure battlefield built around multiâ€‘route maneuvering, strategic control of intersections, logistics warfare, and flexible deployment paths. The terrain features rolling farmland, utility substations, rail spurs, fuel depots, bridges, overpasses, and a major highway interchange that forms the strategic heart of the map. The environment emphasizes Obsidian Protocolâ€™s autonomous route â€‘selection logic, sensorâ€‘driven positioning, commsâ€‘relay networks, and logistics under openâ€‘terrain constraints. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of rural infrastructure. Key features include multi â€‘route intersections, bridge chokepoints, railâ€‘yard flanking paths, and strategic control of the central interchange.

## Design Goals & Gameplay


Multiâ€‘Route Maneuver Warfare
Crossroads prioritizes flexible movement across multiple parallel and intersecting routes. Players must control intersections, bridges, and overpasses to dictate the flow of battle.

Strategic Infrastructure Control
Fuel depots, substations, and rail yards serve as critical strategic points. Holding them grants logistics and sensor advantages.
Autonomy & Routeâ€‘Selection Logic
Autonomous AI chooses between highway lanes, rural roads, railâ€‘side paths, bridge crossings, or offâ€‘road fields. Units adapt to blocked routes and enemy control of intersections.

## Sensor & Information Warfare

Dust, smoke, and infrastructure clutter distort sensors. Relay towers on overpasses restore clarity. Destroying relays creates blind zones across the road network.

## Logistics & Resources

Fuel depots, rail yards, and utility substations serve as resupply nodes. Convoys must navigate exposed stretches and avoid ambush â€‘prone intersections.

## Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between convoy escorts, fast recon units, or heavy armor. Blue deploys from the southwest farmstead; Red deploys from the northeast interchange command.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² rural zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System

1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations

Blue HQ (Farmstead Command) â€” (0,0), 140Ã—140â€¯m Barns, fuel tanks, recon vehicles, rural staging area.
Red HQ (Interchange Command Node) â€” (1000,1000), 140Ã—140â€¯m Elevated highway command post with radar mast and AA emplacements.
Central Interchange Hub â€” (500,450), 300Ã—250â€¯m Highway overpasses, ramps, intersections, multiâ€‘lane combat zone.
Fuel & Logistics Depot â€” (250,800), 100Ã—100â€¯m Fuel tanks, trucks, pipelines, cargo crates.
Rail Spur & Yard â€” (800,300), 150Ã—150â€¯m Rail tracks, cargo cars, loading cranes, flanking routes.
Bridge Crossing â€” (0,450 â†’ 400,900) Critical chokepoint; river + bridge + roadside cover.
Rural Road Route â€” (300,0 â†’ 450,350) Safer but slower path with partial cover.
Overpass A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Overpass B â€” (150,650), +260â€¯m Comms relay tower and recon hub.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Bridge Crossing
Major chokepoint; controlling it determines eastâ€‘west movement.

Interchange Hub
Multiâ€‘lane combat zone; ideal for armor and ambushes.

Rail Spur

Provides stealthy flanking routes behind industrial cover.

Rural Road Route
Predictable but safe; vulnerable to overwatch from Overpass A.

## Sightlines

Overpass A provides longâ€‘range sensor coverage (~350â€¯m). Dust and smoke create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW farmstead + convoy staging.
Red Deployment: NE interchange command + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain

Valley floor, farmland, river, bridge, overpasses, rail yard.

## Vegetation

Fields, shrubs, scattered trees, grass patches.

## Buildings & Props

Military: Watchtowers, bunkers, radar dishes, AA guns. Industrial: Fuel tanks, pipelines, generators, cargo yards. Logistics: Trucks, crates, fuel drums, convoy vehicles.

## Effects

Dust clouds, exhaust haze, smoke plumes, wind gusts.

## Audio

Engine noise, wind, distant machinery, radio chatter.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Cliffs/Rock Walls 3      80k       320k
50k       750k
Roads/Highways 4         30k       120k
2k        240k
Buildings/Industri1a5l   10k       500k
â€”         ~2.53M
Bridges/Overpasse4s

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

1. Prototype Layout â€” Month 1 Blockâ€‘out interchange, bridge, rail yard, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, routeâ€‘selection logic, autonomy behaviors.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (dust, smoke).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify routeâ€‘selection logic, convoy pathing, ambush detection.

## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.

## Performance

â‰¥60â€¯Hz AR render on target device.

## QA Checklist

Navigation, chokepoint behavior, convoy logic, deployment budget enforcement, anchor drift, sensor accuracy.

