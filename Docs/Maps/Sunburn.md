# Sunburn

Sunburn (Desert / Canyon / Longâ€‘Range Fire & Air Superiority)

## Executive Summary


Sunburn is a blistering desertâ€‘canyon battlefield built for longâ€‘range firepower, air superiority, highâ€‘altitude reconnaissance, and autonomous maneuvering across open and vertical terrain. The environment features sandstone canyons, windâ€‘carved ridges, dry riverbeds, elevated plateaus, abandoned mining sites, and sparse desert settlements. The map emphasizes Obsidian Protocolâ€™s autonomous long â€‘range targeting logic, airâ€‘ground coordination, sensorâ€‘fusion under heat distortion, and logistics under extreme desert conditions. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of canyon â€‘desert terrain. Key features include canyon sightlines, highâ€‘ground air corridors, longâ€‘range artillery zones, and heatâ€‘driven sensor degradation.

## Design Goals & Gameplay


Longâ€‘Range Desert Firepower
Sunburn prioritizes snipers, artillery, missile vehicles, and longâ€‘range drones. Open desert flats and canyon ridges create extreme sightlines.

Air Superiority & Highâ€‘Altitude Recon

Aircraft, VTOLs, and drones dominate the vertical dimension. Air corridors between canyon walls define recon, interception, and strike patterns.

Canyon Maneuvering & Vertical Combat
Ground units navigate narrow canyon passes, elevated ridges, and dry riverbeds. Verticality creates ambush zones and longâ€‘range overwatch positions.

Autonomy & Heatâ€‘Aware Pathfinding

Autonomous units choose routes based on heat shimmer, elevation, cover scarcity, and sensor clarity. Units avoid exposed flats during peak heat unless necessary.

Sensor & Information Warfare

Heat distortion, dust storms, mirage effects, and thermal plumes degrade sensors. Relay towers on ridges restore clarity. Destroying relays creates blind pockets across the canyon network.

Logistics & Resources
Fuel depots, mining stations, and desert outposts serve as resupply nodes. Convoys must navigate exposed roads and avoid ambush â€‘prone canyon passes.

Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between longâ€‘range artillery, air dominance, or canyonâ€‘adapted armor. Blue deploys from the southwest dryâ€‘river staging zone; Red deploys from the northeast ridgeâ€‘top command.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² canyonâ€‘desert zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations

Blue HQ (Dryâ€‘River Staging Zone) â€” (0,0), 150Ã—150â€¯m Cargo crates, armored vehicles, drone pads.
Red HQ (Ridgeâ€‘Top Command) â€” (1000,1000), 150Ã—150â€¯m Elevated bunker with radar mast, AA emplacements, and longâ€‘range sensors.
Central Canyon Basin â€” (500,450), 300Ã—300â€¯m Wide canyon floor, long sightlines, heat shimmer, artillery duels.
Mining Station Depot â€” (250,800), 120Ã—120â€¯m Abandoned machinery, fuel tanks, interior flanking routes.
Ridge Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch, sniper nests, airâ€‘corridor control.
Canyon Pass Corridor â€” (0,450 â†’ 400,900) Primary chokepoint; narrow, exposed, ideal for ambushes.
Desert Road Route â€” (300,0 â†’ 450,350) Safer but predictable; vulnerable to overwatch from ridges.
Ridge Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Ridge Relay B â€” (150,650), +260â€¯m Comms relay hub and airâ€‘control node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Canyon Pass Corridor

Major chokepoint; controlling it determines ground movement.

Ridge Network
Extreme longâ€‘range sightlines; ideal for snipers and airâ€‘ground coordination.

Canyon Basin
Longâ€‘range artillery and missile engagements; heat shimmer affects accuracy.

Desert Road Route

Predictable but safe; vulnerable to ridge overwatch.

## Sightlines

Ridge Relay A provides longâ€‘range sensor coverage (~400â€¯m). Heat distortion and dust storms create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW dryâ€‘river staging + armor.
Red Deployment: NE ridgeâ€‘top command + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Canyons, ridges, desert flats, dry riverbeds, mining stations.

## Vegetation


Sparse shrubs, desert grass, small trees.

## Buildings & Props


Military: AA guns, radar dishes, bunkers. Industrial: Mining equipment, generators, pipelines. Logistics: Trucks, crates, fuel drums, drones.

## Effects


Dust storms, heat shimmer, mirage distortion, wind gusts.

## Audio


Wind, distant machinery, radio chatter, aircraft.

## LOD & Budget


Asset CategCooruynt Tri/Item  Total Tris
600k
Canyon/Ridge Terr3ain200k     320k
750k
Industrial/Mining 4 80k       120k
500k
Buildings/Military15 50k      ~2.29M

Roads/Paths 4 30k

Props        50 10k

Total â€”            â€”

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

1. Prototype Layout â€” Month 1 Blockâ€‘out canyon basin, ridges, mining station, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, longâ€‘range autonomy behaviors, airâ€‘ground coordination.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (dust storms, heat shimmer).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify longâ€‘range targeting, airâ€‘ground coordination, heatâ€‘aware navigation.

## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under heat distortion.

## Performance


â‰¥60â€¯Hz AR render on target device.

## QA Checklist

Navigation, longâ€‘range logic, deployment budget enforcement, anchor drift, sensor accuracy.
