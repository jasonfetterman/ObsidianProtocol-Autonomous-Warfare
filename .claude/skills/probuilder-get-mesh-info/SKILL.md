---
name: probuilder-get-mesh-info
description: "Inspect a `ProBuilderMesh` — face/vertex/edge counts plus optional face-by-face detail. Use `detail=\"summary\"` for a token-efficient overview, `detail=\"full\"` for detailed face data. Often skippable when other tools accept `faceDirection` semantic selection."
metadata:
  generated-by: mcp-plugin-dotnet
---

# Get ProBuilder mesh information

Inspect a `ProBuilderMesh` — totals (face/vertex/edge counts) plus optional face-by-face detail (per-face vertex positions, edges, semantic direction). The other ProBuilder tools accept `faceDirection` (semantic selection like `"up"`, `"down"`) — when that suffices, you can skip this call entirely.

## Inputs

- `gameObjectRef` — the GameObject hosting the `ProBuilderMesh` component.
- `detail` — `Summary` (default, cheap overview with face directions) or `Full` (per-face data).
- `includeVertexPositions` — `Full` only. Include each face's vertex positions in the response.
- `includeEdges` — `Full` only. Include each face's edges. Default `true`.
- `maxFacesToShow` — `Full` only. Cap on face-detail entries (default `20`; pass `-1` for all). Keeps the response small for high-face meshes.

## Tip

With semantic face selection (`faceDirection`) available on Extrude / DeleteFaces / SetFaceMaterial and friends, you often don't need this tool — just pass `faceDirection="up"` etc. directly.

## How to Call

```bash
unity-mcp-cli run-tool probuilder-get-mesh-info --input '{
  "gameObjectRef": "string_value",
  "detail": "string_value",
  "includeVertexPositions": false,
  "includeEdges": false,
  "maxFacesToShow": 0
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool probuilder-get-mesh-info --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool probuilder-get-mesh-info --input-file - <<'EOF'
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
| `detail` | `string` | No | Detail level for output. |
| `includeVertexPositions` | `boolean` | No | If true, includes detailed vertex positions for each face (only with detail='full'). |
| `includeEdges` | `boolean` | No | If true, includes edge information for each face (only with detail='full'). |
| `maxFacesToShow` | `integer` | No | Maximum number of faces to include in detail (only with detail='full'). Use -1 for all faces. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "detail": {
      "type": "string",
      "enum": [
        "Summary",
        "Full"
      ]
    },
    "includeVertexPositions": {
      "type": "boolean"
    },
    "includeEdges": {
      "type": "boolean"
    },
    "maxFacesToShow": {
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-GetMeshInfoResponse"
    }
  },
  "$defs": {
    "UnityEngine.EntityId": {
      "type": "string",
      "pattern": "^[0-9]+$"
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-BoundsInfo": {
      "type": "object",
      "properties": {
        "center": {
          "type": "string"
        },
        "size": {
          "type": "string"
        },
        "min": {
          "type": "string"
        },
        "max": {
          "type": "string"
        }
      }
    },
    "System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-FaceDirectionInfo)": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-FaceDirectionInfo"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-FaceDirectionInfo": {
      "type": "object",
      "properties": {
        "direction": {
          "type": "string"
        },
        "faceIndices": {
          "$ref": "#/$defs/System.Int32-1"
        },
        "firstFaceCenter": {
          "type": "string"
        }
      }
    },
    "System.Int32-1": {
      "type": "array",
      "items": {
        "type": "integer"
      }
    },
    "System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-FaceInfo)": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-FaceInfo"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-FaceInfo": {
      "type": "object",
      "properties": {
        "index": {
          "type": "integer"
        },
        "vertexCount": {
          "type": "integer"
        },
        "triangleCount": {
          "type": "integer"
        },
        "center": {
          "type": "string"
        },
        "vertices": {
          "$ref": "#/$defs/System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-VertexInfo)"
        },
        "edges": {
          "$ref": "#/$defs/System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-EdgeInfo)"
        }
      },
      "required": [
        "index",
        "vertexCount",
        "triangleCount"
      ]
    },
    "System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-VertexInfo)": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-VertexInfo"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-VertexInfo": {
      "type": "object",
      "properties": {
        "index": {
          "type": "integer"
        },
        "position": {
          "type": "string"
        }
      },
      "required": [
        "index"
      ]
    },
    "System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-EdgeInfo)": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-EdgeInfo"
      }
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-EdgeInfo": {
      "type": "object",
      "properties": {
        "vertexA": {
          "type": "integer"
        },
        "vertexB": {
          "type": "integer"
        },
        "positionA": {
          "type": "string"
        },
        "positionB": {
          "type": "string"
        }
      },
      "required": [
        "vertexA",
        "vertexB"
      ]
    },
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-GetMeshInfoResponse": {
      "type": "object",
      "properties": {
        "gameObjectName": {
          "type": "string"
        },
        "instanceId": {
          "$ref": "#/$defs/UnityEngine.EntityId"
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
        "triangleCount": {
          "type": "integer"
        },
        "bounds": {
          "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-BoundsInfo"
        },
        "faceDirections": {
          "$ref": "#/$defs/System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-FaceDirectionInfo)"
        },
        "faces": {
          "$ref": "#/$defs/System.Collections.Generic.List(com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-FaceInfo)"
        },
        "facesShown": {
          "type": "integer"
        },
        "facesTotal": {
          "type": "integer"
        },
        "uniqueEdgeCount": {
          "type": "integer"
        }
      },
      "required": [
        "instanceId",
        "faceCount",
        "vertexCount",
        "edgeCount",
        "triangleCount"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

