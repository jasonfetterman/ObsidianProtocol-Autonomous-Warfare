# Floodplain

Floodplain (River / Rural / Dynamic Water & Terrain Conditions)

## Executive Summary


Floodplain is a riverâ€‘rural hybrid battlefield built for dynamic waterâ€‘level combat, shifting terrain conditions, amphibious mobility, and autonomous environmental adaptation. The terrain features braided rivers, seasonal flood zones, rural farmlands, levees, wooden bridges, irrigation canals, wetlands, and small villages. The map emphasizes Obsidian Protocolâ€™s autonomous terrainâ€‘reactive logic, sensorâ€‘fusion under humidity and fog, commsâ€‘relay warfare, and logistics under constantly changing river conditions. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of rural river terrain. Key features include floodâ€‘driven chokepoints, amphibious routes, shifting mudflats, and dynamic cover created by rising or falling water.

## Design Goals & Gameplay


Dynamic Waterâ€‘Level Combat
Floodplain prioritizes combat where water levels rise and fall. Bridges become chokepoints, mudflats appear or vanish, and river channels shift midâ€‘match.
Terrainâ€‘Reactive Movement
Heavy armor struggles in soft soil; light vehicles excel. Infantry and drones adapt to flooded fields, washedâ€‘out roads, and shifting wetlands.
Amphibious & Rural Operations

Players conduct river crossings, canal assaults, and farmland pushes. Rural structures provide cover but collapse under sustained fire.
Autonomy & Environmental Adaptation

Autonomous units choose routes based on water depth, soil stability, vegetation density, and sensor clarity. Units reroute when terrain becomes impassable.
Sensor & Information Warfare

Fog, humidity, river spray, and vegetation distort sensors. Relay towers on levees restore clarity. Destroying relays creates blind pockets across the floodplain.
Logistics & Resources
Fuel depots, farm silos, and river docks serve as resupply nodes. Convoys must navigate washedâ€‘out roads and avoid ambushâ€‘prone wetlands.
Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between amphibious units, rural infantry, or heavy armor. Blue deploys from the southwest river dock; Red deploys from the northeast levee stronghold.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² rural river zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (River Dock Command) â€” (0,0), 150Ã—150â€¯m Boats, amphibious vehicles, cargo crates, rural staging.
Red HQ (Levee Stronghold) â€” (1000,1000), 150Ã—150â€¯m Elevated bunker with radar mast, AA emplacements, and floodâ€‘control systems.
Central Floodplain Basin â€” (500,450), 300Ã—300â€¯m Braided rivers, wetlands, mudflats, shifting terrain.
Farmstead Depot â€” (250,800), 120Ã—120â€¯m Barns, silos, fuel tanks, interior flanking routes.
Wetland Forest Network â€” (800,300), 200Ã—200â€¯m Dense vegetation, shallow water, stealth movement.
River Crossing Corridor â€” (0,450 â†’ 400,900) Primary amphibious chokepoint; bridges, fords, and shifting water levels.
Rural Road Route â€” (300,0 â†’ 450,350) Safer land route with partial cover.
Levee Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Levee Relay B â€” (150,650), +260â€¯m Comms relay hub and floodâ€‘monitoring node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


River Crossing Corridor

Major chokepoint; controlling it determines amphibious movement.
Floodplain Basin
Dynamic terrain; longâ€‘range engagements shift as water rises.
Wetland Forest Network

Interior flanking routes; stealthy but slow.
Rural Road Route

Predictable but safe; vulnerable to overwatch from levees.
## Sightlines

Levee Relay A provides longâ€‘range sensor coverage (~300â€¯m). Fog and humidity create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW river dock + amphibious staging.
Red Deployment: NE levee stronghold + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Rivers, wetlands, mudflats, levees, rural fields, farmsteads.
## Vegetation


Grass, reeds, wetland trees, farmland crops.
## Buildings & Props


Military: AA guns, radar dishes, bunkers. Rural: Barns, silos, houses, bridges. Logistics: Trucks, crates, fuel drums, boats, drones.
## Effects


Fog, humidity haze, water spray, mud splashes.
## Audio


River flow, insects, machinery, distant artillery, radio chatter.

## LOD & Budget


Asset CategCooruynt Tri/Item  Total Tris
600k
River/Wetland Terr3ain200k    320k
750k
Rural/Industrial 4 80k        120k
500k
Buildings/Rural 15 50k        ~2.29M

Roads/Paths 4 30k

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

1. Prototype Layout â€” Month 1 Blockâ€‘out rivers, wetlands, farmsteads, AR anchor test.
2. Core Systems â€” Month 2 NavMesh (land + water), amphibious autonomy, dynamic terrain logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (fog, water spray, mud).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify amphibious routing, dynamic terrain adaptation, hazard avoidance.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under humidity.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, dynamic terrain logic, deployment budget enforcement, anchor drift, sensor accuracy.
