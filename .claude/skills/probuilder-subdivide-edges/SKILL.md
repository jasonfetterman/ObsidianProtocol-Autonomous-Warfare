---
name: probuilder-subdivide-edges
description: Insert new vertices on selected edges of a `ProBuilderMesh`, splitting each edge into smaller segments. Supply either `edges` (explicit list) or `faceDirection` (subdivides all edges of faces facing that direction); exactly one is required.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Subdivide edges in a ProBuilder mesh

Insert new vertices on selected edges of a `ProBuilderMesh`, subdividing each into smaller segments. Useful for adding detail to specific edges for further manipulation (extrude, bevel, etc.).

## Inputs

- `gameObjectRef` — the GameObject hosting the `ProBuilderMesh` component.
- `edges` — explicit list of edges to subdivide, each as `[vertexA, vertexB]`. Use 'probuilder-get-mesh-info' to discover valid edges.
- `faceDirection` — semantic alternative: subdivides all edges of faces pointing this direction (`Up`, `Down`, `Left`, `Right`, `Forward`, `Back`). Exactly one of `edges` / `faceDirection` is required.
- `subdivisions` — number of subdivisions per edge. `1` splits each edge in half, `2` into thirds, and so on. Default `1`.

## Examples

- Subdivide all edges of the top face: `faceDirection="up"`, `subdivisions=2`.
- Subdivide specific edges: `edges=[[0,1], [2,3]]`, `subdivisions=1`.

## How to Call

```bash
unity-mcp-cli run-tool probuilder-subdivide-edges --input '{
  "gameObjectRef": "string_value",
  "edges": "string_value",
  "faceDirection": "string_value",
  "subdivisions": 0
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool probuilder-subdivide-edges --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool probuilder-subdivide-edges --input-file - <<'EOF'
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
| `faceDirection` | `any` | No | Semantic face selection - subdivide all edges of faces facing this direction. |
| `subdivisions` | `integer` | No | Number of subdivisions per edge. 1 = splits edge in half, 2 = splits into thirds, etc. Default is 1. |

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
    },
    "subdivisions": {
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-SubdivideEdgesResponse"
    }
  },
  "$defs": {
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_ProBuilder-SubdivideEdgesResponse": {
      "type": "object",
      "properties": {
        "selectionMethod": {
          "type": "string"
        },
        "edgesSubdivided": {
          "type": "integer"
        },
        "subdivisionsPerEdge": {
          "type": "integer"
        },
        "newEdgesCreated": {
          "type": "integer"
        },
        "vertexCountBefore": {
          "type": "integer"
        },
        "vertexCountAfter": {
          "type": "integer"
        },
        "verticesAdded": {
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
        "totalFaceCount": {
          "type": "integer"
        }
      },
      "required": [
        "edgesSubdivided",
        "subdivisionsPerEdge",
        "newEdgesCreated",
        "vertexCountBefore",
        "vertexCountAfter",
        "verticesAdded",
        "edgeCountBefore",
        "edgeCountAfter",
        "edgesAdded",
        "totalFaceCount"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

