---
name: tilemap-create-tile-asset
description: Create a UnityEngine.Tilemaps.Tile asset at an Assets/-rooted path and assign a Sprite (loaded from a sprite asset path) plus an optional color and collider type. Returns the created asset path.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Tilemap / Create Tile Asset

Create a `Tile` ScriptableObject asset and wire its sprite so it can be painted into a Tilemap.

## Inputs

- `assetPath` — `Assets/`-rooted path ending in `.asset` for the new Tile.
- `spriteAssetPath` — optional `Assets/`-rooted path to a `Sprite` asset to assign.
- `color` — optional tile color (default white).
- `colliderType` — optional `None` / `Sprite` / `Grid` (default Sprite).

## Behavior

Creates the intermediate folders, instantiates a `Tile`, assigns sprite/color/collider type, writes the asset via `AssetDatabase.CreateAsset`, saves + refreshes, and returns the asset path. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool tilemap-create-tile-asset --input '{
  "assetPath": "string_value",
  "spriteAssetPath": "string_value",
  "color": "string_value",
  "colliderType": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool tilemap-create-tile-asset --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool tilemap-create-tile-asset --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `assetPath` | `string` | Yes | Assets/-rooted path ending in '.asset' for the new Tile asset. |
| `spriteAssetPath` | `string` | No | Optional Assets/-rooted path to a Sprite asset to assign to the tile. |
| `color` | `any` | No | Optional tile color (default white). |
| `colliderType` | `string` | No | Optional collider type (default Sprite). |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "assetPath": {
      "type": "string"
    },
    "spriteAssetPath": {
      "type": "string"
    },
    "color": {
      "$ref": "#/$defs/UnityEngine.Color"
    },
    "colliderType": {
      "type": "string",
      "enum": [
        "None",
        "Sprite",
        "Grid"
      ]
    }
  },
  "$defs": {
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
    }
  },
  "required": [
    "assetPath"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Tilemap-CreateTileAssetResponse"
    }
  },
  "$defs": {
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Tilemap-CreateTileAssetResponse": {
      "type": "object",
      "properties": {
        "assetPath": {
          "type": "string",
          "description": "Path of the created Tile asset."
        },
        "spriteAssetPath": {
          "type": "string",
          "description": "Path of the assigned Sprite asset, or null."
        },
        "color": {
          "$ref": "#/$defs/UnityEngine.Color",
          "description": "Tile color."
        },
        "colliderType": {
          "type": "string",
          "description": "Tile collider type."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "color",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

