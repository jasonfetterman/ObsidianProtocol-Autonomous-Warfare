# Blackwater

**River / Wetlands Battlefield**

## Executive Summary

Blackwater is a dense river‑wetlands battlefield designed for water crossings, naval support, and ambush‑heavy ground combat. The terrain features winding rivers, marshes, flooded forests, unstable wetlands, elevated levees, fishing docks, and military river outposts.

The map emphasizes Obsidian Protocol’s autonomous amphibious tactics, sensor‑limited environments, comms‑relay networks, and logistics under wetland constraints. In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of river territory.

Key features include multi‑route river crossings, swamp ambush zones, naval patrol lanes, elevated levee roads, and hidden infiltration paths.

## Design Goals & Gameplay

### Amphibious & Wetland Combat
Blackwater supports coordinated ground pushes, riverine naval patrols, and shallow‑water vehicle crossings. Marsh terrain slows movement and encourages ambushes.

### Autonomy & Ambush Logic
Autonomous AI chooses between river crossings, levee routes, swamp infiltration, or dockside flanking. Units adapt to shifting water levels and visibility.

### Sensor & Information Warfare
Fog, humidity, dense vegetation, and water reflections distort sensors. Relay towers on levees restore clarity. Destroying relays creates fog‑of‑war pockets across wetlands and river channels.

### Logistics & Resources
River docks, fuel depots, and levee checkpoints serve as resupply nodes. Supply convoys must navigate unstable wetland roads and exposed river crossings.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between amphibious units, heavy ground armor, or stealth infantry.  
Blue deploys from the southwest riverbank; Red deploys from the northeast levee fortress.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² wetlands zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over the real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Riverbank Command)** — (0,0), 140×140 m  
- **Red HQ (Levee Fortress)** — (1000,1000), 140×140 m  

- **Central Wetland Basin** — (500,450), 250×250 m  
- **Fuel & Logistics Dock** — (250,800), 100×100 m  
- **Fishing Village Ruins** — (800,300), 150×150 m  

- **Main River Channel** — (0,450 → 400,900)  
  Deep river with crossings at (150,600) and (350,500).

- **Swamp Infiltration Route** — (300,0 → 450,350)  
  Dense vegetation; ideal for stealth ambushes.

- **Levee Road Network** — (650,150), +260 m  
  Elevated road with long‑range sightlines.

- **Relay Tower Ridge** — (150,650), +240 m  
  Comms relay tower and sensor hub.

- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### River Crossings
Critical for movement; naval units can support ground forces.

### Swamp Infiltration Route
Dense vegetation; perfect for ambushes and stealth.

### Levee Road Network
Long‑range sightlines; ideal for artillery and recon.

### Fishing Village Ruins
Close‑range combat with interior flanking routes.

### Sightlines
Relay Tower Ridge provides long‑range sensor coverage (~300 m). Fog and vegetation create fog‑of‑war pockets.

## Deployment Zones
- **Blue Deployment:** SW riverbank + amphibious staging  
- **Red Deployment:** NE levee fortress + artillery positions  
- **Neutral Forward Zone:** Outpost at (500,950)

## Assets & Environment

### Terrain
Rivers, marshes, wetlands, levees, flooded forests.

### Vegetation
Dense reeds, swamp trees, mangroves, grass patches.

### Buildings & Props
Military: Bunkers, watchtowers, radar dishes  
Industrial: Docks, pipelines, generators, pumps  
Logistics: Boats, trucks, crates, fuel drums

### Effects
Fog, humidity haze, water reflections, smoke plumes.

### Audio
Water movement, insects, distant machinery, radio chatter.

## LOD & Budget

Target total: ~2.59M tris

- Wetland Trees: ~600k  
- Marsh/River Meshes: ~320k  
- Buildings/Industrial: ~750k  
- Roads/Levees: ~120k  
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
   Block out river, wetlands, levees. AR anchor test.

2. **Month 2 — Core Systems**  
   NavMesh, amphibious pathfinding, autonomy behaviors.

3. **Month 3 — AR Integration**  
   Anchors, plane detection, basic interactivity.

4. **Months 4–5 — Content Fill**  
   Final models, textures, lighting, audio.

5. **Months 6–7 — Gameplay & Polish**  
   Objectives, balance, VFX (fog, water reflections).

6. **Month 8 — Testing & QA**  
   Multiplayer stress test, sensor validation.

### Testing Plan

#### Autonomy
Verify amphibious logic, swamp infiltration, levee pathfinding.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, multi‑domain coordination, deployment budget enforcement, anchor drift, sensor accuracy.
