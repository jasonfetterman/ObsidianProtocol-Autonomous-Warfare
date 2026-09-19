---
name: timeline-clip-add
description: Add a clip to a track on a `TimelineAsset`. For AnimationTracks pass an `animationClipPath`; for AudioTracks pass an `audioClipPath`; otherwise a default clip is created. Optionally set start, duration and display name.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Timeline / Add Clip

Add a clip to a track. The clip type depends on the track: an `AnimationTrack` hosts an `AnimationClip`, an `AudioTrack` hosts an `AudioClip`, and other tracks get a default clip. When no source asset is supplied a default (empty) clip is created.

## Inputs

- `assetPath` — required path to the `.playable` TimelineAsset.
- `trackName` / `trackIndex` — which track to add the clip to (name wins; index default 0).
- `animationClipPath` — optional Assets path to an `AnimationClip` (AnimationTrack only).
- `audioClipPath` — optional Assets path to an `AudioClip` (AudioTrack only).
- `start` — optional clip start time in seconds (default 0).
- `duration` — optional clip duration in seconds; when > 0 overrides the default duration.
- `displayName` — optional clip display name.

## Behavior

Resolves the track, creates the appropriate clip (typed when a source asset is given, else a default clip), applies start/duration/name, saves the asset, and returns the new clip's index and timing. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool timeline-clip-add --input '{
  "assetPath": "string_value",
  "trackName": "string_value",
  "trackIndex": 0,
  "animationClipPath": "string_value",
  "audioClipPath": "string_value",
  "start": 0,
  "duration": 0,
  "displayName": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool timeline-clip-add --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool timeline-clip-add --input-file - <<'EOF'
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
| `trackName` | `string` | No | Name of the target track. Takes precedence over trackIndex when provided. |
| `trackIndex` | `integer` | No | Zero-based root-track index, used when trackName is null/empty. |
| `animationClipPath` | `string` | No | Optional Assets path to an AnimationClip (used for AnimationTracks). |
| `audioClipPath` | `string` | No | Optional Assets path to an AudioClip (used for AudioTracks). |
| `start` | `number` | No | Clip start time in seconds (default 0). |
| `duration` | `number` | No | Clip duration in seconds; when > 0 overrides the default duration. |
| `displayName` | `string` | No | Optional clip display name. |

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
    },
    "animationClipPath": {
      "type": "string"
    },
    "audioClipPath": {
      "type": "string"
    },
    "start": {
      "type": "number"
    },
    "duration": {
      "type": "number"
    },
    "displayName": {
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-ClipAddResponse"
    }
  },
  "$defs": {
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-ClipAddResponse": {
      "type": "object",
      "properties": {
        "assetPath": {
          "type": "string",
          "description": "Project path of the TimelineAsset."
        },
        "trackName": {
          "type": "string",
          "description": "Name of the track the clip was added to."
        },
        "clipIndex": {
          "type": "integer",
          "description": "Index of the new clip on its track."
        },
        "displayName": {
          "type": "string",
          "description": "Display name of the clip."
        },
        "start": {
          "type": "number",
          "description": "Clip start time in seconds."
        },
        "duration": {
          "type": "number",
          "description": "Clip duration in seconds."
        },
        "end": {
          "type": "number",
          "description": "Clip end time in seconds."
        },
        "assetType": {
          "type": "string",
          "description": "Type name of the clip's PlayableAsset, or null."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "clipIndex",
        "start",
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

