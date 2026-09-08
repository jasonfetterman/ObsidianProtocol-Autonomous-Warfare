# AmyTheHedgehog learning references

OPAW includes a focused learning subset from [AlonWoof/AmyTheHedgehog](https://github.com/AlonWoof/AmyTheHedgehog), with permission, for studying water, terrain, environment, and gameplay-support techniques.

## Imported content

The reusable reference files are isolated under:

`Assets/ThirdParty/AmyTheHedgehog-Learning/`

Included:

- Legacy Unity water systems, Gerstner displacement, planar reflections, water materials, and water shaders
- Beach scene references and water FX prefabs
- Water volumes, swimming, wet-surface effects, underwater post-processing, and water-camera scripts
- Terrain assets, terrain layers, terrain materials, terrain shaders, terrain properties, and terrain export tooling
- Garden, Mushroom Valley, and Sunset Hill scene references for environment composition
- Terrain and water utility scripts such as animated UVs and sine-wave motion

Excluded:

- The Amy character/gameplay project and unrelated game scripts
- Vendor/editor packages such as DynamicBone, Amplify Shader Editor, InfiniGRASS, TerrainComposer, and other bundled dependencies
- Unrelated character, NPC, story, vehicle, UI, and audio content

## Compatibility note

The source project targets Unity 2020.3.43f1 and legacy built-in-render-pipeline APIs. OPAW uses a newer Unity/URP configuration, so these files are learning and migration references rather than drop-in production systems. The source package manifest is not copied into OPAW, and no package changes were made.

## Attribution

Source repository: https://github.com/AlonWoof/AmyTheHedgehog
