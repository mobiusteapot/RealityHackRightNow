using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BranchDebugger))]
public class BranchDebuggerEditor : Editor
{
    private BranchDebugger branchDebugger;
    private BranchNode branchNode;

    private string newBranchTopic = "";
    private string targetTopic = "";
    private string childTopic = "";
    private string topicToCheck = "";
    private bool getFirstBranchPrefab = false;

    private void OnEnable()
    {
        branchDebugger = (BranchDebugger)target;
        branchNode = branchDebugger.GetComponent<BranchNode>();
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector for `BranchDebugger`
        DrawDefaultInspector();

        if (branchNode == null)
        {
            EditorGUILayout.HelpBox("BranchNode component not found. Ensure BranchNode is attached.", MessageType.Error);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Branch Debugging Tools", EditorStyles.boldLabel);

        DrawAddNewBranchSection();
        DrawAddBranchToTopicSection();
        DrawBranchStateSection();
    }

    private void DrawAddNewBranchSection()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Branch", EditorStyles.boldLabel);

        // Add checkbox for forcing the first branch prefab
        getFirstBranchPrefab = EditorGUILayout.Toggle("Force First Branch Prefab", getFirstBranchPrefab);
        newBranchTopic = EditorGUILayout.TextField("New Branch Topic", newBranchTopic);
        if (GUILayout.Button("Add New Branch"))
        {
            var sequenceState = getFirstBranchPrefab ? SequenceState.FirstBranch : SequenceState.Default;
            if (string.IsNullOrWhiteSpace(newBranchTopic))
            {
                Debug.LogWarning("New branch topic cannot be empty.");
            }
            else if (!branchNode.TryAddNewBranch(newBranchTopic, sequenceState))
            {
                Debug.LogWarning($"Failed to add new branch with topic '{newBranchTopic}'. All child branches might be full.");
            }
            else
            {
                Debug.Log($"Successfully added new branch with topic '{newBranchTopic}'.");
            }
        }
    }

    private void DrawAddBranchToTopicSection()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add Branch to Specific Topic", EditorStyles.boldLabel);

        targetTopic = EditorGUILayout.TextField("Target Branch Topic", targetTopic);
        childTopic = EditorGUILayout.TextField("Child Branch Topic", childTopic);

        if (GUILayout.Button("Add Branch to Topic"))
        {
            if (string.IsNullOrWhiteSpace(targetTopic) || string.IsNullOrWhiteSpace(childTopic))
            {
                Debug.LogWarning("Both target and child topics must be provided.");
            }
            else if (!branchNode.AddBranchAtTopic(targetTopic, childTopic))
            {
                Debug.LogWarning($"Failed to add branch with topic '{childTopic}' under topic '{targetTopic}'. Ensure the target branch exists.");
            }
            else
            {
                Debug.Log($"Successfully added branch with topic '{childTopic}' under topic '{targetTopic}'.");
            }
        }
    }

    private void DrawBranchStateSection()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Branch State", EditorStyles.boldLabel);

        if (branchNode.BranchSockets != null && branchNode.BranchSockets.Count > 0)
        {
            foreach (var socket in branchNode.BranchSockets)
            {
                string socketState = socket.BranchNode != null
                    ? $"Occupied by '{socket.BranchNode.BranchTopic}'"
                    : "Empty";
                EditorGUILayout.LabelField($"Socket: {socket.name}", socketState);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No sockets available on this branch.", MessageType.Info);
        }
    }
}
