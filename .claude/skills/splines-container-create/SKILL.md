---
name: splines-container-create
description: Create a new GameObject with a `SplineContainer` (and one initial empty `Spline`) in the active scene. Optionally set name, position and rotation. Returns the new GameObject reference and instanceId.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Splines / Create Container

Create a new GameObject hosting a `SplineContainer` component in the active scene. The `SplineContainer` is the authoring component that holds one or more `Spline`s; this tool seeds it with a single empty spline ready for knots.

## Inputs

- `name` — optional GameObject name (default `Spline`).
- `position` — optional world `Vector3` position (default zero).
- `rotation` — optional euler-degrees `Vector3` rotation (default zero).
- `closed` — optional; when true the seeded spline is a closed loop (default false).

## Behavior

Creates the GameObject, adds a `SplineContainer` with one empty spline, applies transform and the `Closed` flag, marks the scene dirty, repaints the Editor, and returns the new GameObject reference and instanceId. The whole call runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool splines-container-create --input '{
  "name": "string_value",
  "position": "string_value",
  "rotation": "string_value",
  "closed": false
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool splines-container-create --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool splines-container-create --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `name` | `string` | No | Name of the new SplineContainer GameObject. |
| `position` | `any` | No | World-space position of the GameObject. |
| `rotation` | `any` | No | World-space rotation of the GameObject in euler angles (degrees). |
| `closed` | `boolean` | No | If true, the seeded spline is a closed loop. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "name": {
      "type": "string"
    },
    "position": {
      "$ref": "#/$defs/UnityEngine.Vector3"
    },
    "rotation": {
      "$ref": "#/$defs/UnityEngine.Vector3"
    },
    "closed": {
      "type": "boolean"
    }
  },
  "$defs": {
    "UnityEngine.Vector3": {
      "type": "object",
      "properties": {
        "x": {
          "type": "number"
        },
        "y": {
          "type": "number"
        },
        "z": {
          "type": "number"
        }
      },
      "required": [
        "x",
        "y",
        "z"
      ],
      "additionalProperties": false
    }
  }
}
```

## Output

### Output JSON Schema

```json
{
  "type": "object",
  "properties": {
    "result": {
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Splines-ContainerCreateResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Splines-ContainerCreateResponse": {
      "type": "object",
      "properties": {
        "gameObjectRef": {
          "$ref": "#/$defs/AIGD.GameObjectRef",
          "description": "Reference to the created SplineContainer GameObject."
        },
        "containerRef": {
          "$ref": "#/$defs/AIGD.ComponentRef",
          "description": "Reference to the created SplineContainer component."
        },
        "instanceId": {
          "$ref": "#/$defs/UnityEngine.EntityId",
          "description": "Instance id of the created GameObject."
        },
        "gameObjectName": {
          "type": "string",
          "description": "Name of the created GameObject."
        },
        "splineCount": {
          "type": "integer",
          "description": "Number of splines in the container."
        },
        "closed": {
          "type": "boolean",
          "description": "Whether the seeded spline is a closed loop."
        }
      },
      "required": [
        "instanceId",
        "splineCount",
        "closed"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

