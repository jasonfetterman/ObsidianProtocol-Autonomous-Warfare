# Trenchline

Trenchline (Coastal / Defensive / Fortifications & Defensive Operations)

## Executive Summary


Trenchline is a fortified coastal battlefield built for defensive warfare, entrenched positions, layered fortifications, and siegeâ€‘driven coastal operations. The terrain features cliffside bunkers, beach obstacles, trench networks, artillery batteries, hardened pillboxes, coastal roads, and inland fallback lines. The map emphasizes Obsidian Protocolâ€™s autonomous defensive logic, sensorâ€‘network routing, commsâ€‘relay warfare, and logistics under sustained siege conditions. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of coastal defensive terrain. Key features include trench networks, beach chokepoints, cliffside overwatch, and multi â€‘layer defensive rings.

## Design Goals & Gameplay


Layered Coastal Defense
Trenchline prioritizes multiâ€‘ring defense: beach obstacles, trench networks, pillboxes, and inland bunkers. Attackers must breach sequential layers while defenders coordinate autonomous countermeasures.
Siege & Breach Mechanics
Artillery, naval bombardment, drones, and armored vehicles play major roles. Trenches, bunkers, and pillboxes can be destroyed or partially collapsed.
Autonomy & Defensive Coordination
Autonomous units prioritize chokepoints, fallback positions, counterâ€‘battery fire, and overwatch from cliffs. Units adapt to breached trenches, destroyed bunkers, and shifting defensive priorities.
Sensor & Information Warfare
Sea spray, smoke, dust, and cliff occlusion distort sensors. Relay towers on cliffs restore clarity. Destroying relays creates blind pockets across the defensive line.
Logistics & Resources
Fuel depots, armories, and coastal roads serve as resupply nodes. Convoys must navigate interior routes and avoid ambushâ€‘prone breach corridors.
Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between siege units, defensive armor, or autonomous infantry. Blue deploys from the southwest coastal trenchline; Red deploys from the northeast inland command bunker.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² fortified coastal zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System

1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations

Blue HQ (Coastal Trench Command) â€” (0,0), 150Ã—150â€¯m Trenches, sandbags, pillboxes, armored staging.
Red HQ (Inland Command Bunker) â€” (1000,1000), 150Ã—150â€¯m Hardened bunker with radar mast, AA emplacements, and artillery.
Central Trench Network â€” (500,450), 300Ã—300â€¯m Primary defensive zone; trenches, firing ports, bunkers.
Coastal Battery & Depot â€” (250,800), 120Ã—120â€¯m Artillery guns, fuel tanks, repair bays.
Cliffside Overwatch Line â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch; ideal for sensors and longâ€‘range fire.
Beach Landing Corridor â€” (0,450 â†’ 400,900) Primary amphibious chokepoint; layered defenses and obstacles.
Inland Road Route â€” (300,0 â†’ 450,350) Safer interior path with partial cover.
Cliff Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Cliff Relay B â€” (150,650), +260â€¯m Comms relay hub and counterâ€‘battery node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Beach Landing Corridor
Primary chokepoint; attackers must breach obstacles and pillboxes.
Central Trench Network
Interior flanking routes; extremely strong defensive positions.
Cliffside Overwatch
Longâ€‘range sightlines; ideal for artillery and sensors.
Inland Road Route
Predictable but safe; vulnerable to overwatch from cliffs.
## Sightlines

Cliff Relay A provides longâ€‘range sensor coverage (~350â€¯m). Sea spray and smoke create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW coastal trenchline + defensive staging.
Red Deployment: NE inland command bunker + artillery.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain

Beaches, cliffs, trenches, bunkers, pillboxes, coastal roads.
## Vegetation

Sparse shrubs, grass patches, coastal vegetation.
## Buildings & Props

Military: AA guns, radar dishes, bunkers, pillboxes. Industrial: Generators, pipelines, fuel tanks. Logistics: Trucks, crates, fuel drums, drones.
## Effects

Smoke, sparks, dust, explosions, sea spray.
## Audio

Artillery, waves, machinery, alarms, radio chatter.

## LOD & Budget


Asset CategCooryunt Tri/Item Total Tris

Trenches/Bunkers 3 200k    600k

Cliff/Industrial 4 80k     320k

Buildings/Military 15 50k  750k

Roads/Paths     4 30k      120k

Props        50 10k        500k

Total  â€”        â€”          ~2.29M

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

1. Prototype Layout â€” Month 1 Blockâ€‘out trenches, cliffs, beach corridor, AR anchor test.
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
