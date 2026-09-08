# Downloaded RTS asset and code extraction

The user authorized use of the downloaded projects below. The extraction is intentionally
focused on material that can help build OPAW without copying trademarked game content,
bundled commercial plugins, or unrelated third-party packs.

## WarKingdoms

Source: [skyteks/WarKingdoms](https://github.com/skyteks/WarKingdoms)

Imported under `Assets/ThirdParty/WarKingdoms-Learning/`:

- RTS runtime scripts for buildings, units, commands, resources, movement, selection,
  fog of war, health, factions, camera management, minimap, and UI
- Generic technical shaders for fog-of-war, projection, tinting, circles, and always-on-top
  tactical overlays
- Supporting tactical materials for fog, minimap, movement cursor, and selection cursor

The repository README identifies several purchased Asset Store packs. Those models,
textures, audio, effects, and third-party packs were not copied. OPAW should replace
them with its own assets or separately licensed equivalents.

## Starcraft Unity3D

Source: [coconauts/startcraft-unity3d](https://github.com/coconauts/startcraft-unity3d)

Imported under `Tools/ThirdParty/Startcraft-Unity3D-Learning/`:

- GPL source-code examples for resources, research, production queues, workers, buildings,
  units, AI rush behavior, camera control, and UI flow
- Original GPL notice and README

StarCraft-derived models, animations, audio, maps, sprites, and other game content were
not copied. The source is reference material only and is not wired into OPAW production.

## Honnisha Unity RTS

Source: [honnisha/unity-rts](https://gitlab.com/honnisha/unity-rts)

Imported under `Tools/ThirdParty/Honnisha-Unity-RTS-Learning/`:

- GPL source-code examples for RTS camera, unit/building behavior, selection, fog of war,
  terrain generation, menus, save/load, map scripting, and outline effects
- Original GPL notice and README

Photon, PowerUI, bundled models, effects, sounds, and other packages were not copied.
The source is reference material only and is not wired into OPAW production.

## Production guidance

The WarKingdoms extraction is the practical Unity asset reference in this batch. Its
systems should be adapted to OPAW-native autonomous command, logistics, sensor confidence,
networking, and URP architecture rather than attached directly to the imported sample
content. The two GPL code references should remain isolated unless the legal distribution
model for derivative code is intentionally addressed.
