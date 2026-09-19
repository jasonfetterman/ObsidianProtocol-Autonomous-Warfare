---
name: navigation-set-bake-settings
description: Tune NavMesh bake settings. Agent radius / height / max-slope / step-height belong to a NavMesh agent type (in project NavMesh settings) and apply to every surface baking for that type. The voxel size is a per-surface override; pass `gameObjectRef` (a NavMeshSurface) with `voxelSize` to set it.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Navigation / Set Bake Settings

Configure the parameters that control how a NavMesh is voxelized and baked.

Agent radius / height / max-slope / step-height are stored on the **agent type** (id `agentTypeId`, default 0 = Humanoid) in the project's NavMesh settings, so they affect all surfaces baking for that type. The **voxel size** is a per-`NavMeshSurface` override.

## Inputs

- `agentTypeId` — the NavMesh agent type to edit (default 0 = Humanoid).
- `agentRadius` — optional agent radius (meters).
- `agentHeight` — optional agent height (meters).
- `maxSlope` — optional maximum walkable slope (degrees, 0–60).
- `stepHeight` — optional maximum step / climb height (meters).
- `gameObjectRef` — optional NavMeshSurface GameObject whose voxel-size override to set.
- `voxelSize` — optional voxel size for that surface (only applied when `gameObjectRef` is provided; sets `overrideVoxelSize = true`).

## Behavior

Edits the agent type's bake settings via the `NavMeshProjectSettings` serialized object (only the provided values change), and optionally enables and sets the surface voxel-size override. Marks dirty and repaints. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool navigation-set-bake-settings --input '{
  "agentTypeId": 0,
  "agentRadius": "string_value",
  "agentHeight": "string_value",
  "maxSlope": "string_value",
  "stepHeight": "string_value",
  "gameObjectRef": "string_value",
  "voxelSize": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool navigation-set-bake-settings --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool navigation-set-bake-settings --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `agentTypeId` | `integer` | No | NavMesh agent-type id to edit (0 = Humanoid by default). |
| `agentRadius` | `any` | No | Agent radius in meters. |
| `agentHeight` | `any` | No | Agent height in meters. |
| `maxSlope` | `any` | No | Maximum walkable slope in degrees (0-60). |
| `stepHeight` | `any` | No | Maximum step / climb height in meters. |
| `gameObjectRef` | `any` | No | Optional NavMeshSurface GameObject whose voxel-size override to set. |
| `voxelSize` | `any` | No | Voxel size for the surface (applied only when gameObjectRef is provided). |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "agentTypeId": {
      "type": "integer"
    },
    "agentRadius": {
      "$ref": "#/$defs/System.Single"
    },
    "agentHeight": {
      "$ref": "#/$defs/System.Single"
    },
    "maxSlope": {
      "$ref": "#/$defs/System.Single"
    },
    "stepHeight": {
      "$ref": "#/$defs/System.Single"
    },
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "voxelSize": {
      "$ref": "#/$defs/System.Single"
    }
  },
  "$defs": {
    "System.Single": {
      "type": "number"
    },
    "UnityEngine.EntityId": {
      "type": "string",
      "pattern": "^[0-9]+$"
    },
    "System.Type": {
      "type": "string"
    },
    "AIGD.GameObjectRef": {
      "type": "object",
      "properties": {
        "instanceID": {
          "$ref": "#/$defs/UnityEngine.EntityId",
          "description": "instanceID of the UnityEngine.Object. If it is '0' and 'path', 'name', 'assetPath' and 'assetGuid' is not provided, empty or null, then it will be used as 'null'. Priority: 1 (Recommended)"
        },
        "path": {
          "type": "string",
          "description": "Path of a GameObject in the hierarchy Sample 'character/hand/finger/particle'. Priority: 2."
        },
        "name": {
          "type": "string",
          "description": "Name of a GameObject in hierarchy. Priority: 3."
        },
        "assetType": {
          "$ref": "#/$defs/System.Type",
          "description": "Type of the asset."
        },
        "assetPath": {
          "type": "string",
          "description": "Path to the asset within the project. Starts with 'Assets/'"
        },
        "assetGuid": {
          "type": "string",
          "description": "Unique identifier for the asset."
        }
      },
      "required": [
        "instanceID"
      ],
      "description": "Find GameObject in opened Prefab or in the active Scene."
    }
  }
}
```

## Output

### Output JSON Schema

```json
{
  "type": "object",
  "properties": {
    "result": {
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Navigation-SetBakeSettingsResponse"
    }
  },
  "$defs": {
    "AIGD.ComponentRef": {
      "type": "object",
      "properties": {
        "index": {
          "type": "integer",
          "description": "Component 'index' attached to a gameObject. The first index is '0' and that is usually Transform or RectTransform. Priority: 2. Default value is -1."
        },
        "typeName": {
          "type": "string",
          "description": "Component type full name. Sample 'UnityEngine.Transform'. If the gameObject has two components of the same type, the output component is unpredictable. Priority: 3. Default value is null."
        },
        "instanceID": {
          "$ref": "#/$defs/UnityEngine.EntityId",
          "description": "instanceID of the UnityEngine.Object. If this is '0', then it will be used as 'null'."
        }
      },
      "required": [
        "index",
        "instanceID"
      ],
      "description": "Component reference. Used to find a Component at GameObject."
    },
    "UnityEngine.EntityId": {
      "type": "string",
      "pattern": "^[0-9]+$"
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Navigation-SetBakeSettingsResponse": {
      "type": "object",
      "properties": {
        "agentTypeId": {
          "type": "integer",
          "description": "The edited agent-type id."
        },
        "agentSettingsApplied": {
          "type": "boolean",
          "description": "Whether agent-type bake settings were applied."
        },
        "agentRadius": {
          "type": "number",
          "description": "Resolved agent radius after the edit."
        },
        "agentHeight": {
          "type": "number",
          "description": "Resolved agent height after the edit."
        },
        "maxSlope": {
          "type": "number",
          "description": "Resolved maximum slope after the edit."
        },
        "stepHeight": {
          "type": "number",
          "description": "Resolved step / climb height after the edit."
        },
        "surfaceRef": {
          "$ref": "#/$defs/AIGD.ComponentRef",
          "description": "Reference to the surface whose voxel size was set, if any."
        },
        "voxelSizeApplied": {
          "type": "boolean",
          "description": "Whether the surface voxel size override was applied."
        },
        "voxelSize": {
          "type": "number",
          "description": "Resolved voxel size for the surface, if set."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "agentTypeId",
        "agentSettingsApplied",
        "agentRadius",
        "agentHeight",
        "maxSlope",
        "stepHeight",
        "voxelSizeApplied",
        "voxelSize",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

