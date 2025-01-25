using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CanvasButtonSimulator))]
public class CanvasButtonSimulatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CanvasButtonSimulator simulator = (CanvasButtonSimulator)target;

        if (simulator.targetButton != null)
        {
            if (GUILayout.Button("Simulate UI Press"))
            {
                simulator.targetButton.onClick.Invoke();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Assign a UI Button to simulate clicks.", MessageType.Info);
        }
    }
}
