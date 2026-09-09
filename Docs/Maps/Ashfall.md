# Ashfall

**Volcanic / Industrial Battlefield**

## Executive Summary

Ashfall is a hostile volcanic‑industrial battlefield defined by unstable terrain, low visibility, and constant environmental hazards. Lava flows, ash storms, fissures, and geothermal machinery create a dynamic, high‑risk combat zone.

The map emphasizes Obsidian Protocol’s autonomous decision‑making, sensor degradation, comms‑relay warfare, and logistics under extreme environmental pressure. In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of volcanic terrain.

Key features include active lava channels, industrial extraction rigs, geothermal plants, hardened bunkers, and critical chokepoints formed by collapsed rock and molten rivers.

## Design Goals & Gameplay

### Volcanic Hazard Combat
Ashfall focuses on infantry and light‑vehicle combat in unstable terrain. Lava flows block paths, ash reduces visibility, and eruptions create temporary hazards. Units must adapt quickly.

### Autonomy & Hazard Avoidance
Autonomous AI chooses safe routes around fissures, avoids lava surges, and repositions when ash storms reduce sensor range. Units adapt without micromanagement.

### Sensor & Information Warfare
Volcanic heat, smoke, and ash distort radar, thermal, and acoustic sensors. Relay towers on hardened ridges restore clarity. Destroying relays plunges entire sectors into fog‑of‑war.

### Logistics & Resources
Geothermal plants and extraction rigs serve as resource nodes. Supply convoys must navigate unstable roads and avoid lava flows. Controlling the Refinery and Geothermal Plant grants major resupply advantages.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between heavy hazard‑resistant units or fast recon squads.  
Blue deploys from the southwest hardened bunker; Red deploys from the northeast industrial ridge.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² volcanic district (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over the real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Hardened Bunker)** — (0,0), 120×120 m  
- **Red HQ (Industrial Ridge)** — (1000,1000), 120×120 m  

- **Central Geothermal Plant** — (500,450), 200×200 m  
- **Extraction Rig Complex** — (300,250), 150×150 m  
- **Volcanic Refinery** — (800,300), 100×100 m  

- **Lava Channels** — (0,600 → 400,900)  
  Active molten river with hardened crossings at (150,700) and (350,650).

- **Fissure Network** — (400,0 → 600,350)  
  Unstable cracked terrain; hazard zone.

- **Ridge A** — (650,150), +280 m  
- **Ridge B** — (150,650), +240 m  

- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### Lava Crossings
Two hardened bridges; controlling them prevents flanking.

### Fissure Network
Unstable terrain; close‑range combat and hazard avoidance.

### Steam Vent Fields
Visibility drops to near zero during vent surges.

### Sightlines
Ridge A provides long‑range sensor coverage (~250 m). Ash storms create heavy fog‑of‑war pockets.

## Deployment Zones
- **Blue Deployment:** SW hardened bunker + rally point  
- **Red Deployment:** NE industrial ridge + rally point  
- **Neutral Forward Zone:** Outpost at (500,950)

## Assets & Environment

### Terrain
Volcanic rock, lava flows, fissures, ash fields, hardened industrial platforms.

### Vegetation
None; occasional dead trees and scorched shrubs.

### Buildings & Props
Industrial: Turbines, smelters, pipes, extraction rigs  
Military: Hardened bunkers, watchtowers, comms masts  
Logistics: Trucks, crates, fuel drums, hazard‑resistant containers

### Effects
Lava glow, ash storms, smoke plumes, steam vents, sparks.

### Audio
Eruptions, rumbling earth, machinery hum, radio chatter.

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
Verify hazard‑avoidance logic and pathfinding around lava and fissures.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, and LOS checks.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, hazard response, deployment budget enforcement, anchor drift, sensor accuracy.
