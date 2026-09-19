---
name: timeline-track-add
description: Add a track to a `TimelineAsset`. `trackType` accepts a kind keyword (Animation, Activation, Audio, Signal, Control, Playable, Group) or a full TrackAsset type name. Optionally nest the new track under an existing GroupTrack.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Timeline / Add Track

Add a new track to a `TimelineAsset`. Tracks are the lanes a timeline plays: an `AnimationTrack` drives an Animator, an `ActivationTrack` toggles a GameObject active, an `AudioTrack` plays clips, a `SignalTrack` emits signals, a `ControlTrack` drives nested directors/particles, and a `GroupTrack` organizes other tracks.

## Inputs

- `assetPath` — required path to the `.playable` TimelineAsset.
- `trackType` — required kind keyword or full TrackAsset type name.
- `trackName` — optional display name for the new track.
- `parentGroupName` — optional name of an existing GroupTrack to nest the new track under.

## Behavior

Resolves the track type, creates the track via `TimelineAsset.CreateTrack`, parents it under the named group when provided, saves the asset, and returns the new track's name, type and root index. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool timeline-track-add --input '{
  "assetPath": "string_value",
  "trackType": "string_value",
  "trackName": "string_value",
  "parentGroupName": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool timeline-track-add --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool timeline-track-add --input-file - <<'EOF'
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
| `trackType` | `string` | Yes | Track kind keyword (Animation/Activation/Audio/Signal/Control/Playable/Group) or full TrackAsset type name. |
| `trackName` | `string` | No | Optional display name for the new track. |
| `parentGroupName` | `string` | No | Optional name of an existing GroupTrack to nest the new track under. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "assetPath": {
      "type": "string"
    },
    "trackType": {
      "type": "string"
    },
    "trackName": {
      "type": "string"
    },
    "parentGroupName": {
      "type": "string"
    }
  },
  "required": [
    "assetPath",
    "trackType"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-TrackAddResponse"
    }
  },
  "$defs": {
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-TrackAddResponse": {
      "type": "object",
      "properties": {
        "assetPath": {
          "type": "string",
          "description": "Project path of the TimelineAsset."
        },
        "trackName": {
          "type": "string",
          "description": "Name of the created track."
        },
        "trackType": {
          "type": "string",
          "description": "Type name of the created track."
        },
        "rootIndex": {
          "type": "integer",
          "description": "Index of the track among the root tracks (-1 if nested under a group)."
        },
        "parentGroup": {
          "type": "string",
          "description": "Name of the parent GroupTrack, or null."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "rootIndex",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

