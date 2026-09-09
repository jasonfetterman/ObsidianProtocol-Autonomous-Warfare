# Dead River

**Industrial River / Bridges, Water, Industry, Logistics**

## Executive Summary

Dead River is an industrial river‑zone battlefield built for bridge control, waterway dominance, logistics warfare, and autonomous maneuvering through dense industrial infrastructure. The terrain features cargo docks, steel bridges, refineries, pumping stations, rail spurs, container yards, floodgates, and polluted river channels.

The map emphasizes Obsidian Protocol’s autonomous logistics‑aware routing, sensor‑fusion under industrial smoke and water reflection, comms‑relay warfare, and multi‑domain operations across land, water, and elevated bridge networks. In AR (Nreal Light/Air), the map anchors at ~4–5 m viewing distance, covering a 10×10 m physical area representing ~1 km² of industrial river terrain.

Key features include bridge chokepoints, waterborne routes, industrial hazards, and logistics‑driven strategic play.

## Design Goals & Gameplay

### Bridge Control & Chokepoint Warfare
Dead River prioritizes control of steel bridges, overpasses, and elevated walkways. Holding bridges determines movement between industrial sectors.

### Waterway Operations
Boats, amphibious armor, and drones navigate polluted river channels, floodgates, and narrow industrial waterways.

### Industrial Logistics Warfare
Container yards, rail spurs, and refineries create logistics hubs. Destroying or capturing them affects reinforcement flow and resource availability.

### Autonomy & Infrastructure‑Aware Routing
Autonomous units choose routes based on bridge stability, water depth, industrial hazards, and sensor clarity. Units adapt to blocked roads, collapsed walkways, and shifting river conditions.

## Sensor & Information Warfare

Smoke, steam, water reflection, metallic clutter, and industrial noise degrade sensors. Relay towers on refinery rooftops restore clarity. Destroying relays creates blind pockets across the river zone.

## Logistics & Resources

Cargo terminals, rail yards, refineries, and pumping stations serve as resupply nodes. Convoys must navigate exposed docks, narrow industrial corridors, and ambush‑prone bridge approaches.

## Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between amphibious units, logistics‑focused armor, or industrial‑sector infantry.  
Blue deploys from the southwest cargo dock; Red deploys from the northeast refinery command.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² industrial river zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations

- **Blue HQ (Cargo Dock Command)** — (0,0), 150×150 m  
  Boats, cranes, containers, amphibious staging.

- **Red HQ (Refinery Command Spire)** — (1000,1000), 150×150 m  
  Refinery tower with radar mast, AA emplacements, and sensor arrays.

- **Central Bridge Network** — (500,450), 300×300 m  
  Steel bridges, elevated walkways, chokepoints, long‑range overwatch.

- **Industrial Depot Cluster** — (250,800), 120×120 m  
  Warehouses, rail spurs, generators, interior flanking routes.

- **Floodgate Sector** — (800,300), 200×200 m  
  Water‑control structures, narrow channels, ambush zones.

- **River Crossing Corridor** — (0,450 → 400,900)  
  Primary chokepoint; bridges, water hazards, industrial clutter.

- **Service Road Route** — (300,0 → 450,350)  
  Safer but predictable; vulnerable to overwatch from bridges.

- **Refinery Relay A** — (650,150), +240 m  
  Long‑range sensor vantage point.

- **Refinery Relay B** — (150,650), +260 m  
  Comms relay hub and logistics‑monitoring node.

- **Neutral Forward Outpost** — (500,950)  
  Drone resupply zone.

## Chokepoints & Sightlines

### River Crossing Corridor
Major chokepoint; controlling bridges determines movement between sectors.

### Floodgate Sector
Narrow water routes; ideal for ambushes and amphibious traps.

### Bridge Network
Long‑range engagements; elevation provides overwatch.

### Service Road Route
Predictable but safe; vulnerable to elevated fire.

### Sightlines
Refinery Relay A provides long‑range sensor coverage (~350 m).  
Smoke and water reflection create fog‑of‑war pockets.

## Deployment Zones

- **Blue Deployment:** SW cargo dock + amphibious staging  
- **Red Deployment:** NE refinery command + AA emplacements  
- **Neutral Forward Zone:** Outpost at (500,950)

## Assets & Environment

### Terrain
River channels, bridges, docks, refineries, rail yards, floodgates.

### Vegetation
Sparse industrial weeds, riverbank grass.

### Buildings & Props
Military: AA guns, radar dishes, bunkers  
Industrial: Warehouses, cranes, pipelines, refineries  
Logistics: Trucks, crates, fuel drums, boats, drones

### Effects
Smoke, steam, water spray, sparks, industrial noise.

### Audio
Machinery, water flow, distant alarms, radio chatter.

## LOD & Budget

Target total: ~2.29M tris

- River/Industrial Terrain: ~600k  
- Industrial/Logistics Structures: ~320k  
- Buildings/Industrial: ~750k  
- Roads/Paths: ~120k  
- Props: ~500k  

### Texture Budget
2048² for terrain/buildings  
1024² for props  
DXT1/5 compression  
Target ≤100 MB

## Unreal / Nreal Integration

### Engine & Plugins
Unreal ARTemplate + Nreal SDK (XREAL).  
ARKit/ARCore enabled.

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

1. **Prototype Layout — Month 1**  
   Block out river channels, bridges, depots. AR anchor test.

2. **Core Systems — Month 2**  
   NavMesh (land + water), logistics autonomy, industrial hazard logic.

3. **AR Integration — Month 3**  
   Anchors, plane detection, basic interactivity.

4. **Content Fill — Months 4–5**  
   Final models, textures, lighting, audio.

5. **Gameplay & Polish — Months 6–7**  
   Objectives, balance, VFX (smoke, steam, water spray).

6. **Testing & QA — Month 8**  
   Multiplayer stress test, sensor validation.

## Testing Plan

### Autonomy
Verify amphibious routing, bridge‑aware navigation, hazard avoidance.

### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks under industrial smoke.

### Performance
≥60 Hz AR render on target device.

### QA Checklist
Navigation, logistics logic, deployment budget enforcement, anchor drift, sensor accuracy.
