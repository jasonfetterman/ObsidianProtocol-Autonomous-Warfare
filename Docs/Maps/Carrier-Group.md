# Carrier Group

Carrier Group (Ocean / Naval / Fleet Warfare)

## Executive Summary


Carrier Group is a deepâ€‘ocean naval battlefield built for fleetâ€‘scale warfare, carrierâ€‘based air operations, autonomous maritime coordination, and multiâ€‘domain combat across sea, air, and longâ€‘range missile envelopes. The environment features open ocean sectors, shifting wave patterns, carrier decks, destroyer screens, submarine lanes, missile arcs, radar pickets, and logistics ships. The map emphasizes Obsidian Protocolâ€™s autonomous naval routing, sensor â€‘fusion under sea clutter and radar interference, commsâ€‘relay warfare, and logistics across a vast, mobile battlespace. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of ocean terrain. Key features include carrier air wings, destroyer screens, submarine shadow zones, and long â€‘range missile duels.

## Design Goals & Gameplay


Fleetâ€‘Scale Naval Warfare

Carrier Group prioritizes coordinated fleets: carriers, destroyers, cruisers, submarines, drones, and aircraft. Engagements occur across massive distances.

Carrier Air Wing Operations

Fighters, bombers, VTOLs, and recon drones launch from carrier decks. Air superiority determines strike capability and fleet survival.

Destroyer Screens & Missile Defense

Destroyers and cruisers form layered defense rings using AA guns, CIWS, missile interceptors, and EW systems.

Submarine Shadow Zones
Submarines exploit deepâ€‘water lanes, thermal layers, and sonar shadows to ambush surface ships or disrupt logistics.

Autonomous Maritime Routing

Autonomous units choose routes based on wave patterns, radar clarity, sonar returns, missile arcs, and threat vectors.

Sensor & Information Warfare

Radar clutter, sea spray, thermal layers, jamming, and EW interference degrade sensors. Relay ships restore clarity. Destroying relays creates blind pockets across the ocean.

Logistics & Resources

Fuel ships, supply vessels, and repair barges serve as resupply nodes. Protecting logistics ships is critical for sustained operations.

Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between air dominance, missile supremacy, submarine control, or balanced fleet composition. Blue deploys from the southwest carrier strike group; Red deploys from the northeast cruiser â€‘led battle group.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² ocean zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations


Blue HQ (Carrier Strike Group) â€” (0,0), 150Ã—150â€¯m Carrier deck, destroyer screen, drone pads, logistics ship.
Red HQ (Cruiser Battle Group) â€” (1000,1000), 150Ã—150â€¯m Cruiser flagship, radar mast, AA emplacements, missile batteries.
Central Ocean Combat Basin â€” (500,450), 300Ã—300â€¯m Open ocean, wave patterns, missile arcs, airâ€‘sea engagements.
Logistics Ship Cluster â€” (250,800), 120Ã—120â€¯m Fuel ships, repair barges, interior escort routes.
Radar Picket Line â€” (800,300), 200Ã—200â€¯m Highâ€‘range radar ships, EW nodes, airâ€‘corridor control.
Missile Corridor â€” (0,450 â†’ 400,900) Primary longâ€‘range missile lane; interception routes and EW traps.
Submarine Shadow Route â€” (300,0 â†’ 450,350) Deepâ€‘water thermal layer; ideal for stealth submarine movement.
Relay Ship A â€” (650,150), +240â€¯m Longâ€‘range radar vantage point.
Relay Ship B â€” (150,650), +260â€¯m Comms relay hub and EWâ€‘control node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Missile Corridor
Major engagement lane; controlling it determines longâ€‘range strike capability.

Radar Picket Line
Extreme longâ€‘range sightlines; ideal for airâ€‘sea coordination and EW control.

Ocean Combat Basin
Airâ€‘sea engagements distorted by radar clutter and wave interference.

Submarine Shadow Route
Predictable but stealthâ€‘heavy; vulnerable to ASW patrols.

## Sightlines

Relay Ship A provides longâ€‘range radar coverage (~400â€¯m). Sea clutter and EW interference create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW carrier strike group + air wing.
Red Deployment: NE cruiser battle group + missile batteries.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Open ocean, wave patterns, carrier decks, destroyers, cruisers, submarines.

## Vegetation


None â€” ocean surface only.

## Buildings & Props


Military: AA guns, radar dishes, missile launchers, EW towers. Naval: Ships, cranes, decks, lifeboats. Logistics: Fuel ships, repair barges, drones.

## Effects


Sea spray, smoke, missile trails, radar waves, static bursts.

## Audio


Waves, engines, radar hum, radio chatter, jet noise.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Ocean/Ship Terrain3      80k       320k
50k       750k
Naval/Industrial 4       30k       120k
10k       500k
Ships/Military 15        â€”         ~2.29M

Routes/Paths 4

Props                50

Total â€”

## Texture Budget


2048Â² for ships/terrain, 1024Â² for props. DXT1/5 compression. Target â‰¤100â€¯MB.

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

1. Prototype Layout â€” Month 1 Blockâ€‘out ocean basin, ship positions, radar picket line, AR anchor test.
2. Core Systems â€” Month 2 NavMesh (sea + air), missile autonomy behaviors, radar logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (missile trails, radar waves).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify airâ€‘sea coordination, missileâ€‘aware routing, hazard avoidance.

## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under EW interference.

## Performance


â‰¥60â€¯Hz AR render on target device.

## QA Checklist


Navigation, naval logic, deployment budget enforcement, anchor drift, sensor accuracy.
