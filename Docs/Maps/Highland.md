# Highland

Highland (Mountain / Elevation, Airspace, Defensive Warfare) Map Design

## Executive Summary


Highland is a highâ€‘altitude mountain battlefield built for elevationâ€‘based combat, airspace control, and layered defensive warfare. The terrain features steep ridges, alpine cliffs, fortified passes, elevated artillery platforms, windâ€‘exposed plateaus, and narrow mountain roads. Airspace plays a major role: drones, VTOL craft, and highâ€‘ground sensors dominate. The map emphasizes Obsidian Protocolâ€™s autonomous elevation logic, airâ€‘ground coordination, commsâ€‘relay networks, and logistics under harsh mountain constraints. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of mountainous terrain. Key features include multi â€‘tier elevation combat, cliffside defensive positions, airspace corridors, and chokeâ€‘heavy mountain passes.

## Design Goals & Gameplay


Elevationâ€‘Driven Combat
Highland prioritizes verticality. Highâ€‘ground positions dominate sightlines, artillery arcs, and sensor coverage. Controlling ridges is essential.

Airspace & Aerial Coordination
Air units use mountain updrafts, ridgeâ€‘line corridors, and cliffside landing pads. Ground units rely on aerial recon for targeting and movement.

Autonomy & Elevation Logic
Autonomous AI selects ridge routes, avoids exposed plateaus during enemy airstrikes, and uses cliff cover to break lineâ€‘ofâ€‘sight.

## Sensor & Information Warfare

Thin air, wind, fog pockets, and elevation gradients distort sensors. Relay towers on peaks restore clarity. Destroying relays creates massive blind zones.

## Logistics & Resources

Mountain depots, helipads, and fortified bunkers serve as resupply nodes. Supply convoys must navigate narrow roads and avoid ambush â€‘prone passes.

## Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between heavy artillery, air support, or mobile mountain infantry. Blue deploys from the southwest valley base; Red deploys from the northeast peak fortress.

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

Blue HQ (Valley Base) â€” (0,0), 140Ã—140â€¯m Mountainâ€‘foot base with artillery and recon drones.
Red HQ (Peak Fortress) â€” (1000,1000), 140Ã—140â€¯m Fortified highâ€‘ground base with radar mast and AA emplacements.
Central Highland Plateau â€” (500,450), 250Ã—250â€¯m Windâ€‘exposed plateau with longâ€‘range sightlines.
Mountain Depot â€” (250,800), 100Ã—100â€¯m Fuel tanks, crates, helipad, supply trucks.
Cliffside Bunker Network â€” (800,300), 150Ã—150â€¯m Interior tunnels, firing ports, defensive positions.
Ridge Pass â€” (0,450 â†’ 400,900) Narrow elevated corridor; critical choke zone.
Lower Valley Trail â€” (300,0 â†’ 450,350) Safer movement route with partial cover.
Peak A â€” (650,150), +300â€¯m Longâ€‘range sensor vantage point.
Peak B â€” (150,650), +280â€¯m Comms relay tower and AA platform.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Ridge Pass
Extremely narrow; ideal for defensive artillery and ambushes.

Cliffside Bunker Network
Interior routes and firing ports; strong defensive positions.

Highland Plateau
Longâ€‘range sightlines; airâ€‘ground coordination essential.

Lower Valley Trail
Safer but predictable; vulnerable to aerial recon.

## Sightlines

Peak A provides longâ€‘range sensor coverage (~350â€¯m). Fog pockets and elevation gradients create blind zones.

## Deployment Zones


Blue Deployment: SW valley base + rear rally point.
Red Deployment: NE peak fortress + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain

Cliffs, ridges, plateaus, valleys, rock formations.

## Vegetation

Sparse alpine shrubs, pine clusters, grass patches.

## Buildings & Props

Military: Bunkers, AA guns, radar dishes, helipads. Industrial: Generators, pipelines, comms towers. Logistics: Trucks, crates, fuel drums, drones.

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
2. Core Systems â€” Month 2 NavMesh, elevationâ€‘aware autonomy behaviors.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (fog, wind, dust).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify elevation logic, ridge navigation, airâ€‘ground coordination.

## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.

## Performance

â‰¥60â€¯Hz AR render on target device.

## QA Checklist

Navigation, elevation behavior, deployment budget enforcement, anchor drift, sensor accuracy.

