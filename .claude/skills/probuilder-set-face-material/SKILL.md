---
name: probuilder-set-face-material
description: Assign a material to specific faces of a `ProBuilderMesh`, enabling multi-material meshes (e.g., grass on top, dirt on sides). Supply either `faceIndices` (explicit) or `faceDirection` (semantic); exactly one is required.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Set material on ProBuilder faces

Assign a material to specific faces of a `ProBuilderMesh`, enabling multi-material meshes where different faces have different materials (e.g., grass on top, dirt on the sides).

## Inputs

- `gameObjectRef` — the GameObject hosting the `ProBuilderMesh` component.
- `materialPath` — project path to the material asset (e.g., `Assets/Materials/Grass.mat`) or a bare material name. Required.
- `faceIndices` — explicit array of face indices.
- `faceDirection` — semantic alternative (`Up`, `Down`, `Left`, `Right`, `Forward`, `Back`). Exactly one of `faceIndices` / `faceDirection` is required.

## Examples

- Set material on the top face: `faceDirection="up"`.
- Set material on specific faces: `faceIndices=[0, 2, 4]`.

## Behavior

The mesh is rebuilt (`ToMesh` → `Refresh`), dirty-flagged, and the Editor repaints. The whole call runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool probuilder-set-face-material --input '{
  "gameObjectRef": "string_value",
  "materialPath": "string_value",
  "faceIndices": "string_value",
  "faceDirection": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool probuilder-set-face-material --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool probuilder-set-face-material --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `gameObjectRef` | `any` | Yes | Reference to the GameObject with a ProBuilderMesh component. |
| `materialPath` | `string` | Yes | Path to the material asset (e.g., 'Assets/Materials/MyMaterial.mat') or material name. |
| `faceIndices` | `any` | No | Array of face indices to apply the material to. Use this OR faceDirection, not both. Use ProBuilder_GetMeshInfo to get valid face indices. |
| `faceDirection` | `any` | No | Semantic face selection by direction. Use this OR faceIndices, not both. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "materialPath": {
      "type": "string"
    },
    "faceIndices": {
      "$ref": "#/$defs/System.Int32-1"
    },
    "faceDirection": {
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.FaceDirection"
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
    "System.Int32-1": {
      "type": "array",
      "items": {
        "type": "integer"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.FaceDirection": {
      "type": "string",
      "enum": [
        "Up",
        "Down",
        "Left",
        "Right",
        "Forward",
        "Back"
      ]
    }
  },
  "required": [
    "gameObjectRef",
    "materialPath"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-SetFaceMaterialResponse"
    }
  },
  "$defs": {
    "System.Int32-1": {
      "type": "array",
      "items": {
        "type": "integer"
      }
    },
    "System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-MaterialInfo)": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-MaterialInfo"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-MaterialInfo": {
      "type": "object",
      "properties": {
        "index": {
          "type": "integer"
        },
        "name": {
          "type": "string"
        }
      },
      "required": [
        "index"
      ]
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-SetFaceMaterialResponse": {
      "type": "object",
      "properties": {
        "materialName": {
          "type": "string"
        },
        "materialIndex": {
          "type": "integer"
        },
        "selectionMethod": {
          "type": "string"
        },
        "facesUpdated": {
          "$ref": "#/$defs/System.Int32-1"
        },
        "meshMaterials": {
          "$ref": "#/$defs/System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-MaterialInfo)"
        }
      },
      "required": [
        "materialIndex"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

