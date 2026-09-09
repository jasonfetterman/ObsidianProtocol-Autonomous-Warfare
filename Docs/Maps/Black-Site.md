# Black Site

**Secret Military Facility / Experimental Warfare, Sensors, EW**

## Executive Summary

Black Site is a clandestine military research and operations complex built for experimental warfare, advanced sensors, electronic warfare (EW), and autonomous force testing. The terrain features hardened bunkers, underground labs, sensor arrays, comms vaults, prototype hangars, electromagnetic test chambers, and perimeter kill‑zones.

The map emphasizes Obsidian Protocol’s autonomous EW logic, sensor‑fusion under interference, comms‑relay disruption, and logistics inside a compartmentalized facility. In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of secret‑facility terrain.

Key features include EW corridors, stealth‑sensor blind zones, prototype weapon platforms, and multi‑layer interior combat.

## Design Goals & Gameplay

### Experimental Warfare & EW Dominance
Black Site prioritizes electronic warfare, sensor disruption, stealth infiltration, and prototype weapon deployment. EW zones dynamically alter unit behavior.

### Compartmentalized Interior Combat
The facility is divided into sealed wings, labs, vaults, and hangars. Combat shifts between tight corridors, open testing chambers, and fortified control rooms.

### Autonomy & EW‑Aware Pathfinding
Autonomous units choose routes based on EW interference, sensor blackout zones, and prototype hazards. Units adapt to shifting electromagnetic conditions.

### Sensor & Information Warfare
EMP bursts, jamming fields, thermal distortion, and active countermeasures degrade sensors. Relay towers inside comms vaults restore clarity. Destroying relays creates cascading blackout zones.

### Logistics & Resources
Prototype hangars, fuel vaults, and research depots serve as resupply nodes. Convoys must navigate narrow interior lanes and avoid ambush‑prone EW corridors.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between stealth units, EW specialists, or heavy breach teams.  
Blue deploys from the southwest access wing; Red deploys from the northeast command vault.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² secret‑facility zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over the real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Access Wing Command)** — (0,0), 150×150 m  
- **Red HQ (Command Vault)** — (1000,1000), 150×150 m  

- **Central Test Chamber** — (500,450), 300×300 m  
- **Research Lab Cluster** — (250,800), 120×120 m  
- **Prototype Hangar** — (800,300), 200×200 m  

- **EW Corridor** — (0,450 → 400,900)  
  Heavy jamming field; extreme sensor degradation.

- **Maintenance Route** — (300,0 → 450,350)  
  Safer interior path with partial cover.

- **Relay Tower A** — (650,150), +240 m  
- **Relay Tower B** — (150,650), +260 m  

- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### EW Corridor
Severe sensor disruption; ideal for stealth ambushes.

### Prototype Hangar
Interior flanking routes; unpredictable hazards.

### Test Chamber
Open interior space; long‑range experimental weapon lines.

### Research Labs
Tight corridors; close‑range combat.

### Sightlines
Relay Tower A provides long‑range sensor coverage (~300 m). EW fields create fog‑of‑war pockets and blackout zones.

## Deployment Zones
- **Blue Deployment:** SW access wing + breach staging  
- **Red Deployment:** NE command vault + EW defenses  
- **Neutral Forward Zone:** Outpost at (500,950)

## Assets & Environment

### Terrain
Labs, vaults, hangars, EW corridors, test chambers, maintenance tunnels.

### Vegetation
None — sterile industrial interior.

### Buildings & Props
Military: EW towers, radar dishes, AA guns, bunkers  
Industrial: Servers, generators, pipelines, control panels  
Logistics: Trucks, crates, fuel drums, drones

### Effects
EMP bursts, sparks, smoke, heat shimmer, holographic displays.

### Audio
Machinery hum, alarms, radio chatter, ventilation systems.

## LOD & Budget

Target total: ~2.29M tris

- Heavy Machinery: ~320k  
- Industrial Meshes: ~120k  
- Buildings/Interior: ~750k  
- Corridors/Paths: ~120k  
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
   Block out labs, hangars, EW corridor. AR anchor test.

2. **Month 2 — Core Systems**  
   NavMesh, EW‑aware autonomy behaviors, sensor logic.

3. **Month 3 — AR Integration**  
   Anchors, plane detection, basic interactivity.

4. **Months 4–5 — Content Fill**  
   Final models, textures, lighting, audio.

5. **Months 6–7 — Gameplay & Polish**  
   Objectives, balance, VFX (EMP bursts, sparks, holograms).

6. **Month 8 — Testing & QA**  
   Multiplayer stress test, sensor validation.

### Testing Plan

#### Autonomy
Verify EW‑aware navigation, blackout‑zone behavior, hazard avoidance.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks under EW.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, EW logic, deployment budget enforcement, anchor drift, sensor accuracy.
