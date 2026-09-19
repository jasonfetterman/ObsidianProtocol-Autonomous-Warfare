---
name: navigation-surface-add
description: Add and configure a `NavMeshSurface` component (from `com.unity.ai.navigation`) on a GameObject — or create a new GameObject to host it. Configure agent type, geometry-collection mode, layer mask, default area, and the box volume (size/center). Returns the GameObject reference and instanceId.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Navigation / Add NavMeshSurface

Add a `NavMeshSurface` to a GameObject. A NavMeshSurface defines a region of the scene that is baked into a runtime NavMesh that agents can traverse. Use 'navigation-set-bake-settings' to tune the agent radius/height/slope/step and voxel size, then 'navigation-surface-bake' to bake.

## Inputs

- `gameObjectRef` — optional GameObject to add the surface to. When omitted, a new GameObject is created.
- `name` — optional name for the new GameObject (default `NavMesh Surface`); ignored when `gameObjectRef` is given.
- `agentTypeId` — optional NavMesh agent-type id this surface bakes for (default 0 = Humanoid).
- `collectObjects` — optional geometry collection mode: `0` All, `1` Volume, `2` Children (default 0 All).
- `defaultArea` — optional default NavMesh area index applied to collected geometry (default 0 = Walkable).
- `layerMask` — optional include-layers bitmask for geometry collection (default -1 = Everything).
- `size` — optional box volume size (only used when `collectObjects` = Volume).
- `center` — optional box volume center offset.

## Behavior

Adds (or reuses) a `NavMeshSurface`, assigns the configured properties, marks the scene dirty, repaints, and returns the GameObject reference, the surface component reference, and the instanceId. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool navigation-surface-add --input '{
  "gameObjectRef": "string_value",
  "name": "string_value",
  "agentTypeId": 0,
  "collectObjects": 0,
  "defaultArea": 0,
  "layerMask": 0,
  "size": "string_value",
  "center": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool navigation-surface-add --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool navigation-surface-add --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `gameObjectRef` | `any` | No | Optional GameObject to add the NavMeshSurface to. When omitted, a new GameObject is created. |
| `name` | `string` | No | Name for the new GameObject (ignored when gameObjectRef is provided). |
| `agentTypeId` | `integer` | No | NavMesh agent-type id this surface bakes for (0 = Humanoid by default). |
| `collectObjects` | `integer` | No | Geometry collection mode: 0 = All, 1 = Volume, 2 = Children. |
| `defaultArea` | `integer` | No | Default NavMesh area index applied to collected geometry (0 = Walkable). |
| `layerMask` | `integer` | No | Include-layers bitmask for geometry collection (-1 = Everything). |
| `size` | `any` | No | Box volume size (used only when collectObjects = Volume). |
| `center` | `any` | No | Box volume center offset. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "name": {
      "type": "string"
    },
    "agentTypeId": {
      "type": "integer"
    },
    "collectObjects": {
      "type": "integer"
    },
    "defaultArea": {
      "type": "integer"
    },
    "layerMask": {
      "type": "integer"
    },
    "size": {
      "$ref": "#/$defs/UnityEngine.Vector3"
    },
    "center": {
      "$ref": "#/$defs/UnityEngine.Vector3"
    }
  },
  "$defs": {
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
    },
    "UnityEngine.Vector3": {
      "type": "object",
      "properties": {
        "x": {
          "type": "number"
        },
        "y": {
          "type": "number"
        },
        "z": {
          "type": "number"
        }
      },
      "required": [
        "x",
        "y",
        "z"
      ],
      "additionalProperties": false
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Navigation-SurfaceAddResponse"
    }
  },
  "$defs": {
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
    },
    "UnityEngine.EntityId": {
      "type": "string",
      "pattern": "^[0-9]+$"
    },
    "System.Type": {
      "type": "string"
    },
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Navigation-SurfaceAddResponse": {
      "type": "object",
      "properties": {
        "gameObjectRef": {
          "$ref": "#/$defs/AIGD.GameObjectRef",
          "description": "Reference to the NavMeshSurface GameObject."
        },
        "surfaceRef": {
          "$ref": "#/$defs/AIGD.ComponentRef",
          "description": "Reference to the NavMeshSurface component."
        },
        "instanceId": {
          "$ref": "#/$defs/UnityEngine.EntityId",
          "description": "Instance id of the GameObject hosting the surface."
        },
        "gameObjectName": {
          "type": "string",
          "description": "Name of the GameObject."
        },
        "agentTypeId": {
          "type": "integer",
          "description": "Resolved agent-type id."
        },
        "collectObjects": {
          "type": "integer",
          "description": "Resolved geometry collection mode."
        },
        "defaultArea": {
          "type": "integer",
          "description": "Resolved default area index."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "instanceId",
        "agentTypeId",
        "collectObjects",
        "defaultArea",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

