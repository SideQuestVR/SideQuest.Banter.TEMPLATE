using UnityEngine;
using UnityEditor;

public class PrefabAssignerEditor : EditorWindow
{
    private GameObject prefabToAssign;
    private float yOffset = 0.0f;

    [MenuItem("Banter/Tools/Prefab Assigner")]
    public static void ShowWindow()
    {
        GetWindow<PrefabAssignerEditor>("Prefab Assigner");
    }

    void OnGUI()
    {
        prefabToAssign = (GameObject)EditorGUILayout.ObjectField("Prefab to assign", prefabToAssign, typeof(GameObject), false);
        yOffset = EditorGUILayout.FloatField("Y Offset", yOffset);

        if (GUILayout.Button("Assign Prefab to Selected"))
        {
            AssignPrefabToSelected();
        }
    }

    void AssignPrefabToSelected()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefabToAssign);
            newObject.transform.parent = obj.transform; // make the new object a child of the selected object
            newObject.transform.localPosition = new Vector3(0, yOffset, 0);
            newObject.transform.localRotation = Quaternion.identity;
            newObject.transform.localScale = Vector3.one;
        }
    }
}
