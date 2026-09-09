# Canyon Run

**Canyon / Highway — Mobility, Ambushes, Route Control**

## Executive Summary

Canyon Run is a high‑speed canyon‑highway battlefield built for mobility‑focused combat, ambush tactics, route control, and autonomous maneuvering through narrow vertical terrain. The environment features winding canyon highways, cliffside overhangs, collapsed tunnels, dry riverbeds, abandoned checkpoints, and elevated ridgelines.

The map emphasizes Obsidian Protocol’s autonomous route‑selection logic, sensor‑fusion under dust and heat distortion, comms‑relay warfare, and logistics under fast‑moving highway conditions. In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of canyon‑highway terrain.

Key features include ambush corridors, high‑speed traversal lanes, cliffside overwatch, and choke‑controlled mobility warfare.

## Design Goals & Gameplay

### Mobility‑Driven Combat
Canyon Run prioritizes fast movement, convoy operations, and rapid flanking. Highway lanes allow high‑speed traversal; canyon walls restrict lateral movement.

### Ambush & Route Control
Narrow passes, blind corners, and cliff overhangs create ideal ambush zones. Controlling key segments of the highway determines reinforcement flow.

### Vertical Overwatch & Canyon Tactics
Ridges and cliffs provide overwatch positions for snipers, artillery, and drones. Verticality shapes sightlines and engagement ranges.

### Autonomy & Route‑Selection Logic
Autonomous units choose between highway lanes, cliffside trails, dry riverbeds, and tunnel bypasses. Units adapt to blocked roads, collapsed tunnels, and shifting ambush threats.

### Sensor & Information Warfare
Dust storms, heat shimmer, canyon occlusion, and thermal plumes degrade sensors. Relay towers on ridges restore clarity. Destroying relays creates blind pockets across the canyon.

### Logistics & Resources
Fuel depots, checkpoints, and roadside maintenance yards serve as resupply nodes. Convoys must navigate exposed roads and avoid ambush‑prone canyon passes.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between fast armor, ambush specialists, or long‑range overwatch units.  
Blue deploys from the southwest canyon highway gate; Red deploys from the northeast ridge‑top command.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² canyon‑highway zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Highway Gate Command)** — (0,0), 150×150 m  
  Armored vehicles, drone pads, convoy staging.

- **Red HQ (Ridge‑Top Command Spire)** — (1000,1000), 150×150 m  
  Elevated bunker with radar mast, AA emplacements, long‑range sensors.

- **Central Canyon Highway** — (500,450), 300×300 m  
  High‑speed traversal zone; long sightlines, ambush corners, dust plumes.

- **Checkpoint Depot** — (250,800), 120×120 m  
  Barricades, fuel tanks, repair bays, interior flanking routes.

- **Ridge Network** — (800,300), 200×200 m  
  High‑ground overwatch, sniper nests, drone launch pads.

- **Ambush Corridor** — (0,450 → 400,900)  
  Primary chokepoint; narrow, exposed, ideal for ambushes.

- **Dry Riverbed Route** — (300,0 → 450,350)  
  Safer but slow; unstable terrain and limited cover.

- **Ridge Relay A** — (650,150), +240 m  
- **Ridge Relay B** — (150,650), +260 m  

- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### Ambush Corridor
Major chokepoint; controlling it determines convoy movement.

### Ridge Network
Extreme long‑range sightlines; ideal for overwatch and artillery.

### Canyon Highway
High‑speed engagements; dust plumes affect accuracy.

### Dry Riverbed Route
Predictable but safe; vulnerable to ridge overwatch.

### Sightlines
Ridge Relay A provides long‑range sensor coverage (~350 m). Dust storms and heat shimmer create fog‑of‑war pockets.

## Deployment Zones
- **Blue Deployment:** SW highway gate + convoy staging  
- **Red Deployment:** NE ridge‑top command + AA emplacements  
- **Neutral Forward Zone:** Outpost at (500,950)

## Assets & Environment

### Terrain
Canyons, ridges, highways, dry riverbeds, checkpoints.

### Vegetation
Sparse shrubs, desert grass, small trees.

### Buildings & Props
Military: AA guns, radar dishes, bunkers  
Industrial: Checkpoints, generators, pipelines  
Logistics: Trucks, crates, fuel drums, drones

### Effects
Dust storms, heat shimmer, mirage distortion, wind gusts.

### Audio
Wind, engines, distant artillery, radio chatter.

## LOD & Budget

Target total: ~2.29M tris

- Canyon/Ridge Terrain: ~600k  
- Industrial/Checkpoints: ~320k  
- Buildings/Military: ~750k  
- Roads/Paths: ~120k  
- Props: ~500k  

### Textures
2048² for terrain/buildings  
1024² for props  
DXT1/5 compression  
Target ≤100 MB

## Unreal / Nreal Integration

### Engine & Plugins
Unreal ARTemplate + Nreal SDK (XREAL).  
Enable ARKit/ARCore for anchors and plane detection.

### Rendering
Forward renderer, stationary lights, baked GI, aggressive culling.

### Anchors
Single spatial anchor for entire map.

### Interaction
Phone pointer for pan/zoom; HUD as screen‑space widgets.

### Multiplayer
Client‑server architecture; offload AI/physics to server.

## Implementation Plan & Testing

### Milestones
1. **Month 1 — Prototype Layout**  
   Block out canyon highway, ridges, checkpoints. AR anchor test.

2. **Month 2 — Core Systems**  
   NavMesh, mobility autonomy behaviors, ambush logic.

3. **Month 3 — AR Integration**  
   Anchors, plane detection, basic interactivity.

4. **Months 4–5 — Content Fill**  
   Final models, textures, lighting, audio.

5. **Months 6–7 — Gameplay & Polish**  
   Objectives, balance, VFX (dust storms, heat shimmer).

6. **Month 8 — Testing & QA**  
   Multiplayer stress test, sensor validation.

### Testing Plan

#### Autonomy
Verify mobility routing, ambush‑aware navigation, hazard avoidance.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks under dust and heat distortion.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, mobility logic, deployment budget enforcement, anchor drift, sensor accuracy.
