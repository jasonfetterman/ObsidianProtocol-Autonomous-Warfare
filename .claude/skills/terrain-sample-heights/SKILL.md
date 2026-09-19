---
name: terrain-sample-heights
description: Read heightmap values over a rectangular region (or the whole terrain) of a `Terrain`. Returns the normalized [0,1] heights plus min/max/average statistics. Read-only.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Terrain / Sample Heights

Sample heightmap values from a `Terrain`'s `TerrainData`.

## Inputs

- `gameObjectRef` — the GameObject hosting the `Terrain` (required).
- `xBase` / `yBase` — top-left heightmap cell of the region (default 0,0).
- `width` / `height` — region size in heightmap cells. When &lt;= 0, the region spans to the edge of the heightmap from the base.
- `includeArray` — when `true`, include the full row-major `[height][width]` heights array in the response (can be large); otherwise only statistics are returned (default false).

## Behavior

Resolves and clamps the region against `heightmapResolution`, calls `TerrainData.GetHeights`, computes min/max/average over the sampled cells, and optionally returns the full array. Read-only. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool terrain-sample-heights --input '{
  "gameObjectRef": "string_value",
  "xBase": 0,
  "yBase": 0,
  "width": 0,
  "height": 0,
  "includeArray": false
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool terrain-sample-heights --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool terrain-sample-heights --input-file - <<'EOF'
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
| `includeArray` | `boolean` | No | If true, include the full row-major [height][width] heights array in the response. |

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
    "includeArray": {
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Terrain-TerrainSampleHeightsResponse"
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
    "System.Single-1-1": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/System.Single-1"
      }
    },
    "System.Single-1": {
      "type": "array",
      "items": {
        "type": "number"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Terrain-TerrainSampleHeightsResponse": {
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
        "min": {
          "type": "number",
          "description": "Minimum normalized [0,1] height in the region."
        },
        "max": {
          "type": "number",
          "description": "Maximum normalized [0,1] height in the region."
        },
        "average": {
          "type": "number",
          "description": "Average normalized [0,1] height in the region."
        },
        "heights": {
          "$ref": "#/$defs/System.Single-1-1",
          "description": "Row-major [height][width] heights array, or null when includeArray is false."
        }
      },
      "required": [
        "xBase",
        "yBase",
        "width",
        "height",
        "min",
        "max",
        "average"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

