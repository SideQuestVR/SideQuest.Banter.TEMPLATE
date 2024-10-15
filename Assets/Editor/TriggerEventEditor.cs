using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(TriggerEvent))]
public class TriggerEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the default inspector

        TriggerEvent triggerEventScript = (TriggerEvent)target;

        // Add a button to trigger the OnTriggerEnter event
        if (GUILayout.Button("Trigger Enter Event"))
        {
            MethodInfo onTriggerEnterMethod = typeof(TriggerEvent).GetMethod("OnTriggerEnter", BindingFlags.NonPublic | BindingFlags.Instance);
            onTriggerEnterMethod.Invoke(triggerEventScript, new object[] { null });
        }

        // Add a button to trigger the OnTriggerExit event
        if (GUILayout.Button("Trigger Exit Event"))
        {
            MethodInfo onTriggerExitMethod = typeof(TriggerEvent).GetMethod("OnTriggerExit", BindingFlags.NonPublic | BindingFlags.Instance);
            onTriggerExitMethod.Invoke(triggerEventScript, new object[] { null });
        }
    }
}
