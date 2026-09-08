# Foundry

Foundry (Heavy Industry / Closeâ€‘Range Ground Combat & Destruction)

## Executive Summary


Foundry is a brutal heavyâ€‘industry battlefield built for closeâ€‘range ground combat, destructive engagements, interior firefights, and autonomous maneuvering through dense industrial clutter. The terrain features steel mills, blast furnaces, conveyor tunnels, slag pits, loading bays, pipe corridors, moltenâ€‘metal hazards, and tight industrial choke zones. The map emphasizes Obsidian Protocolâ€™s autonomous interiorâ€‘navigation logic, sensorâ€‘degraded combat, commsâ€‘relay warfare, and logistics under extreme industrial density. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of heavy â€‘industry terrain. Key features include furnaceâ€‘side combat, pipeâ€‘maze infiltration, slagâ€‘pit hazards, and destructive closeâ€‘quarters engagements.

## Design Goals & Gameplay


Closeâ€‘Range Industrial Combat
Foundry prioritizes brutal interior firefights, short sightlines, ambushes, and destructive engagements. Heavy machinery creates unpredictable cover and hazards.
Destructionâ€‘Driven Gameplay
Pipes rupture, slag pits ignite, conveyor belts jam, and machinery collapses. Environmental destruction reshapes routes and cover dynamically.

Autonomy & Interior Pathfinding
Autonomous units choose between pipe corridors, furnace catwalks, loadingâ€‘bay flanks, or slagâ€‘pit bypasses. Units adapt to blocked machinery and shifting hazards.

## Sensor & Information Warfare

Heat shimmer, smoke, sparks, steam, and metal clutter distort sensors. Relay towers on gantries restore clarity. Destroying relays creates blind pockets across the foundry.

## Logistics & Resources

Fuel tanks, maintenance bays, and loading docks serve as resupply nodes. Convoys must navigate narrow industrial lanes and avoid ambush â€‘prone machinery corridors.

## Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between heavy armor, demolition units, or autonomous infantry. Blue deploys from the southwest loading bay; Red deploys from the northeast furnace control tower.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² industrial zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System

1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations

Blue HQ (Loading Bay Command) â€” (0,0), 150Ã—150â€¯m Cargo loaders, forklifts, armored staging area.
Red HQ (Furnace Control Tower) â€” (1000,1000), 150Ã—150â€¯m Highâ€‘heat command node with radar mast and AA emplacements.
Central Foundry Core â€” (500,450), 300Ã—300â€¯m Blast furnaces, moltenâ€‘metal channels, catwalks, heavy machinery.
Maintenance Depot â€” (250,800), 120Ã—120â€¯m Tools, generators, repair bays, fuel drums.
Pipeâ€‘Maze Corridor â€” (800,300), 200Ã—200â€¯m Dense pipe networks, steam vents, interior flanking routes.
Slag Pit Zone â€” (0,450 â†’ 400,900) Hazardous chokepoint; molten slag, collapsing platforms.
Conveyor Route â€” (300,0 â†’ 450,350) Moving belts, machinery cover, narrow traversal.
Gantry Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Rooftop Relay B â€” (150,650), +260â€¯m Comms relay tower and recon hub.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Slag Pit Zone
Hazardous chokepoint; environmental destruction reshapes combat.
Pipeâ€‘Maze Corridor
Interior flanking routes; extremely tight sightlines.

Foundry Core

Heavy machinery creates dynamic cover; ideal for demolition units.

Conveyor Route
Predictable movement path; vulnerable to ambushes.

## Sightlines

Gantry Relay A provides longâ€‘range sensor coverage (~300â€¯m). Heat shimmer and smoke create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW loading bay + armored staging.
Red Deployment: NE furnace control tower + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain

Furnaces, slag pits, conveyor belts, pipe corridors, loading bays.

## Vegetation

None â€” industrial debris only.

## Buildings & Props

Military: AA guns, radar dishes, watchtowers. Industrial: Furnaces, cranes, pipelines, generators. Logistics: Trucks, crates, fuel drums, drones.

## Effects

Smoke, sparks, dust, heat shimmer, steam vents, fire bursts.

## Audio

Machinery, metal impacts, steam, radio chatter.

## LOD & Budget


Asset CategCooruynt Tri/Item    Total Tris
600k
Furnaces/Heavy M3ach2in0e0rky   320k
750k
Pipes/Industrial Me4sh8e0sk     120k
500k
Buildings/Industri1a5l 50k      ~2.29M

Roads/Paths 4 30k

Props         50 10k

Total â€”       â€”

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

1. Prototype Layout â€” Month 1 Blockâ€‘out furnaces, pipe maze, slag pits, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, interior autonomy behaviors, destruction logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (sparks, heat shimmer, steam).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify interior navigation, destructionâ€‘aware pathfinding, hazard avoidance.

## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.

## Performance

â‰¥60â€¯Hz AR render on target device.

## QA Checklist

Navigation, destruction logic, deployment budget enforcement, anchor drift, sensor accuracy.

