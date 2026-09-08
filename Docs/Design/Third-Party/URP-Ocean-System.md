# URP Ocean System

OPAW includes the Unity asset tree from [DanielAskerov/URP-Ocean-System](https://github.com/DanielAskerov/URP-Ocean-System) as a third-party ocean-system reference and integration starting point.

## Imported content

The imported assets are isolated under:

`Assets/ThirdParty/URP-Ocean-System/`

This preserves the source system's scripts, compute shaders, ocean materials, meshes, prefabs, sample scene, settings, and supporting textures without overwriting OPAW's existing project settings or scenes.

## Compatibility note

The source project targets Unity 2021.3.6f1 and URP 12.1.7. OPAW uses a newer Unity/URP configuration, so the imported sample scene and settings are retained for reference and migration. The ocean scripts depend on Unity URP, which is already included by OPAW; no additional package dependency was identified during import.

## Attribution

Source repository: https://github.com/DanielAskerov/URP-Ocean-System
