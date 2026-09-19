---
name: tilemap-create-rule-tile
description: Create a UnityEngine.RuleTile asset (from the 2D Tilemap Extras package) at an Assets/-rooted path, with an optional default Sprite. RuleTiles auto-pick sprites based on neighbour rules — paint them like any tile and add rules afterward via the Inspector or 'tilemap-modify'.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Tilemap / Create Rule Tile

Create a `RuleTile` ScriptableObject asset (provided by `com.unity.2d.tilemap.extras`).

## Inputs

- `assetPath` — `Assets/`-rooted path ending in `.asset` for the new RuleTile.
- `defaultSpriteAssetPath` — optional `Assets/`-rooted path to a `Sprite` used as the RuleTile's default sprite (shown when no rule matches).

## Behavior

Creates the intermediate folders, instantiates a `RuleTile`, assigns the default sprite when supplied, writes the asset via `AssetDatabase.CreateAsset`, saves + refreshes, and returns the asset path. Neighbour rules can then be authored via `tilemap-modify` or the Inspector. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool tilemap-create-rule-tile --input '{
  "assetPath": "string_value",
  "defaultSpriteAssetPath": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool tilemap-create-rule-tile --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool tilemap-create-rule-tile --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `assetPath` | `string` | Yes | Assets/-rooted path ending in '.asset' for the new RuleTile asset. |
| `defaultSpriteAssetPath` | `string` | No | Optional Assets/-rooted path to a Sprite used as the RuleTile default sprite. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "assetPath": {
      "type": "string"
    },
    "defaultSpriteAssetPath": {
      "type": "string"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Tilemap-CreateRuleTileResponse"
    }
  },
  "$defs": {
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Tilemap-CreateRuleTileResponse": {
      "type": "object",
      "properties": {
        "assetPath": {
          "type": "string",
          "description": "Path of the created RuleTile asset."
        },
        "defaultSpriteAssetPath": {
          "type": "string",
          "description": "Path of the assigned default Sprite, or null."
        },
        "ruleCount": {
          "type": "integer",
          "description": "Number of tiling rules (0 for a fresh RuleTile)."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "ruleCount",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

