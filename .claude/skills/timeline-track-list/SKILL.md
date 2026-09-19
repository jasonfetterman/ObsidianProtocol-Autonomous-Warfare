---
name: timeline-track-list
description: List the tracks of a `TimelineAsset` — name, type, mute/lock state, clip count, and each track's clips (with timing). Read-only.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Timeline / List Tracks

List the tracks of a `TimelineAsset`. For each output track returns its name, type, muted / locked flags, clip count, and (optionally) the clips with their timing.

## Inputs

- `assetPath` — required path to the `.playable` TimelineAsset.
- `includeClips` — when `true` (default) include each track's clip list; when `false`, counts only.

## Behavior

Enumerates `TimelineAsset.GetOutputTracks()` and reports each track. Read-only. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool timeline-track-list --input '{
  "assetPath": "string_value",
  "includeClips": false
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool timeline-track-list --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool timeline-track-list --input-file - <<'EOF'
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
| `includeClips` | `boolean` | No | If true (default), include each track's clips and their timing; if false, only counts. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "assetPath": {
      "type": "string"
    },
    "includeClips": {
      "type": "boolean"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-TrackListResponse"
    }
  },
  "$defs": {
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-TrackInfo-1": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-TrackInfo"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-TrackInfo": {
      "type": "object",
      "properties": {
        "name": {
          "type": "string",
          "description": "Track name."
        },
        "trackType": {
          "type": "string",
          "description": "Track type name."
        },
        "isRoot": {
          "type": "boolean",
          "description": "Whether the track is a root (top-level) track."
        },
        "rootIndex": {
          "type": "integer",
          "description": "Root-track index, or -1 when nested in a group."
        },
        "muted": {
          "type": "boolean",
          "description": "Whether the track is muted."
        },
        "locked": {
          "type": "boolean",
          "description": "Whether the track is locked."
        },
        "clipCount": {
          "type": "integer",
          "description": "Number of clips on the track."
        },
        "clips": {
          "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-ClipInfo-1",
          "description": "The clips on the track (empty when includeClips is false)."
        }
      },
      "required": [
        "isRoot",
        "rootIndex",
        "muted",
        "locked",
        "clipCount"
      ]
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-ClipInfo-1": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-ClipInfo"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-ClipInfo": {
      "type": "object",
      "properties": {
        "index": {
          "type": "integer",
          "description": "Zero-based index of the clip on its track."
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
        "blendInDuration": {
          "type": "number",
          "description": "Blend-in duration in seconds."
        },
        "blendOutDuration": {
          "type": "number",
          "description": "Blend-out duration in seconds."
        },
        "assetType": {
          "type": "string",
          "description": "Type name of the clip's PlayableAsset, or null."
        }
      },
      "required": [
        "index",
        "start",
        "duration",
        "end",
        "blendInDuration",
        "blendOutDuration"
      ]
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-TrackListResponse": {
      "type": "object",
      "properties": {
        "assetPath": {
          "type": "string",
          "description": "Project path of the TimelineAsset."
        },
        "durationMode": {
          "type": "string",
          "description": "Duration mode of the timeline."
        },
        "duration": {
          "type": "number",
          "description": "Computed duration of the timeline in seconds."
        },
        "count": {
          "type": "integer",
          "description": "Number of output tracks."
        },
        "tracks": {
          "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-TrackInfo-1",
          "description": "The output tracks of the timeline."
        }
      },
      "required": [
        "duration",
        "count"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

