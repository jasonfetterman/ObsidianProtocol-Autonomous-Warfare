# RTS learning references

OPAW includes focused learning subsets from:

- [AtahanEkici/Shattered-World](https://github.com/AtahanEkici/Shattered-World)
- [elichen/MiniRTS](https://github.com/elichen/MiniRTS)
- [RichyPier/Dystopia](https://github.com/RichyPier/Dystopia)

## Imported content

The references are isolated under:

`Assets/ThirdParty/RTS-Learning/`

### Shattered-World

- Building placement and object-drag systems
- Camera movement, selection rectangles, unit selection, and global selection state
- Inventory and item ScriptableObjects
- Unit controller examples
- Focused camera, inventory, tilemap-building, and unit test scenes
- Minimal test material, prefab, and scene support assets

### MiniRTS

- A complete compact RTS architecture for study
- A* and walk-grid pathfinding
- Selection, control groups, camera, minimap, and fog of war
- Economy, resource nodes, worker gathering, production queues, factories, refineries, and headquarters
- Combat targeting, combat math, unit movement, building placement, supply depots, and game outcomes
- Enemy AI build orders, attack waves, scaling, and win/lose logic
- EditMode tests covering the major systems
- Main scene, input actions, and small terrain/resource textures

### Dystopia

- Lightweight RTS camera and rotation examples
- Simple unit-controller example
- Small scene references for camera and unit experiments

## Excluded content

The import excludes generated Unity project folders, vendor/package caches, unrelated characters and story systems, large game-specific worlds, and nonessential art/audio. The source package manifests are not copied and OPAW package configuration was not changed.

## Compatibility note

The references span older Unity projects and a newer Unity 6 MiniRTS project. They are isolated learning material, not drop-in production code. MiniRTS is the closest architectural reference for OPAW's modern RTS systems; Shattered-World and Dystopia are narrower examples for selection, building, camera, inventory, and unit control.

## Attribution

Source repositories:

- https://github.com/AtahanEkici/Shattered-World
- https://github.com/elichen/MiniRTS
- https://github.com/RichyPier/Dystopia
