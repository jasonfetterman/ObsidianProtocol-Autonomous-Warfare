---
name: terrain-set-neighbors
description: Set the neighboring `Terrain`s of a center `Terrain` (left / top / right / bottom) so Unity blends seams and LOD across tiles. Omitted neighbors are treated as null.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Terrain / Set Neighbors

Wire up the neighbor terrains of a center `Terrain` via `Terrain.SetNeighbors`.

## Inputs

- `gameObjectRef` — the center Terrain GameObject (required).
- `leftRef` / `topRef` / `rightRef` / `bottomRef` — optional GameObjects hosting the neighboring `Terrain`s. Any omitted side is set to null.

## Behavior

Resolves each provided neighbor GameObject to its `Terrain` component, calls `center.SetNeighbors(left, top, right, bottom)`, and triggers `Terrain.Flush()`. Marks the scene dirty and repaints. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool terrain-set-neighbors --input '{
  "gameObjectRef": "string_value",
  "leftRef": "string_value",
  "topRef": "string_value",
  "rightRef": "string_value",
  "bottomRef": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool terrain-set-neighbors --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool terrain-set-neighbors --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `gameObjectRef` | `any` | Yes | Reference to the center Terrain GameObject. |
| `leftRef` | `any` | No | Optional GameObject hosting the left neighbor Terrain. |
| `topRef` | `any` | No | Optional GameObject hosting the top neighbor Terrain. |
| `rightRef` | `any` | No | Optional GameObject hosting the right neighbor Terrain. |
| `bottomRef` | `any` | No | Optional GameObject hosting the bottom neighbor Terrain. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "leftRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "topRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "rightRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "bottomRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
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
    }
  },
  "required": [
    "gameObjectRef"
  ]
}
```

## Output

### Output JSON Schema

```json
{
  "type": "object",
  "properties": {
    "result": {
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Terrain-TerrainSetNeighborsResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Terrain-TerrainSetNeighborsResponse": {
      "type": "object",
      "properties": {
        "gameObjectRef": {
          "$ref": "#/$defs/AIGD.GameObjectRef",
          "description": "Reference to the center Terrain GameObject."
        },
        "terrainRef": {
          "$ref": "#/$defs/AIGD.ComponentRef",
          "description": "Reference to the center Terrain component."
        },
        "left": {
          "type": "string",
          "description": "Name of the left neighbor, or null."
        },
        "top": {
          "type": "string",
          "description": "Name of the top neighbor, or null."
        },
        "right": {
          "type": "string",
          "description": "Name of the right neighbor, or null."
        },
        "bottom": {
          "type": "string",
          "description": "Name of the bottom neighbor, or null."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

