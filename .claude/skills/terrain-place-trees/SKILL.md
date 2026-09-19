---
name: terrain-place-trees
description: Place trees on a `Terrain` using an existing tree prototype. Either scatter `count` trees randomly across a normalized [0,1] sub-rectangle, or place trees at explicit normalized positions. Optionally clear existing trees first.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Terrain / Place Trees

Add `TreeInstance`s to a `Terrain`. Tree positions are normalized [0,1] across the terrain.

## Inputs

- `gameObjectRef` — the GameObject hosting the `Terrain` (required).
- `prototypeIndex` — index of the tree prototype to place (must already be on the terrain, default 0).
- `count` — number of trees to scatter randomly (used when `positions` is omitted; default 100).
- `minX` / `minZ` / `maxX` / `maxZ` — normalized [0,1] sub-rectangle to scatter within (default 0..1 on both axes).
- `positions` — optional explicit normalized [0,1] XZ positions (each a `Vector2`, x→X, y→Z). When provided, overrides random scatter.
- `heightScale` / `widthScale` — per-instance scale (default 1).
- `clearExisting` — when `true`, removes existing trees before placing (default false).

## Behavior

Validates the prototype index, builds `TreeInstance`s (sampling the terrain height at each normalized position), and assigns them via `TerrainData.SetTreeInstances(..., snapToHeightmap:true)`. Marks dirty + repaints. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool terrain-place-trees --input '{
  "gameObjectRef": "string_value",
  "prototypeIndex": 0,
  "count": 0,
  "minX": 0,
  "minZ": 0,
  "maxX": 0,
  "maxZ": 0,
  "positions": "string_value",
  "heightScale": 0,
  "widthScale": 0,
  "clearExisting": false
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool terrain-place-trees --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool terrain-place-trees --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `gameObjectRef` | `any` | Yes | Reference to the GameObject containing the Terrain component. |
| `prototypeIndex` | `integer` | No | Index of the tree prototype to place (must already be on the terrain). |
| `count` | `integer` | No | Number of trees to scatter randomly when 'positions' is omitted. |
| `minX` | `number` | No | Normalized [0,1] minimum X of the scatter sub-rectangle. |
| `minZ` | `number` | No | Normalized [0,1] minimum Z of the scatter sub-rectangle. |
| `maxX` | `number` | No | Normalized [0,1] maximum X of the scatter sub-rectangle. |
| `maxZ` | `number` | No | Normalized [0,1] maximum Z of the scatter sub-rectangle. |
| `positions` | `any` | No | Optional explicit normalized [0,1] XZ positions (Vector2: x->X, y->Z). |
| `heightScale` | `number` | No | Per-instance height scale. |
| `widthScale` | `number` | No | Per-instance width scale. |
| `clearExisting` | `boolean` | No | If true, removes existing trees before placing. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "prototypeIndex": {
      "type": "integer"
    },
    "count": {
      "type": "integer"
    },
    "minX": {
      "type": "number"
    },
    "minZ": {
      "type": "number"
    },
    "maxX": {
      "type": "number"
    },
    "maxZ": {
      "type": "number"
    },
    "positions": {
      "$ref": "#/$defs/UnityEngine.Vector2-1"
    },
    "heightScale": {
      "type": "number"
    },
    "widthScale": {
      "type": "number"
    },
    "clearExisting": {
      "type": "boolean"
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
    "UnityEngine.Vector2": {
      "type": "object",
      "properties": {
        "x": {
          "type": "number"
        },
        "y": {
          "type": "number"
        }
      },
      "required": [
        "x",
        "y"
      ],
      "additionalProperties": false
    },
    "UnityEngine.Vector2-1": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/UnityEngine.Vector2"
      }
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Terrain-TerrainPlaceTreesResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Terrain-TerrainPlaceTreesResponse": {
      "type": "object",
      "properties": {
        "gameObjectRef": {
          "$ref": "#/$defs/AIGD.GameObjectRef",
          "description": "Reference to the Terrain GameObject."
        },
        "terrainRef": {
          "$ref": "#/$defs/AIGD.ComponentRef",
          "description": "Reference to the Terrain component."
        },
        "prototypeIndex": {
          "type": "integer",
          "description": "Index of the placed tree prototype."
        },
        "treesAdded": {
          "type": "integer",
          "description": "Number of trees added by this call."
        },
        "treeInstanceCount": {
          "type": "integer",
          "description": "Total tree instance count after the call."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "prototypeIndex",
        "treesAdded",
        "treeInstanceCount",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

