using UnityEngine;

// Lightweight monobehaviour for editor scripts to debug a Branch Node
[RequireComponent(typeof(BranchNode))]
public class BranchDebugger : MonoBehaviour
{
    [SerializeField]
    private BranchNode branchNode;

    private void Reset() {
        TryGetComponent(out branchNode);
    }
}
