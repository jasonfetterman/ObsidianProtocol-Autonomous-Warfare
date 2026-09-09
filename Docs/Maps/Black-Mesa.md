# Black Mesa

**Mountain / Industrial Battlefield**

## Executive Summary

Black Mesa is a sprawling mountainous‑industrial battlefield designed for AR command. The map emphasizes combined‑arms warfare across ground vehicles, infantry, and air support, with dramatic elevation differences and industrial facilities. It incorporates Obsidian Protocol’s core systems: autonomous decision‑making, sensor‑driven tactics, communications networks, and logistical nodes.

In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area (e.g., a conference table) to represent about 1 km² of terrain. Key features include high ridges for aerial and sensor advantage, narrow mountain passes for chokepoints, and an industrial valley with supply depots and objectives.

## Design Goals & Gameplay

### Combined Arms & Scale
Black Mesa supports infantry, armor, and air simultaneously. Wide valley floors enable ground maneuver, while towering peaks and plateaus provide air and artillery vantage points.

### Autonomy & Flanking
Multiple routes — mountain passes, tunnels, and rivers — create non‑linear paths. Autonomous AI can flank, ambush, retreat, or reposition without micromanagement.

### Sensor & Information Warfare
High peaks host sensors and relays; deep valleys create occlusions and radar dead zones. Destroying ridge‑top relays degrades network clarity and expands fog‑of‑war.

### Logistics & Resources
An industrial plant and mine in the valley serve as logistics nodes. Fuel depots and fabrication facilities support resupply. Long supply lines reinforce the importance of logistics.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces strategic choices: heavy armor up the passes or mobile recon around the flanks. Deployment zones sit at opposite corners and elevated terrain.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Content should be placed several meters away for comfortable viewing.

### Playable Area & Scaling
A 10×10 m anchored AR map at ~4 m distance represents ~1 km² (1:100 scale). Fits on a conference table.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over real‑world view by default.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **West Base (Blue HQ)** — (0,0), 100×100 m  
  High plateau north of base overlooks the valley.

- **East Base (Red HQ)** — (1000,1000), 100×100 m  
  Mountain spur with radio tower.

- **Industrial Complex** — (500,300), ~150×150 m  
  Factory, storage tanks, warehouses.

- **Power Plant / Refinery** — (800,200), 80×80 m  
  Pipes, cooling towers, river access.

- **Supply Depot** — (200,800), 60×60 m  
  Crates, fuel tanks.

### Mountain Passes & Tunnels
- **North Pass** — (400,1000 → 600,600)  
  Narrow defile between cliffs.

- **South Tunnel** — (300,0 → 400,400)  
  Road tunnel through ridge; close‑range combat.

### Rivers & Bridges
River runs from (0,500) → (400,900).  
Bridges at (150,600) and (350,500).

### Terrain Heights
- **Peak A** — (600,100), +300 m  
- **Peak B** — (100,600), +250 m  
Valley floor baseline = 0.

## Chokepoints & Sightlines

### North Pass
Narrow, high‑risk chokepoint; limited sightlines except from air.

### South Tunnel
Confined combat zone; ideal for ambushes.

### Bridges
Critical river crossings; controlling them prevents flanking.

### Sightlines
Peak A offers ~300 m sensor range across valley.  
East Base tower provides long‑range radar.  
Deep valleys create heavy fog‑of‑war.

### Deployment Zones
Blue: West Base  
Red: East Base  
Neutral forward zone: Airfield at (500,950)

## Assets & Environment

### Terrain
Mountains, cliffs, riverbeds; rocky cliffs, dirt, grass.

### Vegetation
Sparse conifer trees (~2k tris each), shrubs, grass patches.

### Buildings & Props
Industrial: Factory shells, tanks, pipelines  
Military: Radar dish, antennas, watchtowers  
Logistics: Crates, barrels, trucks  
Bridges/Roads: Metal truss bridges, asphalt roads

### Effects
Steam vents, furnace glow, caution lights.

### Audio
Wind, distant rumble, machinery hum, radio chatter.

## LOD & Budget

Target total: ~2.59M tris

- Mountains/Cliffs: ~600k  
- Slopes/Hills: ~320k  
- Industrial Buildings: ~750k  
- Bridges/Roads: ~120k  
- Vegetation: ~300k  
- Props/Vehicles: ~500k  

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
Forward renderer, limited dynamic lights, baked GI.

### Anchors
Single spatial anchor for entire map.

### Interaction
Phone pointer for pan/zoom; HUD as screen‑space widgets.

### Multiplayer
Client‑server architecture; offload physics/AI to server.

## Implementation Plan & Testing

### Milestones
1. **Month 1 — Prototype Layout**  
   Block out terrain, bases, objectives. AR anchor test.

2. **Month 2 — Core Systems**  
   NavMesh, pathfinding, autonomous behaviors, sensor raycasts.

3. **Month 3 — AR Integration**  
   Anchors, plane detection, basic interactivity.

4. **Months 4–5 — Content Fill**  
   Final models, textures, lighting, audio.

5. **Months 6–7 — Gameplay & Polish**  
   Objectives, balance, VFX.

6. **Month 8 — Testing & QA**  
   Multiplayer stress test, sensor validation, hazard simulation.

### Testing Plan

#### Autonomy
Verify flanking, retreat, hazard avoidance.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, deployment budget enforcement, anchor stability, sensor accuracy.
