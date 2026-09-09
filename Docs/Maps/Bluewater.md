# Bluewater

**Open Ocean / Primarily Naval Warfare**

## Executive Summary

Bluewater is a vast open‑ocean battlefield built for pure naval warfare, long‑range engagements, carrier operations, submarine combat, and autonomous fleet coordination. The terrain is dominated by deep water, rolling swells, scattered reefs, thermal vents, and a few remote support platforms.

The map emphasizes Obsidian Protocol’s autonomous naval routing, sensor‑fusion across sonar and radar, comms‑relay warfare, and logistics under wide‑open maritime conditions. In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of open ocean.

Key features include long‑range naval duels, carrier air operations, submarine stealth zones, and strategic control of deep‑water lanes.

## Design Goals & Gameplay

### Pure Naval Combat
Bluewater prioritizes ship‑to‑ship warfare: missile duels, torpedo lanes, long‑range gunnery, and carrier‑based air support. No land combat except on small support platforms.

### Carrier & Air Operations
Aircraft carriers launch drones, VTOLs, and strike craft. Airspace corridors above the ocean define recon, interception, and strike patterns.

### Submarine Warfare & Stealth
Thermal vents, deep trenches, and sonar‑scatter zones create stealth pockets. Submarines excel in ambushes and long‑range torpedo strikes.

### Autonomy & Fleet Coordination
Autonomous units coordinate across surface, air, and subsurface domains. Fleets adapt to shifting sonar conditions, radar interference, and missile trajectories.

### Sensor & Information Warfare
Sea fog, humidity, thermal distortion, sonar scatter, and magnetic interference degrade sensors. Relay buoys restore clarity. Destroying relays creates blind pockets across the ocean.

### Logistics & Resources
Fuel barges, offshore platforms, and carrier decks serve as resupply nodes. Convoys must navigate open water and avoid ambush‑prone deep‑water lanes.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between carrier dominance, submarine superiority, or missile‑heavy surface fleets.  
Blue deploys from the southwest carrier group; Red deploys from the northeast strike flotilla.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² open‑ocean zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Carrier Group Bravo)** — (0,0), 150×150 m  
  Carrier deck, drone pads, missile destroyers.

- **Red HQ (Strike Flotilla Command)** — (1000,1000), 150×150 m  
  Cruisers, radar ships, submarine tenders.

- **Central Deep‑Water Lane** — (500,450), 350×300 m  
  Long‑range naval combat zone; ideal for missile duels.

- **Offshore Support Platform** — (250,800), 120×120 m  
  Fuel barges, repair cranes, resupply docks.

- **Thermal Vent Field** — (800,300), 200×200 m  
  Sonar distortion; submarine stealth routes.

- **Open‑Ocean Corridor** — (0,450 → 400,900)  
  Primary naval chokepoint; long sightlines and minimal cover.

- **Surface Patrol Route** — (300,0 → 450,350)  
  Safer but predictable; vulnerable to submarine ambush.

- **Relay Buoy A** — (650,150), +240 m  
- **Relay Buoy B** — (150,650), +260 m  

- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### Open‑Ocean Corridor
Major naval chokepoint; controlling it determines fleet movement.

### Thermal Vent Field
Sonar distortion; ideal for submarine ambushes.

### Deep‑Water Lane
Long‑range missile and gunnery engagements.

### Surface Patrol Route
Predictable but safe; vulnerable to subsurface attacks.

### Sightlines
Relay Buoy A provides long‑range sensor coverage (~400 m). Sea fog and humidity create fog‑of‑war pockets.

## Deployment Zones
- **Blue Deployment:** SW carrier group + air staging  
- **Red Deployment:** NE strike flotilla + radar ships  
- **Neutral Forward Zone:** Outpost at (500,950)

## Assets & Environment

### Terrain
Open ocean, deep‑water lanes, thermal vents, offshore platforms.

### Vegetation
None — maritime zone only.

### Buildings & Props
Military: Radar ships, AA guns, sonar arrays, carriers  
Industrial: Platforms, cranes, fuel barges  
Logistics: Ships, cargo crates, drones

### Effects
Sea spray, fog, humidity haze, wave impacts, thermal distortion.

### Audio
Waves, wind, ship engines, sonar pings, radio chatter.

## LOD & Budget

Target total: ~2.29M tris

- Ocean/Terrain: ~600k  
- Naval/Industrial: ~320k  
- Ships/Military: ~750k  
- Platforms/Paths: ~120k  
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
   Block out ocean lanes, platforms, vent field. AR anchor test.

2. **Month 2 — Core Systems**  
   NavMesh (sea), naval autonomy, air‑sea coordination.

3. **Month 3 — AR Integration**  
   Anchors, plane detection, basic interactivity.

4. **Months 4–5 — Content Fill**  
   Final models, textures, lighting, audio.

5. **Months 6–7 — Gameplay & Polish**  
   Objectives, balance, VFX (sea spray, fog, thermal distortion).

6. **Month 8 — Testing & QA**  
   Multiplayer stress test, sensor validation.

### Testing Plan

#### Autonomy
Verify naval routing, air‑sea coordination, submarine stealth logic.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, sonar interference.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, naval logic, deployment budget enforcement, anchor drift, sensor accuracy.
