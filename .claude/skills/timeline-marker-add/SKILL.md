---
name: timeline-marker-add
description: Add a marker at a time on a track (or the timeline marker track). `markerType` accepts 'Signal' (a SignalEmitter, default) or a full IMarker/ScriptableObject marker type name.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Timeline / Add Marker

Add a marker at a given time. By default a `SignalEmitter` is created (the standard Timeline signal marker). A full marker type name can be supplied to create a custom marker. The marker is placed on the named track, or on the timeline's marker track when no track is given.

## Inputs

- `assetPath` — required path to the `.playable` TimelineAsset.
- `time` — required marker time in seconds.
- `markerType` — 'Signal' (default) or a full marker type name (must implement `IMarker`).
- `trackName` — optional track to host the marker; when null, the timeline's marker track is used.
- `trackIndex` — root-track index used when `trackName` is null AND `useMarkerTrack` is false.
- `useMarkerTrack` — when true (default), null trackName means the timeline marker track; when false, the marker is placed on the track at `trackIndex`.

## Behavior

Resolves the marker holder (a `TrackAsset` or the timeline's marker track), creates the marker via `CreateMarker`, sets its time, saves the asset, and returns the marker type and time. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool timeline-marker-add --input '{
  "assetPath": "string_value",
  "time": 0,
  "markerType": "string_value",
  "trackName": "string_value",
  "trackIndex": 0,
  "useMarkerTrack": false
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool timeline-marker-add --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool timeline-marker-add --input-file - <<'EOF'
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
| `time` | `number` | Yes | Marker time in seconds. |
| `markerType` | `string` | No | Marker type: 'Signal' (default) or a full marker type name implementing IMarker. |
| `trackName` | `string` | No | Optional track to host the marker. Takes precedence over the marker-track / index logic. |
| `trackIndex` | `integer` | No | Root-track index used when trackName is null and useMarkerTrack is false. |
| `useMarkerTrack` | `boolean` | No | When true (default), a null trackName targets the timeline's marker track; when false, the track at trackIndex. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "assetPath": {
      "type": "string"
    },
    "time": {
      "type": "number"
    },
    "markerType": {
      "type": "string"
    },
    "trackName": {
      "type": "string"
    },
    "trackIndex": {
      "type": "integer"
    },
    "useMarkerTrack": {
      "type": "boolean"
    }
  },
  "required": [
    "assetPath",
    "time"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-MarkerAddResponse"
    }
  },
  "$defs": {
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-MarkerAddResponse": {
      "type": "object",
      "properties": {
        "assetPath": {
          "type": "string",
          "description": "Project path of the TimelineAsset."
        },
        "holderTrackName": {
          "type": "string",
          "description": "Name of the track (or marker track) the marker was placed on."
        },
        "markerType": {
          "type": "string",
          "description": "Type name of the created marker."
        },
        "time": {
          "type": "number",
          "description": "Marker time in seconds."
        },
        "markerCountOnHolder": {
          "type": "integer",
          "description": "Number of markers on the holding track after adding."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "time",
        "markerCountOnHolder",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

