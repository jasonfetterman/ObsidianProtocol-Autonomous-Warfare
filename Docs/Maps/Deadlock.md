# Deadlock

Deadlock (Urban / Closeâ€‘Range Urban Warfare) Map Design

## Executive Summary


Deadlock is a dense, warâ€‘torn urban battlefield designed for brutal closeâ€‘quarters combat, rapid autonomous decisionâ€‘making, and highâ€‘intensity chokepoint engagements. The map features collapsed buildings, tight alley networks, fortified intersections, underground metro tunnels, and rooftop traversal routes. It emphasizes Obsidian Protocolâ€™s autonomous tactics, sensorâ€‘limited environments, commsâ€‘relay warfare, and logistics under urban constraint. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of dense city terrain. Key features include multiâ€‘level combat zones, interior building routes, barricaded streets, and critical objective hubs.

## Design Goals & Gameplay


Closeâ€‘Range Urban Combat

Deadlock focuses on infantry and lightâ€‘vehicle engagements. Streets are narrow, visibility is limited, and combat is fast. Buildings create verticality and interior routes that allow ambushes and flanking.
Autonomy & Microâ€‘Flanking

Autonomous AI can choose alley shortcuts, breach interior rooms, or reposition to rooftops. Units adapt to collapsing structures, blocked roads, and shifting cover.
## Sensor & Information Warfare


Urban clutter creates radar scatter, thermal occlusion, and acoustic distortion. Rooftop relay towers restore sensor clarity. Destroying relays plunges entire districts into fogâ€‘ofâ€‘war.
## Logistics & Resources


Supply trucks must navigate blocked roads and debris. Controlling the Central Transit Hub and Logistics Depot provides major resupply advantages. Urban logistics are slow and vulnerable.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between heavy breaching units or fast recon squads. Blue deploys from the southwest district; Red deploys from the northeast highâ€‘rise zone.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² urban district (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Location CoordinatesSize            Description

Blue HQ (SW(0D,0i)strict) 120Ã—120â€¯m Police station + fortified street barricades
Red HQ (NE H##ig#h#â€‘#R#i#s#e Z1o2n0eÃ—)120â€¯m Corporate tower plaza with comms mast

Central Tran-s5it0H0,u5b00 200Ã—200â€¯m Metro entrances, bus terminals, collapsed platforms

Logistics Dep-3o0t0,800 80Ã—80â€¯m Supply trucks, crates, fuel drums
Highâ€‘Rise Cl-u7s0te0r,300 150Ã—150â€¯m Tall buildings, rooftop traversal routes

Old Market D-2i0st0r,i3c0t 0 100Ã—100â€¯m Tight alleys, shops, interior routes

Undergroun(d40M0e,0trâ†’o T6u0â€”n0n,4e0l 0) Subterranean flanking route

Skybridge N(e6t5w0o,7r0k0 â†’â€”850,900) Elevated walkway connecting rooftops

River & Brid(g0e,4s50 â†’ 35â€”0,450) Two urban bridges at (150,450) and (300,450)

Observation-9To0w0,e2r00 +200â€¯m Sensor vantage point

Neutral Forw-5a0rd0,S9a5f0ehoâ€”use   Drone resupply zone

## Chokepoints & Sightlines


Main Intersection (Deadlock Point)

The central intersection at (520,520) is the namesake chokepoint â€” four streets converge, all partially blocked by debris.
Metro Tunnel

Low visibility, tight corridors, ideal for ambushes.
Skybridge Network

Highâ€‘ground traversal with long sightlines but exposed positions.
Bridges

Critical for crossâ€‘district movement; controlling them prevents flanking.
## Sightlines


Observation Tower provides longâ€‘range sensor coverage (~250â€¯m). Alley networks create heavy fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW District + rear rally point.
Red Deployment: NE Highâ€‘Rise Zone + rear rally point.

Neutral Forward Zone: Safehouse at (500,950) for drone drops.

## Assets & Environment


Terrain / Structures

Collapsed buildings, debris piles, broken asphalt, interior rooms, rooftops, skybridges.
## Vegetation


Sparse urban trees, planter boxes, grass patches.
## Buildings & Props


Urban: Apartments, shops, offices, parking garages.

Military: Barricades, sandbags, watchtowers.

Logistics: Trucks, crates, forklifts, fuel drums.

Transit: Metro entrances, platforms, rails.
## Effects


Smoke plumes, sparks, flickering lights, dust clouds.
## Audio


Sirens, distant explosions, radio chatter, wind between buildings.

## LOD & Budget


Asset CategCooruynt Tri/Item Total Tris

Buildings/Structure2s0 60k          1.2M

Roads/Bridges 6 30k                 180k

Debris/Props 80 8k                  640k

Vegetation     100 2k               200k

Transit Assets 20 20k               400k

Total       â€”     â€”                 ~2.62M

## Texture Budget


2048Â² for buildings/roads, 1024Â² for props. DXT1/5 compression. Target â‰¤100â€¯MB.

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


Milestone Timeframe Deliverables
1. PrototypeMLoanytohu1t Blockâ€‘out buildings, streets, metro, AR anchor test

2. Core SystMemonsth 2 NavMesh, pathfinding, autonomy behaviors

3. AR IntegrMatoionnth 3 Anchors, plane detection, basic interactivity

4. Content FMillonth 4â€“5 Final models, textures, lighting, audio

5. GameplayM&onPtohli6sâ€“h7 Objectives, balance, VFX (smoke, sparks)

6. Testing &MQoAnth 8 Multiplayer stress test, sensor validation

## Testing Plan


## Autonomy


Verify AI breaching logic, alley navigation, rooftop traversal.
## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, cover usage, deployment budget enforcement, anchor drift, sensor accuracy.

