# Black Mesa

Black Mesa (Mountain/Industrial) Map Design
Executive Summary: Black Mesa is a sprawling mountainous-industrial battlefield designed for AR command. The map emphasizes combined-arms warfare (ground vehicles, infantry, and air support) with dramatic elevation differences and industrial facilities. It incorporates Obsidian Protocolâ€™s key systems: autonomous decision-making, sensor-driven tactics, communications networks, and logistical nodes. In AR (Nreal Light/Air), the map will be anchored at ~4â€“5â€¯m viewing distance, covering roughly a 10â€¯Ã—â€¯10â€¯m physical area (e.g. a conference table) to represent ~1â€¯kmÂ² of terrain. Key features include high ridges for aerial and sensor advantage, narrow mountain passes for chokepoints, and an industrial valley with supply depots and objectives. We propose detailed layout coordinates, asset budgets, and an implementation/test plan for Unreal Engine with Nreal support.

## Design Goals & Gameplay

Combined Arms & Scale: Black Mesa supports infantry, armor, and air simultaneously. Wide open valley floors enable ground maneuver, while towering peaks and plateaus allow air/Artillery vantage. This enforces Obsidian Protocolâ€™s combined-arms fantasy.
Autonomy & Flanking: Multiple routes (mountain passes, road tunnels, rivers) create non-linear paths. Autonomous AI can choose flanks or ambushes, fulfilling the intent-driven design where units adapt tactics (e.g. retreat under fire or flank) without micromanagement.
Sensor & Information Warfare: High peaks host sensors and relays; deep valleys create occlusions and radar dead zones. Units must scout and share intel. Communication relays near ridges can be destroyed, degrading network (information warfare). This enforces Fog-of-War/uncertainty themes.
Logistics & Resources: An industrial plant and mine in the valley serve as resource/logistics nodes. Players may secure fuel depots or fabrication facilities to resupply units. Distances matter: long supply lines and resupply times exemplify logistics.
Deployment & Progress: The fixed deployment budget (~10,000 points) forces strategic choices: send heavy armor up the passes or mobile recon around the flanks? Deployment zones are at far corners or on heights, making positioning decisions meaningful.
## Spatial Constraints (Nreal AR)

Device Capabilities: Nreal Light/Air have ~53Â° diagonal FOV and 1920Ã—1080 per-eye resolution. The display covers ~70% of the lens width and 85% height. In practice, content should be placed a few meters away so the user can see it comfortably.
Playable Area & Scaling: We assume an anchored AR map projected ~4â€¯m in front of the user. At this distance, a 10â€¯Ã—â€¯10â€¯m physical area can represent roughly a 1â€¯kmÂ² battlefield (1:100 scale). This fits on a conference table. (If larger maps are needed, multi-anchor streaming or marker tracking can extend coverage.)
Tracking & Occlusion: Nrealâ€™s inside-out tracking can detect horizontal planes (table/floor), but earlier versions lack true depth occlusion. The SDKâ€™s depth-mesh (if using the latest XREAL SDK) can create an environment mesh for occlusion and collision, but we assume minimal occlusion otherwise. All virtual objects render on top of real-world view by default. Hence, design the map to work as an overlay on empty floor/ground.
## Layout Overview

Axes & Coordinates: We define a 1000Ã—1000 unit grid (1 unit â‰ˆ 1â€¯m). The origin (0,0) is the southwest corner. Key points (with sizes):

