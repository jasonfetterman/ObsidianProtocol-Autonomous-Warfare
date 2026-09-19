---
name: tilemap-set-tile-flags
description: Set the per-cell tint color and/or transform (flip X/Y, Z rotation, scale) of a tile already painted into a Tilemap. Unlocks the relevant TileFlags so the overrides take effect.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Tilemap / Set Tile Color + Transform

Override the per-cell color and transform of a single tile in a `Tilemap`.

## Inputs

- `gameObjectRef` — the GameObject hosting the `Tilemap` (required).
- `x`, `y`, `z` — the cell coordinate (z defaults to 0).
- `color` — optional per-cell tint color; unlocks `TileFlags.LockColor` when applied.
- `flipX`, `flipY` — optional booleans to mirror the tile.
- `rotationZ` — optional Z rotation in degrees.
- `scale` — optional uniform/again per-axis scale (default identity).

## Behavior

When a color is given, removes `TileFlags.LockColor` then calls `SetColor`. When any transform argument is given, removes `TileFlags.LockTransform` then builds a TRS matrix and calls `SetTransformMatrix`. Marks the scene dirty and repaints. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool tilemap-set-tile-flags --input '{
  "gameObjectRef": "string_value",
  "x": 0,
  "y": 0,
  "z": 0,
  "color": "string_value",
  "flipX": "string_value",
  "flipY": "string_value",
  "rotationZ": "string_value",
  "scale": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool tilemap-set-tile-flags --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool tilemap-set-tile-flags --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `gameObjectRef` | `any` | Yes | Reference to the GameObject containing the Tilemap component. |
| `x` | `integer` | Yes | Cell X coordinate. |
| `y` | `integer` | Yes | Cell Y coordinate. |
| `z` | `integer` | No | Cell Z coordinate (default 0). |
| `color` | `any` | No | Optional per-cell tint color. |
| `flipX` | `any` | No | Optional: mirror the tile horizontally. |
| `flipY` | `any` | No | Optional: mirror the tile vertically. |
| `rotationZ` | `any` | No | Optional: Z rotation in degrees. |
| `scale` | `any` | No | Optional per-axis scale (default (1,1,1)). |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "x": {
      "type": "integer"
    },
    "y": {
      "type": "integer"
    },
    "z": {
      "type": "integer"
    },
    "color": {
      "$ref": "#/$defs/UnityEngine.Color"
    },
    "flipX": {
      "$ref": "#/$defs/System.Boolean"
    },
    "flipY": {
      "$ref": "#/$defs/System.Boolean"
    },
    "rotationZ": {
      "$ref": "#/$defs/System.Single"
    },
    "scale": {
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
    "UnityEngine.Color": {
      "type": "object",
      "properties": {
        "r": {
          "type": "number",
          "minimum": 0,
          "maximum": 1
        },
        "g": {
          "type": "number",
          "minimum": 0,
          "maximum": 1
        },
        "b": {
          "type": "number",
          "minimum": 0,
          "maximum": 1
        },
        "a": {
          "type": "number",
          "minimum": 0,
          "maximum": 1
        }
      },
      "required": [
        "r",
        "g",
        "b",
        "a"
      ],
      "additionalProperties": false
    },
    "System.Boolean": {
      "type": "boolean"
    },
    "System.Single": {
      "type": "number"
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
  },
  "required": [
    "gameObjectRef",
    "x",
    "y"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Tilemap-SetTileFlagsResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Tilemap-SetTileFlagsResponse": {
      "type": "object",
      "properties": {
        "gameObjectRef": {
          "$ref": "#/$defs/AIGD.GameObjectRef",
          "description": "Reference to the Tilemap GameObject."
        },
        "tilemapRef": {
          "$ref": "#/$defs/AIGD.ComponentRef",
          "description": "Reference to the Tilemap component."
        },
        "cellX": {
          "type": "integer",
          "description": "Cell X coordinate."
        },
        "cellY": {
          "type": "integer",
          "description": "Cell Y coordinate."
        },
        "cellZ": {
          "type": "integer",
          "description": "Cell Z coordinate."
        },
        "colorApplied": {
          "type": "boolean",
          "description": "True when a color override was applied."
        },
        "transformApplied": {
          "type": "boolean",
          "description": "True when a transform override was applied."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "cellX",
        "cellY",
        "cellZ",
        "colorApplied",
        "transformApplied",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

