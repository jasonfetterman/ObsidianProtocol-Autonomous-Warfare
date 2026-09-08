# MiniRTS

MiniRTS is a compact, runtime-built real-time strategy game for Unity 6000.3. The player gathers minerals and gas, expands production, and defeats an autonomous enemy base.

## Run

1. Open the project with Unity `6000.3.20f1`.
2. Open `Assets/Scenes/Main.unity`.
3. Press **Play**.

## Controls

| Action | Control |
| --- | --- |
| Select unit/building | Left-click |
| Box-select units | Left-click and drag |
| Add/remove units from selection | Shift + click or Shift + drag |
| Move / gather / attack target / set rally point | Right-click the destination or target |
| Attack-move | **A**, then left-click the ground |
| Stop and hold | **H** |
| Assign control group | **Ctrl + 1–5** |
| Recall control group | **1–5** |
| Build with a selected worker | **D** Supply Depot, **B** Barracks, **F** Factory, **R** Refinery |
| Train from a selected building | **S** Worker, **M** Marine, **T** Tank |
| Pan camera | **WASD**, arrow keys, or screen edge |
| Zoom camera | Mouse wheel |
| Reposition camera from minimap | Left-click or drag on the minimap |

Right-click cancels building placement or pending attack-move targeting.

## Architecture

`GameBootstrap` constructs the map, factions, resources, camera, UI, and runtime materials from one authored scene object. `WalkGrid`, `AStar`, and `UnitMover` handle deterministic grid navigation; unit, building, economy, production, gathering, and combat components own gameplay state. `EnemyAIController` runs the opponent, while `FogOfWar`, `MinimapUI`, `SelectionController`, and the runtime HUD provide the player-facing layer. Balance and presentation constants live in `BalanceConfig`, with unit/building data exposed through their definition types.

## Tests

Open **Window → General → Test Runner**, select **EditMode**, and choose **Run All**. The suite contains 52 EditMode tests.

With the Unity Editor closed, the same suite can be run from a shell:

```sh
<UNITY_EXECUTABLE> -batchmode -nographics -projectPath "$(pwd)" \
  -runTests -testPlatform EditMode \
  -testResults /tmp/minirts-editmode-results.xml -quit
```
