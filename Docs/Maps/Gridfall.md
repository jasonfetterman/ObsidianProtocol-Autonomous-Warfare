# Gridfall

Gridfall (Suburban / Urban / Distributed Objectives & Autonomous Forces)

## Executive Summary


Gridfall is a suburbanâ€‘urban hybrid battlefield built for distributed objectives, autonomous force coordination, multiâ€‘node control, and decentralized engagements. The terrain features residential blocks, commercial strips, utility corridors, parks, overpasses, culâ€‘deâ€‘sacs, and a fractured power grid that shapes the tactical landscape. The map emphasizes Obsidian Protocolâ€™s autonomous multiâ€‘node decisionâ€‘making, sensorâ€‘network routing, commsâ€‘relay warfare, and logistics across a dense suburban grid. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of mixed suburban territory. Key features include distributed objective nodes, multi â€‘route street networks, interior combat zones, and autonomous force coordination across a fractured grid.

## Design Goals & Gameplay


Distributed Objective Control
Gridfall prioritizes simultaneous control of multiple nodes: substations, intersections, relay hubs, and commercial blocks. Victory depends on coordinated multi â€‘front pressure.

Autonomous Force Coordination
Units operate semiâ€‘independently across the grid, selecting optimal nodes to attack or defend based on sensor data, enemy movement, and objective priority.

Urban/Suburban Hybrid Combat
Wide suburban streets contrast with tight commercial interiors. Culâ€‘deâ€‘sacs create deadâ€‘end traps; parks provide open sightlines; strip malls create interior flanking routes.

## Sensor & Information Warfare

Power outages, smoke, and dense building layouts distort sensors. Relay towers on rooftops restore clarity. Destroying relays creates blind pockets across the grid.

## Logistics & Resources

Utility substations, commercial depots, and residential garages serve as resupply nodes. Convoys must navigate unpredictable street networks and avoid ambush â€‘prone intersections.

## Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between distributed autonomous squads, heavy urban armor, or recon drones. Blue deploys from the southwest residential block; Red deploys from the northeast commercial hub.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² suburban grid (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System

1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations

Blue HQ (Residential Block Command) â€” (0,0), 140Ã—140â€¯m Garages, culâ€‘deâ€‘sacs, small parks, recon vehicles.
Red HQ (Commercial Hub Command) â€” (1000,1000), 140Ã—140â€¯m Strip malls, rooftop relays, hardened storefronts.
Central Grid Square â€” (500,450), 250Ã—250â€¯m Intersections, shops, apartments, interior combat zones.
Utility Substation â€” (250,800), 100Ã—100â€¯m Transformers, power lines, relay equipment.
Strip Mall Complex â€” (800,300), 150Ã—150â€¯m Interior corridors, loading docks, closeâ€‘range combat.
Parkland Corridor â€” (0,450 â†’ 400,900) Open sightlines; ideal for longâ€‘range engagements.
Residential Backroad Route â€” (300,0 â†’ 450,350) Narrow streets, fences, garages; stealth movement.
Rooftop Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Rooftop Relay B â€” (150,650), +260â€¯m Comms relay tower and recon hub.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Parkland Corridor
Open terrain; longâ€‘range engagements dominate.

Strip Mall Interior
Closeâ€‘range combat with flanking through service corridors.

Central Grid Square
Highâ€‘traffic node; controlling it shapes the entire battle.

Residential Backroads
Stealthy but narrow; ideal for ambushes.

## Sightlines

Rooftop Relay A provides longâ€‘range sensor coverage (~300â€¯m). Dense buildings create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW residential block + recon staging.
Red Deployment: NE commercial hub + rooftop relays.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain

Streets, intersections, parks, culâ€‘deâ€‘sacs, commercial zones.

## Vegetation

Trees, shrubs, lawns, park grass.

## Buildings & Props

Military: Watchtowers, antennas, bunkers. Civilian: Houses, shops, strip malls, garages. Industrial: Substations, transformers, loading docks. Logistics: Trucks, crates, fuel drums, drones.

## Effects

Smoke, dust, fog, sparks, flickering lights.

## Audio

Traffic hum, distant machinery, radio chatter, wind.

## LOD & Budget


Asset CategCooruynt Tri/Item       Total Tris
600k
Buildings/Suburban3 200k           320k
750k
Roads/Streets 4 80k                300k
500k
Commercial/Indu1st5rial50k         ~2.47M

Parks/Vegetatio1n50 2k

Props    50 10k

Total â€”  â€”

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

1. Prototype Layout â€” Month 1 Blockâ€‘out grid, intersections, strip mall, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, autonomous multiâ€‘node logic, sensor routing.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (smoke, flickering lights).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify multiâ€‘node coordination, distributed objective logic, route selection.

## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.

## Performance

â‰¥60â€¯Hz AR render on target device.

## QA Checklist

Navigation, distributed objective behavior, deployment budget enforcement, anchor drift, sensor accuracy.

