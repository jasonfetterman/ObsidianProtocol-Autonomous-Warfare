---
name: probuilder-connect-edges
description: Insert new edges connecting the midpoints of selected edges within faces of a `ProBuilderMesh` — adds edge loops and extra geometry detail. Supply either `edges` (explicit list) or `faceDirection` (semantic selection); exactly one is required.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Connect edges in a ProBuilder mesh

Insert new edges connecting the midpoints of selected edges within faces of a `ProBuilderMesh`. When a face has more than two edges to connect, a center vertex is added. Useful for creating new edge loops and adding geometry detail.

## Inputs

- `gameObjectRef` — the GameObject hosting the `ProBuilderMesh` component.
- `edges` — explicit list of edges to connect, each as `[vertexA, vertexB]`. Use 'probuilder-get-mesh-info' to discover valid indices.
- `faceDirection` — semantic alternative: connects all edges of faces pointing this direction (`Up`, `Down`, `Left`, `Right`, `Forward`, `Back`). Exactly one of `edges` / `faceDirection` is required.

## Examples

- Connect opposite edges of the top face: `faceDirection="up"`.
- Connect specific edges: `edges=[[0,1], [2,3]]`.

## How to Call

```bash
unity-mcp-cli run-tool probuilder-connect-edges --input '{
  "gameObjectRef": "string_value",
  "edges": "string_value",
  "faceDirection": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool probuilder-connect-edges --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool probuilder-connect-edges --input-file - <<'EOF'
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
| `edges` | `any` | No | Array of edge definitions. Each edge is [vertexA, vertexB]. Use ProBuilder_GetMeshInfo to get vertex indices. |
| `faceDirection` | `any` | No | Semantic face selection - connect edges of faces facing this direction. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "edges": {
      "$ref": "#/$defs/System.Int32-1-1"
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
    "System.Int32-1-1": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/System.Int32-1"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-ConnectEdgesResponse"
    }
  },
  "$defs": {
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-ConnectEdgesResponse": {
      "type": "object",
      "properties": {
        "selectionMethod": {
          "type": "string"
        },
        "edgesConnected": {
          "type": "integer"
        },
        "newFacesCreated": {
          "type": "integer"
        },
        "newEdgesCreated": {
          "type": "integer"
        },
        "faceCountBefore": {
          "type": "integer"
        },
        "faceCountAfter": {
          "type": "integer"
        },
        "facesAdded": {
          "type": "integer"
        },
        "edgeCountBefore": {
          "type": "integer"
        },
        "edgeCountAfter": {
          "type": "integer"
        },
        "edgesAdded": {
          "type": "integer"
        },
        "totalVertexCount": {
          "type": "integer"
        }
      },
      "required": [
        "edgesConnected",
        "newFacesCreated",
        "newEdgesCreated",
        "faceCountBefore",
        "faceCountAfter",
        "facesAdded",
        "edgeCountBefore",
        "edgeCountAfter",
        "edgesAdded",
        "totalVertexCount"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

