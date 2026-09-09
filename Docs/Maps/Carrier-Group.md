# Carrier Group

**Ocean / Naval / Fleet Warfare**

## Executive Summary

Carrier Group is a deep‑ocean naval battlefield built for fleet‑scale warfare, carrier‑based air operations, autonomous maritime coordination, and multi‑domain combat across sea, air, and long‑range missile envelopes. The environment features open ocean sectors, shifting wave patterns, carrier decks, destroyer screens, submarine lanes, missile arcs, radar pickets, and logistics ships.

The map emphasizes Obsidian Protocol’s autonomous naval routing, sensor‑fusion under sea clutter and radar interference, comms‑relay warfare, and logistics across a vast, mobile battlespace. In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of ocean terrain.

Key features include carrier air wings, destroyer screens, submarine shadow zones, and long‑range missile duels.

## Design Goals & Gameplay

### Fleet‑Scale Naval Warfare
Carrier Group prioritizes coordinated fleets: carriers, destroyers, cruisers, submarines, drones, and aircraft. Engagements occur across massive distances.

### Carrier Air Wing Operations
Fighters, bombers, VTOLs, and recon drones launch from carrier decks. Air superiority determines strike capability and fleet survival.

### Destroyer Screens & Missile Defense
Destroyers and cruisers form layered defense rings using AA guns, CIWS, missile interceptors, and EW systems.

### Submarine Shadow Zones
Submarines exploit deep‑water lanes, thermal layers, and sonar shadows to ambush surface ships or disrupt logistics.

### Autonomous Maritime Routing
Autonomous units choose routes based on wave patterns, radar clarity, sonar returns, missile arcs, and threat vectors.

### Sensor & Information Warfare
Radar clutter, sea spray, thermal layers, jamming, and EW interference degrade sensors. Relay ships restore clarity. Destroying relays creates blind pockets across the ocean.

### Logistics & Resources
Fuel ships, supply vessels, and repair barges serve as resupply nodes. Protecting logistics ships is critical for sustained operations.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between air dominance, missile supremacy, submarine control, or balanced fleet composition.  
Blue deploys from the southwest carrier strike group; Red deploys from the northeast cruiser‑led battle group.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² ocean zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Carrier Strike Group)** — (0,0), 150×150 m  
  Carrier deck, destroyer screen, drone pads, logistics ship.

- **Red HQ (Cruiser Battle Group)** — (1000,1000), 150×150 m  
  Cruiser flagship, radar mast, AA emplacements, missile batteries.

- **Central Ocean Combat Basin** — (500,450), 300×300 m  
  Open ocean, wave patterns, missile arcs, air‑sea engagements.

- **Logistics Ship Cluster** — (250,800), 120×120 m  
  Fuel ships, repair barges, interior escort routes.

- **Radar Picket Line** — (800,300), 200×200 m  
  High‑range radar ships, EW nodes, air‑corridor control.

- **Missile Corridor** — (0,450 → 400,900)  
  Primary long‑range missile lane; interception routes and EW traps.

- **Submarine Shadow Route** — (300,0 → 450,350)  
  Deep‑water thermal layer; ideal for stealth submarine movement.

- **Relay Ship A** — (650,150), +240 m  
- **Relay Ship B** — (150,650), +260 m  

- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### Missile Corridor
Major engagement lane; controlling it determines long‑range strike capability.

### Radar Picket Line
Extreme long‑range sightlines; ideal for air‑sea coordination and EW control.

### Ocean Combat Basin
Air‑sea engagements distorted by radar clutter and wave interference.

### Submarine Shadow Route
Predictable but stealth‑heavy; vulnerable to ASW patrols.

### Sightlines
Relay Ship A provides long‑range radar coverage (~400 m). Sea clutter and EW interference create fog‑of‑war pockets.

## Deployment Zones
- **Blue Deployment:** SW carrier strike group + air wing  
- **Red Deployment:** NE cruiser battle group + missile batteries  
- **Neutral Forward Zone:** Outpost at (500,950)

## Assets & Environment

### Terrain
Open ocean, wave patterns, carrier decks, destroyers, cruisers, submarines.

### Vegetation
None — ocean surface only.

### Buildings & Props
Military: AA guns, radar dishes, missile launchers, EW towers  
Naval: Ships, cranes, decks, lifeboats  
Logistics: Fuel ships, repair barges, drones

### Effects
Sea spray, smoke, missile trails, radar waves, static bursts.

### Audio
Waves, engines, radar hum, radio chatter, jet noise.

## LOD & Budget

Target total: ~2.29M tris

- Ocean/Ship Terrain: ~600k  
- Naval/Industrial Meshes: ~320k  
- Ships/Military: ~750k  
- Routes/Paths: ~120k  
- Props: ~500k  

### Textures
2048² for ships/terrain  
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
   Block out ocean basin, ship positions, radar picket line. AR anchor test.

2. **Month 2 — Core Systems**  
   NavMesh (sea + air), missile autonomy behaviors, radar logic.

3. **Month 3 — AR Integration**  
   Anchors, plane detection, basic interactivity.

4. **Months 4–5 — Content Fill**  
   Final models, textures, lighting, audio.

5. **Months 6–7 — Gameplay & Polish**  
   Objectives, balance, VFX (missile trails, radar waves).

6. **Month 8 — Testing & QA**  
   Multiplayer stress test, sensor validation.

### Testing Plan

#### Autonomy
Verify air‑sea coordination, missile‑aware routing, hazard avoidance.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks under EW interference.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, naval logic, deployment budget enforcement, anchor drift, sensor accuracy.
