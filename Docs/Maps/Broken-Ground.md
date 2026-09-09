# Broken Ground

**Destroyed Urban / Battlefield Aftermath & Dynamic Terrain**

## Executive Summary

Broken Ground is a destroyed‑urban battlefield built for post‑combat operations, dynamic shifting terrain, autonomous hazard navigation, and close‑quarters engagements amid ruins. The environment features collapsed buildings, cratered streets, shattered overpasses, unstable rubble piles, burned‑out vehicles, improvised fortifications, and fractured utility lines.

The map emphasizes Obsidian Protocol’s autonomous rubble‑aware navigation, sensor‑fusion under smoke and dust, comms‑relay warfare, and logistics across unstable, constantly changing terrain. In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of destroyed‑urban terrain.

Key features include shifting rubble fields, collapsed chokepoints, interior ruin routes, and dynamic cover created by ongoing structural instability.

## Design Goals & Gameplay

### Post‑Battlefield Urban Combat
Broken Ground prioritizes combat in a city already devastated by prior fighting. Terrain is unstable, unpredictable, and constantly shifting.

### Dynamic Terrain & Structural Instability
Rubble collapses, fires flare up, dust clouds shift, and debris falls. Autonomous units must adapt to terrain changes mid‑engagement.

### Close‑Quarters Urban Warfare
Interior ruins, alleyways, collapsed basements, and broken stairwells create tight, brutal combat zones.

### Autonomy & Hazard Navigation
Autonomous units choose routes based on rubble stability, fire hazards, dust visibility, and sensor clarity. Units reroute when terrain becomes impassable.

### Sensor & Information Warfare
Smoke, dust, fire heat, metallic clutter, and collapsed structures degrade sensors. Relay towers on surviving rooftops restore clarity. Destroying relays creates blind pockets across the ruins.

### Logistics & Resources
Fuel caches, abandoned depots, and makeshift shelters serve as resupply nodes. Convoys must navigate cratered roads and avoid ambush‑prone rubble corridors.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between urban infantry, rubble‑capable drones, or heavy armor adapted for unstable terrain.  
Blue deploys from the southwest collapsed highway; Red deploys from the northeast fortified ruin.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² destroyed‑urban zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Collapsed Highway Gate)** — (0,0), 150×150 m  
  Burned‑out vehicles, barricades, drone pads, rubble staging.

- **Red HQ (Fortified Ruin Command)** — (1000,1000), 150×150 m  
  Reinforced ruin with radar mast, AA emplacements, sensor arrays.

- **Central Ruin Basin** — (500,450), 300×300 m  
  Collapsed buildings, shifting rubble, fires, dust clouds, close‑quarters combat.

- **Abandoned Depot Cluster** — (250,800), 120×120 m  
  Warehouses, broken machinery, interior flanking routes.

- **Rubble Ridge Network** — (800,300), 200×200 m  
  High‑ground overwatch created by collapsed structures; unstable but tactically valuable.

- **Collapse Corridor** — (0,450 → 400,900)  
  Primary chokepoint; unstable rubble, fires, falling debris.

- **Crater Road Route** — (300,0 → 450,350)  
  Safer but slow; crater hazards and limited cover.

- **Rooftop Relay A** — (650,150), +240 m  
- **Rooftop Relay B** — (150,650), +260 m  

- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### Collapse Corridor
Major chokepoint; terrain shifts unpredictably, ideal for ambushes.

### Rubble Ridge Network
Interior flanking routes; unstable high‑ground with long sightlines.

### Ruin Basin
Close‑quarters engagements broken by rubble; dust reduces accuracy.

### Crater Road Route
Predictable but safe; vulnerable to overwatch from rubble ridges.

### Sightlines
Rooftop Relay A provides long‑range sensor coverage (~300 m). Smoke and dust create fog‑of‑war pockets.

## Deployment Zones
- **Blue Deployment:** SW collapsed highway + rubble staging  
- **Red Deployment:** NE fortified ruin + AA emplacements  
- **Neutral Forward Zone:** Outpost at (500,950)

## Assets & Environment

### Terrain
Collapsed buildings, rubble fields, craters, broken roads, ruined depots.

### Vegetation
Sparse weeds, dead trees, overgrown patches.

### Buildings & Props
Military: AA guns, radar dishes, bunkers  
Urban: Ruins, collapsed towers, vehicles, debris piles  
Logistics: Trucks, crates, fuel drums, drones

### Effects
Smoke, dust, fire bursts, sparks, falling debris.

### Audio
Distant explosions, collapsing rubble, machinery, radio chatter.

## LOD & Budget

Target total: ~2.29M tris

- Ruin/Rubble Terrain: ~600k  
- Industrial/Urban Meshes: ~320k  
- Buildings/Ruins: ~750k  
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
   Block out ruin basin, rubble ridges, depots. AR anchor test.

2. **Month 2 — Core Systems**  
   NavMesh, rubble‑aware autonomy behaviors, hazard logic.

3. **Month 3 — AR Integration**  
   Anchors, plane detection, basic interactivity.

4. **Months 4–5 — Content Fill**  
   Final models, textures, lighting, audio.

5. **Months 6–7 — Gameplay & Polish**  
   Objectives, balance, VFX (smoke, dust, fire bursts).

6. **Month 8 — Testing & QA**  
   Multiplayer stress test, sensor validation.

### Testing Plan

#### Autonomy
Verify rubble navigation, hazard‑aware pathfinding, collapse‑zone avoidance.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks under smoke and dust.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, dynamic terrain logic, deployment budget enforcement, anchor drift, sensor accuracy.
