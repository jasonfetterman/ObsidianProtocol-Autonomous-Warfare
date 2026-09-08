# Red Basin

Red Basin (Desert / Reconnaissance & Longâ€‘Range Warfare) Map Design

## Executive Summary


Red Basin is a vast desertâ€‘plateau battlefield built for longâ€‘range engagements, reconnaissance, and sensorâ€‘driven tactics. The terrain features wide open basins, sandstone ridges, dry riverbeds, abandoned industrial pump stations, and sparse cover. The map emphasizes Obsidian Protocolâ€™s autonomous scouting behaviors, longâ€‘range sensor warfare, commsâ€‘relay networks, and logistics stretched across large open distances. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of desert terrain. Key features include elevated mesas for snipers and sensors, dry canyons for flanking, industrial pump stations, and critical recon hubs.

## Design Goals & Gameplay


Longâ€‘Range Desert Combat

Red Basin prioritizes longâ€‘range engagements. Open sightlines allow snipers, artillery, and recon drones to dominate. Sparse cover forces careful movement and positioning.
Autonomy & Recon Behavior

Autonomous AI scouts ahead, uses ridges for vantage, and avoids exposed basins. Units adapt to shifting sandstorms and sensor disruptions.
## Sensor & Information Warfare


Heat distortion, dust, and mirage effects degrade sensors. Relay towers on mesas restore clarity. Destroying relays creates massive fogâ€‘ofâ€‘war zones across the basin.
## Logistics & Resources


Supply convoys must cross long open roads vulnerable to ambush. Controlling the Pump Station and Desert Depot provides major resupply advantages.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between longâ€‘range units or fast recon vehicles. Blue deploys from the southwest canyon mouth; Red deploys from the northeast plateau.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² desert basin (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Canyon Mouth) â€” (0,0), 120Ã—120â€¯m Fortified canyon entrance with recon vehicles.

Red HQ (High Plateau) â€” (1000,1000), 120Ã—120â€¯m Elevated sandstone plateau with comms tower.
Central Basin â€” (500,500), 300Ã—300â€¯m Wide open desert floor; longâ€‘range combat zone.

Desert Depot â€” (250,800), 80Ã—80â€¯m Supply crates, fuel drums, recon drones.

Industrial Pump Station â€” (800,300), 120Ã—120â€¯m Pipes, pumps, water tanks, abandoned machinery.

Dry Riverbed â€” (0,450 â†’ 400,900) Shallow canyon providing partial cover.

Mesa A â€” (600,150), +280â€¯m Sniper and sensor vantage point.

Mesa B â€” (150,650), +260â€¯m Relay tower and recon hub.

Canyon Pass â€” (300,0 â†’ 450,350) Narrow flanking route with limited cover.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Canyon Pass

Narrow, winding route ideal for ambushes.
Dry Riverbed

Provides partial cover; key flanking path.
Open Basin

Extreme longâ€‘range sightlines; artillery and snipers dominate.
## Sightlines


Mesa A provides longâ€‘range sensor coverage (~350â€¯m). Heat distortion and dust storms create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW canyon mouth + rear rally point.

Red Deployment: NE plateau + rear rally point.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Sandstone cliffs, dunes, dry riverbeds, mesas, desert rock formations.
## Vegetation


Sparse shrubs, dry grass patches, small desert trees.
## Buildings & Props


Industrial: Pumps, tanks, pipes, generators. Military: Watchtowers, antennas, sandbag positions. Logistics: Trucks, crates, fuel drums, recon drones.
## Effects


Dust storms, heat shimmer, smoke plumes, wind gusts.
## Audio


Wind, distant machinery, radio chatter, sand movement.

## LOD & Budget


Asset CategCooruynt Tri/Item Total Tris

Cliffs/Mesas    3 200k              600k

Dunes/Rocks     4 80k               320k

Buildings/Industria1l5 50k          750k

Roads/Paths     4 30k               120k

Vegetation     150 2k               300k

Props          50 10k               500k

Total       â€”     â€”                 ~2.59M

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


1. Prototype Layout â€” Month 1 Blockâ€‘out mesas, basin, canyon, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, recon logic, autonomy behaviors.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (dust storms, heat shimmer).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify recon behavior, longâ€‘range targeting, pathfinding.
## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, cover usage, deployment budget enforcement, anchor drift, sensor accuracy.

