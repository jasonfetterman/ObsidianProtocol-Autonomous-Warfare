---
name: timeline-clip-move
description: Move a clip along its track — either to an absolute `start` time or by a relative `deltaSeconds`. Duration is preserved.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Timeline / Move Clip

Reposition a clip on its track without changing its duration. Provide either an absolute `start` or a relative `deltaSeconds` (delta is ignored when `start` is supplied).

## Inputs

- `assetPath` — required path to the `.playable` TimelineAsset.
- `trackName` / `trackIndex` — which track holds the clip (name wins; index default 0).
- `clipIndex` — required zero-based clip index.
- `start` — optional absolute new start time (seconds).
- `deltaSeconds` — optional relative shift (seconds); used only when `start` is null.

## Behavior

Computes the new start (clamped to >= 0), sets `clip.start`, saves the asset, and returns the old and new start. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool timeline-clip-move --input '{
  "assetPath": "string_value",
  "clipIndex": 0,
  "trackName": "string_value",
  "trackIndex": 0,
  "start": "string_value",
  "deltaSeconds": 0
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool timeline-clip-move --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool timeline-clip-move --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `assetPath` | `string` | Yes | Assets-rooted path to the TimelineAsset (.playable). |
| `clipIndex` | `integer` | Yes | Zero-based index of the clip on its track. |
| `trackName` | `string` | No | Name of the track holding the clip. Takes precedence over trackIndex. |
| `trackIndex` | `integer` | No | Zero-based root-track index, used when trackName is null/empty. |
| `start` | `any` | No | Optional absolute new start time in seconds. |
| `deltaSeconds` | `number` | No | Optional relative shift in seconds; used only when start is null. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "assetPath": {
      "type": "string"
    },
    "clipIndex": {
      "type": "integer"
    },
    "trackName": {
      "type": "string"
    },
    "trackIndex": {
      "type": "integer"
    },
    "start": {
      "$ref": "#/$defs/System.Double"
    },
    "deltaSeconds": {
      "type": "number"
    }
  },
  "$defs": {
    "System.Double": {
      "type": "number"
    }
  },
  "required": [
    "assetPath",
    "clipIndex"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-ClipMoveResponse"
    }
  },
  "$defs": {
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-ClipMoveResponse": {
      "type": "object",
      "properties": {
        "assetPath": {
          "type": "string",
          "description": "Project path of the TimelineAsset."
        },
        "trackName": {
          "type": "string",
          "description": "Name of the track holding the clip."
        },
        "clipIndex": {
          "type": "integer",
          "description": "Index of the clip on its track."
        },
        "oldStart": {
          "type": "number",
          "description": "Previous start time in seconds."
        },
        "newStart": {
          "type": "number",
          "description": "New start time in seconds."
        },
        "duration": {
          "type": "number",
          "description": "Clip duration in seconds (unchanged)."
        },
        "end": {
          "type": "number",
          "description": "New end time in seconds."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "clipIndex",
        "oldStart",
        "newStart",
        "duration",
        "end",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

