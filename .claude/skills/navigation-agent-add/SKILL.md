---
name: navigation-agent-add
description: Add and configure a `NavMeshAgent` (from the built-in `com.unity.modules.ai`) on a GameObject — or create a new GameObject to host it. Configure agent type, speed, acceleration, angular speed, stopping distance, and the area mask. Returns the GameObject reference and instanceId.
metadata:
  generated-by: mcp-plugin-dotnet
---

# Navigation / Add NavMeshAgent

Add a `NavMeshAgent` to a GameObject so it can pathfind across a baked NavMesh. Use 'navigation-agent-set-destination' to move it.

## Inputs

- `gameObjectRef` — optional GameObject to add the agent to. When omitted, a new GameObject is created.
- `name` — optional name for the new GameObject (default `NavMeshAgent`); ignored when `gameObjectRef` is given.
- `agentTypeId` — optional NavMesh agent-type id (default 0 = Humanoid).
- `speed` — optional max movement speed (default 3.5).
- `acceleration` — optional max acceleration (default 8).
- `angularSpeed` — optional max turning speed in deg/s (default 120).
- `stoppingDistance` — optional stop distance from the destination (default 0).
- `areaMask` — optional traversable-area bitmask (default -1 = all areas).

## Behavior

Adds (or reuses) a `NavMeshAgent`, assigns the configured properties, marks the scene dirty, repaints, and returns the GameObject reference, the agent component reference, and the instanceId. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool navigation-agent-add --input '{
  "gameObjectRef": "string_value",
  "name": "string_value",
  "agentTypeId": 0,
  "speed": 0,
  "acceleration": 0,
  "angularSpeed": 0,
  "stoppingDistance": 0,
  "areaMask": 0
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool navigation-agent-add --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool navigation-agent-add --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `gameObjectRef` | `any` | No | Optional GameObject to add the NavMeshAgent to. When omitted, a new GameObject is created. |
| `name` | `string` | No | Name for the new GameObject (ignored when gameObjectRef is provided). |
| `agentTypeId` | `integer` | No | NavMesh agent-type id (0 = Humanoid by default). |
| `speed` | `number` | No | Maximum movement speed. |
| `acceleration` | `number` | No | Maximum acceleration. |
| `angularSpeed` | `number` | No | Maximum turning speed in degrees per second. |
| `stoppingDistance` | `number` | No | Distance from the destination at which the agent stops. |
| `areaMask` | `integer` | No | Traversable-area bitmask (-1 = all areas). |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "name": {
      "type": "string"
    },
    "agentTypeId": {
      "type": "integer"
    },
    "speed": {
      "type": "number"
    },
    "acceleration": {
      "type": "number"
    },
    "angularSpeed": {
      "type": "number"
    },
    "stoppingDistance": {
      "type": "number"
    },
    "areaMask": {
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
  }
}
```

## Output

### Output JSON Schema

```json
{
  "type": "object",
  "properties": {
    "result": {
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Navigation-AgentAddResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Navigation-AgentAddResponse": {
      "type": "object",
      "properties": {
        "gameObjectRef": {
          "$ref": "#/$defs/AIGD.GameObjectRef",
          "description": "Reference to the NavMeshAgent GameObject."
        },
        "agentRef": {
          "$ref": "#/$defs/AIGD.ComponentRef",
          "description": "Reference to the NavMeshAgent component."
        },
        "instanceId": {
          "$ref": "#/$defs/UnityEngine.EntityId",
          "description": "Instance id of the GameObject hosting the agent."
        },
        "gameObjectName": {
          "type": "string",
          "description": "Name of the GameObject."
        },
        "agentTypeId": {
          "type": "integer",
          "description": "Resolved agent-type id."
        },
        "speed": {
          "type": "number",
          "description": "Resolved speed."
        },
        "acceleration": {
          "type": "number",
          "description": "Resolved acceleration."
        },
        "stoppingDistance": {
          "type": "number",
          "description": "Resolved stopping distance."
        },
        "areaMask": {
          "type": "integer",
          "description": "Resolved area mask."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "instanceId",
        "agentTypeId",
        "speed",
        "acceleration",
        "stoppingDistance",
        "areaMask",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

