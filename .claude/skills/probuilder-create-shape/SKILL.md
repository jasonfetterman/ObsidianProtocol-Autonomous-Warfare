---
name: probuilder-create-shape
description: Create a new editable `ProBuilderMesh` GameObject in the active scene from a `ShapeType` primitive (Cube, Cylinder, Sphere, Plane, Prism, Cone, Stair, etc.). Optionally set name, parent, transform, size, and world/local space.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Create a ProBuilder shape

Create a new editable `ProBuilderMesh` GameObject in the active scene. ProBuilder shapes are editable 3D meshes that can be further modified with the rest of the ProBuilder tools (extrude, bevel, subdivide, etc.).

## Inputs

- `shapeType` — `ShapeType` enum (Cube, Cylinder, Sphere, Plane, Prism, Cone, Stair, Door, Pipe, Arch, Sprite, Torus, etc.).
- `name` — optional GameObject name.
- `parentGameObjectRef` — optional parent; root of the scene when omitted.
- `position`, `rotation`, `scale` — optional `Vector3` transform values. Rotation is euler degrees. Defaults: zero / zero / one.
- `size` — `Vector3` width/height/depth of the generated shape. Default `(1, 1, 1)`.
- `isLocalSpace` — when `true`, position/rotation/scale are interpreted in the parent's local space; otherwise world space.

## Behavior

Uses `ShapeGenerator.CreateShape` with `PivotLocation.Center`, applies the transform values, rebuilds the mesh, marks dirty, and repaints the Editor. The whole call runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool probuilder-create-shape --input '{
  "shapeType": "string_value",
  "name": "string_value",
  "parentGameObjectRef": "string_value",
  "position": "string_value",
  "rotation": "string_value",
  "scale": "string_value",
  "size": "string_value",
  "isLocalSpace": false
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool probuilder-create-shape --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool probuilder-create-shape --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `shapeType` | `string` | Yes | The type of shape to create. |
| `name` | `string` | No | Name of the new GameObject. |
| `parentGameObjectRef` | `any` | No | Parent GameObject reference. If not provided, the shape will be created at the root of the scene. |
| `position` | `any` | No | Position of the shape in world or local space. |
| `rotation` | `any` | No | Rotation of the shape in euler angles (degrees). |
| `scale` | `any` | No | Scale of the shape. |
| `size` | `any` | No | Size of the shape (width, height, depth). Default is (1, 1, 1). |
| `isLocalSpace` | `boolean` | No | If true, position/rotation/scale are in local space relative to parent. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "shapeType": {
      "type": "string",
      "enum": [
        "Cube",
        "Stair",
        "CurvedStair",
        "Prism",
        "Cylinder",
        "Plane",
        "Door",
        "Pipe",
        "Cone",
        "Sprite",
        "Arch",
        "Sphere",
        "Torus"
      ]
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
    "scale": {
      "$ref": "#/$defs/UnityEngine.Vector3"
    },
    "size": {
      "$ref": "#/$defs/UnityEngine.Vector3"
    },
    "isLocalSpace": {
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
    "shapeType"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-CreateShapeResponse"
    }
  },
  "$defs": {
    "UnityEngine.EntityId": {
      "type": "string",
      "pattern": "^[0-9]+$"
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-CreateShapeResponse": {
      "type": "object",
      "properties": {
        "gameObjectName": {
          "type": "string"
        },
        "instanceId": {
          "$ref": "#/$defs/UnityEngine.EntityId"
        },
        "shapeType": {
          "type": "string"
        },
        "position": {
          "type": "string"
        },
        "rotation": {
          "type": "string"
        },
        "scale": {
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
        }
      },
      "required": [
        "instanceId",
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

