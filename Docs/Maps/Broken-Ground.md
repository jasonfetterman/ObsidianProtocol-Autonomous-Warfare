# Broken Ground

Broken Ground (Destroyed Urban / Battlefield Aftermath & Dynamic Terrain)

## Executive Summary


Broken Ground is a destroyedâ€‘urban battlefield built for postâ€‘combat operations, dynamic shifting terrain, autonomous hazard navigation, and closeâ€‘quarters engagements amid ruins. The environment features collapsed buildings, cratered streets, shattered overpasses, unstable rubble piles, burned â€‘out vehicles, improvised fortifications, and fractured utility lines. The map emphasizes Obsidian Protocolâ€™s autonomous rubble â€‘aware navigation, sensorâ€‘fusion under smoke and dust, commsâ€‘relay warfare, and logistics across unstable, constantly changing terrain. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of destroyed â€‘urban terrain. Key features include shifting rubble fields, collapsed chokepoints, interior ruin routes, and dynamic cover created by ongoing structural instability.

## Design Goals & Gameplay


Postâ€‘Battlefield Urban Combat

Broken Ground prioritizes combat in a city already devastated by prior fighting. Terrain is unstable, unpredictable, and constantly shifting.

Dynamic Terrain & Structural Instability

Rubble collapses, fires flare up, dust clouds shift, and debris falls. Autonomous units must adapt to terrain changes mid â€‘engagement.
Closeâ€‘Quarters Urban Warfare

Interior ruins, alleyways, collapsed basements, and broken stairwells create tight, brutal combat zones.

Autonomy & Hazard Navigation

Autonomous units choose routes based on rubble stability, fire hazards, dust visibility, and sensor clarity. Units reroute when terrain becomes impassable.

Sensor & Information Warfare

Smoke, dust, fire heat, metallic clutter, and collapsed structures degrade sensors. Relay towers on surviving rooftops restore clarity. Destroying relays creates blind pockets across the ruins.

Logistics & Resources

Fuel caches, abandoned depots, and makeshift shelters serve as resupply nodes. Convoys must navigate cratered roads and avoid ambush â€‘prone rubble corridors.

Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between urban infantry, rubble â€‘capable drones, or heavy armor adapted for unstable terrain. Blue deploys from the southwest collapsed highway; Red deploys from the northeast fortified ruin.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² destroyedâ€‘urban zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations


Blue HQ (Collapsed Highway Gate) â€” (0,0), 150Ã—150â€¯m Burnedâ€‘out vehicles, barricades, drone pads, rubble staging.

Red HQ (Fortified Ruin Command) â€” (1000,1000), 150Ã—150â€¯m Reinforced ruin with radar mast, AA emplacements, and sensor arrays.

Central Ruin Basin â€” (500,450), 300Ã—300â€¯m Collapsed buildings, shifting rubble, fires, dust clouds, closeâ€‘quarters combat.

Abandoned Depot Cluster â€” (250,800), 120Ã—120â€¯m Warehouses, broken machinery, interior flanking routes.

Rubble Ridge Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch created by collapsed structures; unstable but tactically valuable.

Collapse Corridor â€” (0,450 â†’ 400,900) Primary chokepoint; unstable rubble, fires, falling debris.

Crater Road Route â€” (300,0 â†’ 450,350) Safer but slow; crater hazards and limited cover.

Rooftop Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.

Rooftop Relay B â€” (150,650), +260â€¯m Comms relay hub and hazardâ€‘monitoring node.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Collapse Corridor

Major chokepoint; terrain shifts unpredictably, ideal for ambushes.

Rubble Ridge Network

Interior flanking routes; unstable highâ€‘ground with long sightlines.

Ruin Basin

Closeâ€‘quarters engagements broken by rubble; dust reduces accuracy.

Crater Road Route

Predictable but safe; vulnerable to overwatch from rubble ridges.

## Sightlines


Rooftop Relay A provides longâ€‘range sensor coverage (~300â€¯m). Smoke and dust create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW collapsed highway + rubble staging.

Red Deployment: NE fortified ruin + AA emplacements.

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


Asset CategoCroyunt Tri/Item Total Tris

Ruin/Rubble Terrain 3 200k          600k

Industrial/Urban 4 80k              320k

Buildings/Ruins 15 50k              750k

Roads/Paths     4 30k               120k

Props          50 10k               500k

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


1. Prototype Layout â€” Month 1 Blockâ€‘out ruin basin, rubble ridges, depots, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, rubbleâ€‘aware autonomy behaviors, hazard logic.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (smoke, dust, fire bursts).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify rubble navigation, hazardâ€‘aware pathfinding, collapseâ€‘zone avoidance.

## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under smoke and dust.

## Performance


â‰¥60â€¯Hz AR render on target device.

## QA Checklist


Navigation, dynamic terrain logic, deployment budget enforcement, anchor drift, sensor accuracy.
