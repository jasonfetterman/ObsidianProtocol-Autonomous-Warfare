# Fortress

Fortress (Military Base / Defensive Warfare & Siege)

## Executive Summary


Fortress is a hardened militaryâ€‘base battlefield built for defensive warfare, siege operations, layered fortifications, and autonomous defensive coordination. The terrain features perimeter walls, watchtowers, bunkers, artillery emplacements, vehicle yards, command centers, killâ€‘zones, and multiâ€‘layer defensive rings. The map emphasizes Obsidian Protocolâ€™s autonomous defensive logic, sensorâ€‘network routing, commsâ€‘relay warfare, and logistics under siege conditions. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of fortified terrain. Key features include layered defenses, breach points, interior strongholds, and siegeâ€‘driven chokepoints.

## Design Goals & Gameplay


Layered Defensive Warfare

Fortress prioritizes multiâ€‘ring defense: outer walls, inner bunkers, command citadel. Attackers must breach sequential layers while defenders coordinate autonomous countermeasures.
Siege Operations & Breach Mechanics

Artillery, breaching units, drones, and armored vehicles play major roles. Walls, gates, and bunkers can be destroyed or partially collapsed.
Autonomy & Defensive Coordination

Autonomous units prioritize chokepoints, fallback positions, and counterâ€‘battery fire. Units adapt to breached walls, destroyed gates, and shifting defensive priorities.
## Sensor & Information Warfare


Smoke, dust, explosions, and bunker occlusion distort sensors. Relay towers on watchtowers restore clarity. Destroying relays creates blind pockets inside the base.
## Logistics & Resources


Fuel depots, armories, and vehicle yards serve as resupply nodes. Convoys must navigate interior roads and avoid ambushâ€‘prone breach corridors.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between siege units, defensive armor, or autonomous infantry. Blue deploys from the southwest outer gate; Red deploys from the northeast command citadel.

## Spatial Constraints (Nreal AR)


## Device Capabilities


Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling


10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² fortified zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion


Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Outer Gate Command) â€” (0,0), 150Ã—150â€¯m Gatehouse, barricades, armored staging area.

Red HQ (Command Citadel) â€” (1000,1000), 150Ã—150â€¯m Hardened command tower with radar mast and AA emplacements.

Central Parade Ground â€” (500,450), 300Ã—300â€¯m Open interior zone; ideal for armor and artillery duels.

Armory & Vehicle Yard â€” (250,800), 120Ã—120â€¯m Fuel tanks, repair bays, vehicle shelters.

Bunker Network â€” (800,300), 200Ã—200â€¯m Interior tunnels, firing ports, hardened defensive positions.

Outer Wall Line â€” (0,450 â†’ 400,900) Primary defensive ring; breachable gates and towers.

Inner Road Route â€” (300,0 â†’ 450,350) Safer interior path with partial cover.
Watchtower Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Watchtower Relay B â€” (150,650), +260â€¯m Comms relay hub and counterâ€‘battery node.

Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Outer Wall Line

Primary chokepoint; attackers must breach gates or collapse wall segments.
Bunker Network

Interior flanking routes; extremely strong defensive positions.
Parade Ground

Open sightlines; ideal for armor and artillery.
Inner Road Route

Predictable but safe; vulnerable to overwatch from bunkers.
## Sightlines


Watchtower Relay A provides longâ€‘range sensor coverage (~350â€¯m). Smoke and dust create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW outer gate + siege staging.

Red Deployment: NE command citadel + AA emplacements.

Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Walls, bunkers, towers, parade grounds, vehicle yards, interior roads.
## Vegetation


Sparse shrubs, grass patches, military landscaping.
## Buildings & Props


Military: AA guns, radar dishes, bunkers, watchtowers. Industrial: Generators, pipelines, fuel tanks. Logistics: Trucks, crates, fuel drums, drones.
## Effects


Smoke, sparks, dust, explosions, fire bursts.
## Audio


Artillery, machinery, alarms, radio chatter.

## LOD & Budget


Asset CategCooruynt Tri/Item Total Tris

Walls/Towers 3 200k                 600k

Bunkers/Industrial 4 80k            320k

Buildings/Military15 50k            750k

Roads/Paths      4 30k              120k

Props        50 10k                 500k

Total    â€”        â€”                 ~2.29M

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


1. Prototype Layout â€” Month 1 Blockâ€‘out walls, bunkers, parade ground, AR anchor test.

2. Core Systems â€” Month 2 NavMesh, defensive autonomy behaviors, siege logic.

3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.

4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.

5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (smoke, sparks, explosions).

6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy


Verify defensive logic, fallback behavior, breachâ€‘zone navigation.
## Comms/Sensors


Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, siege logic, deployment budget enforcement, anchor drift, sensor accuracy.

