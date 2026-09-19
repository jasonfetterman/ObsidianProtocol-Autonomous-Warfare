---
name: navigation-modifier-add
description: Add and configure a `NavMeshModifier` on a GameObject. A NavMeshModifier overrides how that object (and optionally its children) is treated during baking — its NavMesh area, whether it is ignored, and link generation.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Navigation / Add NavMeshModifier

Add a `NavMeshModifier` to a GameObject so its geometry is baked with overridden parameters.

## Inputs

- `gameObjectRef` — the GameObject to add the modifier to (required).
- `overrideArea` — when `true`, the object's NavMesh area is overridden with `area`.
- `area` — the NavMesh area index to assign when `overrideArea` is true (default 0 = Walkable).
- `ignoreFromBuild` — when `true`, the object is excluded from the bake entirely.
- `applyToChildren` — when `true`, the override also applies to child objects.

## Behavior

Adds (or reuses) a `NavMeshModifier`, assigns the configured flags, marks the scene dirty, and repaints. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool navigation-modifier-add --input '{
  "gameObjectRef": "string_value",
  "overrideArea": false,
  "area": 0,
  "ignoreFromBuild": false,
  "applyToChildren": false
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool navigation-modifier-add --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool navigation-modifier-add --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `gameObjectRef` | `any` | Yes | Reference to the GameObject to add the NavMeshModifier to. |
| `overrideArea` | `boolean` | No | If true, override the NavMesh area of this object with 'area'. |
| `area` | `integer` | No | NavMesh area index to assign when overrideArea is true (0 = Walkable). |
| `ignoreFromBuild` | `boolean` | No | If true, exclude this object from the NavMesh bake. |
| `applyToChildren` | `boolean` | No | If true, apply the override to child objects as well. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "overrideArea": {
      "type": "boolean"
    },
    "area": {
      "type": "integer"
    },
    "ignoreFromBuild": {
      "type": "boolean"
    },
    "applyToChildren": {
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Navigation-ModifierAddResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Navigation-ModifierAddResponse": {
      "type": "object",
      "properties": {
        "gameObjectRef": {
          "$ref": "#/$defs/AIGD.GameObjectRef",
          "description": "Reference to the GameObject hosting the modifier."
        },
        "modifierRef": {
          "$ref": "#/$defs/AIGD.ComponentRef",
          "description": "Reference to the NavMeshModifier component."
        },
        "overrideArea": {
          "type": "boolean",
          "description": "Resolved overrideArea flag."
        },
        "area": {
          "type": "integer",
          "description": "Resolved area index."
        },
        "ignoreFromBuild": {
          "type": "boolean",
          "description": "Resolved ignoreFromBuild flag."
        },
        "applyToChildren": {
          "type": "boolean",
          "description": "Resolved applyToChildren flag."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "overrideArea",
        "area",
        "ignoreFromBuild",
        "applyToChildren",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

