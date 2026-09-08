# Procedural Desert Terrain

OPAW includes the Unity asset tree from [Arielv1/unity-procedural-desert-terrain](https://github.com/Arielv1/unity-procedural-desert-terrain) as a learning and prototyping reference for procedural desert terrain.

## Imported content

The imported assets are isolated under:

`Assets/ThirdParty/Procedural-Desert-Terrain/`

The reference includes:

- `Scripts/ProceduralTerrain.cs` for Perlin-noise mesh generation
- `Editor/TerrainEditor.cs` for Inspector controls
- Terrain and gizmo ScriptableObjects
- A sample prefab, material, and scene

The sample scene is retained for reference and is not added to OPAW's build scenes.

## Compatibility note

The source project targets Unity 2020.2.7f1 and uses built-in Unity APIs without an additional package dependency. OPAW uses a newer Unity/URP configuration, so the reference scripts and assets may require migration before production use. The imported files do not replace OPAW scenes, settings, materials, or terrain data.

## Attribution

Source repository: https://github.com/Arielv1/unity-procedural-desert-terrain
