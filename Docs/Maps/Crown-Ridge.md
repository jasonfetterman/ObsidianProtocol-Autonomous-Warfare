# Crown Ridge

Crown Ridge (Mountain / Military / Command Networks & Strategic Positioning)

## Executive Summary


Crown Ridge is a fortified mountainâ€‘military battlefield built for commandâ€‘network warfare, strategic positioning, elevation control, and longâ€‘range coordination. The terrain features steep alpine ridges, hardened bunkers, command relays, fortified passes, elevated artillery platforms, and multiâ€‘tier mountain roads. The map emphasizes Obsidian Protocolâ€™s autonomous commandâ€‘network logic, sensorâ€‘relay chaining, airâ€‘ground coordination, and logistics under highâ€‘altitude constraints. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of mountainous territory. Key features include commandâ€‘relay chains, multiâ€‘tier defensive positions, elevationâ€‘driven chokepoints, and strategic highâ€‘ground control.

## Design Goals & Gameplay


Commandâ€‘Network Warfare
Crown Ridge prioritizes commandâ€‘relay nodes, sensor hubs, and strategic communication lines. Destroying or capturing relays shifts control of entire sectors.
Elevation & Strategic Positioning
Highâ€‘ground positions dominate artillery arcs, recon coverage, and defensive lines. Controlling ridges is essential for longâ€‘range dominance.
Autonomy & Commandâ€‘Chain Logic
Autonomous AI selects routes that maintain commandâ€‘network integrity, avoids exposed plateaus, and uses bunkers to break lineâ€‘ofâ€‘sight.
## Sensor & Information Warfare


Thin air, fog pockets, and elevation gradients distort sensors. Relay towers on peaks restore clarity. Destroying relays creates massive blind zones across the ridge.
## Logistics & Resources

Mountain depots, command bunkers, and helipads serve as resupply nodes. Supply convoys must navigate narrow roads and avoid ambushâ€‘prone passes.
## Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between heavy artillery, commandâ€‘relay units, or mobile mountain infantry. Blue deploys from the southwest valley command; Red deploys from the northeast ridge citadel.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² mountain zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations

Blue HQ (Valley Command Base) â€” (0,0), 140Ã—140â€¯m Mountainâ€‘foot command center with relay uplinks and artillery.
Red HQ (Ridge Citadel) â€” (1000,1000), 140Ã—140â€¯m Fortified highâ€‘ground citadel with radar mast and AA emplacements.
Central Crown Plateau â€” (500,450), 250Ã—250â€¯m Windâ€‘exposed plateau with longâ€‘range sightlines and command relay nodes.
Command Relay Depot â€” (250,800), 100Ã—100â€¯m Relay towers, generators, comms equipment, supply crates.
Cliffside Bunker Network â€” (800,300), 150Ã—150â€¯m Interior tunnels, firing ports, hardened defensive positions.
Ridge Pass Corridor â€” (0,450 â†’ 400,900) Narrow elevated corridor; critical choke zone for commandâ€‘network control.
Lower Valley Road â€” (300,0 â†’ 450,350) Safer movement route with partial cover.
Peak A (Crown Peak) â€” (650,150), +300â€¯m Primary commandâ€‘relay hub with longâ€‘range sensor coverage.
Peak B (Sentinel Peak) â€” (150,650), +280â€¯m Secondary relay tower and AA platform.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Ridge Pass Corridor
Extremely narrow; ideal for defensive artillery and commandâ€‘relay protection.

Cliffside Bunker Network

Interior routes and firing ports; strong defensive positions.
Crown Plateau
Longâ€‘range sightlines; commandâ€‘network dominance essential.
Lower Valley Road

Safer but predictable; vulnerable to aerial recon.
## Sightlines

Peak A provides longâ€‘range sensor coverage (~350â€¯m). Fog pockets and elevation gradients create blind zones.

## Deployment Zones


Blue Deployment: SW valley command base + rear rally point.
Red Deployment: NE ridge citadel + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Cliffs, ridges, plateaus, valleys, rock formations.
## Vegetation


Sparse alpine shrubs, pine clusters, grass patches.
## Buildings & Props


Military: Bunkers, AA guns, radar dishes, helipads, command relays. Industrial: Generators, pipelines, comms towers. Logistics: Trucks, crates, fuel drums, drones.
## Effects


Fog, wind gusts, dust, snow flurries (optional), rockfall debris.
## Audio


Wind, distant artillery, radio chatter, aircraft movement.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Cliffs/Ridges        3   80k       320k
50k       750k
Rock Formations 4        30k       120k
2k        300k
Buildings/Military15     10k       500k
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

1. Prototype Layout â€” Month 1 Blockâ€‘out ridges, plateau, bunkers, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, commandâ€‘network autonomy behaviors.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (fog, wind, dust).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify commandâ€‘network logic, ridge navigation, airâ€‘ground coordination.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist

Navigation, elevation behavior, commandâ€‘network integrity, deployment budget enforcement, anchor drift, sensor accuracy.

