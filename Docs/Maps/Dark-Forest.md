# Dark Forest

**Dense Forest / Night Operations**

## Executive Summary

Dark Forest is a night‑time dense woodland battlefield built for thermal warfare, sensor disruption, stealth movement, and information denial. The terrain features thick canopy cover, tangled undergrowth, moonlit clearings, abandoned research cabins, shallow creeks, and elevated ridgelines. Darkness, humidity, and foliage create extreme visibility challenges.

The map emphasizes Obsidian Protocol’s autonomous night‑combat behaviors, thermal‑sensor logic, comms‑relay warfare, and logistics under low‑light constraints. In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of nighttime forest terrain.

Key features include thermal chokepoints, sensor‑blind zones, hidden trails, and ambush‑heavy infiltration routes.

## Design Goals & Gameplay

### Night Combat & Thermal Warfare
Dark Forest prioritizes engagements where thermal imaging, IR sensors, and low‑light optics dominate. Units rely on heat signatures rather than visual clarity.

### Autonomy & Low‑Visibility Pathfinding
Autonomous AI selects concealed trails, avoids moonlit clearings, and uses darkness for stealth. Units adapt to shifting fog density and thermal interference.

### Sensor & Information Warfare
Dense canopy, humidity, fog, and temperature gradients distort sensors. Relay towers on ridges restore clarity. Destroying relays creates massive thermal‑blind zones.

### Logistics & Resources
Ranger cabins, research outposts, and forest depots serve as resupply nodes. Supply convoys must navigate narrow trails and avoid ambush‑prone darkness pockets.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between stealth infantry, thermal drones, or light vehicles.  
Blue deploys from the southwest night‑ops camp; Red deploys from the northeast ridge bunker.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² forest zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Night‑Ops Camp)** — (0,0), 140×140 m  
  Thermal‑equipped forward base with recon drones.

- **Red HQ (Ridge Bunker)** — (1000,1000), 140×140 m  
  Elevated bunker with comms tower and IR floodlights.

- **Central Shadow Basin** — (500,450), 250×250 m  
  Dense canopy, fog pockets, minimal visibility.

- **Research Outpost** — (250,800), 100×100 m  
  Abandoned labs, generators, thermal equipment.

- **Old Ranger Cabins** — (800,300), 120×120 m  
  Interior routes, close‑range ambush zones.

- **Creek Network** — (0,450 → 400,900)  
  Shallow water crossings with thermal distortion.

- **Hidden Night Trail** — (300,0 → 450,350)  
  Concealed path ideal for stealth infiltration.

- **Thermal Ridge A** — (650,150), +260 m  
- **Relay Ridge B** — (150,650), +240 m  

- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### Hidden Night Trail
Perfect for ambushes; extremely narrow and concealed.

### Creek Crossings
Water cools heat signatures; thermal sensors degrade.

### Cabin Cluster
Interior flanking routes; close‑range engagements.

### Shadow Basin
Heavy foliage + darkness = unpredictable sightlines.

### Sightlines
Thermal Ridge A provides long‑range sensor coverage (~300 m). Fog and temperature gradients create thermal‑blind pockets.

## Deployment Zones
- **Blue Deployment:** SW night‑ops camp + rear rally point  
- **Red Deployment:** NE ridge bunker + IR floodlights  
- **Neutral Forward Zone:** Outpost at (500,950)

## Assets & Environment

### Terrain
Forest hills, undergrowth, creeks, ridges, fallen logs.

### Vegetation
Dense trees, shrubs, ferns, moss, fog pockets.

### Buildings & Props
Military: IR floodlights, antennas, bunkers  
Civilian: Cabins, sheds, research equipment  
Logistics: Trucks, crates, fuel drums, thermal drones

### Effects
Fog, humidity haze, drifting mist, falling leaves, thermal distortion.

### Audio
Wind, insects, distant machinery, radio chatter, night wildlife.

## LOD & Budget

Target total: ~2.69M tris

- Forest Trees: ~600k  
- Underbrush/Rocks: ~320k  
- Buildings/Cabins: ~750k  
- Roads/Trails: ~400k  
- Vegetation: ~200k  
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
   Block out forest basin, ridges, cabins. AR anchor test.

2. **Month 2 — Core Systems**  
   NavMesh, thermal‑aware pathfinding, autonomy behaviors.

3. **Month 3 — AR Integration**  
   Anchors, plane detection, basic interactivity.

4. **Months 4–5 — Content Fill**  
   Final models, textures, lighting, audio.

5. **Months 6–7 — Gameplay & Polish**  
   Objectives, balance, VFX (fog, thermal distortion).

6. **Month 8 — Testing & QA**  
   Multiplayer stress test, sensor validation.

### Testing Plan

#### Autonomy
Verify stealth logic, thermal‑aware navigation, ambush behavior.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks under night conditions.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, concealment behavior, thermal response, deployment budget enforcement, anchor drift, sensor accuracy.
