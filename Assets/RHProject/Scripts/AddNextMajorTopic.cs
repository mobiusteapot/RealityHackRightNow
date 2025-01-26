using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Fusion;

public class AddNextMajorTopic : NetworkBehaviour
{
    // Dynamically fetched on start with delay....?
    public float GetBranchDelay = 1.0f;
    [ReadOnly, SerializeField]
    private BranchNode parentBranchNode;

    [SerializeField]
    private InputField inputField;

    private void Start()
    {
        // Get first component of class BranchParentSocket
        StartCoroutine(DelayedGetBranchNode());
    }
    private IEnumerator DelayedGetBranchNode()
    {
        yield return new WaitForSeconds(GetBranchDelay);
        parentBranchNode = FindAnyObjectByType<BranchParentSocket>().BranchNode;
    }

    public void OnInputSent()
    {
        Debug.Log("OnInputSent from ChangeTreeTest");
        if (string.IsNullOrWhiteSpace(inputField.text))
            return;

            RPC_RequestAddBranch(inputField.text);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestAddBranch(string topic, RpcInfo info = default)
    {
        bool success = parentBranchNode.TryAddNewBranch(topic, TreeScriptedSequence.Instance.GetCurrentState());
        Debug.Log($"[Server] TryAddNewBranch({topic}) success = {success}");

        RPC_ApplyAddBranch(topic);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ApplyAddBranch(string topic, RpcInfo info = default)
    {
        parentBranchNode.TryAddNewBranch(topic, TreeScriptedSequence.Instance.GetCurrentState());

        Debug.Log($"[All Clients] TryAddNewBranch({topic})");
    }
}
