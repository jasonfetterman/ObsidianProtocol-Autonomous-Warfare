using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

[CustomEditor(typeof(UnitTakeoverController))]
public sealed class UnitTakeoverCameraSceneHandle : Editor
{
    private void OnSceneGUI()
    {
        var controller = (UnitTakeoverController)target;
        Transform drone = controller.ControlledUnit;

        if (drone == null)
        {
            MonoBehaviour[] behaviours =
                Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

            float nearest = float.MaxValue;

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (!(behaviour is IUnitManualControl))
                    continue;

                float distance = Vector3.Distance(
                    controller.transform.position,
                    behaviour.transform.position);

                if (distance < nearest)
                {
                    nearest = distance;
                    drone = behaviour.transform;
                }
            }
        }

        if (drone == null)
        {
            Handles.Label(
                controller.transform.position + Vector3.up * 2f,
                "TAKEOVER CAMERA: No compatible unit found");
            return;
        }

        Vector3 position = drone.TransformPoint(controller.CameraOffset);
        Quaternion rotation = drone.rotation * controller.CameraRotationOffset;

        Handles.color = Color.cyan;
        Handles.DrawLine(drone.position, position);
        Handles.Label(position + Vector3.up * 0.2f, "DRONE TAKEOVER CAMERA");

        EditorGUI.BeginChangeCheck();
        Vector3 moved = Handles.PositionHandle(position, drone.rotation);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(controller, "Move Drone Takeover Camera");
            controller.CameraOffset = drone.InverseTransformPoint(moved);
            EditorUtility.SetDirty(controller);
        }

        EditorGUI.BeginChangeCheck();
        Quaternion rotated = Handles.RotationHandle(rotation, position);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(controller, "Rotate Drone Takeover Camera");
            controller.CameraRotationOffset =
                Quaternion.Inverse(drone.rotation) * rotated;
            EditorUtility.SetDirty(controller);
        }

        Handles.color = Color.yellow;
        Handles.ArrowHandleCap(
            0,
            position,
            rotation,
            HandleUtility.GetHandleSize(position),
            EventType.Repaint);
    }
}