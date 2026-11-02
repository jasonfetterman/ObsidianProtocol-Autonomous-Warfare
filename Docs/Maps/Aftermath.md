# Aftermath

Aftermath (Warâ€‘torn City / Destructionâ€‘Focused Combined Arms)

## Executive Summary


Aftermath is a warâ€‘torn urban combinedâ€‘arms battlefield built for heavy destruction, multiâ€‘domain coordination, rubbleâ€‘based maneuvering, and autonomous adaptation to collapsing terrain. The environment features shattered skyscrapers, cratered boulevards, burnedâ€‘out industrial zones, fractured bridges, improvised fortifications, underground transit ruins, and smoke â€‘filled avenues. The map emphasizes Obsidian Protocolâ€™s autonomous combinedâ€‘arms logic, sensorâ€‘fusion under smoke and debris, commsâ€‘relay warfare, and logistics across unstable, partially collapsing terrain. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of warâ€‘torn urban terrain. Key features include destructionâ€‘driven chokepoints, multiâ€‘layer vertical combat, rubbleâ€‘adaptive navigation, and combinedâ€‘arms engagements across armor, infantry, drones, and artillery.

## Design Goals & Gameplay


Destructionâ€‘Focused Combined Arms

Aftermath prioritizes armor pushes, drone reconnaissance, infantry clearing, artillery strikes, and air support. Terrain destruction directly reshapes tactical options.
Dynamic Urban Terrain

Buildings collapse, fires spread, dust clouds shift, and rubble piles grow. Autonomous units must constantly re â€‘evaluate terrain viability.
Vertical & Interior Combat

Ruined towers, broken stairwells, collapsed floors, and underground transit tunnels create multi â€‘layer combat zones.
Autonomy & Hazardâ€‘Aware Maneuvering

Units choose routes based on rubble stability, fire hazards, smoke density, and sensor clarity. Autonomous logic reroutes when terrain becomes impassable or collapses.
Sensor & Information Warfare

Smoke, dust, fire heat, metallic clutter, and collapsed structures degrade sensors. Relay towers on surviving rooftops restore clarity. Destroying relays creates blind pockets across the city.
Logistics & Resources

Fuel caches, industrial depots, and makeshift shelters serve as resupply nodes. Convoys must navigate cratered roads and avoid ambush â€‘prone rubble corridors.
Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between heavy armor, urban infantry, drones, or artillery. Blue deploys from the southwest industrial ruins; Red deploys from the northeast fortified tower.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² warâ€‘torn urban zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Industrial Ruin Command) â€” (0,0), 150Ã—150â€¯m Burnedâ€‘out factories, armored staging, drone pads.
Red HQ (Fortified Tower Spire) â€” (1000,1000), 150Ã—150â€¯m Reinforced ruin with radar mast, AA emplacements, and sensor arrays.
Central Ruin Basin â€” (500,450), 300Ã—300â€¯m Collapsed skyscrapers, shifting rubble, fires, dust clouds, multiâ€‘domain combat.
Transit Tunnel Network â€” (250,800), 120Ã—120â€¯m Underground flanking routes, collapsed platforms, interior combat.
Rubble Ridge Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch created by collapsed structures; unstable but tactically valuable.
Destruction Corridor â€” (0,450 â†’ 400,900) Primary chokepoint; unstable rubble, fires, falling debris.
Crater Boulevard Route â€” (300,0 â†’ 450,350) Safer but slow; crater hazards and limited cover.
Rooftop Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Rooftop Relay B â€” (150,650), +260â€¯m Comms relay hub and hazardâ€‘monitoring node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Destruction Corridor

Major chokepoint; terrain shifts unpredictably, ideal for ambushes.
Rubble Ridge Network

Interior flanking routes; unstable highâ€‘ground with long sightlines.
Ruin Basin

Combinedâ€‘arms engagements broken by rubble; smoke reduces accuracy.
Crater Boulevard Route

Predictable but safe; vulnerable to overwatch from rubble ridges.
## Sightlines


Rooftop Relay A provides longâ€‘range sensor coverage (~300â€¯m). Smoke and dust create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW industrial ruins + armor staging.
Red Deployment: NE fortified tower + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Collapsed buildings, rubble fields, craters, broken roads, ruined depots.
## Vegetation


Sparse weeds, dead trees, overgrown patches.
## Buildings & Props


Military: AA guns, radar dishes, bunkers. Urban: Ruins, collapsed towers, vehicles, debris piles. Logistics: Trucks, crates, fuel drums, drones.
## Effects


Smoke, dust, fire bursts, sparks, falling debris.
## Audio


Distant explosions, collapsing rubble, machinery, radio chatter.

## LOD & Budget


Asset CategCooryunt      Tri/Item  Total Tris
200k      600k
Ruin/Rubble Terrain3     80k       320k
50k       750k
Industrial/Urban 4       30k       120k
10k       500k
Buildings/Ruins 15       â€”         ~2.29M

Roads/Paths          4

Props                50

Total    â€”

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


1. Prototype Layout â€” Month 1 Blockâ€‘out ruin basin, rubble ridges, transit tunnels, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, combinedâ€‘arms autonomy behaviors, hazard logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (smoke, dust, fire bursts).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify rubble navigation, combinedâ€‘arms coordination, hazard avoidance.
## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under smoke and dust.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, dynamic terrain logic, deployment budget enforcement, anchor drift, sensor accuracy.
