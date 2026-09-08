# Greenline

Greenline (Forest / Recon, Concealment, Ambush Warfare) Map Design

## Executive Summary


Greenline is a dense forestâ€‘recon battlefield designed for concealment, stealth, ambush warfare, and sensorâ€‘limited engagements. The terrain features thick woodland, rolling hills, hidden trails, logging sites, ranger stations, shallow streams, and dense undergrowth. The map emphasizes Obsidian Protocolâ€™s autonomous stealth behaviors, sensorâ€‘fusion under heavy occlusion, commsâ€‘relay networks, and logistics under forest constraints. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of forest territory. Key features include ambushâ€‘heavy choke zones, concealed movement paths, elevated ridgelines, and multiâ€‘route infiltration corridors.

## Design Goals & Gameplay


Forest Recon & Concealment Combat

Greenline prioritizes stealth, scouting, and ambush tactics. Dense vegetation and uneven terrain create natural concealment and unpredictable sightlines.
Autonomy & Stealth Pathfinding

Autonomous AI selects hidden trails, avoids open clearings, and uses undergrowth for concealment. Units adapt to shifting foliage density and sensor occlusion.
## Sensor & Information Warfare


Trees, foliage, humidity, and terrain occlusion degrade sensors. Relay towers on ridges restore clarity. Destroying relays creates fogâ€‘ofâ€‘war pockets across forest sectors.
## Logistics & Resources


Logging sites, ranger stations, and forest depots serve as resupply nodes. Supply convoys must navigate narrow trails and avoid ambushâ€‘prone choke zones.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between stealth infantry, recon drones, or light vehicles. Blue deploys from the southwest ranger station; Red deploys from the northeast logging ridge.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² forest zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Ranger Station Command) â€” (0,0), 140Ã—140â€¯m Forest outpost with recon vehicles and drone pads.

Red HQ (Logging Ridge Base) â€” (1000,1000), 140Ã—140â€¯m Elevated ridge with logging machinery and comms tower.

Central Forest Basin â€” (500,450), 250Ã—250â€¯m Dense woodland, undergrowth, fallen trees, low visibility.

Logging Depot â€” (250,800), 100Ã—100â€¯m Cut timber stacks, machinery, fuel drums.
Old Cabin Cluster â€” (800,300), 120Ã—120â€¯m Abandoned cabins, interior routes, closeâ€‘range combat.

Stream Network â€” (0,450 â†’ 400,900) Shallow water crossings with partial cover.

Hidden Trail Route â€” (300,0 â†’ 450,350) Narrow concealed path ideal for stealth infiltration.
Ridgeline A â€” (650,150), +260â€¯m Longâ€‘range sensor vantage point.

Ridgeline B â€” (150,650), +240â€¯m Comms relay tower and recon hub.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Hidden Trail Route

Perfect for ambushes; extremely narrow and concealed.
Stream Crossings

Shallow water slows movement; vulnerable during crossing.
Cabin Cluster

Interior flanking routes; closeâ€‘range engagements.
Forest Basin

Heavy foliage; unpredictable sightlines and stealth combat.
## Sightlines


Ridgeline A provides longâ€‘range sensor coverage (~300â€¯m). Dense foliage creates fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW ranger station + rear rally point.

Red Deployment: NE logging ridge + elevated artillery.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Forest hills, undergrowth, streams, ridges, fallen logs.
## Vegetation


Dense trees, shrubs, ferns, grass patches, moss.
## Buildings & Props


Military: Watchtowers, antennas, bunkers. Civilian: Cabins, sheds, logging machinery. Logistics: Trucks, crates, fuel drums, recon drones.
## Effects


Fog, humidity haze, falling leaves, dust, insects.
## Audio


Wind through trees, birds, insects, distant machinery, radio chatter.

## LOD & Budget


Asset CategCooruynt Tri/Item Total Tris

Forest Trees    3 200k              600k

Underbrush/Rocks 4 80k              320k

Buildings/Cabins 15 50k             750k

Roads/Trails    4 30k               120k

Vegetation     200 2k               400k

Props          50 10k               500k

Total       â€”     â€”                 ~2.69M

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


1. Prototype Layout â€” Month 1 Blockâ€‘out forest basin, ridges, cabins, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, stealth pathfinding, autonomy behaviors.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (fog, leaves, humidity haze).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify stealth logic, hidden trail navigation, ambush behavior.
## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, concealment behavior, deployment budget enforcement, anchor drift, sensor accuracy.

