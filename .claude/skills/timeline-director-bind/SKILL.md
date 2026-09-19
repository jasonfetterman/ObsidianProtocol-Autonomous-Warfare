---
name: timeline-director-bind
description: Bind a `TimelineAsset` to a scene GameObject's `PlayableDirector` (adding the component if missing). Optionally set the play-on-awake flag. Returns the director's GameObject and instanceId.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Timeline / Bind PlayableDirector

Assign a `TimelineAsset` as the playable of a scene `PlayableDirector` so the timeline can be played at runtime/in the editor. If the GameObject has no `PlayableDirector`, one is added.

## Inputs

- `gameObjectRef` — the scene GameObject to host/own the `PlayableDirector` (required).
- `assetPath` — the `.playable` TimelineAsset to bind (required).
- `playOnAwake` — optional flag to set `PlayableDirector.playOnAwake`.

## Behavior

Resolves the GameObject, ensures a `PlayableDirector`, sets `playableAsset` to the loaded `TimelineAsset`, applies `playOnAwake` when provided, marks the scene dirty, and returns the director GameObject reference + instanceId. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool timeline-director-bind --input '{
  "gameObjectRef": "string_value",
  "assetPath": "string_value",
  "playOnAwake": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool timeline-director-bind --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool timeline-director-bind --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `gameObjectRef` | `any` | Yes | Reference to the scene GameObject that should own the PlayableDirector. |
| `assetPath` | `string` | Yes | Assets-rooted path to the TimelineAsset (.playable) to bind. |
| `playOnAwake` | `any` | No | Optional value for PlayableDirector.playOnAwake. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "assetPath": {
      "type": "string"
    },
    "playOnAwake": {
      "$ref": "#/$defs/System.Boolean"
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
    },
    "System.Boolean": {
      "type": "boolean"
    }
  },
  "required": [
    "gameObjectRef",
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-DirectorBindResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Timeline-DirectorBindResponse": {
      "type": "object",
      "properties": {
        "gameObjectRef": {
          "$ref": "#/$defs/AIGD.GameObjectRef",
          "description": "Reference to the GameObject hosting the PlayableDirector."
        },
        "directorRef": {
          "$ref": "#/$defs/AIGD.ComponentRef",
          "description": "Reference to the PlayableDirector component."
        },
        "instanceId": {
          "$ref": "#/$defs/UnityEngine.EntityId",
          "description": "Instance id of the director GameObject."
        },
        "assetPath": {
          "type": "string",
          "description": "Project path of the bound TimelineAsset."
        },
        "playOnAwake": {
          "type": "boolean",
          "description": "Resulting playOnAwake flag."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "instanceId",
        "playOnAwake",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

