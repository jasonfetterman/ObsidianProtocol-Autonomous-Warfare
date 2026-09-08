# Long Range

Long Range (Open Plains / Maximumâ€‘Distance Combat)

## Executive Summary


Long Range is a wideâ€‘open plains battlefield built for maximumâ€‘distance engagements, extreme sightlines, longâ€‘range artillery duels, drone reconnaissance, and autonomous maneuvering across unobstructed terrain. The environment features rolling grasslands, sparse tree lines, shallow ridges, abandoned farm structures, wind farms, dry creek beds, and isolated observation towers. The map emphasizes Obsidian Protocolâ€™s autonomous long â€‘range targeting logic, sensorâ€‘fusion under heat shimmer and dust, commsâ€‘relay warfare, and logistics across vast, open terrain. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of openâ€‘plains terrain. Key features include extreme sightlines, longâ€‘range fire corridors, droneâ€‘heavy recon, and maneuver warfare across minimal cover.

## Design Goals & Gameplay


Maximumâ€‘Distance Combat

Long Range prioritizes engagements at the absolute limits of weapon range. Artillery, missile vehicles, snipers, and longâ€‘range drones dominate.
Openâ€‘Terrain Maneuver Warfare

Minimal cover forces reliance on speed, spacing, and autonomous movement. Units must avoid predictable lines of advance.
Recon & Counterâ€‘Recon

Drones, sensor towers, and longâ€‘range optics define the information war. Destroying recon assets creates massive blind zones.
Autonomy & Distanceâ€‘Aware Pathfinding

Autonomous units choose routes based on elevation, cover scarcity, heat shimmer, and sensor clarity. Units avoid exposed flats during peak heat unless necessary.

Sensor & Information Warfare

Heat distortion, dust haze, mirage effects, and longâ€‘range interference degrade sensors. Relay towers on ridges restore clarity. Destroying relays creates blind pockets across the plains.

Logistics & Resources

Fuel depots, farm structures, and windâ€‘farm maintenance yards serve as resupply nodes. Convoys must navigate exposed roads and avoid longâ€‘range ambush corridors.

Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between artillery dominance, recon superiority, or fast armor. Blue deploys from the southwest dry â€‘creek staging zone; Red deploys from the northeast ridgeâ€‘top command.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² openâ€‘plains zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations


Blue HQ (Dryâ€‘Creek Staging Zone) â€” (0,0), 150Ã—150â€¯m Cargo crates, armored vehicles, drone pads.

Red HQ (Ridgeâ€‘Top Command) â€” (1000,1000), 150Ã—150â€¯m Elevated bunker with radar mast, AA emplacements, and longâ€‘range sensors.

Central Plains Basin â€” (500,450), 300Ã—300â€¯m Wide open flats, extreme sightlines, heat shimmer, artillery duels.

Wind Farm Depot â€” (250,800), 120Ã—120â€¯m Turbines, maintenance sheds, interior flanking routes.

Ridge Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch, sniper nests, drone launch pads.

Longâ€‘Range Fire Corridor â€” (0,450 â†’ 400,900) Primary engagement zone; extreme range, minimal cover.

Service Road Route â€” (300,0 â†’ 450,350) Safer but predictable; vulnerable to overwatch from ridges.

Ridge Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.

Ridge Relay B â€” (150,650), +260â€¯m Comms relay hub and reconâ€‘control node.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Longâ€‘Range Fire Corridor

Major engagement zone; extreme sightlines and maximumâ€‘range duels.

Ridge Network

Highâ€‘ground overwatch; ideal for snipers, artillery, and drones.

Plains Basin

Longâ€‘range engagements distorted by heat shimmer and dust haze.

Service Road Route

Predictable but safe; vulnerable to ridge overwatch.

## Sightlines


Ridge Relay A provides longâ€‘range sensor coverage (~400â€¯m). Heat distortion and dust haze create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW dryâ€‘creek staging + armor.

Red Deployment: NE ridgeâ€‘top command + AA emplacements.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Plains, ridges, dry creek beds, wind farms, farm structures.

## Vegetation


Sparse grass, shrubs, scattered trees.

## Buildings & Props


Military: AA guns, radar dishes, bunkers. Industrial: Turbines, sheds, pipelines. Logistics: Trucks, crates, fuel drums, drones.

## Effects


Dust haze, heat shimmer, mirage distortion, wind gusts.

## Audio


Wind, distant machinery, radio chatter, drone hum.

## LOD & Budget


Asset CategoCroyunt Tri/Item Total Tris

Plains/Ridge Terrain 3 200k         600k

Industrial/Wind Farm4 80k           320k

Buildings/Military 15 50k           750k

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


1. Prototype Layout â€” Month 1 Blockâ€‘out plains basin, ridges, wind farm, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, longâ€‘range autonomy behaviors, recon logic.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (dust haze, heat shimmer).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify longâ€‘range targeting, reconâ€‘aware pathfinding, hazard avoidance.

## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under heat distortion.

## Performance


â‰¥60â€¯Hz AR render on target device.

## QA Checklist


Navigation, longâ€‘range logic, deployment budget enforcement, anchor drift, sensor accuracy.