West Base (Blue HQ): at (0,0), 100Ã—100â€¯m. High plateau north of this base overlooks the valley.
East Base (Red HQ): at (1000,1000), 100Ã—100â€¯m. Situated on a mountain spur with a radio tower.
Industrial Complex: centered at (500,300), occupies ~150Ã—150â€¯m. Includes a factory, storage tanks, and warehouses.
Power Plant / Refinery: at (800,200) by a river, 80Ã—80â€¯m. Heavy pipes and cooling towers.
Supply Depot: at (200,800), 60Ã—60â€¯m, with crates and fuel tanks.
Mountain Passes: â€œNorth Passâ€ linking (400,1000) to (600,600); â€œSouth Tunnelâ€ at (300,0) to (400,400) through a ridge.
Rivers/Bridges: A river runs from (0,500) to (400,900). Two bridges at (150,600) and (350,500).
Terrain: Two mountain peaks: Peak A (600,100) at +300â€¯m above valley; Peak B (100,600) at +250â€¯m. Valley floor baseline = 0.
Chokepoints & Sightlines:

The North Pass (400,1000 â†’ 600,600) is a narrow defile between cliffs (line-of-sight blocked except from air).
The South Tunnel (entrance at (300,0), exit (400,400)) is a road cut in the cliff: close-range combat.
Bridges: control of the two river crossings is critical (inhibits flanking).
Sightlines: From Peak A, units can see northwards across the valley (sensor range ~300â€¯m). From the East Base tower, long-range radar covers most of the map. Heavy fog of war in deep valleys.
Deployment Zones: Blue forces start around West Base; Red around East Base. (Exact spawn points at map edges facing inward). Both have fallback rally points at rear. A neutral forward deployment zone (airfield at (500,950)) allows limited forward resupply (drone drops).

sql

Copy

stateDiagram-v2

[*] --> Deployment : Player deploys forces in start zones

Deployment --> Recon : Initial scouting of terrain/objectives

Recon --> Engage : Forces clash at chokepoints and objectives

Engage --> Hold : Secure captured ground and key targets

Hold --> [*] : Battle concludes (Victory/Defeat)

## Assets & Environment


Terrain: Procedurally sculpted mountains, cliffs, and riverbed. Terrain material: rocky cliffs, dirt, grass textures.

Vegetation: Sparse conifer trees on slopes (LOD models, ~2k tris each, ~100 instances), shrubs and grass patches (billboard billboards).

Buildings/Props:

Industrial: Factory shells, oil tanks, pipelines.

Military: Radar dish, antennas, watchtowers.

Logistics: Supply crates, barrels, trucks (static scatter).

Bridges/Roads: Metal truss bridges at river crossings, asphalt roads with potholes.

Reinforcements: At spawn zones â€“ vehicles/transport.

Effect: Smokestacks emit steam (VFX sprites), furnaces glow. Dynamic lights in factories (caution lights).

Audio: Ambient wind in mountains, distant rumble, machinery hum at factory, radio chatter, footsteps on gravel, bridge creak. Subtly dynamic (stronger wind on peaks).

LOD & Budget: Aim for mobile-level assets. Use 3-LOD hierarchy. For example, large cliff meshes (~200k tris LOD0, 100k LOD1, 40k LOD2). Buildings ~50â€“80k tris LOD0 down to 5k. Vehicles ~20k. Keep most meshes â‰¤50k tris after LOD0. Modern phones can handle high-poly objects individually, but the entire scene should target <3â€“4M total tris (AR rendering is per-eye).

Materials: Mostly opaque Lit materials. Use simpler (Flat/Unlit) shaders for UI overlays. Minimal transparency (translucent smoke is expensive â€“ use particle billboards).

Navigation: Mark traversable surfaces (roads/paths) and obstacles (cliffs, rivers). Bake a NavMesh for AI (roughly following valley floor and passes). Bridges and tunnels have their own nav links.

Asset CategoCroyunt Tri/Item Total Tris

Mountains/Cliffs (me3she20s)0k     600k

Slopes/Hills      4 80k            320k

Buildings (industrial1,5etc5.0) k  750k

Bridges/Roads     4 30k            120k

Vegetation (trees/1s5h0rub2sk)     300k

Props (vehicles, crat5e0s) 10k     500k

Total (approx.)                    2.59M

Texture Budget: Use 2048Â² textures for large ground/buildings; 1024Â² for props. Use DXT1/5 compression (4â€“8â€¯bpp). Limit to ~100â€¯MB total.

