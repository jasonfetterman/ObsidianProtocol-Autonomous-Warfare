using UnityEditor;
using UnityEngine;

public static class DeploymentPlayerControllerAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Player Controller")]
    public static void Audit()
    {
        GameObject player = GameObject.Find("[PLAYER] COMMANDER");

        if (player == null)
        {
            Debug.LogError("[PLAYER CONTROLLER] PLAYER NOT FOUND");
            return;
        }

        CharacterController cc =
            player.GetComponent<CharacterController>();

        if (cc != null)
        {
            Debug.Log(
                "[PLAYER CONTROLLER] CHARACTER CONTROLLER" +
                " | Height=" + cc.height +
                " | Radius=" + cc.radius +
                " | Center=" + cc.center +
                " | SkinWidth=" + cc.skinWidth +
                " | StepOffset=" + cc.stepOffset
            );
        }

        CommanderPlayerMovement movement =
            player.GetComponent<CommanderPlayerMovement>();

        if (movement != null)
        {
            SerializedObject so = new SerializedObject(movement);
            SerializedProperty prop = so.GetIterator();

            while (prop.NextVisible(true))
            {
                if (prop.name == "m_Script")
                    continue;

                Debug.Log(
                    "[PLAYER CONTROLLER] MOVEMENT PROPERTY" +
                    " | " + prop.name +
                    " = " + prop.displayName +
                    " | Value=" + PropertyValue(prop)
                );
            }
        }
    }

    private static string PropertyValue(SerializedProperty prop)
    {
        switch (prop.propertyType)
        {
            case SerializedPropertyType.Float:
                return prop.floatValue.ToString();

            case SerializedPropertyType.Integer:
                return prop.intValue.ToString();

            case SerializedPropertyType.Boolean:
                return prop.boolValue.ToString();

            case SerializedPropertyType.Vector2:
                return prop.vector2Value.ToString();

            case SerializedPropertyType.Vector3:
                return prop.vector3Value.ToString();

            case SerializedPropertyType.Enum:
                return prop.enumDisplayNames[prop.enumValueIndex];

            case SerializedPropertyType.ObjectReference:
                return prop.objectReferenceValue != null
                    ? prop.objectReferenceValue.name
                    : "NULL";

            default:
                return "(complex)";
        }
    }
}
