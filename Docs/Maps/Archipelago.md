# Archipelago

**Island Chain / Distributed Warfare & Naval Control**

## Executive Summary

Archipelago is a multi‑island chain battlefield built for distributed warfare, naval control, amphibious maneuvering, and autonomous multi‑node coordination. The terrain features scattered islands, coral shelves, deep‑water channels, mangrove swamps, cliffside fortifications, fishing villages, and offshore naval platforms.

The map emphasizes Obsidian Protocol’s autonomous distributed‑node logic, naval‑air coordination, sensor‑fusion across water and land, and logistics under fragmented terrain constraints. In AR (Nreal Light/Air), the map anchors at roughly a 4–5 m viewing distance, covering a 10×10 m physical area representing about 1 km² of island‑chain terrain.

Key features include multi‑island objective nodes, naval chokepoints, amphibious landing zones, and distributed control of sea lanes.

## Design Goals & Gameplay

### Distributed Multi‑Island Warfare
Archipelago prioritizes simultaneous control of multiple islands, each with unique terrain, resources, and defensive positions. Victory depends on coordinated multi‑front pressure.

### Naval Control & Sea‑Lane Dominance
Deep‑water channels, coral shelves, and narrow straits create naval chokepoints. Controlling sea lanes determines reinforcement speed and amphibious landing viability.

### Amphibious Operations & Island Hopping
Players conduct coordinated landings using amphibious armor, drones, and infantry. Beaches, mangroves, and cliffs create layered defensive zones.

### Air Operations & Reconnaissance
VTOL aircraft, drones, and long‑range recon planes operate above the island chain. Airspace corridors between cliffs and offshore platforms define movement and targeting.

### Autonomy & Distributed Node Coordination
Autonomous units coordinate across multiple islands, selecting optimal nodes to attack or defend based on sensor data, enemy movement, and objective priority.

### Sensor & Information Warfare
Sea spray, humidity, thermal distortion, sonar scatter, and magnetic interference degrade sensors. Relay towers on cliffs restore clarity. Destroying relays creates blind pockets across the archipelago.

### Logistics & Resources
Fuel caches, island depots, and offshore platforms serve as resupply nodes. Convoys must navigate beach approaches and avoid ambush‑prone mangrove corridors.

### Deployment & Progress
A fixed deployment budget (~10,000 points) forces players to choose between naval dominance, air superiority, or distributed amphibious mobility.  
Blue deploys from the southwest offshore staging platform; Red deploys from the northeast cliffside command island.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² island‑chain zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over the real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations
- **Blue HQ (Offshore Staging Platform)** — (0,0), 150×150 m  
- **Red HQ (Cliffside Command Island)** — (1000,1000), 150×150 m  
- **Central Island Cluster** — (500,450), 350×300 m  
- **Island Depot & Village** — (250,800), 120×120 m  
- **Mangrove Swamp Network** — (800,300), 200×200 m  
- **Deep‑Water Channel** — (0,450 → 400,900)  
- **Cliffside Trail Route** — (300,0 → 450,350)  
- **Cliff Relay A** — (650,150), +240 m  
- **Cliff Relay B** — (150,650), +260 m  
- **Neutral Forward Outpost** — (500,950)

## Chokepoints & Sightlines

### Deep‑Water Channel
Major naval chokepoint; controlling it determines sea movement.

### Mangrove Swamp Network
Interior flanking routes; stealthy but slow.

### Central Island Cluster
Multi‑node combat zone; ideal for distributed warfare.

### Cliffside Trail
Predictable but safe; vulnerable to overwatch from cliffs.

### Sightlines
Cliff Relay A provides long‑range sensor coverage (~350 m). Sea spray and humidity create fog‑of‑war pockets.

## Deployment Zones
- **Blue Deployment:** SW offshore platform + naval staging  
- **Red Deployment:** NE cliffside command island + AA emplacements  
- **Neutral Forward Zone:** Outpost at (500,950) for drone drops

## Assets & Environment

### Terrain
Beaches, cliffs, coral shelves, mangroves, offshore platforms, island villages.

### Vegetation
Palm trees, mangroves, shrubs, beach grass.

### Buildings & Props
Military: AA guns, radar dishes, sonar arrays, bunkers  
Industrial: Docks, generators, pipelines  
Logistics: Ships, cargo crates, fuel drums, drones

### Effects
Sea spray, fog, humidity haze, wave impacts, thermal distortion.

### Audio
Waves, wind, ship engines, sonar pings, radio chatter.

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
Verify naval routing, amphibious landings, distributed node coordination.

#### Comms/Sensors
Test relay destruction, fog‑of‑war updates, sonar interference.

#### Performance
≥60 Hz AR render on target device.

#### QA Checklist
Navigation, amphibious logic, distributed objective behavior, anchor drift, sensor accuracy.
