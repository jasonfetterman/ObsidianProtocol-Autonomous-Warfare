---
name: navigation-agent-set-destination
description: Set the destination of a `NavMeshAgent` so it pathfinds toward a world-space point. Optionally provide a target GameObject instead of explicit coordinates. The agent must be on a baked NavMesh for a path to be computed (most relevant in play mode).
metadata:
  generated-by: mcp-plugin-dotnet
---

# Navigation / Set Agent Destination

Set a `NavMeshAgent`'s destination. The agent computes a path across the baked NavMesh and begins moving toward the destination (path computation/movement happens in play mode; in edit mode the destination is still assigned).

## Inputs

- `gameObjectRef` — the GameObject hosting the `NavMeshAgent` (required).
- `destination` — the world-space destination point. Ignored when `targetRef` is provided.
- `targetRef` — optional GameObject whose position is used as the destination.

## Behavior

Resolves the agent, computes the destination (from `targetRef` if given, else `destination`), and — when the agent is active and placed on a NavMesh — calls `SetDestination`. Off the NavMesh (e.g. before baking, in edit mode) it reports the intended destination without mutating, so the call is always safe. Returns `accepted` plus the agent's `hasPath` / `pathPending` state. Runs on the Unity main thread.

## How to Call

```bash
unity-mcp-cli run-tool navigation-agent-set-destination --input '{
  "gameObjectRef": "string_value",
  "destination": "string_value",
  "targetRef": "string_value"
}'
```

> For complex input (multi-line strings, code), save the JSON to a file and use:
> ```bash
> unity-mcp-cli run-tool navigation-agent-set-destination --input-file args.json
> ```
>
> Or pipe via stdin (recommended):
> ```bash
> unity-mcp-cli run-tool navigation-agent-set-destination --input-file - <<'EOF'
> {"param": "value"}
> EOF
> ```


### Troubleshooting

If `unity-mcp-cli` is not found, either install it globally (`npm install -g unity-mcp-cli`) or use `npx unity-mcp-cli` instead.
Read the /unity-initial-setup skill for detailed installation instructions.

## Input

| Name | Type | Required | Description |
|------|------|----------|-------------|
| `gameObjectRef` | `any` | Yes | Reference to the GameObject containing the NavMeshAgent component. |
| `destination` | `any` | No | World-space destination point (ignored when targetRef is provided). |
| `targetRef` | `any` | No | Optional GameObject whose position is used as the destination. |

### Input JSON Schema

```json
{
  "type": "object",
  "properties": {
    "gameObjectRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
    },
    "destination": {
      "$ref": "#/$defs/UnityEngine.Vector3"
    },
    "targetRef": {
      "$ref": "#/$defs/AIGD.GameObjectRef"
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
      "$ref": "#/$defs/com.IvanMurzak.Unity.MCP.Editor.API.Tool_Navigation-AgentSetDestinationResponse"
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
    "com.IvanMurzak.Unity.MCP.Editor.API.Tool_Navigation-AgentSetDestinationResponse": {
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
        "destination": {
          "$ref": "#/$defs/UnityEngine.Vector3",
          "description": "The resolved destination point."
        },
        "accepted": {
          "type": "boolean",
          "description": "Whether SetDestination accepted the request (false if the agent is off the NavMesh)."
        },
        "isOnNavMesh": {
          "type": "boolean",
          "description": "Whether the agent is currently on a baked NavMesh."
        },
        "hasPath": {
          "type": "boolean",
          "description": "Whether the agent has a computed path."
        },
        "pathPending": {
          "type": "boolean",
          "description": "Whether a path is still being computed."
        },
        "success": {
          "type": "boolean",
          "description": "Whether the operation succeeded."
        }
      },
      "required": [
        "destination",
        "accepted",
        "isOnNavMesh",
        "hasPath",
        "pathPending",
        "success"
      ]
    }
  },
  "required": [
    "result"
  ]
}
```

