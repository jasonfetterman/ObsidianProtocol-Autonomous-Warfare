# Skyline

Skyline (Urban / Highâ€‘Rise / Air Reconnaissance & Vertical Battlefield)

## Executive Summary


Skyline is a towering highâ€‘rise urban battlefield built for air reconnaissance, vertical combat, rooftop warfare, and multiâ€‘level autonomous operations. The terrain features skyscraper clusters, rooftop landing pads, elevated skybridges, interior atriums, ventilation corridors, commercial towers, and dense streetâ€‘level grids. The map emphasizes Obsidian Protocolâ€™s autonomous verticalâ€‘navigation logic, airâ€‘ground coordination, sensorâ€‘relay networks, and logistics under extreme verticality. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of highâ€‘rise urban terrain. Key features include rooftop vantage points, skybridge chokepoints, multiâ€‘tier combat zones, and strategic control of airspace corridors.

## Design Goals & Gameplay


Air Reconnaissance & Vertical Dominance

Skyline prioritizes aerial recon, rooftop overwatch, VTOL deployment, drone scouting, and vertical flanking. Airspace corridors between towers define movement and targeting.
Multiâ€‘Level Urban Combat
Combat unfolds across street level, midâ€‘tower floors, rooftops, and skybridges. Units must navigate elevators, stairwells, maintenance shafts, and rooftop pads.
Autonomy & Vertical Pathfinding
Autonomous units choose between rooftop routes, interior corridors, skybridge traversal, or streetâ€‘level infiltration. Units adapt to blocked floors and shifting airspace control.
## Sensor & Information Warfare


Glass reflections, heat plumes, smoke, and vertical occlusion distort sensors. Relay towers on rooftops restore clarity. Destroying relays creates blind pockets across tower clusters.
## Logistics & Resources

Commercial loading bays, rooftop pads, and maintenance depots serve as resupply nodes. Convoys must navigate narrow streets and avoid ambushâ€‘prone tower bases.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between air support, rooftop infantry, or heavy urban armor. Blue deploys from the southwest tower plaza; Red deploys from the northeast corporate spire.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² highâ€‘rise zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Tower Plaza Command) â€” (0,0), 150Ã—150â€¯m Rooftop pads, drone bays, armored staging area.
Red HQ (Corporate Spire Command) â€” (1000,1000), 150Ã—150â€¯m Skyscraper command node with radar mast and AA emplacements.
Central Skyline Cluster â€” (500,450), 300Ã—300â€¯m Highâ€‘rise towers, skybridges, rooftop combat zones.
Commercial Loading Bay â€” (250,800), 120Ã—120â€¯m Cargo docks, trucks, interior flanking routes.
Atrium Tower Complex â€” (800,300), 200Ã—200â€¯m Multiâ€‘level interior combat, elevators, stairwells.
Skybridge Network â€” (0,450 â†’ 400,900) Elevated traversal routes; critical vertical chokepoint.
Streetâ€‘Level Grid â€” (300,0 â†’ 450,350) Dense urban streets; ideal for armor and autonomous squads.
Rooftop Relay A â€” (650,150), +280â€¯m Longâ€‘range sensor vantage point.
Rooftop Relay B â€” (150,650), +300â€¯m Comms relay tower and drone hub.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Skybridge Network
Major vertical chokepoint; controlling it determines towerâ€‘toâ€‘tower movement.

Atrium Tower Complex

Interior vertical combat; elevators and stairwells create unpredictable engagements.
Skyline Cluster Rooftops
Longâ€‘range sightlines; ideal for airâ€‘ground coordination.
Streetâ€‘Level Grid
Closeâ€‘range combat; armor and autonomous infantry excel.
## Sightlines

Rooftop Relay A provides longâ€‘range sensor coverage (~350â€¯m). Glass reflections and smoke create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW tower plaza + rooftop pads.
Red Deployment: NE corporate spire + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain

Highâ€‘rises, skybridges, rooftops, atriums, street grids.
## Vegetation


Planters, rooftop gardens, sparse trees.
## Buildings & Props


Military: AA guns, radar dishes, watchtowers. Urban: Offices, apartments, commercial towers, skybridges. Industrial: Loading bays, generators, HVAC units. Logistics: Trucks, crates, drones, fuel drums.
## Effects


Smoke, sparks, dust, heat shimmer, glass reflections.
## Audio


Wind between towers, machinery, distant aircraft, radio chatter.

## LOD & Budget


Asset CategCooruynt Tri/Item     Total Tris
600k
Highâ€‘Rise Buildings3 200k        320k
750k
Skybridges/Urban S4tru8c0tkures  120k
240k
Buildings/Comme1rc5ial50k        500k
~2.53M
Roads/Paths 4 30k

Vegetation 120 2k

Props     50 10k

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

1. Prototype Layout â€” Month 1 Blockâ€‘out towers, skybridges, atriums, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, vertical autonomy behaviors, airâ€‘ground coordination.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (glass reflections, smoke).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify vertical navigation, rooftop logic, skybridge traversal.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, vertical behavior, deployment budget enforcement, anchor drift, sensor accuracy.

