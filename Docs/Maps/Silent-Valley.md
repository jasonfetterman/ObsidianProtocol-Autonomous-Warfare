# Silent Valley

Silent Valley (Rural / Mountain / Recon & Stealth)

## Executive Summary


Silent Valley is a ruralâ€‘mountain reconnaissance battlefield built for stealth operations, autonomous search, lowâ€‘visibility movement, and precision recon across layered elevation. The terrain features forested slopes, narrow mountain passes, abandoned farmsteads, rocky outcrops, mistâ€‘filled valleys, cliffside trails, and hidden observation posts. The map emphasizes Obsidian Protocolâ€™s autonomous recon logic, sensorâ€‘fusion under mountain fog and foliage occlusion, commsâ€‘relay warfare, and logistics across steep, uneven terrain. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of rural â€‘mountain terrain. Key features include stealth corridors, elevationâ€‘driven sightlines, concealed movement routes, and reconâ€‘focused engagements.

## Design Goals & Gameplay


Stealthâ€‘Focused Mountain Recon
Silent Valley prioritizes scouting, tracking, and infiltration. Forest cover, fog pockets, and elevation shifts create natural blind zones.
Autonomous Search & Terrainâ€‘Aware Pathfinding
Units navigate steep slopes, narrow passes, rocky ledges, and forested trails. Autonomous logic selects routes based on concealment, elevation advantage, and sensor clarity.

Concealment & Ambush Warfare
Trees, rocks, mist, and rural structures create stealth pockets. Ambushes, flanking, and sudden closeâ€‘range engagements dominate.

Sensor & Information Warfare
Fog, humidity, vegetation clutter, thermal distortion, and mountain occlusion degrade sensors. Relay towers on ridges restore clarity. Destroying relays creates blind pockets across the valley.

Logistics & Resources
Fuel caches, farmsteads, and mountain cabins serve as resupply nodes. Convoys must navigate narrow roads and avoid ambushâ€‘prone forest corridors.

Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between stealth infantry, recon drones, or mountainâ€‘adapted armor. Blue deploys from the southwest rural farmstead; Red deploys from the northeast ridgeâ€‘top command cabin.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² ruralâ€‘mountain zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System

1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations

Blue HQ (Farmstead Recon Camp) â€” (0,0), 150Ã—150â€¯m Barns, crates, recon drones, rural staging.
Red HQ (Ridgeâ€‘Top Command Cabin) â€” (1000,1000), 150Ã—150â€¯m Elevated bunker with radar mast, thermal relays, and overwatch.
Central Valley Basin â€” (500,450), 300Ã—300â€¯m Fog pockets, forest cover, unpredictable sightlines, stealthâ€‘heavy combat.
Abandoned Cabin Cluster â€” (250,800), 120Ã—120â€¯m Ruins, generators, interior flanking routes.
Ridge Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch, sniper nests, recon vantage points.
Stealth Corridor â€” (0,450 â†’ 400,900) Heavy vegetation zone; extreme sensor degradation.
Mountain Trail Route â€” (300,0 â†’ 450,350) Safer but slow; rock hazards and vegetation cover.
Ridge Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Ridge Relay B â€” (150,650), +260â€¯m Comms relay hub and reconâ€‘monitoring node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Stealth Corridor
Severe visibility loss; ideal for stealth ambushes and sensor deception.

Ridge Network
Interior flanking routes; unpredictable cover due to elevation and foliage.

Valley Basin
Longâ€‘range engagements broken by vegetation; fog reduces accuracy.

Mountain Trail Route
Predictable but safe; vulnerable to ridge overwatch.

## Sightlines

Ridge Relay A provides longâ€‘range sensor coverage (~300â€¯m). Fog and canopy shadows create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW farmstead + recon staging.
Red Deployment: NE ridgeâ€‘top command cabin + thermal defenses.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain

Forests, cliffs, valleys, mountain trails, rural cabins.

## Vegetation

Dense trees, shrubs, ferns, underbrush.

## Buildings & Props

Military: Radar dishes, thermal relays, bunkers. Rural: Cabins, barns, fences, wooden structures. Logistics: Trucks, crates, fuel drums, drones.

## Effects

Fog, humidity haze, rain, canopy shadows, dust.

## Audio

Wind, insects, distant machinery, river flow, radio chatter.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Mountain Terrain 3       80k       320k
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

1. Prototype Layout â€” Month 1 Blockâ€‘out valley basin, ridges, cabins, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, stealth autonomy behaviors, recon logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (fog, humidity haze, canopy shadows).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify stealth navigation, reconâ€‘aware pathfinding, hazard avoidance.

## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under mountain occlusion.

## Performance

â‰¥60â€¯Hz AR render on target device.

## QA Checklist

Navigation, recon logic, deployment budget enforcement, anchor drift, sensor accuracy.
