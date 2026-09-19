---
name: probuilder-merge-objects
description: Combine multiple `ProBuilderMesh` GameObjects into one merged mesh. The first GameObject in the list becomes the merge target. Source GameObjects are deleted by default. Useful for optimizing draw calls and unifying composite props.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Merge multiple ProBuilder meshes into one

Combine multiple `ProBuilderMesh` GameObjects into a single merged mesh. The first GameObject in `gameObjectRefs` becomes the merge target — subsequent meshes are absorbed into it. Useful for optimizing draw calls or creating a unified object from parts.

## Inputs

- `gameObjectRefs` — array of GameObject references (≥2). Each must carry a `ProBuilderMesh` component. The first reference is the merge target.
- `deleteSourceObjects` — when `true` (default), delete the non-target source GameObjects after merging. Set `false` to keep them in the scene.

## Example

Merge a table assembled from four leg meshes and a top into a single GameObject for shipping.

## Behavior

All meshes are rebuilt (`ToMesh` → `Refresh`), the resulting GameObject is marked dirty, and the Editor repaints. The whole call runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool probuilder-merge-objects --input '{
  "gameObjectRefs": "string_value",
  "deleteSourceObjects": false
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool probuilder-merge-objects --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool probuilder-merge-objects --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `gameObjectRefs` | `any` | Yes | Array of GameObject references with ProBuilderMesh components to merge. First object becomes the merge target. |
| `deleteSourceObjects` | `boolean` | No | If true, delete the source GameObjects after merging (except the target). Default is true. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRefs": {
      "$ref": "#/$defs/AIGD.GameObjectRef-1"
    },
    "deleteSourceObjects": {
      "type": "boolean"
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
    "AIGD.GameObjectRef-1": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/AIGD.GameObjectRef",
        "description": "Find GameObject in opened Prefab or in the active Scene."
      }
    }
  },
  "required": [
    "gameObjectRefs"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-MergeObjectsResponse"
    }
  },
  "$defs": {
    "UnityEngine.EntityId": {
      "type": "string",
      "pattern": "^[0-9]+$"
    },
    "System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-SourceObjectInfo)": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-SourceObjectInfo"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-SourceObjectInfo": {
      "type": "object",
      "properties": {
        "index": {
          "type": "integer"
        },
        "name": {
          "type": "string"
        },
        "status": {
          "type": "string"
        }
      },
      "required": [
        "index"
      ]
    },
    "System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-AdditionalMeshInfo)": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-AdditionalMeshInfo"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-AdditionalMeshInfo": {
      "type": "object",
      "properties": {
        "name": {
          "type": "string"
        },
        "instanceId": {
          "$ref": "#/$defs/UnityEngine.EntityId"
        }
      },
      "required": [
        "instanceId"
      ]
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-MergeObjectsResponse": {
      "type": "object",
      "properties": {
        "mergedMeshCount": {
          "type": "integer"
        },
        "resultMeshCount": {
          "type": "integer"
        },
        "targetObjectName": {
          "type": "string"
        },
        "targetInstanceId": {
          "$ref": "#/$defs/UnityEngine.EntityId"
        },
        "objectsDeleted": {
          "type": "integer"
        },
        "totalFacesBefore": {
          "type": "integer"
        },
        "totalFacesAfter": {
          "type": "integer"
        },
        "totalVerticesBefore": {
          "type": "integer"
        },
        "totalVerticesAfter": {
          "type": "integer"
        },
        "sourceObjects": {
          "$ref": "#/$defs/System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-SourceObjectInfo)"
        },
        "additionalMeshes": {
          "$ref": "#/$defs/System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-AdditionalMeshInfo)"
        }
      },
      "required": [
        "mergedMeshCount",
        "resultMeshCount",
        "targetInstanceId",
        "objectsDeleted",
        "totalFacesBefore",
        "totalFacesAfter",
        "totalVerticesBefore",
        "totalVerticesAfter"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

