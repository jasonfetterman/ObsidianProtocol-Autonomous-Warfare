# Aftermath

Aftermath (War‑torn City / Destruction‑Focused Combined Arms)

## Executive Summary

Aftermath is a war‑torn urban combined‑arms battlefield built for heavy destruction, multi‑domain coordination, rubble‑based maneuvering, and autonomous adaptation to collapsing terrain. The environment features shattered skyscrapers, cratered boulevards, burned‑out industrial zones, fractured bridges, improvised fortifications, underground transit ruins, and smoke‑filled avenues. The map emphasizes Obsidian Protocol’s autonomous combined‑arms logic, sensor‑fusion under smoke and debris, comms‑relay warfare, and logistics across unstable, partially collapsing terrain.

In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of war‑torn urban terrain. Key features include destruction‑driven chokepoints, multi‑layer vertical combat, rubble‑adaptive navigation, and combined‑arms engagements across armor, infantry, drones, and artillery.

## Design Goals & Gameplay

### Destruction‑Focused Combined Arms
Aftermath prioritizes armor pushes, drone reconnaissance, infantry clearing, artillery strikes, and air support. Terrain destruction directly reshapes tactical options.

### Dynamic Urban Terrain
Buildings collapse, fires spread, dust clouds shift, and rubble piles grow. Autonomous units must constantly re‑evaluate terrain viability.

### Vertical & Interior Combat
Ruined towers, broken stairwells, collapsed floors, and underground transit tunnels create multi‑layer combat zones.

### Autonomy & Hazard‑Aware Maneuvering
Units choose routes based on rubble stability, fire hazards, smoke density, and sensor clarity. Autonomous logic reroutes when terrain becomes impassable or collapses.

### Sensor & Information Warfare
Smoke, dust, heat, metallic clutter, and collapsed structures degrade sensors. Relay towers on surviving rooftops restore clarity. Destroying relays creates blind pockets across the city.

### Logistics & Resources
Fuel caches, industrial depots, and makeshift shelters serve as resupply nodes. Convoys must navigate cratered roads and avoid ambush‑prone rubble corridors.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between heavy armor, urban infantry, drones, or artillery. Blue deploys from the southwest industrial ruins; Red deploys from the northeast fortified tower.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² war‑torn urban zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over the real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Industrial Ruin Command)** — (0,0), 150×150 m  
- **Red HQ (Fortified Tower Spire)** — (1000,1000), 150×150 m  
- **Central Ruin Basin** — (500,450), 300×300 m  
- **Transit Tunnel Network** — (250,800), 120×120 m  
- **Rubble Ridge Network** — (800,300), 200×200 m  
- **Destruction Corridor** — (0,450 → 400,900)  
- **Crater Boulevard Route** — (300,0 → 450,350)  
- **Rooftop Relay A** — (650,150), +240 m  
- **Rooftop Relay B** — (150,650), +260 m  
- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### Destruction Corridor
Major chokepoint; terrain shifts unpredictably, ideal for ambushes.

### Rubble Ridge Network
Interior flanking routes; unstable high‑ground with long sightlines.

### Ruin Basin
Combined‑arms engagements broken by rubble; smoke reduces accuracy.

### Crater Boulevard Route
Predictable but safe; vulnerable to overwatch from rubble ridges.

### Sightlines
Rooftop Relay A provides long‑range sensor coverage (~300 m). Smoke and dust create fog‑of‑war pockets.

## Deployment Zones
- **Blue Deployment:** SW industrial ruins + armor staging  
- **Red Deployment:** NE fortified tower + AA emplacements  
- **Neutral Forward Zone:** Outpost at (500,950) for drone drops

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

## Implementation Plan & Testing

### Milestones
1. Prototype Layout — Month 1  
2. Core Systems — Month 2  
3. AR Integration — Month 3  
4. Content Fill — Months 4–5  
5. Gameplay & Polish — Months 6–7  
6. Testing & QA — Month 8

### Testing Plan

#### Autonomy
Verify rubble navigation, combined‑arms coordination, hazard avoidance.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks under smoke and dust.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, dynamic terrain logic, deployment budget enforcement, anchor drift, sensor accuracy.
