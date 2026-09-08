# The Divide

The Divide (Mountain Pass / Strategic Chokepoint Warfare)

## Executive Summary


The Divide is a highâ€‘altitude mountainâ€‘pass battlefield built for strategic chokepoint warfare, elevationâ€‘driven combat, autonomous route control, and multiâ€‘domain coordination across narrow, highâ€‘risk terrain. The environment features sheer cliffs, winding passes, avalancheâ€‘scarred slopes, fortified ridgelines, collapsed tunnels, old military outposts, and steep switchback roads. The map emphasizes Obsidian Protocolâ€™s autonomous elevation â€‘aware routing, sensorâ€‘fusion under mountain fog and rock occlusion, commsâ€‘relay warfare, and logistics under severe terrain constraints. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of mountain â€‘pass terrain. Key features include chokepoint control, overwatch ridges, ambush corners, and highâ€‘risk mobility corridors.

## Design Goals & Gameplay


Chokepointâ€‘Focused Warfare

The Divide prioritizes control of narrow passes, switchbacks, and cliffside trails. Holding key chokepoints determines reinforcement flow and strategic dominance.

Elevationâ€‘Driven Combat
Highâ€‘ground ridges provide overwatch for snipers, artillery, and drones. Verticality shapes sightlines, movement, and engagement ranges.

Ambush & Route Denial

Blind corners, collapsed tunnels, and narrow ledges create ideal ambush zones. Route denial becomes a core strategic tool.

Autonomy & Terrainâ€‘Aware Routing

Autonomous units choose routes based on elevation advantage, rockfall hazards, fog density, and sensor clarity. Units adapt to blocked roads, unstable slopes, and shifting mountain conditions.

Sensor & Information Warfare

Fog, dust, rock occlusion, thermal plumes, and elevation clutter degrade sensors. Relay towers on ridges restore clarity. Destroying relays creates blind pockets across the pass.

Logistics & Resources
Fuel caches, mountain cabins, and old military outposts serve as resupply nodes. Convoys must navigate narrow roads and avoid ambush â€‘prone switchbacks.

Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between fast armor, overwatch specialists, or stealth mountain infantry. Blue deploys from the southwest lower â€‘pass gate; Red deploys from the northeast ridgeâ€‘top fortress.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.

## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² mountainâ€‘pass zone (1:100 scale). Designed for tabletop AR.

## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.

## Key Locations

Blue HQ (Lowerâ€‘Pass Gate Command) â€” (0,0), 150Ã—150â€¯m Armored vehicles, drone pads, mountain staging.
Red HQ (Ridgeâ€‘Top Fortress) â€” (1000,1000), 150Ã—150â€¯m Elevated bunker with radar mast, AA emplacements, and overwatch.
Central Pass Corridor â€” (500,450), 300Ã—300â€¯m Narrow pass, steep cliffs, fog pockets, longâ€‘range overwatch.
Abandoned Outpost Cluster â€” (250,800), 120Ã—120â€¯m Ruins, generators, interior flanking routes.
Ridge Network â€” (800,300), 200Ã—200â€¯m Highâ€‘ground overwatch, sniper nests, drone launch pads.
Switchback Chokepoint â€” (0,450 â†’ 400,900) Primary chokepoint; narrow, exposed, ideal for ambushes.
Mountain Trail Route â€” (300,0 â†’ 450,350) Safer but slow; rock hazards and limited cover.
Ridge Relay A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Ridge Relay B â€” (150,650), +260â€¯m Comms relay hub and overwatch control node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


Switchback Chokepoint

Major chokepoint; controlling it determines convoy movement.

Ridge Network
Extreme longâ€‘range sightlines; ideal for overwatch and artillery.

Pass Corridor
Longâ€‘range engagements; fog and rock occlusion affect accuracy.

Mountain Trail Route

Predictable but safe; vulnerable to ridge overwatch.

## Sightlines

Ridge Relay A provides longâ€‘range sensor coverage (~350â€¯m). Fog and dust create fogâ€‘ofâ€‘war pockets.

## Deployment Zones


Blue Deployment: SW lowerâ€‘pass gate + armor staging.
Red Deployment: NE ridgeâ€‘top fortress + AA emplacements.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Cliffs, ridges, passes, switchbacks, outposts, mountain trails.

## Vegetation


Sparse shrubs, pine trees, alpine grass.

## Buildings & Props


Military: AA guns, radar dishes, bunkers. Rural: Cabins, fences, wooden structures. Logistics: Trucks, crates, fuel drums, drones.

## Effects


Fog, dust, wind gusts, rockfall debris.

## Audio


Wind, distant machinery, rock impacts, radio chatter.

## LOD & Budget


Asset CategCooruynt      Tri/Item  Total Tris
200k      600k
Mountain Terrain 3       80k       320k
50k       750k
Industrial/Rural 4       30k       120k
10k       500k
Buildings/Rural 15       â€”         ~2.29M

Roads/Paths 4

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

1. Prototype Layout â€” Month 1 Blockâ€‘out pass corridor, ridges, outposts, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, chokepoint autonomy behaviors, elevation logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (fog, dust, rockfall).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify chokepoint navigation, elevationâ€‘aware pathfinding, hazard avoidance.

## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under mountain occlusion.

## Performance


â‰¥60â€¯Hz AR render on target device.

## QA Checklist


Navigation, chokepoint logic, deployment budget enforcement, anchor drift, sensor accuracy.
