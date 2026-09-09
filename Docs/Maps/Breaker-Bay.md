# Breaker Bay

**Port / Industrial Battlefield**

## Executive Summary

Breaker Bay is a high‑intensity port‑industrial battlefield designed for naval invasion, logistics warfare, and urban combat. The terrain features massive cargo terminals, fortified seawalls, container yards, ship berths, industrial cranes, coastal roads, and dense port‑adjacent housing blocks.

The map emphasizes Obsidian Protocol’s autonomous multi‑domain tactics, sensor‑fusion warfare, comms‑relay networks, and logistics under heavy industrial congestion. In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of port territory.

Key features include naval landing zones, container‑maze combat, elevated crane platforms, industrial choke corridors, and multi‑route amphibious assault paths.

## Design Goals & Gameplay

### Naval Invasion & Coastal Assault
Breaker Bay supports coordinated naval landings, amphibious pushes, and ship‑to‑shore combat. Players must secure beachheads, docks, and industrial platforms.

### Autonomy & Multi‑Route Logistics
Autonomous AI chooses between pier landings, seawall breaches, container‑yard infiltration, or elevated crane traversal. Units adapt to shifting cargo layouts and blocked roads.

### Sensor & Information Warfare
Fog, sea spray, industrial smoke, and metal clutter distort sensors. Relay towers on cranes and rooftops restore clarity. Destroying relays creates fog‑of‑war pockets across port and sea.

### Logistics & Resources
Cargo terminals, fuel depots, and ship berths serve as resupply nodes. Long supply lines across exposed docks and narrow industrial corridors force careful convoy protection.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between naval landing craft, heavy ground armor, or urban infantry.  
Blue deploys from the southwest landing pier; Red deploys from the northeast industrial command tower.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² port zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Landing Pier Command)** — (0,0), 140×140 m  
  Amphibious staging zone with naval craft and armored vehicles.

- **Red HQ (Industrial Command Tower)** — (1000,1000), 140×140 m  
  Fortified port‑authority tower with radar mast and AA emplacements.

- **Central Cargo Terminal** — (500,450), 250×250 m  
  Container stacks, forklifts, cranes, narrow industrial corridors.

- **Fuel & Logistics Depot** — (250,800), 100×100 m  
  Fuel tanks, pipelines, cargo trucks, supply crates.

- **Ship Berth Row** — (800,300), 200×150 m  
  Docked cargo ships, loading cranes, gangways.

- **Seawall & Breakwater** — (0,450 → 400,900)  
  Fortified wall with artillery positions and breach points.

- **Container Maze** — (300,0 → 450,350)  
  Dense stacked containers forming a close‑range combat labyrinth.

- **Crane Ridge** — (650,150), +260 m  
  High‑ground crane platforms with long‑range sensor coverage.

- **Harbor Rooftops** — (150,650), +240 m  
  Comms relay tower and drone pads.

- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### Seawall Breach Points
Critical for naval invasion; heavily fortified.

### Container Maze
Close‑range combat with tight corridors and ambush angles.

### Cargo Terminal Lanes
Long straight industrial lanes ideal for armor and snipers.

### Ship Berth Row
Mixed‑range combat; naval units can support ground forces.

### Sightlines
Crane Ridge provides long‑range sensor coverage (~350 m). Fog and industrial smoke create fog‑of‑war pockets.

## Deployment Zones
- **Blue Deployment:** SW landing pier + naval staging  
- **Red Deployment:** NE industrial command tower + AA emplacements  
- **Neutral Forward Zone:** Outpost at (500,950)

## Assets & Environment

### Terrain
Concrete docks, seawalls, breakwaters, industrial yards, coastal roads.

### Vegetation
Sparse palms, shrubs, industrial planters.

### Buildings & Props
Military: Bunkers, AA guns, radar dishes, watchtowers  
Industrial: Cranes, pipelines, generators, cargo containers  
Logistics: Boats, trucks, forklifts, crates, fuel drums

### Effects
Fog, sea spray, smoke plumes, sparks, wind gusts.

### Audio
Waves, machinery, horns, radio chatter, seagulls.

## LOD & Budget

Target total: ~2.59M tris

- Seawalls/Rock: ~600k  
- Docks/Concrete: ~320k  
- Buildings/Industrial: ~750k  
- Roads/Paths: ~120k  
- Vegetation: ~300k  
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
   Block out docks, cargo terminal, seawall. AR anchor test.

2. **Month 2 — Core Systems**  
   NavMesh, naval pathfinding, autonomy behaviors.

3. **Month 3 — AR Integration**  
   Anchors, plane detection, basic interactivity.

4. **Months 4–5 — Content Fill**  
   Final models, textures, lighting, audio.

5. **Months 6–7 — Gameplay & Polish**  
   Objectives, balance, VFX (fog, smoke, sea spray).

6. **Month 8 — Testing & QA**  
   Multiplayer stress test, sensor validation.

### Testing Plan

#### Autonomy
Verify naval landing logic, container‑maze navigation, aerial flanking.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, multi‑domain coordination, deployment budget enforcement, anchor drift, sensor accuracy.
