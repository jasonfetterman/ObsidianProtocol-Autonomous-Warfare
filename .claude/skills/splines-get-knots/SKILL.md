---
name: splines-get-knots
description: "Read all knots of a spline in a `SplineContainer`: per-knot position, in/out tangents, rotation, and tangent mode, plus the closed flag. Read-only."
metadata:
  generated-by: mcp-plugin-dotnet
---

# Splines / Get Knots

Inspect the knots of a specific spline. Useful before editing to know the indices, positions, and tangent modes.

## Inputs

- `gameObjectRef` — the GameObject hosting the `SplineContainer` (required).
- `splineIndex` — which spline in the container (default 0).

## Behavior

Enumerates every knot of the chosen spline and returns a per-knot summary plus the spline's `Closed` flag. Read-only. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool splines-get-knots --input '{
  "gameObjectRef": "string_value",
  "splineIndex": 0
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool splines-get-knots --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool splines-get-knots --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `gameObjectRef` | `any` | Yes | Reference to the GameObject containing the SplineContainer component. |
| `splineIndex` | `integer` | No | Index of the spline inside the container (default 0). |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "splineIndex": {
      "type": "integer"
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
    "gameObjectRef"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Splines-GetKnotsResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Splines-KnotInfo-1": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Splines-KnotInfo"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Splines-KnotInfo": {
      "type": "object",
      "properties": {
        "index": {
          "type": "integer",
          "description": "Index of the knot inside the spline."
        },
        "position": {
          "$ref": "#/$defs/UnityEngine.Vector3",
          "description": "Local-space position of the knot."
        },
        "tangentIn": {
          "$ref": "#/$defs/UnityEngine.Vector3",
          "description": "In tangent (relative to the knot position)."
        },
        "tangentOut": {
          "$ref": "#/$defs/UnityEngine.Vector3",
          "description": "Out tangent (relative to the knot position)."
        },
        "rotationEuler": {
          "$ref": "#/$defs/UnityEngine.Vector3",
          "description": "Knot rotation in euler angles (degrees)."
        },
        "tangentMode": {
          "type": "string",
          "description": "Tangent mode of the knot."
        }
      },
      "required": [
        "index",
        "position",
        "tangentIn",
        "tangentOut",
        "rotationEuler"
      ]
    },
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
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Splines-GetKnotsResponse": {
      "type": "object",
      "properties": {
        "gameObjectRef": {
          "$ref": "#/$defs/AIGD.GameObjectRef",
          "description": "Reference to the SplineContainer GameObject."
        },
        "containerRef": {
          "$ref": "#/$defs/AIGD.ComponentRef",
          "description": "Reference to the SplineContainer component."
        },
        "splineIndex": {
          "type": "integer",
          "description": "Index of the spline in the container."
        },
        "closed": {
          "type": "boolean",
          "description": "Whether the spline is a closed loop."
        },
        "knotCount": {
          "type": "integer",
          "description": "Number of knots in the spline."
        },
        "knots": {
          "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Splines-KnotInfo-1",
          "description": "Per-knot summaries."
        }
      },
      "required": [
        "splineIndex",
        "closed",
        "knotCount"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

