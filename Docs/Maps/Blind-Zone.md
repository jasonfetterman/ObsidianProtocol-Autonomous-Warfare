# Blind Zone

**Signal Lost — Remote Facility / Communications Degradation & Electronic Warfare**

## Executive Summary

Signal Lost is a remote communications‑facility battlefield built for electronic warfare, comms degradation, autonomous counter‑EW behavior, and precision operations across isolated terrain. The environment features satellite dishes, microwave towers, buried fiber lines, hardened relay bunkers, diesel generators, snow‑scoured ridges or dry scrub flats (season‑dependent), and long‑range sensor arrays.

The map emphasizes Obsidian Protocol’s autonomous EW‑aware routing, sensor‑fusion under jamming and interference, comms‑relay warfare, and logistics across a remote, infrastructure‑dependent battlespace. In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of remote‑facility terrain.

Key features include EW zones, blackout corridors, relay‑tower control, and comms‑dependent maneuver warfare.

## Design Goals & Gameplay

### Communications Degradation Warfare
Signal Lost prioritizes jamming, spoofing, interference, blackout zones, and comms‑dependent tactics. Units operate under partial or total signal loss.

### Electronic Warfare (EW) Dominance
Players deploy EW vehicles, drone jammers, signal scramblers, and counter‑EW units. Destroying or capturing relay towers shifts the entire tactical landscape.

### Autonomous EW‑Aware Routing
Autonomous units choose routes based on signal strength, interference pockets, terrain occlusion, and fallback relay availability.

### Remote‑Facility Combat
Long‑range fire, drone reconnaissance, armored pushes, and stealth infiltration revolve around controlling the facility’s communication backbone.

### Logistics & Resources
Fuel caches, generator stations, and buried cable hubs serve as resupply nodes. Convoys must navigate blackout corridors and avoid EW‑driven ambush zones.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between EW dominance, stealth infiltration, or signal‑resistant armor.  
Blue deploys from the southwest service‑road checkpoint; Red deploys from the northeast relay‑spire command.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² remote‑facility zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Service‑Road Checkpoint)** — (0,0), 150×150 m  
  Armored vehicles, EW trucks, drone pads.

- **Red HQ (Relay‑Spire Command)** — (1000,1000), 150×150 m  
  Elevated bunker with radar mast, EW arrays, AA emplacements.

- **Central Relay Basin** — (500,450), 300×300 m  
  Relay towers, interference pockets, blackout zones, long‑range EW duels.

- **Generator Station Cluster** — (250,800), 120×120 m  
  Diesel generators, transformers, interior flanking routes.

- **Sensor Array Ridge** — (800,300), 200×200 m  
  High‑ground overwatch, signal triangulation, counter‑EW vantage points.

- **Blackout Corridor** — (0,450 → 400,900)  
  Primary EW chokepoint; jamming fields, interference pockets, false signals.

- **Service Road Route** — (300,0 → 450,350)  
  Safer but predictable; vulnerable to EW‑driven ambushes.

- **Relay Tower A** — (650,150), +240 m  
- **Relay Tower B** — (150,650), +260 m  

- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### Blackout Corridor
Major chokepoint filled with jamming fields and false signals.

### Sensor Array Ridge
Extreme long‑range sightlines; ideal for counter‑EW and recon triangulation.

### Relay Basin
Long‑range EW engagements distorted by interference and signal loss.

### Service Road Route
Predictable but safe; vulnerable to EW‑driven ambushes.

### Sightlines
Relay Tower A provides long‑range sensor coverage (~350 m). Interference and jamming create fog‑of‑war pockets.

## Deployment Zones
- **Blue Deployment:** SW service‑road checkpoint + EW vehicles  
- **Red Deployment:** NE relay‑spire command + AA emplacements  
- **Neutral Forward Zone:** Outpost at (500,950)

## Assets & Environment

### Terrain
Ridges, flats, relay towers, generator stations, service roads.

### Vegetation
Sparse shrubs, dry grass, or snow patches depending on climate.

### Buildings & Props
Military: EW trucks, AA guns, radar dishes, bunkers  
Industrial: Generators, transformers, pipelines, relay towers  
Logistics: Trucks, crates, fuel drums, drones

### Effects
Signal distortion, interference waves, smoke, dust, fog.

### Audio
Wind, generator hum, static bursts, radio chatter.

## LOD & Budget

Target total: ~2.29M tris

- Facility/Ridge Terrain: ~600k  
- Industrial/EW Meshes: ~320k  
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
   Block out relay basin, ridges, generator stations. AR anchor test.

2. **Month 2 — Core Systems**  
   NavMesh, EW autonomy behaviors, interference logic.

3. **Month 3 — AR Integration**  
   Anchors, plane detection, basic interactivity.

4. **Months 4–5 — Content Fill**  
   Final models, textures, lighting, audio.

5. **Months 6–7 — Gameplay & Polish**  
   Objectives, balance, VFX (interference waves, static bursts).

6. **Month 8 — Testing & QA**  
   Multiplayer stress test, sensor validation.

### Testing Plan

#### Autonomy
Verify EW‑aware navigation, interference‑zone avoidance, fallback routing.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks under jamming.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, EW logic, deployment budget enforcement, anchor drift, sensor accuracy.