Unreal/Nreal Integration
Engine & Plugins: Use Unrealâ€™s ARTemplate (ARKit for iOS or ARCore for Android via Nreal). Nreal SDK (XREAL) can integrate via the ARCore XR plugin on Android devices. Enable ARKit/ARCore plugins for plane detection and anchors. Use the Nreal depth-mesh feature (if available) for occlusion.
Rendering Settings: Forward renderer with optimizations (msaa off, modest shadow distance). Limit dynamic lights (preferring stationary). Enable GPU-friendly lightmaps.
Spatial Anchors: Place the map as a single large anchored object. Use Nrealâ€™s Spatial Anchor or ARAnchor to lock it to real-world (so it stays in place across sessions).
Interaction: Touch controller (phone as pointer) to pan/zoom the map. HUD overlays should be 2D widgets (screen space) since AR glasses provide limited UI.
Multiplayer: Use Unrealâ€™s networking (client-server). Since AR devices have limited compute, offload physics/AI to server or allocate to more capable hardware if possible.
## Implementation Plan & Testing

Milestone Timeframe Deliverables
1. PrototypeMLaoynothut1 Blocked-out terrain (mountains, valleys, rivers). Initial placement of bases and objectives. AR anchor test with one unit moving.
2. Core SysteMmosnth 2 Deploy/Navigation: AI navmesh, unit pathfinding. Basic autonomous behaviors (move/attack/retreat). Sensor raycasts implemented. Deployment UI placeholder.
3. AR IntegraMtioonnth 3 Nreal AR build: map anchors, plane detection, camera feed. Verify stable tracking and anchoring at target distance. Add simple interactivity (select unit, set intent via gaze/tap).
4. Content FMillonth 4-5 Populate terrain with final models/textures (buildings, props, vegetation). LODs and collisions set. Lighting and audio ambiance. Fallback plan for occlusion: place simple colliders for important real objects.
5. GameplayM&oPnothlis6h-7 Link campaign objectives. Balance deployment costs for units on map. Add VFX (smoke, fire). VR (if any) integration verification. Profile performance, optimize where needed.
6. Testing &MQAonth 8 Multiplayer stress test (50â€“100 units). AI/autonomy test (units handle comms loss, adapt). Sensor tests (LOS, sensor fusion). Communication drop tests (simulate relay loss). Check AR stability (anchors, drift). Fix issues.

Testing Plan:

Autonomy: Use logs/visualization to ensure AI obeys intent (e.g. â€œAttackâ€, â€œHoldâ€). Test corner cases (lost comms, no targets).
Comms/Sensors: Place units and detect others at various line-of-sight angles. Verify fog-of-war updates. Disable a relay node to test network fragmentation.
Multiplayer: Simulate high-latency packet loss. Ensure authoritative physics and prevent unit desync.
Performance: Aim for â‰¥60â€¯Hz AR render. Profile on target phone (e.g. high-end Android). Optimize by culling distant LODs aggressively.
QA Checklist: Unit navigation complete? All deployment zones reachable? No shader or light leaks (framerate dips)? AR anchor reposition stable? Units correctly use cover and flanking? Sensor ping intervals and identification errors within spec? Deployment budget strictly enforced? Network cheat tests?

Visual Aids
(See conceptual illustrations below)

Figure: Concept art sketch of Black Mesaâ€™s industrial valley and mountain passes. Forces engage in combined-arms combat across challenging elevation.

Figure: Simplified top-down schematic (coordinates in meters). Blue (SW) and Red (NE) deployment zones, chokepoints (green), objectives (stars), and terrain heights.

Set up forces at bases

Scout passes and objectives

Combat at passes/valley center

Secure objectives & high ground

Battle concludes (result)

Deployment

Recon

Engage

Hold

Show code
Sources: Nreal (XREAL) SDK and hardware specs, Unreal performance guidelines, mobile AR asset advice, and AR FOV benchmarks informed design decisions.

