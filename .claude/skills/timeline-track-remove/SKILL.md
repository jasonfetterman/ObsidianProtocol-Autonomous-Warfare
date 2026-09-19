---
name: timeline-track-remove
description: Remove a root track (and its clips/markers) from a `TimelineAsset`, identified by `trackName` or by `trackIndex`. Destructive.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Timeline / Remove Track

Remove a track from a `TimelineAsset`. The track is identified by name (preferred) or by its root index. All clips and markers on the track are deleted with it.

## Inputs

- `assetPath` — required path to the `.playable` TimelineAsset.
- `trackName` — name of the track to remove. When provided, takes precedence over `trackIndex`.
- `trackIndex` — zero-based root-track index used when `trackName` is null/empty (default 0).

## Behavior

Resolves the track, deletes it via `TimelineAsset.DeleteTrack`, saves the asset, and returns the remaining track count. Destructive. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool timeline-track-remove --input '{
  "assetPath": "string_value",
  "trackName": "string_value",
  "trackIndex": 0
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool timeline-track-remove --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool timeline-track-remove --input-file - <<'EOF'
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
| `trackName` | `string` | No | Name of the track to remove. Takes precedence over trackIndex when provided. |
| `trackIndex` | `integer` | No | Zero-based root-track index, used when trackName is null/empty. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "assetPath": {
      "type": "string"
    },
    "trackName": {
      "type": "string"
    },
    "trackIndex": {
      "type": "integer"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-TrackRemoveResponse"
    }
  },
  "$defs": {
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-TrackRemoveResponse": {
      "type": "object",
      "properties": {
        "assetPath": {
          "type": "string",
          "description": "Project path of the TimelineAsset."
        },
        "removedTrackName": {
          "type": "string",
          "description": "Name of the removed track."
        },
        "removedTrackType": {
          "type": "string",
          "description": "Type name of the removed track."
        },
        "remainingRootTrackCount": {
          "type": "integer",
          "description": "Number of root tracks remaining after removal."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "remainingRootTrackCount",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

