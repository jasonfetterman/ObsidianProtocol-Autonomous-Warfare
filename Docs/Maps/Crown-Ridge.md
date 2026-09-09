# Crown Ridge

**Mountain / Military / Command Networks & Strategic Positioning**

## Executive Summary

Crown Ridge is a fortified mountain‑military battlefield built for command‑network warfare, strategic positioning, elevation control, and long‑range coordination. The terrain features steep alpine ridges, hardened bunkers, command relays, fortified passes, elevated artillery platforms, and multi‑tier mountain roads.

The map emphasizes Obsidian Protocol’s autonomous command‑network logic, sensor‑relay chaining, air‑ground coordination, and logistics under high‑altitude constraints. In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of mountainous territory.

Key features include command‑relay chains, multi‑tier defensive positions, elevation‑driven chokepoints, and strategic high‑ground control.

## Design Goals & Gameplay

### Command‑Network Warfare
Crown Ridge prioritizes command‑relay nodes, sensor hubs, and strategic communication lines. Destroying or capturing relays shifts control of entire sectors.

### Elevation & Strategic Positioning
High‑ground positions dominate artillery arcs, recon coverage, and defensive lines. Controlling ridges is essential for long‑range dominance.

### Autonomy & Command‑Chain Logic
Autonomous AI selects routes that maintain command‑network integrity, avoids exposed plateaus, and uses bunkers to break line‑of‑sight.

## Sensor & Information Warfare

Thin air, fog pockets, and elevation gradients distort sensors. Relay towers on peaks restore clarity. Destroying relays creates massive blind zones across the ridge.

## Logistics & Resources

Mountain depots, command bunkers, and helipads serve as resupply nodes. Supply convoys must navigate narrow roads and avoid ambush‑prone passes.

## Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between heavy artillery, command‑relay units, or mobile mountain infantry.  
Blue deploys from the southwest valley command; Red deploys from the northeast ridge citadel.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² mountain zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Valley Command Base)** — (0,0), 140×140 m  
  Mountain‑foot command center with relay uplinks and artillery.

- **Red HQ (Ridge Citadel)** — (1000,1000), 140×140 m  
  Fortified high‑ground citadel with radar mast and AA emplacements.

- **Central Crown Plateau** — (500,450), 250×250 m  
  Wind‑exposed plateau with long‑range sightlines and command relay nodes.

- **Command Relay Depot** — (250,800), 100×100 m  
  Relay towers, generators, comms equipment, supply crates.

- **Cliffside Bunker Network** — (800,300), 150×150 m  
  Interior tunnels, firing ports, hardened defensive positions.

- **Ridge Pass Corridor** — (0,450 → 400,900)  
  Narrow elevated corridor; critical choke zone for command‑network control.

- **Lower Valley Road** — (300,0 → 450,350)  
  Safer movement route with partial cover.

- **Peak A (Crown Peak)** — (650,150), +300 m  
  Primary command‑relay hub with long‑range sensor coverage.

- **Peak B (Sentinel Peak)** — (150,650), +280 m  
  Secondary relay tower and AA platform.

- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### Ridge Pass Corridor
Extremely narrow; ideal for defensive artillery and command‑relay protection.

### Cliffside Bunker Network
Interior routes and firing ports; strong defensive positions.

### Crown Plateau
Long‑range sightlines; command‑network dominance essential.

### Lower Valley Road
Safer but predictable; vulnerable to aerial recon.

### Sightlines
Peak A provides long‑range sensor coverage (~350 m). Fog pockets and elevation gradients create blind zones.

## Deployment Zones
- **Blue Deployment:** SW valley command base + rear rally point  
- **Red Deployment:** NE ridge citadel + AA emplacements  
- **Neutral Forward Zone:** Outpost at (500,950)

## Assets & Environment

### Terrain
Cliffs, ridges, plateaus, valleys, rock formations.

### Vegetation
Sparse alpine shrubs, pine clusters, grass patches.

### Buildings & Props
Military: Bunkers, AA guns, radar dishes, helipads, command relays  
Industrial: Generators, pipelines, comms towers  
Logistics: Trucks, crates, fuel drums, drones

### Effects
Fog, wind gusts, dust, snow flurries (optional), rockfall debris.

### Audio
Wind, distant artillery, radio chatter, aircraft movement.

## LOD & Budget

Target total: ~2.59M tris

- Cliffs/Ridges: ~600k  
- Rock Formations: ~320k  
- Buildings/Military: ~750k  
- Roads/Paths: ~300k  
- Vegetation: ~150k  
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
   Block out ridges, plateau, bunkers. AR anchor test.

2. **Month 2 — Core Systems**  
   NavMesh, command‑network autonomy behaviors.

3. **Month 3 — AR Integration**  
   Anchors, plane detection, basic interactivity.

4. **Months 4–5 — Content Fill**  
   Final models, textures, lighting, audio.

5. **Months 6–7 — Gameplay & Polish**  
   Objectives, balance, VFX (fog, wind, dust).

6. **Month 8 — Testing & QA**  
   Multiplayer stress test, sensor validation.

### Testing Plan

#### Autonomy
Verify command‑network logic, ridge navigation, air‑ground coordination.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, elevation behavior, command‑network integrity, deployment budget enforcement, anchor drift, sensor accuracy.
