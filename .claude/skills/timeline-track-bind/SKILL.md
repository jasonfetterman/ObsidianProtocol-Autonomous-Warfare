---
name: timeline-track-bind
description: Bind a scene object to a Timeline output track via a `PlayableDirector` generic binding — e.g. point an AnimationTrack at an `Animator`, or an ActivationTrack at a GameObject. The director must already play the timeline that owns the track.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Timeline / Bind Track

Set the scene-object binding for a specific output track on a `PlayableDirector`. The director's `playableAsset` must be the `TimelineAsset` that owns the track. For an `AnimationTrack` the binding is typically an `Animator`; for an `ActivationTrack`/`AudioTrack` it is the GameObject / AudioSource that the track drives.

## Inputs

- `directorRef` — GameObject hosting the `PlayableDirector` that plays the timeline (required).
- `trackName` / `trackIndex` — which output track to bind (name wins; index default 0).
- `targetRef` — the scene GameObject to bind to the track. When the track expects a `Component` (e.g. Animator/AudioSource), the matching component on this GameObject is used; otherwise the GameObject itself is bound.
- `clearBinding` — when true, clears the binding instead of setting it (ignored if `targetRef` is given).

## Behavior

Resolves the director and its timeline, finds the track, computes the binding object (component when the track output type is a Component, else the GameObject), calls `SetGenericBinding`, marks the scene dirty, and returns the bound object's name. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool timeline-track-bind --input '{
  "directorRef": "string_value",
  "trackName": "string_value",
  "trackIndex": 0,
  "targetRef": "string_value",
  "clearBinding": false
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool timeline-track-bind --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool timeline-track-bind --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `directorRef` | `any` | Yes | Reference to the GameObject hosting the PlayableDirector that plays the timeline. |
| `trackName` | `string` | No | Name of the output track to bind. Takes precedence over trackIndex. |
| `trackIndex` | `integer` | No | Zero-based root-track index, used when trackName is null/empty. |
| `targetRef` | `any` | No | Scene GameObject to bind to the track (its matching component is used when the track expects one). |
| `clearBinding` | `boolean` | No | If true, clears the track binding (ignored when targetRef is provided). |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "directorRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "trackName": {
      "type": "string"
    },
    "trackIndex": {
      "type": "integer"
    },
    "targetRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "clearBinding": {
      "type": "boolean"
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
    "directorRef"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-TrackBindResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-TrackBindResponse": {
      "type": "object",
      "properties": {
        "directorRef": {
          "$ref": "#/$defs/AIGD.GameObjectRef",
          "description": "Reference to the GameObject hosting the PlayableDirector."
        },
        "trackName": {
          "type": "string",
          "description": "Name of the bound track."
        },
        "boundTo": {
          "type": "string",
          "description": "Name of the bound object, or null when cleared."
        },
        "boundType": {
          "type": "string",
          "description": "Type name of the bound object, or null."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

