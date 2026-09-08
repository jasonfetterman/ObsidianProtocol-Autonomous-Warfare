# Additional RTS learning references

Focused, reusable systems from the following MIT-licensed repositories are included under
`Assets/ThirdParty/RTS-Learning/`:

## object_placement_unity

Source: [manlaig/object_placement_unity](https://github.com/manlaig/object_placement_unity)

Included:

- Dynamic grid-sized building placement
- Collider-based placement validation
- Build progress and custom build-time ScriptableObjects
- Production-tile and placement helper examples
- A small isolated example scene and required support assets

The original MIT license is preserved as `LICENSE.txt`.

## Unity-RTS-Selection

Source: [lexonegit/Unity-RTS-Selection](https://github.com/lexonegit/Unity-RTS-Selection)

Included:

- Raycast-based single and multi-selection
- Add/remove selection modifier handling
- Selectable interface and unit example
- Perspective/orthographic RTS camera example
- Small sample scene and only its required materials

The original MIT license is preserved as `LICENSE.txt`.

## Deliberately not imported

- `coconauts/startcraft-unity3d`: GPL-3.0 and contains StarCraft-derived content and bundled dependencies. It is not suitable for direct inclusion in OPAW's commercial codebase.
- `honnisha/unity-rts`: GPL-licensed and bundles Photon, PowerUI, models, effects, and other dependencies with separate provenance concerns.
- `skyteks/WarKingdoms`: no clear repository license was found during inspection, so its code and assets were not copied.

These repositories may still be consulted as external design references, but their files are not part of OPAW.

## Compatibility

The imported systems are isolated learning references, not wired into production scenes or active build settings. They span older Unity versions and should be adapted to OPAW's current URP, input, navigation, networking, and autonomous-command architecture before production use.
