# Black Site

Black Site (Secret Military Facility / Experimental Warfare, Sensors, EW)

## Executive Summary


Black Site is a clandestine military research and operations complex built for experimental warfare, advanced sensors, electronic warfare (EW), and autonomous force testing. The terrain features hardened bunkers, underground labs, sensor arrays, comms vaults, prototype hangars, electromagnetic test chambers, and perimeter killâ€‘zones. The map emphasizes Obsidian Protocolâ€™s autonomous EW logic, sensorâ€‘fusion under interference, commsâ€‘relay disruption, and logistics inside a compartmentalized facility. In AR (Nreal Light/Air), the map anchors at ~4â€“5â€¯m viewing distance, covering a 10Ã—10â€¯m physical area representing ~1â€¯kmÂ² of secretâ€‘facility terrain. Key features include EW corridors, stealthâ€‘sensor blind zones, prototype weapon platforms, and multiâ€‘layer interior combat.

## Design Goals & Gameplay


Experimental Warfare & EW Dominance

Black Site prioritizes electronic warfare, sensor disruption, stealth infiltration, and prototype weapon deployment. EW zones dynamically alter unit behavior.
Compartmentalized Interior Combat

The facility is divided into sealed wings, labs, vaults, and hangars. Combat shifts between tight corridors, open testing chambers, and fortified control rooms.
Autonomy & EWâ€‘Aware Pathfinding

Autonomous units choose routes based on EW interference, sensor blackout zones, and prototype hazards. Units adapt to shifting electromagnetic conditions.
## Sensor & Information Warfare


EMP bursts, jamming fields, thermal distortion, and active countermeasures degrade sensors. Relay towers inside comms vaults restore clarity. Destroying relays creates cascading blackout zones.
## Logistics & Resources

Prototype hangars, fuel vaults, and research depots serve as resupply nodes. Convoys must navigate narrow interior lanes and avoid ambushâ€‘prone EW corridors.
## Deployment & Progress


A fixed deployment budget (~10,000 points) forces players to choose between stealth units, EW specialists, or heavy breach teams. Blue deploys from the southwest access wing; Red deploys from the northeast command vault.

## Spatial Constraints (Nreal AR)


## Device Capabilities

Nreal Light/Air: ~53Â° diagonal FOV, 1920Ã—1080 perâ€‘eye resolution. Optimal viewing distance ~4â€“5â€¯m.
## Playable Area & Scaling

10Ã—10â€¯m physical area â‰ˆ 1â€¯kmÂ² secretâ€‘facility zone (1:100 scale). Designed for tabletop AR.
## Tracking & Occlusion

Insideâ€‘out tracking detects horizontal planes. Depthâ€‘mesh (if available) provides limited occlusion. Virtual objects render over realâ€‘world view; assume minimal realâ€‘world occlusion.

## Layout Overview


## Coordinate System


1000Ã—1000 grid (1 unit â‰ˆ 1â€¯m). Origin (0,0) = southwest corner.
## Key Locations


Blue HQ (Access Wing Command) â€” (0,0), 150Ã—150â€¯m Security gates, staging area, armored breach vehicles.
Red HQ (Command Vault) â€” (1000,1000), 150Ã—150â€¯m Hardened vault with EW control nodes and radar mast.
Central Test Chamber â€” (500,450), 300Ã—300â€¯m Prototype weapons, sensor arrays, open interior combat zone.
Research Lab Cluster â€” (250,800), 120Ã—120â€¯m Experiment rooms, servers, thermal hazards.
Prototype Hangar â€” (800,300), 200Ã—200â€¯m Experimental drones, vehicles, interior flanking routes.
EW Corridor â€” (0,450 â†’ 400,900) Heavy jamming field; extreme sensor degradation.
Maintenance Route â€” (300,0 â†’ 450,350) Safer interior path with partial cover.
Relay Tower A â€” (650,150), +240â€¯m Longâ€‘range sensor vantage point.
Relay Tower B â€” (150,650), +260â€¯m Comms relay hub and EW countermeasure node.
Neutral Forward Outpost â€” (500,950) Drone resupply zone.

## Chokepoints & Sightlines


EW Corridor

Severe sensor disruption; ideal for stealth ambushes.

Prototype Hangar

Interior flanking routes; unpredictable hazards.
Test Chamber
Open interior space; longâ€‘range experimental weapon lines.
Research Labs
Tight corridors; closeâ€‘range combat.
## Sightlines

Relay Tower A provides longâ€‘range sensor coverage (~300â€¯m). EW fields create fogâ€‘ofâ€‘war pockets and blackout zones.

## Deployment Zones


Blue Deployment: SW access wing + breach staging.
Red Deployment: NE command vault + EW defenses.
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment


## Terrain


Labs, vaults, hangars, EW corridors, test chambers, maintenance tunnels.
## Vegetation


None â€” sterile industrial interior.
## Buildings & Props


Military: EW towers, radar dishes, AA guns, bunkers. Industrial: Servers, generators, pipelines, control panels. Logistics: Trucks, crates, fuel drums, drones.
## Effects


EMP bursts, sparks, smoke, heat shimmer, holographic displays.
## Audio


Machinery hum, alarms, radio chatter, ventilation systems.

## LOD & Budget


Asset CategCooruynt      Tri/Item    Total Tris
200k        600k
Heavy Machinery 3        80k         320k
50k         750k
Industrial Meshes 4      30k         120k
10k         500k
Buildings/Interior15     â€”           ~2.29M

Corridors/Paths 4

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

1. Prototype Layout â€” Month 1 Blockâ€‘out labs, hangars, EW corridor, AR anchor test.
2. Core Systems â€” Month 2 NavMesh, EWâ€‘aware autonomy behaviors, sensor logic.
3. AR Integration â€” Month 3 Anchors, plane detection, basic interactivity.
4. Content Fill â€” Month 4â€“5 Final models, textures, lighting, audio.
5. Gameplay & Polish â€” Month 6â€“7 Objectives, balance, VFX (EMP bursts, sparks, holograms).
6. Testing & QA â€” Month 8 Multiplayer stress test, sensor validation.

## Testing Plan


## Autonomy

Verify EWâ€‘aware navigation, blackoutâ€‘zone behavior, hazard avoidance.
## Comms/Sensors

Test relay destruction, fogâ€‘ofâ€‘war updates, LOS checks under EW.
## Performance


â‰¥60â€¯Hz AR render on target device.
## QA Checklist


Navigation, EW logic, deployment budget enforcement, anchor drift, sensor accuracy.

