---
name: terrain-set-heights
description: Set heightmap values over a rectangular region (or the whole terrain) of a `Terrain`. Either fill the region with a uniform normalized height, or supply an explicit row-major 2D heights array. Heights are normalized [0,1] of the terrain's Y size.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Terrain / Set Heights

Write heightmap values into a `Terrain`'s `TerrainData`.

## Inputs

- `gameObjectRef` — the GameObject hosting the `Terrain` (required).
- `xBase` / `yBase` — top-left heightmap cell of the region (default 0,0).
- `width` / `height` — region size in heightmap cells. When &lt;= 0, the region spans to the edge of the heightmap from the base.
- `uniformHeight` — when `heights` is omitted, fill the whole region with this normalized [0,1] value (default 0).
- `heights` — optional explicit row-major `[height][width]` array of normalized [0,1] values. When provided, its dimensions define the region size (overriding width/height).

## Behavior

Resolves and clamps the region against `heightmapResolution`, builds a `[h,w]` height array (uniform fill or from `heights`), and calls `TerrainData.SetHeights(xBase, yBase, array)`. Destructive: overwrites existing heights in the region. Marks dirty + repaints. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool terrain-set-heights --input '{
  "gameObjectRef": "string_value",
  "xBase": 0,
  "yBase": 0,
  "width": 0,
  "height": 0,
  "uniformHeight": 0,
  "heights": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool terrain-set-heights --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool terrain-set-heights --input-file - <<'EOF'
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
| `xBase` | `integer` | No | Top-left heightmap X cell of the region. |
| `yBase` | `integer` | No | Top-left heightmap Y cell of the region. |
| `width` | `integer` | No | Region width in heightmap cells. <= 0 spans to the heightmap edge. |
| `height` | `integer` | No | Region height in heightmap cells. <= 0 spans to the heightmap edge. |
| `uniformHeight` | `number` | No | Uniform normalized [0,1] height used to fill the region when 'heights' is omitted. |
| `heights` | `any` | No | Optional explicit row-major [height][width] array of normalized [0,1] heights. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "xBase": {
      "type": "integer"
    },
    "yBase": {
      "type": "integer"
    },
    "width": {
      "type": "integer"
    },
    "height": {
      "type": "integer"
    },
    "uniformHeight": {
      "type": "number"
    },
    "heights": {
      "$ref": "#/$defs/System.Single-1-1"
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
    "System.Single-1": {
      "type": "array",
      "items": {
        "type": "number"
      }
    },
    "System.Single-1-1": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/System.Single-1"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Terrain-TerrainSetHeightsResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Terrain-TerrainSetHeightsResponse": {
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
        "xBase": {
          "type": "integer",
          "description": "Resolved region top-left X cell."
        },
        "yBase": {
          "type": "integer",
          "description": "Resolved region top-left Y cell."
        },
        "width": {
          "type": "integer",
          "description": "Resolved region width in cells."
        },
        "height": {
          "type": "integer",
          "description": "Resolved region height in cells."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "xBase",
        "yBase",
        "width",
        "height",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

