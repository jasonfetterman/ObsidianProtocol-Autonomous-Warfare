# Dead Air

**Remote Airbase / Air Combat & Anti‑Air Warfare**

## Executive Summary

Dead Air is a remote high‑altitude airbase battlefield built for air‑dominant warfare, long‑range missile duels, autonomous air‑defense coordination, and precision strike operations. The terrain features hardened runways, radar farms, SAM batteries, control towers, fuel depots, aircraft shelters, and exposed desert or tundra flats depending on season.

The map emphasizes Obsidian Protocol’s autonomous air‑combat logic, sensor‑fusion under jamming and radar clutter, comms‑relay warfare, and logistics under extreme open‑air conditions. In AR (Nreal Light/Air), the map anchors at ~4–5 m viewing distance, covering a 10×10 m physical area representing ~1 km² of remote airbase terrain.

Key features include missile‑engagement corridors, radar‑blind pockets, runway‑strike objectives, autonomous interception behavior, and layered anti‑air defense networks.

## Design Goals & Gameplay

### Air Superiority & Missile Warfare
Dead Air prioritizes long‑range missile duels, radar‑guided engagements, and autonomous interception. Air superiority determines control of the entire battlespace.

### Anti‑Air Defense Networks
SAM batteries, CIWS emplacements, and EW towers form layered defense rings. Destroying or capturing these nodes shifts the tactical landscape.

### Autonomous Air‑Combat Logic
Autonomous aircraft choose routes based on radar clarity, jamming pockets, missile arcs, and threat vectors. Units adapt to sensor disruption and thermal gradients.

### Runway Denial & Strike Operations
Runways, hangars, and fuel depots serve as high‑value targets. Disabling airbase infrastructure cripples deployment capability.

## Sensor & Information Warfare

Radar clutter, thermal inversion layers, jamming fields, and EW interference degrade sensors. Relay towers restore clarity. Destroying relays creates massive radar‑blind zones across the airbase.

## Logistics & Resources

Fuel depots, maintenance hangars, and runway‑adjacent supply pads serve as resupply nodes. Convoys must traverse exposed perimeter roads and avoid air‑to‑ground strike lanes.

## Deployment & Progress

A fixed deployment budget (~10,000 points) forces players to choose between air dominance, missile supremacy, or balanced AA networks.  
Blue deploys from the southwest airbase command; Red deploys from the northeast missile‑ridge control.

## Spatial Constraints (Nreal AR)

### Device Capabilities
Nreal Light/Air: ~53° diagonal FOV, 1920×1080 per‑eye resolution. Optimal viewing distance: ~4–5 m.

### Playable Area & Scaling
10×10 m physical area ≈ 1 km² airbase zone (1:100 scale). Designed for tabletop AR.

### Tracking & Occlusion
Inside‑out tracking detects horizontal planes. Depth mesh (if available) provides limited occlusion. Virtual objects render over real‑world view; assume minimal real‑world occlusion.

## Layout Overview

### Coordinate System
1000×1000 grid (1 unit ≈ 1 m). Origin (0,0) = southwest corner.

### Key Locations

Blue HQ (Airbase Command) — (0,0), 150×150 m  
Runway access, hangars, AA batteries, drone pads.

Red HQ (Missile Control Ridge) — (1000,1000), 150×150 m  
Elevated ridge with radar mast, EW towers, long‑range missile launchers.

Central Runway Complex — (500,450), 300×300 m  
Primary air operations zone; strike target for runway denial missions.

Radar Farm Cluster — (250,800), 120×120 m  
Radar dishes, EW nodes, sensor towers.

Hangar Row — (800,300), 150×150 m  
Aircraft shelters, maintenance bays, interior flanking routes.

Missile Engagement Corridor — (0,450 → 400,900)  
Long‑range missile lane; interception routes and EW traps.

Perimeter Access Road — (300,0 → 450,350)  
Safer but predictable; vulnerable to air‑to‑ground strikes.

Relay Tower A — (650,150), +240 m  
Long‑range radar vantage point.

Relay Tower B — (150,650), +260 m  
Comms relay hub and EW‑control node.

Neutral Forward Outpost — (500,950)  
Drone resupply zone.

## Chokepoints & Sightlines

Missile Engagement Corridor  
Primary long‑range strike lane; controlling it determines air superiority.

Radar Farm Cluster  
Critical sensor hub; losing it blinds entire sectors.

Runway Complex  
High‑value target; runway denial cripples air operations.

Hangar Row  
Interior flanking routes; ideal for stealth drones and close‑range AA ambushes.

### Sightlines
Relay Tower A provides long‑range radar coverage (~400 m).  
Thermal gradients and EW interference create fog‑of‑war pockets.

## Deployment Zones

Blue Deployment: SW airbase command + runway staging.  
Red Deployment: NE missile ridge + EW defenses.  
Neutral Forward Zone: Outpost at (500,950) for drone drops.

## Assets & Environment

### Terrain
Runways, taxi lanes, hangars, radar towers, missile pads, perimeter flats.

### Vegetation
Sparse scrub or tundra patches depending on climate.

### Buildings & Props
Military: Radar dishes, AA guns, missile launchers, bunkers  
Industrial: Fuel tanks, generators, pipelines  
Logistics: Trucks, crates, fuel drums, drones

### Effects
Heat shimmer, dust plumes, smoke trails, radar waves, static bursts.

### Audio
Jet engines, wind, radar hum, radio chatter, missile launches.

## LOD & Budget

Target total: ~2.4–2.6M tris

Runway/Terrain Meshes — ~600k  
Radar/EW Structures — ~320k  
Hangars/Buildings — ~750k  
Roads/Paths — ~120k  
Props — ~500k  

### Texture Budget
2048² for terrain/buildings  
1024² for props  
DXT1/5 compression  
Target ≤100 MB

## Unreal / Nreal Integration

### Engine & Plugins
Unreal ARTemplate + Nreal SDK (XREAL).  
ARKit/ARCore enabled.

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

1. **Prototype Layout — Month 1**  
   Block out runway complex, radar farm, hangars. AR anchor test.

2. **Core Systems — Month 2**  
   NavMesh (air + ground), missile autonomy behaviors, radar logic.

3. **AR Integration — Month 3**  
   Anchors, plane detection, basic interactivity.

4. **Content Fill — Months 4–5**  
   Final models, textures, lighting, audio.

5. **Gameplay & Polish — Months 6–7**  
   Objectives, balance, VFX (heat shimmer, radar waves).

6. **Testing & QA — Month 8**  
   Multiplayer stress test, sensor validation.

## Testing Plan

### Autonomy
Verify air‑combat coordination, missile‑aware routing, hazard avoidance.

### Comms/Sensors
Test relay destruction, fog‑of‑war updates, LOS checks under EW interference.

### Performance
≥60 Hz AR render on target device.

### QA Checklist
Navigation, air‑combat logic, deployment budget enforcement, anchor drift, sensor accuracy.
