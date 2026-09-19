---
name: probuilder-create-poly-shape
description: Create a 3D `ProBuilderMesh` from a 2D polygon outline (x,z points) extruded upward by `height`. Perfect for floor plans, room layouts, terrain patches, walls, platforms — any shape definable by a 2D outline. Requires ≥3 points.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Create a ProBuilder shape from polygon points

Create a 3D `ProBuilderMesh` from a 2D polygon outline (x,z points) extruded upward by `height`. Perfect for:

- Floor plans and room layouts.
- Custom terrain patches.
- Architectural elements (walls, platforms).
- Any shape that can be defined by a 2D outline.

## Inputs

- `points` — array of 2D points as `[x, z]`. Minimum 3 points; should be in clockwise or counter-clockwise order.
- `height` — extrusion height upward (Y axis). Default `1`.
- `name` — optional GameObject name.
- `parentGameObjectRef` — optional parent; root of the scene when omitted.
- `position`, `rotation` — optional `Vector3` transform values (rotation in euler degrees).
- `flipNormals` — when `true`, flip the resulting face normals so the shape points inward.
- `isLocalSpace` — when `true`, position/rotation are interpreted in the parent's local space.

## Examples

- Rectangle: `points=[[0,0], [4,0], [4,3], [0,3]]`, `height=2.5`.
- L-shape: `points=[[0,0], [3,0], [3,2], [1,2], [1,3], [0,3]]`, `height=3`.
- Triangle: `points=[[0,0], [2,0], [1,1.7]]`, `height=1`.

## How to Call

```bash
unity-mcp-cli run-tool probuilder-create-poly-shape --input '{
  "points": "string_value",
  "height": 0,
  "name": "string_value",
  "parentGameObjectRef": "string_value",
  "position": "string_value",
  "rotation": "string_value",
  "flipNormals": false,
  "isLocalSpace": false
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool probuilder-create-poly-shape --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool probuilder-create-poly-shape --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `points` | `any` | Yes | 2D polygon points as [x,z] coordinates. Minimum 3 points. Points should be in clockwise or counter-clockwise order. Example: [[0,0], [4,0], [4,3], [0,3]] creates a 4x3 rectangle. |
| `height` | `number` | No | Height to extrude the polygon upward. Default is 1. |
| `name` | `string` | No | Name of the new GameObject. |
| `parentGameObjectRef` | `any` | No | Parent GameObject reference. If not provided, the shape will be created at the root of the scene. |
| `position` | `any` | No | Position of the shape in world or local space. |
| `rotation` | `any` | No | Rotation of the shape in euler angles (degrees). |
| `flipNormals` | `boolean` | No | If true, flip the normals so the faces point inward instead of outward. |
| `isLocalSpace` | `boolean` | No | If true, position/rotation are in local space relative to parent. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "points": {
      "$ref": "#/$defs/System.Single-1-1"
    },
    "height": {
      "type": "number"
    },
    "name": {
      "type": "string"
    },
    "parentGameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "position": {
      "$ref": "#/$defs/UnityEngine.Vector3"
    },
    "rotation": {
      "$ref": "#/$defs/UnityEngine.Vector3"
    },
    "flipNormals": {
      "type": "boolean"
    },
    "isLocalSpace": {
      "type": "boolean"
    }
  },
  "$defs": {
    "System.Single-1": {
      "type": "array",
      "items": {
        "type": "number"
      }
    },
    "System.Single-1-1": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/System.Single-1"
      }
    },
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
  },
  "required": [
    "points"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-CreatePolyShapeResponse"
    }
  },
  "$defs": {
    "UnityEngine.EntityId": {
      "type": "string",
      "pattern": "^[0-9]+$"
    },
    "System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-PointInfo)": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-PointInfo"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-PointInfo": {
      "type": "object",
      "properties": {
        "index": {
          "type": "integer"
        },
        "x": {
          "type": "number"
        },
        "z": {
          "type": "number"
        }
      },
      "required": [
        "index",
        "x",
        "z"
      ]
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-CreatePolyShapeResponse": {
      "type": "object",
      "properties": {
        "gameObjectName": {
          "type": "string"
        },
        "instanceId": {
          "$ref": "#/$defs/UnityEngine.EntityId"
        },
        "position": {
          "type": "string"
        },
        "rotation": {
          "type": "string"
        },
        "pointCount": {
          "type": "integer"
        },
        "height": {
          "type": "number"
        },
        "flipNormals": {
          "type": "boolean"
        },
        "boundsSize": {
          "type": "string"
        },
        "faceCount": {
          "type": "integer"
        },
        "vertexCount": {
          "type": "integer"
        },
        "edgeCount": {
          "type": "integer"
        },
        "inputPoints": {
          "$ref": "#/$defs/System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-PointInfo)"
        }
      },
      "required": [
        "instanceId",
        "pointCount",
        "height",
        "flipNormals",
        "faceCount",
        "vertexCount",
        "edgeCount"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

