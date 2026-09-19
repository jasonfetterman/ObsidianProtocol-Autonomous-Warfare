---
name: tilemap-set-collider-type
description: Set the per-cell Tile.ColliderType (None / Sprite / Grid) of a tile painted into a Tilemap. Pair with a TilemapCollider2D to generate physics colliders from the tiles.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Tilemap / Set Tile Collider Type

Override the collider type of a single tile cell in a `Tilemap`.

## Inputs

- `gameObjectRef` — the GameObject hosting the `Tilemap` (required).
- `x`, `y`, `z` — the cell coordinate (z defaults to 0).
- `colliderType` — `None`, `Sprite`, or `Grid`.

## Behavior

Maps the argument to `UnityEngine.Tilemaps.Tile.ColliderType` and calls `Tilemap.SetColliderType`, marks the scene dirty, and repaints. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool tilemap-set-collider-type --input '{
  "gameObjectRef": "string_value",
  "x": 0,
  "y": 0,
  "colliderType": "string_value",
  "z": 0
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool tilemap-set-collider-type --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool tilemap-set-collider-type --input-file - <<'EOF'
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
| `colliderType` | `string` | Yes | The collider type to set: None, Sprite, or Grid. |
| `z` | `integer` | No | Cell Z coordinate (default 0). |

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
    "colliderType": {
      "type": "string",
      "enum": [
        "None",
        "Sprite",
        "Grid"
      ]
    },
    "z": {
      "type": "integer"
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
    "gameObjectRef",
    "x",
    "y",
    "colliderType"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Tilemap-SetColliderTypeResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Tilemap-SetColliderTypeResponse": {
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
        "colliderType": {
          "type": "string",
          "description": "Resulting collider type."
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
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

