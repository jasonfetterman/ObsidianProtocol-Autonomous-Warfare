# MiniRTS — a StarCraft-style RTS in Unity

Single-player skirmish RTS in the spirit of StarCraft (original placeholder art only:
Unity primitives + colored materials; no Blizzard assets or names).

## Tech constraints (hard requirements)
- Unity 6000.3.20f1, Universal 3D (URP) template, macOS arm64.
- **Everything is generated from code.** No hand-authored scenes/prefabs beyond a single
  bootstrap scene `Assets/Scenes/Main.unity` containing one GameObject with `GameBootstrap`.
  All units, buildings, map, UI are constructed at runtime in C# (UGUI built from code, or
  IMGUI/OnGUI is acceptable for HUD).
- All code under `Assets/Scripts/MiniRTS/`, namespace `MiniRTS`, assembly definition ok.
- Must compile with zero errors and zero warnings-as-errors; keep it deterministic
  (fixed random seed for AI).
- Include EditMode tests under `Assets/Tests/EditMode` (assembly def) covering pure-logic
  systems (pathfinding grid, resource economy, combat damage math, build costs).
- An editor menu item `MiniRTS/Regenerate Main Scene` (static method, also callable from
  batch mode) creates/updates the bootstrap scene and adds it to Build Settings.

## Gameplay (v1 scope)
- Top-down camera (60° tilt), WASD/edge scroll pan, scroll-wheel zoom.
- Map: 96x96 unit flat terrain (a plane), with impassable rock clusters (cubes), two
  start locations, 2 mineral fields (8 crystal nodes each) + 1 gas geyser per start.
- Player faction (blue) vs scripted AI faction (red).
- Resources: Minerals + Gas. Workers gather (move to node, 2s harvest, carry 5, return
  to HQ). Supply cap raised by Depots (like supply depots), max 60.
- Units (all primitives):
  - Worker (capsule, small): gathers, builds buildings. 50 minerals.
  - Marine (capsule): ranged attack. 50 minerals, needs Barracks.
  - Tank (cube, larger): slow, heavy ranged. 150 minerals 50 gas, needs Factory.
- Buildings (colored boxes, placed by worker with ghost preview + grid snap):
  - HQ (drop-off, trains workers, 400m), Depot (+8 supply, 100m),
    Barracks (trains marines, 150m), Factory (trains tanks, 200m 50g),
    Refinery (on geyser, enables gas, 75m).
- Combat: attack-move, auto-acquire targets in range, HP bars (billboard quads or UI),
  death = destroy + small particle burst. Simple balance table in one ScriptableObject-free
  static config class.
- Controls (StarCraft conventions): left-drag box select, left-click select, right-click
  context command (move/gather/attack), A+click attack-move, control groups 1-5
  (ctrl+num set, num recall), building hotkeys shown on a bottom command card UI.
- UI: top bar (minerals, gas, supply), bottom-left minimap (RenderTexture from overhead
  ortho cam, click to jump), bottom-center selection panel, bottom-right command card
  with clickable buttons + hotkeys. Production queues with progress bars.
- Fog of war: hard requirement lite version — 2D grid visibility (radius per unit),
  black undiscovered / grey discovered overlay via a projector-style quad texture updated
  ~5 Hz. Enemy units hidden unless visible.
- AI opponent: scripted build order (workers → depot → barracks → steady marines),
  waves attack player base every ~90s scaled up over time. Loses when all buildings dead.
- Win/Lose screen (destroy all enemy buildings / lose all yours), restart button.
- Pathfinding: A* on a 96x96 walkability grid + simple local avoidance (steer around
  neighbors); units clamp to walkable cells. No navmesh dependency.

## Milestones (each must leave the project compiling + tests green)
1. Bootstrap + map gen + camera + selection + A* movement.
2. Economy: minerals/gas, workers, HQ, depot, supply, top bar UI.
3. Production: barracks/factory/refinery, command card UI, building placement.
4. Combat: marines/tanks, attack-move, HP, death, control groups.
5. Fog of war + minimap.
6. AI opponent + win/lose + polish pass.

## Verification loop (orchestrator runs these; code must keep them green)
- `unity test <project> --edit-mode` — EditMode tests.
- `unity build` or batch `-quit -batchmode -executeMethod` compile check.
- `unity command eval "..."` against a running editor for live smoke checks
  (e.g. enter play mode, query unit counts, simulate a gather cycle).
