# Stormwatch

Stormwatch (Coastal / Storm) Map Design

## Executive Summary


Stormwatch is a violent stormâ€‘ridden coastal battlefield designed around weather disruption, visibility loss, and sensor degradation. The terrain features jagged cliffs, stormâ€‘battered beaches, flooded roads, industrial seawalls, offshore turbines, and emergency shelters. The map emphasizes Obsidian Protocolâ€™s autonomous adaptation to extreme weather, sensorâ€‘fusion under interference, commsâ€‘relay survival, and logistics under rapidly shifting environmental conditions. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of stormâ€‘struck coastline. Key features include lightningâ€‘damaged infrastructure, storm surge zones, flooded combat routes, and multiâ€‘domain operations under severe visibility constraints.

## Design Goals & Gameplay


Extreme Weather Combat

Stormwatch prioritizes combat under heavy rain, fog, lightning, and wind. Visibility constantly shifts, affecting targeting, movement, and sensor reliability.
Autonomy & Weather Adaptation
Autonomous AI reroutes around flooded roads, avoids lightningâ€‘exposed high ground, and uses sheltered structures for cover. Units adapt to rapidly changing visibility.
## Sensor & Information Warfare


Rain scatter, fog density, lightning interference, and turbulent air distort sensors. Relay towers on cliffs and turbines restore clarity. Destroying relays plunges entire sectors into sensor blackout.
## Logistics & Resources

Stormâ€‘damaged docks, emergency shelters, and industrial turbines serve as resupply nodes. Supply convoys must navigate flooded roads and exposed coastal paths.
## Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between stormâ€‘resistant units, fast recon drones, or heavy armor. Blue deploys from the southwest storm shelter; Red deploys from the northeast cliffside command.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² stormâ€‘struck coastal zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations

Blue HQ (Storm Shelter Command) â€” (0,0), 140Ã—140â€¯m Reinforced emergency shelter with stormâ€‘resistant vehicles.
Red HQ (Cliffside Command Tower) â€” (1000,1000), 140Ã—140â€¯m Highâ€‘ground command tower with radar mast and AA emplacements.
Central Floodplain â€” (500,450), 250Ã—250â€¯m Flooded roads, debris, abandoned vehicles, low visibility.
Industrial Turbine Field â€” (250,800), 120Ã—120â€¯m Wind turbines, generators, lightningâ€‘damaged infrastructure.
Stormâ€‘Damaged Dockyard â€” (800,300), 150Ã—150â€¯m Broken piers, boats, cranes, flooded warehouses.
Storm Surge Channel â€” (0,450 â†’ 400,900) Rapid water flow; dangerous crossing zones.
Sheltered Coastal Road â€” (300,0 â†’ 450,350) Partially covered route; reduced wind exposure.
Cliff Relay Ridge â€” (650,150), +260â€¯m Relay tower with longâ€‘range sensor coverage.
Fogâ€‘Heavy Forest Patch â€” (150,650), +240â€¯m Dense fog; stealth and ambush zone.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Storm Surge Channel

Crossings are hazardous; naval units can support ground forces.
Floodplain
Low visibility; ideal for ambushes and closeâ€‘range combat.

Dockyard Ruins
Mixedâ€‘range combat with interior flanking routes.
Cliff Relay Ridge
Longâ€‘range sightlines but exposed to lightning.
## Sightlines

Relay Ridge provides longâ€‘range sensor coverage (~300â€¯m). Fog, rain, and lightning create dynamic fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW storm shelter + rear rally point.
Red Deployment: NE cliffside command tower + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Cliffs, beaches, flooded plains, storm surge channels, coastal roads.
## Vegetation

Sparse coastal shrubs, wetland grasses, windâ€‘bent trees.
## Buildings & Props


Military: Bunkers, AA guns, radar dishes, watchtowers. Industrial: Turbines, generators, cranes, pipelines. Logistics: Boats, trucks, crates, fuel drums.
## Effects


Heavy rain, fog, lightning flashes, wind gusts, water spray.
## Audio


Thunder, waves, wind, machinery, radio chatter.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Cliffs/Coastal Rock3     80k       320k
50k       750k
Floodplain/Roads 4       30k       120k
2k        300k
Buildings/Industri1a5l   10k       500k
â€”         ~2.59M
Roads/Paths 4

Vegetation 150

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

1. Prototype Layout â€” Month 1 Blockâ€‘out cliffs, floodplain, dockyard, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, weatherâ€‘adaptive autonomy behaviors.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (rain, fog, lightning).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify weatherâ€‘adaptive logic, floodplain navigation, stormâ€‘surge avoidance.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under weather.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, weather response, deployment budget enforcement, anchor drift, sensor accuracy.

