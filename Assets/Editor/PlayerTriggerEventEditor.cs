using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(PlayerTriggerEvent))] // Update to target PlayerTriggerEvent
public class PlayerTriggerEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the default inspector

        PlayerTriggerEvent playerTriggerEventScript = (PlayerTriggerEvent)target;

        // Button to trigger OnGrabEvent
        if (GUILayout.Button("Trigger OnGrabEvent"))
        {
            playerTriggerEventScript.onGrabEvent.Invoke();
        }

        // Button to trigger OnReleaseEvent
        if (GUILayout.Button("Trigger OnReleaseEvent"))
        {
            playerTriggerEventScript.onReleaseEvent.Invoke();
        }

        // Button to trigger OnGrabLeftEvent
        if (GUILayout.Button("Trigger OnGrabLeftEvent"))
        {
            playerTriggerEventScript.onGrabLeftEvent.Invoke();
        }

        // Button to trigger OnReleaseLeftEvent
        if (GUILayout.Button("Trigger OnReleaseLeftEvent"))
        {
            playerTriggerEventScript.onReleaseLeftEvent.Invoke();
        }

        // Button to trigger OnGrabRightEvent
        if (GUILayout.Button("Trigger OnGrabRightEvent"))
        {
            playerTriggerEventScript.onGrabRightEvent.Invoke();
        }

        // Button to trigger OnReleaseRightEvent
        if (GUILayout.Button("Trigger OnReleaseRightEvent"))
        {
            playerTriggerEventScript.onReleaseRightEvent.Invoke();
        }

        // Button to trigger OnPointerClickEvent
        if (GUILayout.Button("Trigger OnPointerClickEvent"))
        {
            playerTriggerEventScript.onPointerClickEvent.Invoke();
        }
    }
}
