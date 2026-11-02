# Dark Forest

Dark Forest (Dense Forest / Night) Map Design

## Executive Summary


Dark Forest is a nightâ€‘time dense woodland battlefield built for thermal warfare, sensor disruption, stealth movement, and information denial. The terrain features thick canopy cover, tangled undergrowth, moonlit clearings, abandoned research cabins, shallow creeks, and elevated ridgelines. Darkness, humidity, and foliage create extreme visibility challenges. The map emphasizes Obsidian Protocolâ€™s autonomous night â€‘combat behaviors, thermalâ€‘sensor logic, commsâ€‘relay warfare, and logistics under lowâ€‘light constraints. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of nighttime forest terrain. Key features include thermal chokepoints, sensorâ€‘blind zones, hidden trails, and ambushâ€‘heavy infiltration routes.

## Design Goals & Gameplay


Night Combat & Thermal Warfare
Dark Forest prioritizes engagements where thermal imaging, IR sensors, and lowâ€‘light optics dominate. Units rely on heat signatures rather than visual clarity.
Autonomy & Lowâ€‘Visibility Pathfinding

Autonomous AI selects concealed trails, avoids moonlit clearings, and uses darkness for stealth. Units adapt to shifting fog density and thermal interference.
## Sensor & Information Warfare

Dense canopy, humidity, fog, and temperature gradients distort sensors. Relay towers on ridges restore clarity. Destroying relays creates massive thermalâ€‘blind zones.
## Logistics & Resources

Ranger cabins, research outposts, and forest depots serve as resupply nodes. Supply convoys must navigate narrow trails and avoid ambushâ€‘prone darkness pockets.
## Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between stealth infantry, thermal drones, or light vehicles. Blue deploys from the southwest nightâ€‘ops camp; Red deploys from the northeast ridge bunker.

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

Blue HQ (Nightâ€‘Ops Camp) â€” (0,0), 140Ã—140â€¯m Thermalâ€‘equipped forward base with recon drones.
Red HQ (Ridge Bunker) â€” (1000,1000), 140Ã—140â€¯m Elevated bunker with comms tower and IR floodlights.
Central Shadow Basin â€” (500,450), 250Ã—250â€¯m Dense canopy, fog pockets, minimal visibility.
Research Outpost â€” (250,800), 100Ã—100â€¯m Abandoned labs, generators, thermal equipment.
Old Ranger Cabins â€” (800,300), 120Ã—120â€¯m Interior routes, closeâ€‘range ambush zones.
Creek Network â€” (0,450 â†’ 400,900) Shallow water crossings with thermal distortion.
Hidden Night Trail â€” (300,0 â†’ 450,350) Concealed path ideal for stealth infiltration.
Thermal Ridge A â€” (650,150), +260â€¯m Longâ€‘range thermal vantage point.
Relay Ridge B â€” (150,650), +240â€¯m Comms relay tower and sensor hub.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Hidden Night Trail

Perfect for ambushes; extremely narrow and concealed.
Creek Crossings

Water cools heat signatures; thermal sensors degrade.

Cabin Cluster
Interior flanking routes; closeâ€‘range engagements.
Shadow Basin

Heavy foliage + darkness = unpredictable sightlines.
## Sightlines

Thermal Ridge A provides longâ€‘range sensor coverage (~300â€¯m). Fog and temperature gradients create thermalâ€‘blind pockets.

## Deployment Zones


Blue Deployment: SW nightâ€‘ops camp + rear rally point.
Red Deployment: NE ridge bunker + IR floodlights.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Forest hills, undergrowth, creeks, ridges, fallen logs.
## Vegetation


Dense trees, shrubs, ferns, moss, fog pockets.
## Buildings & Props


Military: IR floodlights, antennas, bunkers. Civilian: Cabins, sheds, research equipment. Logistics: Trucks, crates, fuel drums, thermal drones.
## Effects


Fog, humidity haze, drifting mist, falling leaves, thermal distortion.
## Audio


Wind, insects, distant machinery, radio chatter, night wildlife.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Forest Trees         3   80k       320k
50k       750k
Underbrush/Rocks4        30k       120k
2k        400k
Buildings/Cabins 15      10k       500k
â€”         ~2.69M
Roads/Trails         4

Vegetation 200

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

1. Prototype Layout â€” Month 1 Blockâ€‘out forest basin, ridges, cabins, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, thermalâ€‘aware pathfinding, autonomy behaviors.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (fog, thermal distortion).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify stealth logic, thermalâ€‘aware navigation, ambush behavior.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under night conditions.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, concealment behavior, thermal response, deployment budget enforcement, anchor drift, sensor accuracy.

