---
name: navigation-link-add
description: Add and configure a `NavMeshLink` on a GameObject. A NavMeshLink connects two points on (or off) the NavMesh — e.g. a jump-across or a doorway — so agents can traverse gaps the baked mesh does not cover. Points are local to the GameObject's transform.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Navigation / Add NavMeshLink

Add a `NavMeshLink` to a GameObject. The link bridges `startPoint` and `endPoint` (in the GameObject's local space), letting agents cross between otherwise-disconnected NavMesh regions.

## Inputs

- `gameObjectRef` — the GameObject to add the link to (required).
- `startPoint` — local-space start of the link (default 0,0,-2.5).
- `endPoint` — local-space end of the link (default 0,0,2.5).
- `width` — link width in meters (default 0).
- `bidirectional` — when `true`, the link is traversable both ways (default true).
- `costModifier` — cost override; negative means use the default cost (default -1).
- `area` — NavMesh area index of the link (default 2 = Jump).

## Behavior

Adds (or reuses) a `NavMeshLink`, assigns the configured fields (which auto-rebuild the link), marks the scene dirty, and repaints. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool navigation-link-add --input '{
  "gameObjectRef": "string_value",
  "startPoint": "string_value",
  "endPoint": "string_value",
  "width": 0,
  "bidirectional": false,
  "costModifier": 0,
  "area": 0
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool navigation-link-add --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool navigation-link-add --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `gameObjectRef` | `any` | Yes | Reference to the GameObject to add the NavMeshLink to. |
| `startPoint` | `any` | No | Local-space start point of the link. |
| `endPoint` | `any` | No | Local-space end point of the link. |
| `width` | `number` | No | Link width in meters. |
| `bidirectional` | `boolean` | No | If true, the link is traversable in both directions. |
| `costModifier` | `integer` | No | Cost modifier for traversing the link; negative uses the default cost. |
| `area` | `integer` | No | NavMesh area index of the link (2 = Jump by default). |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "startPoint": {
      "$ref": "#/$defs/UnityEngine.Vector3"
    },
    "endPoint": {
      "$ref": "#/$defs/UnityEngine.Vector3"
    },
    "width": {
      "type": "number"
    },
    "bidirectional": {
      "type": "boolean"
    },
    "costModifier": {
      "type": "integer"
    },
    "area": {
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Navigation-LinkAddResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Navigation-LinkAddResponse": {
      "type": "object",
      "properties": {
        "gameObjectRef": {
          "$ref": "#/$defs/AIGD.GameObjectRef",
          "description": "Reference to the GameObject hosting the link."
        },
        "linkRef": {
          "$ref": "#/$defs/AIGD.ComponentRef",
          "description": "Reference to the NavMeshLink component."
        },
        "startPoint": {
          "$ref": "#/$defs/UnityEngine.Vector3",
          "description": "Resolved local start point."
        },
        "endPoint": {
          "$ref": "#/$defs/UnityEngine.Vector3",
          "description": "Resolved local end point."
        },
        "width": {
          "type": "number",
          "description": "Resolved width."
        },
        "bidirectional": {
          "type": "boolean",
          "description": "Resolved bidirectional flag."
        },
        "area": {
          "type": "integer",
          "description": "Resolved area index."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "startPoint",
        "endPoint",
        "width",
        "bidirectional",
        "area",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

