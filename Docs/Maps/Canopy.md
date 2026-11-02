# Canopy

Canopy (Dense Jungle / Recon & Autonomous Search)

## Executive Summary


Canopy is an ultraâ€‘dense tropical jungle reconnaissance battlefield built for autonomous search, stealth movement, sensorâ€‘limited engagements, and foliageâ€‘driven concealment. The terrain features towering canopy layers, tangled undergrowth, vineâ€‘choked ravines, mossy cliffs, hidden trails, abandoned outposts, and riverâ€‘cut gorges. The map emphasizes Obsidian Protocolâ€™s autonomous recon logic, sensorâ€‘fusion under heavy occlusion, commsâ€‘relay warfare, and logistics under extreme vegetation density. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of deepâ€‘jungle terrain. Key features include multiâ€‘layer canopy concealment, autonomous search corridors, unpredictable sightlines, and stealthâ€‘heavy engagements.

## Design Goals & Gameplay


Denseâ€‘Jungle Reconnaissance

Canopy prioritizes scouting, tracking, and stealth infiltration. Vegetation density creates natural blind zones, forcing reliance on autonomous recon units and sensor triangulation.
Autonomous Search & Pathfinding
Units navigate canopy shadows, narrow trails, rootâ€‘choked paths, and vineâ€‘covered ravines. Autonomous logic selects routes based on concealment, sensor clarity, and terrain stability.
Concealment & Ambush Warfare
Foliage, canopy layers, and humidity fog create stealth pockets. Ambushes, flanking, and sudden closeâ€‘range engagements dominate.
Sensor & Information Warfare

Humidity, fog, vegetation clutter, thermal distortion, and canopy occlusion degrade sensors. Relay towers on cliffs restore clarity. Destroying relays creates blind pockets across the jungle.
Logistics & Resources
Fuel caches, abandoned outposts, and river docks serve as resupply nodes. Convoys must navigate narrow jungle trails and avoid ambushâ€‘prone canopy corridors.
Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between stealth infantry, recon drones, or heavy jungleâ€‘adapted armor. Blue deploys from the southwest riverâ€‘trail camp; Red deploys from the northeast cliffside recon tower.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² denseâ€‘jungle zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations

Blue HQ (Riverâ€‘Trail Recon Camp) â€” (0,0), 150Ã—150â€¯m Boats, crates, recon drones, jungle staging.
Red HQ (Cliffside Recon Tower) â€” (1000,1000), 150Ã—150â€¯m Elevated bunker with radar mast, thermal relays, and canopyâ€‘overwatch.
Central Canopy Basin â€” (500,450), 300Ã—300â€¯m Dense canopy, fog pockets, unpredictable sightlines, stealthâ€‘heavy combat.
Abandoned Outpost Cluster â€” (250,800), 120Ã—120â€¯m Ruins, generators, interior flanking routes.
Ridgeâ€‘Top Canopy Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch, treeâ€‘top concealment, stealth movement.
Recon Corridor â€” (0,450 â†’ 400,900) Heavy vegetation zone; extreme sensor degradation.
Hidden Trail Route â€” (300,0 â†’ 450,350) Safer but slow; root hazards and vegetation cover.
Cliff Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Cliff Relay B â€” (150,650), +260â€¯m Comms relay hub and reconâ€‘monitoring node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Recon Corridor

Severe visibility loss; ideal for stealth ambushes and sensor deception.
Ridgeâ€‘Top Canopy Network

Interior flanking routes; unpredictable cover due to foliage.
Canopy Basin
Longâ€‘range engagements broken by vegetation; humidity reduces accuracy.
Hidden Trail Route

Predictable but safe; vulnerable to overwatch from ridges.
## Sightlines

Cliff Relay A provides longâ€‘range sensor coverage (~300â€¯m). Fog and canopy shadows create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW riverâ€‘trail camp + recon staging.
Red Deployment: NE cliffside recon tower + thermal defenses.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Canopy layers, cliffs, rivers, mud trails, abandoned outposts.
## Vegetation


Dense trees, vines, ferns, underbrush, mossy cliffs.
## Buildings & Props


Military: Radar dishes, thermal relays, bunkers. Rural: Huts, bridges, wooden structures. Logistics: Trucks, crates, fuel drums, boats, drones.
## Effects


Fog, humidity haze, rain, canopy shadows, mud splashes.
## Audio


Insects, rainfall, distant machinery, river flow, radio chatter.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Jungle Terrain 3         80k       320k
50k       750k
Rural/Industrial 4       30k       120k
10k       500k
Buildings/Rural 15       â€”         ~2.29M

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

1. Prototype Layout â€” Month 1 Blockâ€‘out canopy basin, ridges, outposts, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, jungle autonomy behaviors, recon logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (fog, humidity haze, canopy shadows).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify jungle navigation, reconâ€‘aware pathfinding, hazard avoidance.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under canopy occlusion.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, recon logic, deployment budget enforcement, anchor drift, sensor accuracy.
