# Blackwater

Blackwater (River / Wetlands) Map Design

## Executive Summary


Blackwater is a dense riverâ€‘wetlands battlefield designed for water crossings, naval support, and ambushâ€‘heavy ground combat. The terrain features winding rivers, marshes, flooded forests, unstable wetlands, elevated levees, fishing docks, and military river outposts. The map emphasizes Obsidian Protocolâ€™s autonomous amphibious tactics, sensorâ€‘limited environments, commsâ€‘relay networks, and logistics under wetland constraints. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of river territory. Key features include multiâ€‘route river crossings, swamp ambush zones, naval patrol lanes, elevated levee roads, and hidden infiltration paths.

## Design Goals & Gameplay


Amphibious & Wetland Combat

Blackwater supports coordinated ground pushes, riverine naval patrols, and shallowâ€‘water vehicle crossings. Marsh terrain slows movement and encourages ambushes.
Autonomy & Ambush Logic

Autonomous AI chooses between river crossings, levee routes, swamp infiltration, or dockside flanking. Units adapt to shifting water levels and visibility.
## Sensor & Information Warfare


Fog, humidity, dense vegetation, and water reflections distort sensors. Relay towers on levees restore clarity. Destroying relays creates fogâ€‘ofâ€‘war pockets across wetlands and river channels.
## Logistics & Resources


River docks, fuel depots, and levee checkpoints serve as resupply nodes. Supply convoys must navigate unstable wetland roads and exposed river crossings.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between amphibious units, heavy ground armor, or stealth infantry. Blue deploys from the southwest riverbank; Red deploys from the northeast levee fortress.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² wetlands zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Riverbank Command) â€” (0,0), 140Ã—140â€¯m Amphibious staging zone with patrol boats and light vehicles.

Red HQ (Levee Fortress) â€” (1000,1000), 140Ã—140â€¯m Fortified elevated base with radar mast and artillery.

Central Wetland Basin â€” (500,450), 250Ã—250â€¯m Flooded forest, marsh pools, dense vegetation.

Fuel & Logistics Dock â€” (250,800), 100Ã—100â€¯m Fuel tanks, boats, supply crates, pipelines.

Fishing Village Ruins â€” (800,300), 150Ã—150â€¯m Collapsed huts, piers, boats, interior routes.

Main River Channel â€” (0,450 â†’ 400,900) Deep river with two crossings at (150,600) and (350,500).

Swamp Infiltration Route â€” (300,0 â†’ 450,350) Dense vegetation; ideal for stealth ambushes.
Levee Road Network â€” (650,150), +260â€¯m Elevated road with longâ€‘range sightlines.

Relay Tower Ridge â€” (150,650), +240â€¯m Comms relay tower and sensor hub.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


River Crossings

Critical for movement; naval units can support ground forces.
Swamp Infiltration Route

Dense vegetation; perfect for ambushes and stealth.
Levee Road Network

Longâ€‘range sightlines; ideal for artillery and recon.
Fishing Village Ruins

Closeâ€‘range combat with interior flanking routes.
## Sightlines


Relay Tower Ridge provides longâ€‘range sensor coverage (~300â€¯m). Fog and vegetation create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW riverbank + amphibious staging area.

Red Deployment: NE levee fortress + artillery positions.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Rivers, marshes, wetlands, levees, flooded forests.
## Vegetation


Dense reeds, swamp trees, mangroves, grass patches.
## Buildings & Props


Military: Bunkers, watchtowers, radar dishes. Industrial: Docks, pipelines, generators, pumps. Logistics: Boats, trucks, crates, fuel drums.
## Effects


Fog, humidity haze, water reflections, smoke plumes.
## Audio


Water movement, insects, distant machinery, radio chatter.

## LOD & Budget


Asset CategCooruynt Tri/Item Total Tris

Wetland Trees 3 200k                600k

Marsh/River Meshe4s 80k             320k

Buildings/Industria1l5 50k          750k

Roads/Levees 4 30k                  120k

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


1. Prototype Layout â€” Month 1 Blockâ€‘out river, wetlands, levees, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, amphibious pathfinding, autonomy behaviors.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (fog, water reflections).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify amphibious logic, swamp infiltration, levee pathfinding.
## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, multiâ€‘domain coordination, deployment budget enforcement, anchor drift, sensor accuracy.

